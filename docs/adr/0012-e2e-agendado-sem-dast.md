# 0012. Suíte E2E agendada, sem DAST

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

Controles testados isoladamente (testes unitários e de integração narrow) não provam que funcionam juntos: o comportamento real do gateway, do IdP, dos cookies, do TLS e dos headers só se valida com o ambiente completo no ar. Ao mesmo tempo, um teste amplo, ponta a ponta, é lento, mais caro de manter e mais frágil que a base da pirâmide de testes — não deve substituí-la, apenas complementá-la em um ponto específico. Separadamente, existe a questão de varredura dinâmica de vulnerabilidades (DAST): ferramentas como o OWASP ZAP encontram uma classe de problema diferente da que os testes E2E funcionais cobrem.

## Decisão

Uma única suíte **E2E com Playwright**, autorizada explicitamente como a exceção consciente à pirâmide de testes, valida os fluxos de identidade (cadastro, autenticação, MFA, recuperação), casos de abuso e os controles de sessão, acesso e headers contra o ambiente `docker compose` completo. Ela roda por **agendamento** e por disparo manual — **não** como check obrigatório de todo PR. **DAST não faz parte do escopo** do projeto.

## Alternativas consideradas

- **E2E como check obrigatório de PR:** garantiria que todo PR passa pela suíte completa antes do merge, mas o ambiente completo (~12 containers, ordem de subida rígida) é lento e mais frágil de rodar a cada PR do que a cadência de agendamento justifica. Rejeitada; a suíte ainda cobre regressões, só que com atraso de até um ciclo de agendamento — risco aceito.
- **DAST agendado (OWASP ZAP) em paralelo ao E2E:** agregaria uma classe de achado diferente (varredura automática de vulnerabilidades na aplicação em execução), mas exige triagem de falsos positivos e manutenção de uma ferramenta adicional, desproporcional ao escopo de um projeto de estudo com API sem regra de negócio real. Rejeitada; fora do escopo global do projeto.
- **Substituir toda a base da pirâmide (unitários e integração narrow) por mais E2E:** rejeitada explicitamente — é o oposto do que a pirâmide de testes recomenda: testes amplos são mais lentos, mais difíceis de diagnosticar na falha e não substituem a cobertura rápida e granular da base.

## Consequências

- Uma regressão de fluxo só é detectada no próximo agendamento da suíte E2E, não no PR que a introduziu — aceito e registrado como risco global.
- A suíte E2E precisa do ambiente `docker compose` completo no ar para rodar, o que a torna mais cara de executar do que os testes unitários e de integração narrow, reforçando por que ela não é obrigatória por PR.
- Achados de segurança que só uma varredura dinâmica encontraria (fora do que os fluxos funcionais da suíte E2E já cobrem) não são procurados ativamente pelo projeto; é uma lacuna conhecida, não um controle implementado e não testado.
