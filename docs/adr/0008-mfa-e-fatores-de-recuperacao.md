# 0008. MFA obrigatório com WebAuthn, TOTP e códigos de recuperação

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

Senha isolada não resiste a credential stuffing, password spraying nem phishing — os ataques dominantes contra clientes de instituição financeira. Um segundo fator opcional ou condicional ("só para quem já cadastrou") continua permitindo login só com senha para a maioria dos usuários, o que anula o objetivo do MFA. É preciso decidir também como o usuário recupera acesso quando perde o fator, sem que a recuperação vire um atalho que dispensa o MFA.

## Decisão

MFA é **obrigatório para todo usuário**, sem caminho de login que dispense o segundo fator. **WebAuthn** é o fator principal (resistente a phishing), **TOTP** é a alternativa para quem não tem autenticador compatível, e **códigos de recuperação de uso único** cobrem a perda do dispositivo. A recuperação de senha e a recuperação de fator seguem fluxos que não completam a autenticação sem, no mínimo, um fator adicional válido — nunca apenas um link de e-mail.

## Alternativas consideradas

- **MFA condicional ou opcional:** é a configuração mais comum em tutoriais e a mais fácil de configurar no IdP, mas deixa a maioria dos usuários sem proteção real. Rejeitada por não atingir o objetivo de exigir MFA de fato.
- **Somente TOTP, sem WebAuthn:** mais simples de implementar e testar, mas TOTP continua vulnerável a phishing em tempo real (o código pode ser roubado e reutilizado dentro da janela de validade). Rejeitada como único fator; mantida como alternativa.
- **Recuperação de conta só por e-mail (sem exigir outro fator):** é o padrão mais comum e o mais explorado — um atacante que compromete o e-mail do usuário contorna o MFA inteiro. Rejeitada.
- **Passkeys sem senha:** removeria a senha do fluxo inteiramente, mas exigiria redesenhar cadastro e recuperação em torno de um único fator de posse, sem o piso de um segundo fator independente. Fora de escopo deste projeto.

## Consequências

- Todo fluxo de cadastro precisa forçar o registro de pelo menos um fator (WebAuthn ou TOTP) antes de considerar a conta utilizável, e gerar códigos de recuperação nesse momento.
- A ausência de recuperação só por e-mail torna o fluxo de "esqueci meu segundo fator" mais longo para o usuário — aceito como o custo de não abrir um atalho que dispensa o MFA.
- OTP por SMS ou e-mail como fator (não como canal de recuperação) fica fora de escopo: são os métodos de MFA mais fracos contra SIM swap e comprometimento de e-mail.
