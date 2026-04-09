# MiniERP - Visao Geral do Projeto

## Descricao

MiniERP e um sistema de gestao comercial em modelo SaaS multitenant. O objetivo atual do projeto e validar a base tecnica de autenticacao, onboarding de empresas e isolamento por tenant antes da expansao para modulos de negocio mais amplos.

O repositorio foi construido como estudo de arquitetura em camadas, DDD tatico e integracao entre API, front-end SPA e servicos de infraestrutura auxiliares.

## O que ja existe hoje

- cadastro inicial de tenant com CNPJ, nome, e-mail e senha
- criacao automatica de usuario administrador com perfil `Admin`
- envio assincrono de e-mail de confirmacao via RabbitMQ + worker + SMTP
- confirmacao de e-mail com transicao de status do tenant
- login com JWT
- consulta de dados empresariais via ReceitaWS
- conclusao de cadastro com upload de documentos em storage S3-compatible
- telas Angular para onboarding, login, confirmacao, completar cadastro e dashboard
- testes unitarios de use cases no back-end
- compose local e pipeline de deploy versionados

## O que ainda e roadmap

- cadastro de produtos
- cadastro de clientes
- vendas e frente de caixa
- controle de estoque
- financeiro
- dashboard com indicadores reais
- ampliacao de cobertura de testes do front-end e da API

## Stack tecnologica atual

- Back-end: .NET 10 + ASP.NET Core
- Persistencia: EF Core 10 + PostgreSQL
- Front-end: Angular 21
- Mensageria: RabbitMQ
- E-mail local: Mailpit
- Storage: MinIO local / S3-compatible
- Integracao externa: ReceitaWS
- Containers: Docker Compose
- CI/CD: GitHub Actions com build/push/deploy para AWS

## Padroes e direcoes tecnicas

- separacao por camadas: `Api`, `Application`, `Domain`, `Infrastructure`, `UI`
- contratos do dominio centralizados em interfaces do `Domain`
- regras de negocio principais concentradas em use cases
- autenticacao via JWT Bearer
- front-end com rotas protegidas por guard e propagacao automatica do token por interceptor
