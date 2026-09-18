# Política de atualização e remediação de dependências

> Define os prazos de remediação para vulnerabilidades conhecidas em componentes de terceiros e a cadência geral de atualização de bibliotecas (ASVS 15.1.1, `SUP-17`).

## Prazos de remediação por severidade

O relógio começa a contar quando a auditoria diária (`SUP-13`) ou um alerta do Dependabot reporta a vulnerabilidade contra o branch principal.

| Severidade | Prazo |
|---|---|
| Crítica | 7 dias |
| Alta | 30 dias |
| Média | 90 dias |
| Baixa | 180 dias |

Como o build falha em qualquer vulnerabilidade conhecida (`SUP-11`), na prática o branch principal não avança enquanto a vulnerabilidade não é corrigida ou a supressão não é registrada com justificativa (`SUP-12`, ver seção abaixo). Os prazos acima definem até quando essa situação é aceitável antes de virar um problema de processo, não uma trava técnica adicional.

## Atualizações do Dependabot

Pull requests de atualização de versão abertos pelo Dependabot (NuGet e GitHub Actions, `SUP-06`) são avaliados e tratados em até **30 dias** da abertura, independentemente de haver ou não vulnerabilidade associada — parte da cadência geral de manutenção do projeto.

## Supressão justificada de auditoria

Quando corrigir uma vulnerabilidade dentro do prazo não é viável (ex.: sem versão corrigida disponível, ou o componente vulnerável não é exercitado pelo projeto), a auditoria pode ser suprimida pontualmente via `NuGetAuditSuppress` em `Directory.Build.props`. Toda supressão exige os metadados `Justification` (o motivo) e `Issue` (uma issue do repositório que rastreia a remoção da supressão); sem os dois, o build falha (`SUP-12`). A supressão nunca dispensa o prazo da tabela acima — ela documenta por que o prazo ainda não foi cumprido.
