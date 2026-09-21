# Procedimento — Configuração do repositório GitHub

> Executado pelo mantenedor. Cobre tudo o que protege o branch principal e que não está no controle de acesso do código em si: assinatura de commit, configurações do repositório fora do ruleset versionado, e como aplicar o ruleset.

## 1. Assinatura de commit por chave SSH

Todo commit que chega ao branch principal precisa de assinatura verificada (`gpg.format=ssh`). A configuração é feita **por repositório** (`git config` local, sem `--global`), nunca global — este mantenedor trabalha em vários projetos pessoais que não devem herdar essa configuração. Isso implica repetir os três `git config` abaixo **em cada clone** deste repositório.

Este projeto é mantido em dois clones separados do mesmo remoto: um no filesystem do WSL (uso de linha de comando, scripts) e outro no filesystem do Windows (aberto nativamente pelo Visual Studio). Cada clone tem seu próprio `.git/config`, então cada um recebe sua própria configuração local.

### WSL (dentro do clone, ex. `~/projects/SecureBaseline`)

```bash
# gera uma chave dedicada a assinatura, se ainda não existir
ssh-keygen -t ed25519 -C "assinatura-commits-securebaseline" -f ~/.ssh/id_ed25519_signing

cd ~/projects/SecureBaseline
git config gpg.format ssh
git config user.signingkey ~/.ssh/id_ed25519_signing.pub
git config commit.gpgsign true
```

Registrar `~/.ssh/id_ed25519_signing.pub` no GitHub em **Settings → SSH and GPG keys → New SSH key**, com o tipo **Signing Key** (não Authentication Key).

### Git for Windows (clone separado, aberto pelo Visual Studio)

Reaproveitar a mesma chave (copiada para o Windows) evita registrar uma segunda signing key no GitHub:

```powershell
# copia a chave gerada no WSL (distro Ubuntu-24.04) para o perfil do Windows
Copy-Item \\wsl.localhost\Ubuntu-24.04\home\lucas\.ssh\id_ed25519_signing* "$env:USERPROFILE\.ssh\"
```

```powershell
cd C:\Users\lucas\source\repos\SecureBaseline  # ajustar se o clone Windows estiver em outro caminho
git config gpg.format ssh
git config user.signingkey "$env:USERPROFILE\.ssh\id_ed25519_signing.pub"
git config commit.gpgsign true
```

Alternativa: gerar uma segunda chave direto no Windows (`ssh-keygen`, se o OpenSSH Client estiver instalado) e registrá-la como uma **segunda** signing key no GitHub — evita copiar a chave privada entre sistemas, ao custo de mais uma chave para gerenciar.

### Verificação

Fazer um commit de teste em cada clone e confirmar que o GitHub mostra o selo **Verified** no commit. Repetir a partir de cada ambiente (WSL e Visual Studio) antes de habilitar o ruleset — um commit sem assinatura verificada, depois do ruleset ativo, é bloqueado no merge. Como a configuração é local, clonar o repositório de novo em outro lugar exige repetir estes três `git config`.

## 2. Checklist de configurações do repositório

Estas configurações não fazem parte do ruleset versionado (que cobre só o branch); são aplicadas manualmente pela UI do GitHub em **Settings**.

- [x] **Secret scanning** habilitado (`Settings → Code security → Secret scanning`).
- [x] **Push protection** habilitada junto do secret scanning.
- [x] **Permissão padrão do `GITHUB_TOKEN`** definida como **somente leitura** (`Settings → Actions → General → Workflow permissions → Read repository contents permission`).
- [x] **GitHub Actions não pode criar nem aprovar pull requests** (mesma tela, desmarcar "Allow GitHub Actions to create and approve pull requests").
- [x] **Actions pinadas por SHA completo** exigido (`Settings → Actions → General → Require actions to be pinned to a full-length commit SHA`) — `SUP-01`.
- [x] **Nenhum self-hosted runner registrado** (`Settings → Actions → Runners`) — `SUP-03`.
- [x] **Private vulnerability reporting** habilitado (`Settings → Code security → Private vulnerability reporting`) — `SUP-18`.

### Teste manual único de push protection

