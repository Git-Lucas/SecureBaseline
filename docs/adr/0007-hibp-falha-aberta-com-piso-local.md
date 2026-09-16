# 0007. Checagem de senhas vazadas com falha aberta e piso local

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

O padrão de segurança de autenticação recomenda rejeitar senhas conhecidas por vazamento, checadas contra uma base como o *Have I Been Pwned* (HIBP) — a mais completa disponível, distribuída apenas como hashes (k-anonimato por prefixo, a senha em si nunca sai do ambiente). Essa checagem depende de um serviço externo à infraestrutura do projeto. É preciso decidir o que acontece quando esse serviço está indisponível: bloquear todo cadastro e troca de senha (falha fechada) ou aceitar a senha sem a checagem completa (falha aberta)?

## Decisão

A checagem falha **aberta**: se o HIBP estiver indisponível, o cadastro ou a troca de senha não é bloqueado, mas passa por um **piso local** de senhas comuns (uma lista pequena, mantida no próprio ambiente) e a indisponibilidade gera um evento de segurança rastreável.

## Alternativas consideradas

- **Falha fechada (bloquear cadastro/troca de senha se o HIBP estiver fora do ar):** rejeitada. Tornaria a disponibilidade de um serviço de terceiro, fora do controle do projeto, um ponto único de falha para a funcionalidade central de cadastro — um risco de disponibilidade desproporcional ao ganho de segurança marginal da checagem completa numa janela de indisponibilidade.
- **Sem piso local, aceitar qualquer senha durante a indisponibilidade:** rejeitada por abrir mão de toda proteção contra as senhas mais óbvias justamente na janela de falha.
- **Cache local completo da base HIBP:** eliminaria a dependência externa, mas exige armazenar e manter atualizado um corpus de centenas de milhões de hashes — desproporcional ao propósito de demonstração deste projeto.

## Consequências

- Durante uma indisponibilidade do HIBP, uma senha vazada mas ausente da lista local pequena pode ser aceita. Risco aceito, registrado no modelo de ameaças, mitigado por o evento de indisponibilidade ser visível (não silencioso).
- A checagem depende de o proxy de egress permitir a saída do Keycloak para o HIBP e de mais nenhum componente ter acesso à internet — a falha aberta só é aceitável porque é a única saída de rede do ambiente e está sob observação.
- A senha do usuário nunca é enviada por inteiro: apenas um prefixo do hash SHA-1 é consultado (k-anonimato), preservando a confidencialidade mesmo em caso de comprometimento do proxy de egress.
