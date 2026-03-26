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
  
## Stack Tecnológica
- **Back-end:** .NET 10 (Web API)
- **ORM:** Entity Framework Core (Code-First) com Migrations
- **Banco de Dados:** PostgreSQL
- **Front-end:** Angular *(Em desenvolvimento)*

### Qualidade e Testes (QA)
- **Back-end Testing:** xUnit (Framework), NSubstitute (Mocking) e FluentAssertions (Asserts).
- **Front-end Testing:** Jest.

### Infraestrutura & DevOps
- **Containers:** Docker e Docker Compose
- **CI/CD:** GitHub Actions *(Planejado)*
- **Cloud Provider:** AWS *(Planejado)*
- **Ferramentas:** Visual Studio Code, CLI nativa

## Padrões e Arquitetura
- Código escrito mesclando inglês para *Design Patterns* e estrutura core, e português para o domínio de negócio, facilitando a manutenção local.
- *API RESTful*.
- Arquitetura baseada em *Clean Architecture* ou *N-Tier* (a definir conforme evolução).