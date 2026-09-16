# 0004. Autenticação do client OAuth por `private_key_jwt`

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

O gateway precisa se autenticar perante o IdP ao trocar o código de autorização por tokens (o "client authentication" do fluxo OAuth2). O método mais comum é um *client secret* estático, compartilhado entre gateway e IdP e configurado em ambos os lados. Um segredo estático que nunca expira, se vazar (log, imagem de container, repositório), continua válido até alguém perceber e trocá-lo manualmente.

## Decisão

Usar `private_key_jwt`: o gateway se autentica assinando um JWT com uma chave privada própria; o IdP valida a assinatura usando a chave pública, publicada na URL de JWKS do próprio gateway. Nenhum segredo compartilhado (client secret) existe entre gateway e IdP.

## Alternativas consideradas

- **Client secret estático (`client_secret_basic`/`client_secret_post`):** é o padrão mais simples de configurar, mas é exatamente o tipo de segredo de longa duração que o projeto tenta eliminar em todo o resto da arquitetura (ver o ADR de entrega de segredos via Vault). Rejeitado por inconsistência com esse princípio.
- **mTLS entre gateway e IdP:** também elimina o segredo compartilhado, mas exige gestão de certificados de cliente adicional e suporte do IdP para autenticação por certificado no endpoint de token — mais infraestrutura do que o ganho justifica neste projeto.
- **DPoP:** resolve um problema diferente (prova de posse do token pelo cliente que o usa), não a autenticação do client em si; ortogonal a esta decisão e fora do escopo do projeto.

## Consequências

- O gateway precisa gerar e proteger um par de chaves e publicar a chave pública via um endpoint JWKS próprio — mais uma responsabilidade do gateway, mas sem segredo compartilhado para vazar ou rotacionar manualmente.
- Comprometer o gateway ainda permite personificá-lo perante o IdP (ele tem a chave privada), mas comprometer o IdP ou um log não expõe mais nenhum segredo reutilizável do lado do client.
