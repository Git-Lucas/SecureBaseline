# 0011. API sem persistência própria

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

Os três endpoints da API existem apenas como alvo para os controles de segurança do projeto (autenticação, autorização por titularidade, validação de entrada) — eles não têm regra de negócio real. Dar à API um banco de dados próprio adicionaria uma superfície inteira de decisões (schema, migrações, ORM, pool de conexão, SQL injection) que não contribui em nada ao objetivo do projeto, e desviaria esforço de revisão para uma camada que não é o que o projeto quer demonstrar.

## Decisão

A API guarda os dados de conta em uma **coleção em memória**, populada na inicialização com contas de pelo menos dois titulares distintos (necessário para provar que a checagem de titularidade — IDOR — funciona). Nenhum banco de dados sustenta a API.

## Alternativas consideradas

- **Banco relacional (PostgreSQL) dedicado à API:** é o padrão para uma API real, mas introduziria uma camada de persistência sem relação com o propósito do projeto (demonstrar controles de acesso e autenticação), com risco de a atenção do leitor se dispersar para preocupações de modelagem de dados. Rejeitada.
- **Banco embutido (SQLite) ou arquivo local:** reduziria a infraestrutura frente a um banco cliente-servidor, mas ainda introduz uma camada de persistência e um novo modo de falha (arquivo corrompido, concorrência de escrita) que a coleção em memória não tem. Rejeitada pelo mesmo motivo do banco relacional: não agrega ao objetivo do projeto.

## Consequências

- Qualquer estado de conta é perdido a cada reinício da API — coerente com a filosofia de ambiente efêmero do projeto como um todo (ver ADR de infraestrutura sem IaC), não uma limitação isolada da API.
- Ferramentas de infraestrutura do restante do sistema (o próprio Keycloak, por exemplo) continuam livres para usar banco de dados — a decisão de não persistir vale só para a API de demonstração, não para o projeto inteiro.
- Nenhum teste de migração, schema ou SQL injection é necessário para esta API; a validação de entrada continua obrigatória pelos mesmos motivos de qualquer API (dados malformados, limites de tamanho), independentemente de haver banco por trás.
