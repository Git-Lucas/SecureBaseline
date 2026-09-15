# 0002. Keycloak como provedor de identidade pronto

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

O sistema precisa de cadastro de usuários, autenticação, gestão de sessão no IdP, MFA e recuperação de conta — um conjunto de funcionalidades com décadas de vulnerabilidades conhecidas (enumeração de contas, força bruta, fixação de sessão, bypass de MFA). Construir esse conjunto do zero, mesmo em um projeto de estudo, significa reproduzir esses erros e não testar os controles que o projeto quer demonstrar: os controles em volta de um IdP, não a implementação de um IdP.

## Decisão

Usar o **Keycloak** em modo produção, com PostgreSQL como base de dados, como provedor de identidade e servidor de autorização OAuth2/OIDC. Toda configuração (realm, clients, políticas de senha, fluxos de autenticação) é definida como código versionado, sem segredos no repositório.

## Alternativas consideradas

- **Implementação própria de autenticação:** rejeitada. É o anti-padrão mais citado em guias de segurança de autenticação — construir o próprio sistema de login expõe a mais classes de vulnerabilidade do que reutiliza de conhecimento existente, sem agregar valor ao objetivo do projeto (demonstrar controles).
- **Outro IdP open source (Ory Kratos/Hydra, Authentik):** viável, mas o Keycloak tem o conjunto mais completo de funcionalidades prontas (MFA, políticas de senha, eventos, extensibilidade via SPI) exigidas por este projeto em um único produto, o que reduz a integração necessária.
- **IdP como serviço (Auth0, Okta, Entra ID):** rejeitado porque o projeto roda inteiramente local via docker compose, sem dependência de conta externa ou de rede para operar.

## Consequências

- O projeto herda a superfície de configuração do Keycloak: erros de configuração (não de código) tornam-se a principal fonte de risco em torno do IdP.
- Customizações que o Keycloak não oferece nativamente (como a checagem de senhas vazadas) exigem extensão própria via SPI, escrita em Java.
- O console administrativo do Keycloak não é exposto ao host; toda configuração é aplicada por arquivo de realm versionado, não manualmente pela UI.
