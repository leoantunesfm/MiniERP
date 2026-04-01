# Servicos de Infraestrutura

Este documento resume os servicos auxiliares ja usados pela implementacao atual e como eles aparecem no ambiente local e no desenho de producao versionado no repositorio.

## RabbitMQ

### Uso atual

RabbitMQ ja esta integrado ao fluxo de onboarding.

- `RegisterTenantUseCase` publica mensagens na fila `email_queue`
- `EmailWorker` consome a fila em background
- o broker sobe localmente pelo `docker-compose.yml`
- em producao, o `docker-compose.prod.yml` ainda mantem RabbitMQ como container da stack

### Papel na arquitetura

- desacoplar o disparo de e-mail do ciclo HTTP
- permitir reprocessamento e evolucao futura para outras mensagens assincronas

## Mailpit e SMTP

### Uso local atual

Mailpit e o servidor SMTP falso usado no desenvolvimento local.

- SMTP exposto na porta `1025`
- inbox web exposta na porta `8025`
- `EmailService` envia usando configuracoes de `EmailSettings`

### Producao

O desenho versionado de producao troca o SMTP local por credenciais SES/SMTP configuradas por variaveis de ambiente.

## MinIO e S3-compatible storage

### Uso atual

O servico de storage ja esta implementado por `S3StorageService` com `AWSSDK.S3`.

- localmente, a API aponta para o MinIO do `docker-compose.yml`
- arquivos enviados no completar cadastro sao armazenados no bucket configurado
- a entidade `DocumentoEmpresa` persiste nome do arquivo, caminho S3 e data de upload

### Producao

Quando `StorageSettings:Endpoint` nao e informado, o codigo cai no modo de regiao AWS e pode usar bucket real em S3.

## PostgreSQL e PgAdmin

### Uso atual

- PostgreSQL e o banco principal da aplicacao
- EF Core usa migrations versionadas em `MiniERP.Infrastructure`
- PgAdmin esta disponivel no ambiente local como apoio operacional

## ReceitaWS

### Uso atual

A integracao com ReceitaWS ja existe em `ReceitaWsClient`.

- chamada HTTP para `https://receitaws.com.br/v1/cnpj/{cnpj}`
- usada para pre-preencher o formulario de conclusao de cadastro
- falhas na consulta sao registradas em log e retornam objeto vazio no fluxo atual

## GitHub Actions e deploy

### Uso atual

O workflow `.github/workflows/deploy.yml` ja executa:

- testes .NET
- build e push de imagens para o ECR
- deploy remoto em EC2 via SSH

### Observacao

A existencia desse workflow nao significa que todo o ambiente AWS ja esteja provisionado e validado ponta a ponta; ele representa o fluxo de entrega versionado no repositorio.
