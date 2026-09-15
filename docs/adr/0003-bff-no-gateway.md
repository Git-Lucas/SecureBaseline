# 0003. Backend-for-Frontend no gateway

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

A página web precisa autenticar o usuário e chamar a API em nome dele. Se o navegador guardar os tokens OAuth/OIDC (em `localStorage`, `sessionStorage` ou até em cookie legível por JavaScript), qualquer XSS na página ou em uma extensão do navegador rouba o token e assume a sessão do usuário — sem precisar de mais nada. Aplicações financeiras são alvo direto desse tipo de ataque.

## Decisão

O **gateway assume o papel de Backend-for-Frontend (BFF)**: ele conduz o fluxo OIDC com o IdP, recebe e guarda os tokens inteiramente no servidor, e expõe ao navegador apenas um cookie de sessão `HttpOnly` e `Secure`. O navegador nunca vê um token de acesso, de ID ou de atualização. Toda chamada à API passa pelo gateway, que anexa o token guardado no servidor.

## Alternativas consideradas

- **SPA com tokens no navegador (Authorization Code + PKCE "puro", sem BFF):** é o padrão mais comum em tutoriais, mas expõe o token de acesso a qualquer XSS na página. Rejeitado para um baseline que se propõe a seguir o nível 2 do ASVS, que exige que só o backend tenha acesso aos tokens.
- **Cookie de sessão assinado contendo o próprio token (sem estado no servidor):** reduz o problema de XSS lendo o cookie via JavaScript, mas ainda expõe o token a um ataque de repetição se o cookie vazar, e cresce com o tamanho do token. Rejeitado em favor de sessão com estado, mais fácil de revogar.
- **Token guardado em `httpOnly` cookie separado, sem servidor intermediário:** exigiria que o próprio IdP emitisse cookies utilizáveis diretamente pela API, o que não é como o fluxo OIDC padrão funciona e complicaria a validação de audience na API.

## Consequências

- O gateway se torna um componente com estado (guarda sessão e tokens), o que teria custo em um cenário com múltiplas instâncias — aceito porque o projeto roda com uma instância única (ver riscos aceitos no modelo de ameaças).
- A API ainda precisa validar o token recebido do gateway de forma independente (issuer, audience, algoritmo, nível de autenticação), porque o gateway não é uma fronteira de confiança total para a API.
- Toda funcionalidade da página passa a depender do gateway estar no caminho de toda chamada; não há acesso direto do navegador à API.
