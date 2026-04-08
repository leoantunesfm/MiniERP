# MiniERP

MiniERP e um ERP SaaS multitenant em estudo, com foco em onboarding de empresas, autenticacao JWT e isolamento de dados por tenant. O projeto combina back-end em .NET, front-end Angular e servicos auxiliares em containers para reproduzir um ambiente completo de desenvolvimento.

## Estado atual do projeto

Hoje o repositorio implementa o fluxo base de entrada de um tenant:

- cadastro inicial da empresa e do usuario administrador
- publicacao de mensagem em fila para envio de e-mail de confirmacao
- confirmacao de e-mail por link no front-end
- login com JWT e claims de perfis
- consulta de dados da empresa por CNPJ via ReceitaWS
- conclusao de cadastro com upload de documentos em storage S3-compatible
- bloqueio de acesso por status do tenant no front-end
- dashboard inicial protegido por guard

Ainda nao estao implementados modulos de negocio como produtos, clientes, vendas, estoque e financeiro. O dashboard atual e apenas uma tela inicial autenticada.

## Arquitetura

O codigo esta organizado em camadas:

- `src/MiniERP.Api`: ASP.NET Core Web API, controllers, bootstrap de DI, autenticacao, CORS e worker de e-mail
- `src/MiniERP.Application`: DTOs e casos de uso da aplicacao
- `src/MiniERP.Domain`: entidades, enums, value objects, excecoes e contratos centrais
- `src/MiniERP.Infrastructure`: EF Core, repositorios, JWT, hashing, RabbitMQ, SMTP, S3/MinIO e cliente ReceitaWS
- `src/MiniERP.UI`: SPA Angular 21 com guards, interceptor JWT e paginas do fluxo de onboarding
- `src/MiniERP.Application.Tests`: testes unitarios dos use cases do back-end

O diagrama atualizado da arquitetura atual esta em [docs/architecture.md](docs/architecture.md).

## Stack validada

- Back-end: .NET 10, ASP.NET Core Web API
- Persistencia: Entity Framework Core 10 com PostgreSQL
- Seguranca: JWT Bearer + BCrypt
- Front-end: Angular 21, Reactive Forms, guards e interceptor HTTP
- Mensageria: RabbitMQ
- E-mail local: Mailpit via SMTP
- Storage de arquivos: MinIO local e S3-compatible via `AWSSDK.S3`
- Integracao externa: ReceitaWS
- Containers: Docker e Docker Compose
- CI/CD: GitHub Actions + ECR/EC2 em fluxo de deploy ja versionado

## Estrutura de execucao local

O `docker-compose.yml` local sobe:

- `db`: PostgreSQL
- `pgadmin`: interface administrativa do banco
- `api`: back-end .NET
- `ui`: front-end Angular servido por Nginx
- `rabbitmq`: broker e painel de mensageria
- `mailpit`: SMTP falso e inbox web para desenvolvimento
- `minio`: storage S3-compatible e console web

Portas expostas no ambiente local:

- API: `http://localhost:5209`
- UI: `http://localhost:4200`
- PgAdmin: `http://localhost:5050`
- RabbitMQ Management: `http://localhost:15672`
- Mailpit: `http://localhost:8025`
- MinIO API: `http://localhost:9000`
- MinIO Console: `http://localhost:9001`

## Endpoints documentados pela implementacao atual

- `POST /api/Auth/login`
- `POST /api/Tenants/register`
- `GET /api/Tenants/{id}`
- `GET /api/Tenants/confirm-email`
- `GET /api/Tenants/cnpj-data/{cnpj}`
- `POST /api/Tenants/complete-registration`

## Rotas publicas e protegidas no front-end

- `/login`
- `/onboarding`
- `/confirm-email`
- `/complete-registration`
- `/dashboard`

## Como executar localmente

### Pre-requisitos

- .NET 10 SDK
- Node.js 20 ou superior com npm
- Docker Desktop com Docker Compose

### Subindo a stack com Docker

1. Inicie os servicos de infraestrutura e as aplicacoes:

```bash
docker compose up -d --build
```

2. Acesse a interface:

- front-end em `http://localhost:4200`
- API em `http://localhost:5209`

3. Em ambiente de desenvolvimento da API, a documentacao interativa fica disponivel em:

```text
http://localhost:5209/scalar/v1
```

### Rodando manualmente sem subir API/UI pelo Compose

1. Suba apenas a infraestrutura necessaria com Docker, se preferir:

```bash
docker compose up -d db rabbitmq mailpit minio pgadmin
```

2. Aplique as migrations:

```bash
dotnet ef database update --project src/MiniERP.Infrastructure --startup-project src/MiniERP.Api
```

3. Execute a API:

```bash
dotnet run --project src/MiniERP.Api
```

4. Execute o front-end:

```bash
cd src/MiniERP.UI
npm install
npm start
```

## Configuracoes importantes

Os valores padrao locais estao em `src/MiniERP.Api/appsettings.json`:

- conexao com PostgreSQL local
- `FrontendUrl` usada no link de confirmacao de e-mail
- `JwtSettings`
- `RabbitMqSettings`
- `EmailSettings`
- `StorageSettings`

Em producao, o projeto usa `docker-compose.prod.yml` com:

- `api`
- `ui`
- `rabbitmq`

Banco, e-mail e storage sao esperados como servicos externos gerenciados. O workflow de deploy atual esta em `.github/workflows/deploy.yml`.

## Fluxo funcional implementado

### 1. Onboarding inicial

O usuario informa CNPJ, nome, e-mail e senha. A API cria a empresa com status `AguardandoConfirmacaoEmail`, cria o usuario administrador, associa o perfil `Admin` e publica uma mensagem na fila `email_queue`.

### 2. Confirmacao de e-mail

Um worker hospedado na API consome a fila, envia o e-mail via SMTP e o front-end consome o link de confirmacao. A confirmacao altera o status do tenant para `AguardandoDadosCompletos`.

### 3. Login

O login valida credenciais, verifica o status da empresa e retorna JWT com informacoes do usuario e perfis. O front-end salva token e metadados no `localStorage`.

### 4. Completar cadastro

O front busca o CNPJ do tenant autenticado, consulta a ReceitaWS para pre-preenchimento e envia os dados finais com arquivos via `multipart/form-data`. A API faz upload dos documentos, registra os metadados e ativa o tenant.

## Qualidade e validacao

Alertas observados na validacao:

- dependencia transitiva antiga de `Newtonsoft.Json` sinalizada no build/teste do back-end
- warning `xUnit1012` em teste que usa `null` como parametro `string`

## Limitacoes atuais

- o endpoint `POST /api/Tenants/complete-registration` confia no `EmpresaId` enviado pelo formulario, nao no claim do usuario autenticado
- o endpoint `GET /api/Tenants/cnpj-data/{cnpj}` esta publico na implementacao atual
- a producao esta parcialmente descrita em compose e workflow, mas a documentacao nao deve ser lida como garantia de ambiente provisionado
- alguns arquivos de documentacao antigos estavam com encoding corrompido e foram atualizados nesta revisao

## Proximos passos

- endurecer autorizacao nos endpoints de tenant usando claims do token
- expandir cobertura de testes para API e UI
- evoluir os modulos de negocio ainda nao implementados
