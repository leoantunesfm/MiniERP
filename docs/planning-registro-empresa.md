# Planning: Fluxo de Registro de Empresa

Este documento permanece como planning, mas foi atualizado para refletir o estado real da implementacao na codebase.

## Resumo de status

- Implementado: infraestrutura local base, servico de e-mail, publicacao e consumo de mensagens, storage S3-compatible, cadastro inicial simplificado, confirmacao de e-mail, integracao ReceitaWS, completar cadastro, paginas Angular principais
- Parcial: validacao de ponta a ponta operacional, robustez de seguranca no completar cadastro, acabamento do front-end e confiabilidade dos testes Angular
- Pendente: refinamentos de UX, maior endurecimento de autorizacao e validacoes adicionais

## Fase 1: Infraestrutura base

- [x] `docker-compose.yml` com `rabbitmq`
- [x] `docker-compose.yml` com `mailpit`
- [x] `docker-compose.yml` com `minio`
- [ ] evidencias documentadas de validacao operacional continua de todos os paineis locais

## Fase 2: Servicos compartilhados

- [x] interface `IEmailService`
- [x] implementacao SMTP em `EmailService`
- [x] publicador de mensagens para RabbitMQ
- [x] `EmailWorker` como `BackgroundService`
- [x] interface `IStorageService`
- [x] implementacao `S3StorageService` usando `AWSSDK.S3`

## Fase 3: Refatoracao do cadastro inicial

- [x] `RegisterTenantRequestDto` reduzido a `Cnpj`, `Nome`, `Email`, `Senha`
- [x] `RegisterTenantUseCase` sem coleta inicial de razao social e endereco
- [x] geracao de token de confirmacao de e-mail
- [x] publicacao de mensagem para envio de e-mail
- [x] ajuste do endpoint `POST /api/Tenants/register`
- [x] criacao do endpoint `GET /api/Tenants/confirm-email`

## Fase 4: Integracao ReceitaWS

- [x] interface `IReceitaWsClient`
- [x] implementacao com `HttpClient`
- [x] DTO de mapeamento dos dados usados pela tela

## Fase 5: Endpoint de completar cadastro

- [x] `CompleteRegistrationRequestDto`
- [x] `GetCompanyDataByCnpjUseCase`
- [x] `CompleteRegistrationUseCase`
- [x] endpoint `GET /api/Tenants/cnpj-data/{cnpj}`
- [x] endpoint `POST /api/Tenants/complete-registration`
- [x] suporte a upload de arquivos e persistencia de documentos
- [ ] endurecer autorizacao para nao depender de `EmpresaId` vindo do form

## Fase 6: Front-end Angular

- [x] pagina inicial de registro com campos reduzidos
- [x] feedback apos envio do cadastro
- [x] pagina de confirmacao de e-mail
- [x] pagina de completar cadastro
- [x] redirecionamento por status do tenant
- [x] bloqueio ao dashboard ate conclusao do cadastro
- [ ] suite de testes Angular passando
- [ ] refinamentos de UX e tratamento de erros

## Divergencias em relacao ao planejamento original

- a fila implementada foi `email_queue`
- a consulta de CNPJ foi exposta como `GET /api/Tenants/cnpj-data/{cnpj}`
- a validacao automatica de acessibilidade dos paineis Docker nao esta registrada no repositorio

## Proximos passos sugeridos

- corrigir a suite de testes Angular
- vincular completar cadastro ao tenant do token autenticado
- adicionar testes integrados para onboarding completo
