# Matriz ASVS 5.0

> Rastreia a aderência do projeto ao OWASP Application Security Verification Standard (ASVS), versão 5.0.0, para os capítulos V1–V4 e V6–V16, níveis 1 e 2. Cada requisito é atualizado pela change que o implementa; o valor inicial, nesta change, reflete apenas o que M1 (governança do repositório) entrega.

## Fonte e atribuição

OWASP Application Security Verification Standard, versão 5.0.0, © OWASP Foundation, licenciado sob [Creative Commons Attribution-ShareAlike 4.0 International (CC BY-SA 4.0)](https://creativecommons.org/licenses/by-sa/4.0/). Fonte oficial: [OWASP/ASVS, tag v5.0.0](https://github.com/OWASP/ASVS/tree/v5.0.0). Cópia vendorizada e validação automatizada em [`tools/asvs-matrix/`](../tools/asvs-matrix/README.md).

## Legenda

- **Nível:** nível ASVS do requisito (1 ou 2; esta matriz cobre apenas L1 e L2).
- **Status:** `Implementado` (controle existe e tem evidência), `Pendente` (ainda não implementado) ou `N/A` (não se aplica, com justificativa obrigatória).
- **Controle:** o mecanismo que atende o requisito, quando implementado.
- **Evidência:** onde verificar o controle (arquivo, configuração, teste).
- **Justificativa:** obrigatória para todo status `N/A`.

O texto de cada requisito não é reproduzido aqui; siga o ID até a fonte oficial linkada acima.

## Resumo por capítulo

| Capítulo | Nome | Em escopo (L1/L2) | Implementado | Pendente | N/A | Situação |
|---|---|---|---|---|---|---|
| V1 | Encoding and Sanitization | 27 | 0 | 27 | 0 | Em escopo |
| V2 | Validation and Business Logic | 11 | 0 | 11 | 0 | Em escopo |
| V3 | Web Frontend Security | 19 | 0 | 19 | 0 | Em escopo |
| V4 | API and Web Service | 10 | 0 | 10 | 0 | Em escopo |
| V5 | File Handling | 0 | 0 | 0 | 1 | Fora de escopo |
| V6 | Authentication | 35 | 0 | 35 | 0 | Em escopo |
| V7 | Session Management | 18 | 0 | 18 | 0 | Em escopo |
| V8 | Authorization | 7 | 0 | 7 | 0 | Em escopo |
| V9 | Self-contained Tokens | 7 | 0 | 7 | 0 | Em escopo |
| V10 | OAuth and OIDC | 29 | 0 | 29 | 0 | Em escopo |
| V11 | Cryptography | 14 | 0 | 14 | 0 | Em escopo |
| V12 | Secure Communication | 9 | 0 | 9 | 0 | Em escopo |
| V13 | Configuration | 13 | 1 | 12 | 0 | Em escopo |
| V14 | Data Protection | 9 | 0 | 9 | 0 | Parcial |
| V15 | Secure Coding and Architecture | 13 | 3 | 10 | 0 | Em escopo |
| V16 | Security Logging and Error Handling | 16 | 0 | 16 | 0 | Em escopo |
| V17 | WebRTC | 0 | 0 | 0 | 1 | Fora de escopo |

V14 (Data Protection) está marcado como **Parcial**: parte dos requisitos do capítulo pressupõe dados pessoais reais persistidos, o que não existe nesta API (ver [`0011-api-sem-persistencia`](adr/0011-api-sem-persistencia.md)); os requisitos individuais permanecem rastreados linha a linha abaixo, a maioria como `Pendente` até serem avaliados com o código real (PRD-13).

## V1 — Encoding and Sanitization

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V1.1.1 | 2 | Pendente | | | |
| V1.1.2 | 2 | Pendente | | | |
| V1.2.1 | 1 | Pendente | | | |
| V1.2.2 | 1 | Pendente | | | |
| V1.2.3 | 1 | Pendente | | | |
| V1.2.4 | 1 | Pendente | | | |
| V1.2.5 | 1 | Pendente | | | |
| V1.2.6 | 2 | Pendente | | | |
| V1.2.7 | 2 | Pendente | | | |
| V1.2.8 | 2 | Pendente | | | |
| V1.2.9 | 2 | Pendente | | | |
| V1.3.1 | 1 | Pendente | | | |
| V1.3.2 | 1 | Pendente | | | |
| V1.3.3 | 2 | Pendente | | | |
| V1.3.4 | 2 | Pendente | | | |
| V1.3.5 | 2 | Pendente | | | |
| V1.3.6 | 2 | Pendente | | | |
| V1.3.7 | 2 | Pendente | | | |
| V1.3.8 | 2 | Pendente | | | |
| V1.3.9 | 2 | Pendente | | | |
| V1.3.10 | 2 | Pendente | | | |
| V1.3.11 | 2 | Pendente | | | |
| V1.4.1 | 2 | Pendente | | | |
| V1.4.2 | 2 | Pendente | | | |
| V1.4.3 | 2 | Pendente | | | |
| V1.5.1 | 1 | Pendente | | | |
| V1.5.2 | 2 | Pendente | | | |

## V2 — Validation and Business Logic

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V2.1.1 | 1 | Pendente | | | |
| V2.1.2 | 2 | Pendente | | | |
| V2.1.3 | 2 | Pendente | | | |
| V2.2.1 | 1 | Pendente | | | |
| V2.2.2 | 1 | Pendente | | | |
| V2.2.3 | 2 | Pendente | | | |
| V2.3.1 | 1 | Pendente | | | |
| V2.3.2 | 2 | Pendente | | | |
| V2.3.3 | 2 | Pendente | | | |
| V2.3.4 | 2 | Pendente | | | |
| V2.4.1 | 2 | Pendente | | | |

## V3 — Web Frontend Security

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V3.2.1 | 1 | Pendente | | | |
| V3.2.2 | 1 | Pendente | | | |
| V3.3.1 | 1 | Pendente | | | |
| V3.3.2 | 2 | Pendente | | | |
| V3.3.3 | 2 | Pendente | | | |
| V3.3.4 | 2 | Pendente | | | |
| V3.4.1 | 1 | Pendente | | | |
| V3.4.2 | 1 | Pendente | | | |
| V3.4.3 | 2 | Pendente | | | |
| V3.4.4 | 2 | Pendente | | | |
| V3.4.5 | 2 | Pendente | | | |
| V3.4.6 | 2 | Pendente | | | |
| V3.5.1 | 1 | Pendente | | | |
| V3.5.2 | 1 | Pendente | | | |
| V3.5.3 | 1 | Pendente | | | |
| V3.5.4 | 2 | Pendente | | | |
| V3.5.5 | 2 | Pendente | | | |
| V3.7.1 | 2 | Pendente | | | |
| V3.7.2 | 2 | Pendente | | | |

## V4 — API and Web Service

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V4.1.1 | 1 | Pendente | | | |
| V4.1.2 | 2 | Pendente | | | |
| V4.1.3 | 2 | Pendente | | | |
| V4.2.1 | 2 | Pendente | | | |
| V4.3.1 | 2 | Pendente | | | |
| V4.3.2 | 2 | Pendente | | | |
| V4.4.1 | 1 | Pendente | | | |
| V4.4.2 | 2 | Pendente | | | |
| V4.4.3 | 2 | Pendente | | | |
| V4.4.4 | 2 | Pendente | | | |

## V5 — File Handling

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V5 | — | N/A | — | — | Sem upload de arquivos no escopo do projeto (API sem funcionalidade de arquivos). |

## V6 — Authentication

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V6.1.1 | 1 | Pendente | | | |
| V6.1.2 | 2 | Pendente | | | |
| V6.1.3 | 2 | Pendente | | | |
| V6.2.1 | 1 | Pendente | | | |
| V6.2.2 | 1 | Pendente | | | |
| V6.2.3 | 1 | Pendente | | | |
| V6.2.4 | 1 | Pendente | | | |
| V6.2.5 | 1 | Pendente | | | |
| V6.2.6 | 1 | Pendente | | | |
| V6.2.7 | 1 | Pendente | | | |
| V6.2.8 | 1 | Pendente | | | |
| V6.2.9 | 2 | Pendente | | | |
| V6.2.10 | 2 | Pendente | | | |
| V6.2.11 | 2 | Pendente | | | |
| V6.2.12 | 2 | Pendente | | | |
| V6.3.1 | 1 | Pendente | | | |
| V6.3.2 | 1 | Pendente | | | |
| V6.3.3 | 2 | Pendente | | | |
| V6.3.4 | 2 | Pendente | | | |
| V6.4.1 | 1 | Pendente | | | |
| V6.4.2 | 1 | Pendente | | | |
| V6.4.3 | 2 | Pendente | | | |
| V6.4.4 | 2 | Pendente | | | |
| V6.5.1 | 2 | Pendente | | | |
| V6.5.2 | 2 | Pendente | | | |
| V6.5.3 | 2 | Pendente | | | |
| V6.5.4 | 2 | Pendente | | | |
| V6.5.5 | 2 | Pendente | | | |
| V6.6.1 | 2 | Pendente | | | |
| V6.6.2 | 2 | Pendente | | | |
| V6.6.3 | 2 | Pendente | | | |
| V6.8.1 | 2 | Pendente | | | |
| V6.8.2 | 2 | Pendente | | | |
| V6.8.3 | 2 | Pendente | | | |
| V6.8.4 | 2 | Pendente | | | |

## V7 — Session Management

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V7.1.1 | 2 | Pendente | | | |
| V7.1.2 | 2 | Pendente | | | |
| V7.1.3 | 2 | Pendente | | | |
| V7.2.1 | 1 | Pendente | | | |
| V7.2.2 | 1 | Pendente | | | |
| V7.2.3 | 1 | Pendente | | | |
| V7.2.4 | 1 | Pendente | | | |
| V7.3.1 | 2 | Pendente | | | |
| V7.3.2 | 2 | Pendente | | | |
| V7.4.1 | 1 | Pendente | | | |
| V7.4.2 | 1 | Pendente | | | |
| V7.4.3 | 2 | Pendente | | | |
| V7.4.4 | 2 | Pendente | | | |
| V7.4.5 | 2 | Pendente | | | |
| V7.5.1 | 2 | Pendente | | | |
| V7.5.2 | 2 | Pendente | | | |
| V7.6.1 | 2 | Pendente | | | |
| V7.6.2 | 2 | Pendente | | | |

## V8 — Authorization

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V8.1.1 | 1 | Pendente | | | |
| V8.1.2 | 2 | Pendente | | | |
| V8.2.1 | 1 | Pendente | | | |
| V8.2.2 | 1 | Pendente | | | |
| V8.2.3 | 2 | Pendente | | | |
| V8.3.1 | 1 | Pendente | | | |
| V8.4.1 | 2 | Pendente | | | |

## V9 — Self-contained Tokens

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V9.1.1 | 1 | Pendente | | | |
| V9.1.2 | 1 | Pendente | | | |
| V9.1.3 | 1 | Pendente | | | |
| V9.2.1 | 1 | Pendente | | | |
| V9.2.2 | 2 | Pendente | | | |
| V9.2.3 | 2 | Pendente | | | |
| V9.2.4 | 2 | Pendente | | | |

## V10 — OAuth and OIDC

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V10.1.1 | 2 | Pendente | | | |
| V10.1.2 | 2 | Pendente | | | |
| V10.2.1 | 2 | Pendente | | | |
| V10.2.2 | 2 | Pendente | | | |
| V10.3.1 | 2 | Pendente | | | |
| V10.3.2 | 2 | Pendente | | | |
| V10.3.3 | 2 | Pendente | | | |
| V10.3.4 | 2 | Pendente | | | |
| V10.4.1 | 1 | Pendente | | | |
| V10.4.2 | 1 | Pendente | | | |
| V10.4.3 | 1 | Pendente | | | |
| V10.4.4 | 1 | Pendente | | | |
| V10.4.5 | 1 | Pendente | | | |
| V10.4.6 | 2 | Pendente | | | |
| V10.4.7 | 2 | Pendente | | | |
| V10.4.8 | 2 | Pendente | | | |
| V10.4.9 | 2 | Pendente | | | |
| V10.4.10 | 2 | Pendente | | | |
| V10.4.11 | 2 | Pendente | | | |
| V10.5.1 | 2 | Pendente | | | |
| V10.5.2 | 2 | Pendente | | | |
| V10.5.3 | 2 | Pendente | | | |
| V10.5.4 | 2 | Pendente | | | |
| V10.5.5 | 2 | Pendente | | | |
| V10.6.1 | 2 | Pendente | | | |
| V10.6.2 | 2 | Pendente | | | |
| V10.7.1 | 2 | Pendente | | | |
| V10.7.2 | 2 | Pendente | | | |
| V10.7.3 | 2 | Pendente | | | |

## V11 — Cryptography

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V11.1.1 | 2 | Pendente | | | |
| V11.1.2 | 2 | Pendente | | | |
| V11.2.1 | 2 | Pendente | | | |
| V11.2.2 | 2 | Pendente | | | |
| V11.2.3 | 2 | Pendente | | | |
| V11.3.1 | 1 | Pendente | | | |
| V11.3.2 | 1 | Pendente | | | |
| V11.3.3 | 2 | Pendente | | | |
| V11.4.1 | 1 | Pendente | | | |
| V11.4.2 | 2 | Pendente | | | |
| V11.4.3 | 2 | Pendente | | | |
| V11.4.4 | 2 | Pendente | | | |
| V11.5.1 | 2 | Pendente | | | |
| V11.6.1 | 2 | Pendente | | | |

## V12 — Secure Communication

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V12.1.1 | 1 | Pendente | | | |
| V12.1.2 | 2 | Pendente | | | |
| V12.1.3 | 2 | Pendente | | | |
| V12.2.1 | 1 | Pendente | | | |
| V12.2.2 | 1 | Pendente | | | |
| V12.3.1 | 2 | Pendente | | | |
| V12.3.2 | 2 | Pendente | | | |
| V12.3.3 | 2 | Pendente | | | |
| V12.3.4 | 2 | Pendente | | | |

## V13 — Configuration

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V13.1.1 | 2 | Pendente | | | |
| V13.2.1 | 2 | Pendente | | | |
| V13.2.2 | 2 | Pendente | | | |
| V13.2.3 | 2 | Pendente | | | |
| V13.2.4 | 2 | Pendente | | | |
| V13.2.5 | 2 | Pendente | | | |
| V13.3.1 | 2 | Implementado | Secret scanning com push protection habilitados no repositório GitHub | [Log de verificação, 16/09/2026](procedimentos/configuracao-do-repositorio-github.md#6-log-de-verificação) — push com um segredo de teste (PAT real, para validar o checksum) foi recusado pelo GitHub (`GH013`) | |
| V13.3.2 | 2 | Pendente | | | |
| V13.4.1 | 1 | Pendente | | | |
| V13.4.2 | 2 | Pendente | | | |
| V13.4.3 | 2 | Pendente | | | |
| V13.4.4 | 2 | Pendente | | | |
| V13.4.5 | 2 | Pendente | | | |

## V14 — Data Protection

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V14.1.1 | 2 | Pendente | | | |
| V14.1.2 | 2 | Pendente | | | |
| V14.2.1 | 1 | Pendente | | | |
| V14.2.2 | 2 | Pendente | | | |
| V14.2.3 | 2 | Pendente | | | |
| V14.2.4 | 2 | Pendente | | | |
| V14.3.1 | 1 | Pendente | | | |
| V14.3.2 | 2 | Pendente | | | |
| V14.3.3 | 2 | Pendente | | | |

## V15 — Secure Coding and Architecture

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V15.1.1 | 1 | Implementado | Prazos de remediação por severidade e cadência de atualização de bibliotecas, documentados e versionados | [`docs/politica-de-atualizacao-de-dependencias.md`](politica-de-atualizacao-de-dependencias.md) | |
| V15.1.2 | 2 | Implementado | Inventário de componentes de terceiros via SBOM CycloneDX gerado a cada build, e resolução de pacotes restrita e mapeada a uma única fonte | SBOM publicado como artefato de [`ci.yml`](../.github/workflows/ci.yml) (`SUP-15`); [`nuget.config`](../nuget.config) (`SUP-10`) | |
| V15.1.3 | 2 | Pendente | | | |
| V15.2.1 | 1 | Implementado | NuGetAudit falha o build em qualquer vulnerabilidade conhecida (direta ou transitiva); supressão exige justificativa e prazo de remediação documentado | [`Directory.Build.props`](../Directory.Build.props), [`Directory.Build.targets`](../Directory.Build.targets) (`SUP-11`, `SUP-12`); [`docs/politica-de-atualizacao-de-dependencias.md`](politica-de-atualizacao-de-dependencias.md) (`SUP-17`) | |
| V15.2.2 | 2 | Pendente | | | |
| V15.2.3 | 2 | Pendente | | | |
| V15.3.1 | 1 | Pendente | | | |
| V15.3.2 | 2 | Pendente | | | |
| V15.3.3 | 2 | Pendente | | | |
| V15.3.4 | 2 | Pendente | | | |
| V15.3.5 | 2 | Pendente | | | |
| V15.3.6 | 2 | Pendente | | | |
| V15.3.7 | 2 | Pendente | | | |

## V16 — Security Logging and Error Handling

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V16.1.1 | 2 | Pendente | | | |
| V16.2.1 | 2 | Pendente | | | |
| V16.2.2 | 2 | Pendente | | | |
| V16.2.3 | 2 | Pendente | | | |
| V16.2.4 | 2 | Pendente | | | |
| V16.2.5 | 2 | Pendente | | | |
| V16.3.1 | 2 | Pendente | | | |
| V16.3.2 | 2 | Pendente | | | |
| V16.3.3 | 2 | Pendente | | | |
| V16.3.4 | 2 | Pendente | | | |
| V16.4.1 | 2 | Pendente | | | |
| V16.4.2 | 2 | Pendente | | | |
| V16.4.3 | 2 | Pendente | | | |
| V16.5.1 | 2 | Pendente | | | |
| V16.5.2 | 2 | Pendente | | | |
| V16.5.3 | 2 | Pendente | | | |

## V17 — WebRTC

| ID | Nível | Status | Controle | Evidência | Justificativa |
|---|---|---|---|---|---|
| V17 | — | N/A | — | — | Sem WebRTC no escopo do projeto (nenhum componente usa comunicação em tempo real via WebRTC). |

