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

- [ ] **Secret scanning** habilitado (`Settings → Code security → Secret scanning`).
- [ ] **Push protection** habilitada junto do secret scanning.
- [ ] **Permissão padrão do `GITHUB_TOKEN`** definida como **somente leitura** (`Settings → Actions → General → Workflow permissions → Read repository contents permission`).
- [ ] **GitHub Actions não pode criar nem aprovar pull requests** (mesma tela, desmarcar "Allow GitHub Actions to create and approve pull requests").

### Teste manual único de push protection

Fazer uma tentativa de push contendo um segredo de teste em formato reconhecido pelo GitHub (por exemplo, um token de exemplo com o prefixo de um provedor suportado) e confirmar que o push é recusado. Registrar o resultado no log de verificação (seção 5). Este teste é manual e único — não há verificação automatizada de push protection no CI.

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

## 6. Log de verificação

Registrar aqui, com data e resultado, cada cenário abaixo, executado uma vez contra o repositório real após o ruleset completo (com checks obrigatórios) estar ativo:

| Data | Cenário | Resultado |
|---|---|---|
| | Push direto ao branch principal é recusado | |
| | Force push ao branch principal é recusado | |
| | PR com commit não assinado é bloqueado no merge | |
|16/09/2026| PR introduzindo referência a caminho ignorado falha o check `ignored-path-references` e não mescla |Corrigido|
|16/09/2026| Push protection bloqueia um segredo de teste |AKIAIOSFODNN7EXAMPLE|
