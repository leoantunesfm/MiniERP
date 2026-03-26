# MiniERP - Visão Geral do Projeto

## Descrição
Sistema simples de gestão comercial no modelo *SaaS* (*Software as a Service*). A aplicação será uma plataforma web *multitenant*, onde cada usuário pode criar uma conta, cadastrar sua empresa e gerenciar sua própria instância de dados de forma isolada (*Tenant Isolation*).

## Features Principais
- **Cadastro de Produtos:** Gestão do catálogo de itens.
- **Cadastro de Clientes:** Gestão da base de consumidores.
- **Registro de Vendas:** Frente de caixa / processamento de pedidos.
- **Controle de Estoque:** Rastreamento de entradas e saídas.
- **Financeiro:** Contas a receber (crediário) e Contas a pagar.
- **Gestão de Acessos:** Cadastro de usuários e vendedores com *Role-Based Access Control* (RBAC).
- **Dashboard:** Tela inicial com indicadores chave de performance (KPIs) e atalhos de navegação.

## Tech Stack
- **Back-end:** .NET 10 (Web API).
- **ORM:** Entity Framework Core (Code-First) com Migrations para versionamento do *schema* do banco de dados.
- **Banco de Dados:** PostgreSQL.
- **Front-end:** Angular (SPA - *Single Page Application*).
- **Infraestrutura & DevOps:** - *Containers* com Docker e Docker Compose.
  - *Continuous Integration* / *Continuous Deployment* (CI/CD) via GitHub Actions.
  - *Cloud Provider*: AWS.

## Padrões e Arquitetura
- Código escrito mesclando inglês para *Design Patterns* e estrutura core, e português para o domínio de negócio, facilitando a manutenção local.
- *API RESTful*.
- Arquitetura baseada em *Clean Architecture* ou *N-Tier* (a definir conforme evolução).