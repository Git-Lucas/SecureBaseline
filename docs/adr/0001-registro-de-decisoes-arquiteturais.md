# 0001. Registro de decisões arquiteturais

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

O projeto toma decisões de arquitetura e segurança que não são óbvias a partir do código sozinho: por que o IdP é pronto e não construído do zero, por que o BFF guarda os tokens, por que a autenticação entre gateway e IdP não usa segredo compartilhado, e assim por diante. Sem um registro, essas decisões e suas alternativas descartadas se perdem, e mudanças futuras correm o risco de desfazer uma escolha deliberada sem perceber o motivo original.

## Decisão

Toda decisão arquitetural relevante é registrada como um **Architecture Decision Record (ADR)** em `docs/adr/`, em pt-BR, em um formato enxuto inspirado em MADR:

- **título** — a decisão em poucas palavras;
- **status** — Proposto, Aceito, Substituído (por qual ADR) ou Rejeitado;
- **data**;
- **Contexto** — o problema e as forças em jogo;
- **Decisão** — o que foi escolhido;
- **Alternativas consideradas** — as opções descartadas e por quê;
- **Consequências** — o que a decisão implica, inclusive riscos aceitos.

**Numeração:** sequencial, começando em `0001`, sem reuso de números. Este ADR é o `0001` porque registra a própria prática.

**Autocontenção:** um ADR nunca cita IDs de PRD nem caminhos ignorados pelo git — quem lê o repositório clonado não tem acesso a esses arquivos. O "porquê" de cada decisão é explicado inteiramente dentro do próprio ADR.

**Quando criar um ADR:** ao introduzir um componente novo, escolher entre alternativas técnicas com trade-offs relevantes, ou aceitar um risco de segurança de forma deliberada. Decisões de estilo de código ou detalhes de implementação sem alternativa real não geram ADR.

## Alternativas consideradas

- **MADR completo ou formato de Michael Nygard:** ambos pedem mais seções (Decision Drivers, Options detalhadas por prós/contras individuais) do que este projeto de estudo precisa. Rejeitado por excesso de processo para o volume de decisões.
- **Decisões documentadas apenas em `docs/arquitetura.md`:** descartado porque um documento de arquitetura único cresce demais e mistura o estado atual com o histórico de por que se chegou a ele.

## Consequências

- Toda decisão listada nos ADRs seguintes (`0002` em diante) segue este template.
- Uma decisão revisitada não edita o ADR original; cria um novo ADR que marca o anterior como **Substituído**.