**Um valor aleatório com o prefixo/tamanho certos não é suficiente para este teste.** Vários tipos de segredo suportados só são bloqueados quando o valor passa por uma validação estrutural, não só por regex de prefixo:

- **Tokens do GitHub (`ghp_`, `gho_`, etc.):** carregam um checksum CRC32 (Base62) nos últimos 6 caracteres, validado localmente pelo push protection antes de bloquear — isso existe justamente para não acusar qualquer string que comece com `ghp_` por coincidência. Uma string aleatória com o prefixo certo mas checksum inválido passa despercebida.
- **AWS Access Key ID:** só é reconhecida como credencial completa em par com a `aws_secret_access_key` correspondente; o ID sozinho não aciona o bloqueio.

Forma confiável de testar: gerar um fine-grained PAT **real**, de escopo mínimo (ou nenhum) e expiração de 1 dia — o mesmo mecanismo já usado na seção 3 —, colá-lo num arquivo de teste, commitar (assinado) e tentar o push. Por ser um token emitido de fato pelo GitHub, o checksum é válido e o push protection deve recusar o push com `GH013`. **Revogar esse token imediatamente após o teste**, independentemente do resultado, e gerar um token separado para uso real (seção 3).

Registrar o resultado no log de verificação (seção 7). Este teste é manual e único — não há verificação automatizada de push protection no CI.

**Nota:** push protection cobre apenas padrões de alta confiança e pode ser contornada pelo autor do push (o GitHub permite marcar o alerta como "não é um segredo real" e prosseguir); isso gera um alerta de secret scanning, mas não impede o push em definitivo.

## 3. Criar um token de vida curta

O script de aplicação do ruleset (`scripts/github/apply-ruleset.sh`) precisa de um token com permissão de administração do repositório. Ele é criado sob demanda e nunca fica salvo em arquivo:

1. GitHub → **Settings → Developer settings → Personal access tokens → Fine-grained tokens → Generate new token**.
2. **Repository access:** apenas este repositório.
3. **Permissions → Administration:** Read and write.
4. **Expiration:** 1 dia (o menor período disponível que cobrir a sessão de trabalho).
5. Copiar o token — ele só é exibido uma vez.

## 4. Aplicar o ruleset

```bash
scripts/github/apply-ruleset.sh
```

O script pede o token via prompt oculto (não aparece no terminal nem fica em variável de ambiente persistida) e aplica `.github/rulesets/default-branch.json`. Ele é idempotente: rodar de novo com o mesmo arquivo atualiza o ruleset existente em vez de duplicá-lo.

### Bootstrap (primeira aplicação, antes de os checks existirem)

Na primeira aplicação, os checks obrigatórios (`ignored-path-references`, `asvs-matrix`) ainda não têm histórico de execução — se o ruleset já os exigir, nenhum PR consegue passar. Por isso a primeira aplicação usa a flag de bootstrap:

```bash
scripts/github/apply-ruleset.sh --without-required-checks
```

Isso aplica o ruleset sem a regra de `required_status_checks`. Depois que o primeiro PR (o desta própria change) passa pelos checks com sucesso e é mesclado, reaplicar **sem** a flag:

```bash
scripts/github/apply-ruleset.sh
```

Confirmar na UI do GitHub (**Settings → Rules → Rulesets**) que o ruleset está ativo e que os dois checks aparecem como obrigatórios, vinculados à integração GitHub Actions.

**Sempre revogar o token** (**Settings → Developer settings → Personal access tokens**) assim que o script terminar, mesmo antes da expiração de 1 dia.

### Reaplicação com os checks de `add-supply-chain-pipeline`

A change `add-supply-chain-pipeline` acrescenta os checks obrigatórios `build`, `analyze (csharp)`, `analyze (actions)` e `dependency-review`, além de uma regra `code_scanning` que bloqueia o merge em alerta de segurança do CodeQL de severidade `medium` ou maior, ou alerta de qualidade `error`.

A regra `code_scanning` só funciona depois que o CodeQL já tem uma análise do branch principal — aplicada antes disso, ela bloqueia todo PR. Por isso a reaplicação segue esta ordem:

