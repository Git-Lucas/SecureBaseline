# 0006. Infraestrutura efêmera sem IaC

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

O projeto não tem ambiente implantado: ele existe para ser executado localmente, por quem clona o repositório, via `docker compose`. Isso muda o problema que a gestão de infraestrutura precisa resolver — não há necessidade de provisionar, versionar o estado de e dar reconciliação contínua a uma infraestrutura de nuvem real.

## Decisão

O ambiente inteiro é definido em arquivos de **`docker compose`**, recriado do zero a cada subida, sem nenhuma ferramenta de Infrastructure as Code (Terraform/OpenTofu) e sem estado de infraestrutura persistente entre execuções.

## Alternativas consideradas

- **Terraform/OpenTofu com um provider local ou de nuvem:** ferramentas de IaC resolvem provisionamento e deriva de estado ao longo do tempo em ambientes persistentes e compartilhados por equipe — nenhuma dessas condições existe aqui. Rejeitada por adicionar uma camada de abstração e um novo formato de configuração sem resolver um problema que o projeto tem.
- **Kubernetes (kind/minikube) local:** adicionaria complexidade operacional (orquestração, manifests, ingress) desproporcional a um ambiente de estudo de execução única e local.
- **Scripts de provisionamento imperativos (bash/Ansible) sobre uma VM persistente:** contraria o objetivo de recriar o ambiente do zero a cada subida e de não manter infraestrutura viva entre execuções.

## Consequências

- Nenhum estado de infraestrutura sobrevive ao encerramento do ambiente: sessões, dados do Keycloak/PostgreSQL e segredos do Vault são todos recriados na próxima subida.
- A reprodutibilidade do ambiente depende inteiramente do `docker compose` e dos scripts de bootstrap versionados, não de um plano de execução de IaC.
- Cenários que dependem de infraestrutura persistente ou de múltiplas instâncias (alta disponibilidade, sessão distribuída) estão fora de escopo por construção, não por omissão.
