# Planning: Deploy AWS e CI/CD

Este documento permanece como planejamento operacional, agora com status revisado a partir do que ja existe no repositorio.

## Resumo de status

- Implementado no codigo/repositorio: `docker-compose.prod.yml`, workflow GitHub Actions de teste/build/push/deploy, adaptacao do storage para MinIO local ou S3 por configuracao
- Parcial: adaptacao completa para multiplos ambientes, endurecimento operacional do deploy e evidencias de infraestrutura provisionada
- Pendente: documentacao operacional do ambiente AWS real, validacao ponta a ponta de todos os servicos externos e governanca de secrets

## Fase 1: Adaptacao do codigo para multiplos ambientes

- [x] compose de producao separado em `docker-compose.prod.yml`
- [x] compose de producao sem `db`, `pgadmin`, `mailpit` e `minio`
- [x] `S3StorageService` alternando entre `ServiceURL` customizada e regiao AWS
- [x] `EmailService` lendo configuracao por ambiente
- [ ] revisao completa das variaveis de ambiente da API para todos os casos de producao
- [ ] validacao de naming consistente entre compose e appsettings em todos os servicos

## Fase 2: Infraestrutura cloud base

- [ ] IAM programatico validado
- [ ] bucket S3 produtivo validado
- [ ] SES validado com dominio real
- [ ] RDS provisionado e confirmado no repositorio por documentacao operacional

## Fase 3: Servidor de aplicacao

- [ ] EC2 validada e documentada
- [ ] instalacao de Docker/Compose e Nginx documentada como processo operacional confirmado
- [ ] SSL/TLS automatizado documentado e validado

## Fase 4: Repositorio de imagens

- [x] pipeline preparado para publicar API em ECR
- [x] pipeline preparado para publicar UI em ECR
- [ ] confirmacao operacional do provisionamento dos repositorios no ambiente AWS real

## Fase 5: Pipeline CI/CD

- [x] workflow em `.github/workflows/deploy.yml`
- [x] execucao de testes .NET antes do build
- [x] build e push de imagens Docker
- [x] etapa de deploy remoto via SSH para EC2
- [ ] testes de smoke ou health-checks apos deploy
- [ ] estrategia de rollback documentada

## Fase 6: Testes e validacao em producao

- [ ] DNS apontando para ambiente validado
- [ ] onboarding completo validado em dominio real
- [ ] recebimento de e-mail via SES confirmado
- [ ] upload de documentos em S3 real confirmado

## Observacoes importantes

- o workflow atual instala `aws-cli` na maquina remota a cada deploy, o que pode ser simplificado depois
- o pipeline valida apenas os testes .NET; os testes Angular nao participam hoje do gating
- a existencia do compose e do workflow nao substitui a necessidade de runbooks e checklists operacionais
