# 0013. Governança de repositório para mantenedor único

- **Status:** Aceito
- **Data:** 2026-09-15

## Contexto

O repositório tem um único mantenedor: não existe um segundo revisor humano para aprovar pull requests. Ao mesmo tempo, o projeto quer demonstrar que nenhuma mudança chega ao branch principal sem passar por um portão verificável — sem esse portão, qualquer controle implementado por uma change posterior poderia ser removido ou alterado silenciosamente por uma mudança futura, sem deixar rastro de revisão.

## Decisão

- Um único **ruleset do branch principal**, sem atores com bypass (nem o próprio mantenedor escapa dele): exige pull request, bloqueia push direto, force push e deleção, exige commits assinados e checks obrigatórios aprovados.
- O ruleset é **versionado como JSON no repositório** e aplicado por um **script local**, executado manualmente pelo mantenedor com um token de vida curta — nunca por um workflow de CI, o que evitaria colocar um token de administração dentro do próprio pipeline que o ruleset protege.
- A **assinatura de commit do mantenedor** é a atestação de que o conteúdo (inclusive gerado por IA) foi revisado por um humano antes do commit — na ausência de um segundo revisor, é a única evidência de revisão que o repositório consegue oferecer.
- Apenas **squash merge** é permitido: o commit que chega ao branch principal é criado e assinado pelo próprio GitHub através do fluxo autenticado da web, preservando a exigência de assinatura mesmo sem revisor externo.

## Alternativas consideradas

- **Exigir aprovação de um segundo revisor:** é o controle mais forte contra um mantenedor comprometido ou descuidado, mas não existe um segundo humano neste projeto — exigir aprovação sem ter quem aprove travaria todo PR permanentemente. Rejeitada; registrada como risco aceito, não como lacuna ignorada.
- **Bypass actor para o mantenedor no ruleset:** simplificaria hotfixes urgentes, mas anularia a garantia central do controle (nenhuma mudança sem PR e checks) justamente para quem mais precisa dela — se a conta do mantenedor for comprometida, um bypass é a primeira coisa que um atacante usaria. Rejeitada.
- **Aplicar o ruleset a partir de um workflow do GitHub Actions:** exigiria um token com permissão de administração do repositório disponível para o CI — um anti-padrão de segurança (A08): o pipeline que o ruleset deveria proteger passaria a ter o poder de alterar a própria proteção. Rejeitada em favor do script local com token efêmero, usado sob demanda e nunca persistido.
- **Merge commit ou rebase merge em vez de squash:** um merge commit preservaria os commits assinados do mantenedor no branch principal (o rebase, ao reescrever commits, perde a assinatura original), mas produz um histórico mais ruidoso, com mais commits para reverter em conjunto se necessário. Squash foi escolhido pelo histórico linear; o commit assinado do mantenedor permanece disponível no PR (`refs/pull/N/head`), servindo de evidência de revisão mesmo não estando no branch principal.

## Consequências

- O estado real do ruleset no GitHub pode divergir do JSON versionado entre o momento em que o arquivo muda e o momento em que o mantenedor reaplica o script — mitigado por reaplicação disciplinada (documentada no procedimento de configuração) e por um log de verificação, mas não eliminado por uma checagem automatizada de deriva, que está fora de escopo.
- A pontuação de *Code Review* de ferramentas como o OpenSSF Scorecard permanece baixa, por não haver segregação real entre autor e revisor — aceito como uma característica inerente a um projeto de mantenedor único, não como um problema a esconder.
- Renomear um job de verificação (o nome vira o *context* do check obrigatório) exige uma ordem cuidadosa — adicionar o novo job, reaplicar o JSON com os dois contexts, mesclar, só então remover o antigo — documentada no procedimento de configuração, para não deixar um check obrigatório eternamente pendente.
- O token de administração de vida curta usado pelo script fica exposto na estação de trabalho do mantenedor durante o uso; mitigado por ser de repositório único, expirar em um dia, nunca ser persistido em arquivo e ser revogado após o uso.
