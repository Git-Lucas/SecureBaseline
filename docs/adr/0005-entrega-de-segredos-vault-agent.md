# 0005. Entrega de segredos por Vault Agent como arquivos

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

Cada serviço (gateway, API, Keycloak) precisa de segredos e certificados para operar. A forma mais comum de entregá-los — variáveis de ambiente definidas no compose ou copiadas para arquivos de configuração — perde controle de acesso granular, não audita quem leu o quê e não expira: qualquer processo no container lê `/proc/<pid>/environ`, `docker inspect` mostra variáveis de ambiente do host, e um segredo em variável de ambiente frequentemente vaza sem querer para logs ou mensagens de erro.

## Decisão

Um **HashiCorp Vault em modo servidor** gera segredos e certificados sob demanda. Cada serviço tem um **Vault Agent** como sidecar, autenticado com identidade própria, que busca apenas os segredos daquele serviço e os grava como **arquivo** no filesystem do container (nunca como variável de ambiente). O Vault em si não é acessível a partir do host.

## Alternativas consideradas

- **Variáveis de ambiente no compose:** simples, mas sem controle de acesso por serviço, sem expiração e visível via `docker inspect`. Rejeitada.
- **Docker secrets nativos:** funcionam apenas em modo Swarm, não em `docker compose` standalone usado neste projeto. Rejeitada por incompatibilidade com a forma de execução escolhida.
- **Vault com Transit engine para cifrar em vez de gerar segredos:** resolve um problema diferente (cifra de dados em repouso), não a entrega de credenciais operacionais aos serviços. Fora do escopo desta decisão.
- **Arquivo `.env` versionado ou montado manualmente:** contraria diretamente o objetivo de não ter nenhum segredo no repositório, mesmo que "de desenvolvimento". Rejeitada.

## Consequências

- Cada serviço depende do próprio Vault Agent estar pronto antes de iniciar, o que adiciona uma etapa de inicialização e um ponto a mais na ordem de subida do ambiente.
- Segredos e certificados têm vida curta e são recriados a cada subida, coerente com a infraestrutura efêmera do projeto (ver ADR de infraestrutura sem IaC).
- O segredo inicial usado pelo Vault Agent para se autenticar no Vault (AppRole) ainda precisa existir na subida do compose; ele é reduzido (wrapping de uso único, TTL curto) mas não eliminado — risco aceito e registrado no modelo de ameaças.