1. Mesclar o PR desta change (os checks novos rodam, mas ainda não são obrigatórios).
2. Confirmar que o `push` em `master` decorrente do merge executa `codeql.yml` até o fim.
3. Só então reaplicar o script com um novo token de vida curta, agora com o JSON atualizado (`.github/rulesets/default-branch.json`).
4. Confirmar na UI (**Settings → Rules → Rulesets**) os quatro novos required status checks e a regra `code_scanning`.

## 5. Regra: todo novo check obrigatório atualiza o JSON

Uma change que adiciona uma verificação de PR (um novo job de workflow) precisa:

1. Adicionar o novo context em `.github/rulesets/default-branch.json`, dentro de `required_status_checks`.
2. Fazer o PR dessa change passar por todos os checks já existentes (o novo ainda não é obrigatório nesse momento).
3. Depois do merge, reaplicar o script com um novo token de vida curta, agora exigindo o novo context.

### Ordem para renomear um job existente

O nome do job é o *context* do check obrigatório; renomeá-lo sem cuidado deixa o context antigo pendente para sempre (o GitHub não sabe que ele "virou" outro nome):

1. Adicionar o job com o **novo** nome, mantendo o job antigo.
2. Reaplicar o JSON exigindo **ambos** os contexts (antigo e novo) como obrigatórios.
3. Mesclar um PR em que os dois passam.
4. Remover o job antigo do workflow e do JSON, reaplicar o script uma última vez.

## 6. Dependabot: lock files e mudança de versão major do .NET

O Dependabot (`SUP-06`) atualiza `src/SecureBaseline.Api/packages.lock.json` normalmente (projeto único, sem `ProjectReference`), mas **não** atualiza `tools/asvs-matrix/packages.lock.json` — ele não conhece apps file-based. Uma atualização de um pacote global (hoje, só o `SonarAnalyzer.CSharp`, via `GlobalPackageReference`) portanto passa no `build` mas falha `asvs-matrix` com `NU1004` até o lock file do validador ser regenerado manualmente:

```bash
git checkout <branch-do-dependabot>
dotnet run tools/asvs-matrix/ValidateAsvsMatrix.cs   # sem CI=true, reescreve o lock file
git add tools/asvs-matrix/packages.lock.json
git commit -S -m "chore: Atualizado lock file do validador ASVS"
git push
```

**Migração para uma nova versão major do .NET** (ex.: `net10.0` → `net11.0`) é sempre manual, nunca via Dependabot: o `ignore` em `.github/dependabot.yml` bloqueia atualizações major de `Microsoft.AspNetCore.*` e `Microsoft.Extensions.*` porque eles seguem a versão major do .NET, e o Dependabot não confere compatibilidade de target framework de forma confiável. A migração exige alterar junto, na mesma mudança: `global.json` (SDK), `TargetFramework` do(s) projeto(s) e esses pacotes.

## 7. Log de verificação

Registrar aqui, com data e resultado, cada cenário abaixo, executado uma vez contra o repositório real após o ruleset completo (com checks obrigatórios) estar ativo:

| Data | Cenário | Resultado |
|---|---|---|
|16/09/2026| Push direto ao branch principal é recusado |Confirmado|
|16/09/2026| Force push ao branch principal é recusado |Confirmado|
|16/09/2026| PR com commit não assinado é bloqueado no merge |Confirmado|
|16/09/2026| PR introduzindo referência a caminho ignorado falha o check `ignored-path-references` e não mescla |Corrigido|
|16/09/2026| Push protection bloqueia um segredo de teste |Corrigido|
|21/09/2026| PR com um passo de workflow usando uma action de terceiros por tag recebe um alerta do CodeQL (`actions/unpinned-tag`) que bloqueia o merge |Confirmado|
|21/09/2026| Workflow que usa uma action por tag, inclusive de primeira parte, tem a execução recusada pela configuração do repositório |Confirmado|
|21/09/2026| PR com lock file desatualizado falha o check `build` |Confirmado|
|21/09/2026| Dependabot abre as primeiras PRs de atualização para NuGet e GitHub Actions |Nenhuma atualização disponível no momento da checagem|
|21/09/2026| O `build` agendado diário roda em `master` |Confirmado|
