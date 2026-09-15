# SecureBaseline

Baseline de referência para uma API de instituição financeira, construído para que cada controle recomendado pelo **OWASP Top 10:2025** e exigido pelo **OWASP ASVS 5.0, nível 2**, exista no código, na configuração ou no pipeline — com evidência verificável (teste, scan ou configuração auditável). Os endpoints da API não têm regra de negócio: são apenas o que os controles protegem.

O projeto tem um único mantenedor. Toda revisão humana do código, inclusive o gerado com IA, acontece localmente e é atestada pela assinatura do commit; a IA nunca commita nem abre pull requests.

## Documentação

- [`docs/arquitetura.md`](docs/arquitetura.md) — componentes, fluxos e fronteiras de confiança da arquitetura alvo.
- [`docs/adr/`](docs/adr/) — decisões arquiteturais registradas, uma por arquivo.
- [`docs/modelo-de-ameacas.md`](docs/modelo-de-ameacas.md) — modelagem de ameaças (STRIDE) e riscos aceitos.
- [`docs/matriz-asvs.md`](docs/matriz-asvs.md) — rastreabilidade contra o OWASP ASVS 5.0, validada automaticamente a cada pull request.
- [`docs/procedimentos/configuracao-do-repositorio-github.md`](docs/procedimentos/configuracao-do-repositorio-github.md) — como a governança do repositório GitHub é configurada e reaplicada.

## Governança do repositório

O branch principal só aceita mudanças por pull request, com commits assinados e os checks obrigatórios aprovados — sem exceção, nem para o mantenedor. A configuração está versionada em [`.github/rulesets/default-branch.json`](.github/rulesets/default-branch.json) e é aplicada por [`scripts/github/apply-ruleset.sh`](scripts/github/apply-ruleset.sh), executado localmente pelo mantenedor com um token de vida curta — nunca por um workflow de CI.
