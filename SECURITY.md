# Política de segurança

## Reportando uma vulnerabilidade

Reporte vulnerabilidades de forma privada pelo GitHub, em **Security → Report a vulnerability** (private vulnerability reporting), no repositório [`Git-Lucas/SecureBaseline`](https://github.com/Git-Lucas/SecureBaseline). Não abra uma issue pública para uma vulnerabilidade ainda não corrigida.

## Escopo e expectativas

Este é um **projeto de estudo**: um baseline de referência para uma API de instituição financeira, sem ambiente implantado nem dados reais de usuários. Por isso:

- **Não há versões suportadas.** Só o código presente no branch principal (`master`) é considerado.
- **A correção é best effort.** O projeto tem um único mantenedor; não há SLA de resposta ou de correção. Os prazos de referência para vulnerabilidades em dependências de terceiros estão em [`docs/politica-de-atualizacao-de-dependencias.md`](docs/politica-de-atualizacao-de-dependencias.md).
- **Não há ambiente implantado.** Não existe URL pública, credencial real ou dado sensível a proteger fora do próprio repositório.
