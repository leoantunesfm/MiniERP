# Feature: Autenticação e Autorização (Login)

## Descrição
Módulo de segurança responsável por autenticar os usuários registrados e emitir credenciais de acesso seguro (JWT - JSON Web Tokens) para comunicação com áreas restritas da API.

## 1. API Back-end
* **Endpoint:** `POST /api/Auth/login`
* **Use Case:** `LoginUseCase` recebe o E-mail e Senha, busca o usuário e valida o *Hash* criptográfico via BCrypt.
* **Geração do Token:** Caso as credenciais sejam válidas, a camada de *Infrastructure* (`JwtTokenGenerator`) emite um JWT assinado com a chave secreta da aplicação (`SymmetricSecurityKey`).
* **Claims:** O token carrega internamente dados vitais como `sub` (UserId), `email`, as *Roles* do usuário e a Claim customizada `tenantId`.

## 2. Front-end (Angular)
* **LoginComponent:** Tela com *Reactive Forms* para entrada de credenciais, com feedbacks visuais de carregamento e erro.
* **Armazenamento:** O token retornado e o nome do usuário são persistidos no `localStorage` do navegador.
* **Interceptor HTTP:** Implementado o `jwt.interceptor.ts`. Este componente atua como um middleware no Angular, interceptando todas as chamadas HTTP sainte e injetando o cabeçalho `Authorization: Bearer <token>` automaticamente.

## 3. Segurança Multitenant
A injeção do `tenantId` (Id da Empresa) diretamente no payload do JWT é a fundação da arquitetura isolada do MiniERP. Futuramente, as rotas da API irão ler essa *Claim* para aplicar filtros globais (Global Query Filters) no Entity Framework, impedindo o vazamento de dados entre empresas.