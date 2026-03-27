# MiniERP

## Descrição
O MiniERP é um sistema de gestão comercial simples no modelo SaaS (*Software as a Service*) Multitenant. Ele fornece um ambiente de dados isolado (*Tenant Isolation*) para que cada empresa gerencie suas próprias operações, usuários e rotinas diárias com total segurança.

Este projeto foi construído como objeto de estudo para aplicar práticas modernas de engenharia de software, como *Clean Architecture* e *Domain-Driven Design* (DDD). A base de código mescla padrões estruturais em inglês com o domínio de negócio em português para facilitar a manutenção no cenário nacional.

## Principais Funcionalidades
- **Onboarding de Tenants:** Registro inicial da empresa, criação do usuário administrador e vínculo automático de perfis de acesso.
- **Autenticação:** Login seguro utilizando tokens JWT com interceptor HTTP no Front-end.
- *(Em breve)* **Catálogo:** Cadastro e gestão de produtos.
- *(Em breve)* **Vendas e Estoque:** Frente de caixa, registro de vendas e controle de movimentação de estoque.
- *(Em breve)* **Financeiro:** Contas a pagar e a receber (crediário).

## Stack Tecnológica
- **Back-end:** .NET 10 (Web API)
- **ORM:** Entity Framework Core (Code-First)
- **Banco de Dados:** PostgreSQL
- **Front-end:** Angular (Standalone Components) com Tailwind CSS v4
- **Infraestrutura:** Docker & Docker Compose
- **Ferramentas:** Visual Studio Code, CLI nativa

## Como Executar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/) instalado.
- [Docker Desktop](https://www.docker.com/products/docker-desktop) rodando.
- [Node.js LTS](https://nodejs.org/) e Angular CLI (`npm install -g @angular/cli`).

### Configuração Local

1. **Clone o repositório:**
   ```bash
   git clone [https://github.com/SEU-USUARIO/MiniERP.git](https://github.com/SEU-USUARIO/MiniERP.git)
   cd MiniERP
   ```

2. **Inicie a infraestrutura de banco de dados:**
   ```bash
   docker compose up -d
   ```

3. **Inicie a API (.NET):**
   Abra um terminal e execute:
   ```bash
   cd src/MiniERP.Api
   dotnet run
   ```
   *A API ficará disponível e o Swagger/Scalar poderá ser acessado em `http://localhost:<porta>/scalar/v1`.*

4. **Inicie o Front-end (Angular):**
   Abra um novo terminal e execute:
   ```bash
   cd src/MiniERP.UI
   npm install
   ng serve -o
   ```
   *A aplicação abrirá no navegador em `http://localhost:4200`.*