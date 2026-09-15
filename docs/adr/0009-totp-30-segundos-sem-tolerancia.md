# 0009. TOTP de 30 segundos sem janela de tolerância

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

TOTP (RFC 6238) gera um código válido por uma janela de tempo fixa. Implementações comuns aceitam também o código do intervalo anterior e/ou seguinte (tolerância de relógio) para absorver pequenas dessincronizações entre o relógio do autenticador e o do servidor. Cada intervalo de tolerância adicional amplia a janela em que um código roubado (phishing em tempo real, observação de tela) continua utilizável, e permite reutilizar o mesmo código mais de uma vez se o servidor não registrar o último código consumido.

## Decisão

Janela de **30 segundos, sem tolerância** para intervalos adicionais: apenas o código do intervalo corrente é aceito, e cada código é consumível uma única vez.

## Alternativas consideradas

- **Tolerância de ±1 intervalo (padrão comum em muitas bibliotecas):** reduz falhas de login por dessincronização de relógio do usuário, mas triplica a janela de validade efetiva de um código potencialmente roubado. Rejeitada em favor da janela mínima.
- **Intervalo maior que 30 segundos:** reduziria ainda mais a fricção de dessincronização, mas amplia proporcionalmente o tempo de uso de um código interceptado. 30 segundos é o valor de referência da RFC 6238 e o suportado por todo autenticador TOTP comum; não há motivo para ampliá-lo.

## Consequências

- Usuários com relógio de dispositivo significativamente dessincronizado podem ter falhas de login ocasionais; aceito porque a maioria dos autenticadores TOTP sincroniza o relógio automaticamente e o WebAuthn é o fator recomendado como principal (ver ADR de MFA).
- Um código TOTP interceptado tem, na pior hipótese, poucos segundos de validade restante e nunca pode ser reutilizado — reduz significativamente, sem eliminar, o risco de phishing em tempo real contra este fator.
