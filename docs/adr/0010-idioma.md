# 0010. Divisão de idioma entre código e documentação

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

O projeto é mantido por um único desenvolvedor brasileiro, mas segue convenções internacionais de nomenclatura de código e referencia normas publicadas em inglês (OWASP Top 10, ASVS). Misturar os dois idiomas sem uma regra clara leva a inconsistência: identificadores em português, mensagens de log em português dificultando a correlação com ferramentas em inglês, ou documentação em inglês que não serve ao público real do projeto.

## Decisão

- **Código 100% em inglês:** identificadores, rotas, JSON, scopes OAuth, testes e nomes de teste, mensagens de log, arquivos de configuração.
- **Comentários e XML docs em pt-BR**, explicando o porquê de uma decisão não óbvia e citando o ID do requisito de segurança (ASVS/Top 10) que o trecho implementa.
- **Documentação versionada em `docs/`** (arquitetura, ADRs, modelo de ameaças, matriz ASVS, procedimentos) em pt-BR — nomes de arquivo e pasta em pt-BR, minúsculo, sem acento.
- **Textos exibidos ao usuário final da aplicação** em pt-BR; **erros da API** em inglês (consistente com o restante do código).
- Termos técnicos consagrados (ADR, ASVS, GitHub, BFF, IdP, TOTP, MFA, HIBP, E2E, DAST, IaC e nomes de parâmetros como `private_key_jwt`) não são traduzidos, em nenhum dos dois contextos.

## Alternativas consideradas

- **Tudo em inglês, inclusive documentação versionada:** mais alinhado a convenções internacionais de projetos open source, mas o documento deixaria de ser natural para o público real deste projeto. Rejeitada.
- **Tudo em pt-BR, inclusive código:** identificadores e mensagens de log em português quebram convenções da própria stack (.NET, OWASP) e dificultam comparar este código com qualquer exemplo ou documentação de referência, que são majoritariamente em inglês. Rejeitada.
- **Comentários em inglês, só a documentação de `docs/` em pt-BR:** consideraria o código "mais internacional", mas comentários existem para explicar o porquê a quem mantém o projeto — escrevê-los em um idioma que não é o do mantenedor não adiciona valor aqui. Rejeitada.

## Consequências

- Toda revisão de código verifica também a divisão de idioma, não só a correção funcional.
- Nomes de arquivo e pasta em `docs/` removem acentos e cedilha deliberadamente (ex.: `modelo-de-ameacas.md`), para manter caminhos seguros em shells, URLs e checkouts multiplataforma — não é uma inconsistência com "pt-BR", é uma restrição técnica sobreposta a ele.
