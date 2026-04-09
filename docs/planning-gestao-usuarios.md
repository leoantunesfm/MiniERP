# Planning: Gestão de Usuários e Autorização (AuthZ)

## Contexto
Após o Onboarding, o sistema possui um Tenant criado e um usuário `Admin` raiz. O objetivo desta feature é permitir que este administrador convide/cadastre membros para sua equipe, atribuindo a eles perfis de acesso (Roles) que liberarão ou bloquearão módulos inteiros do sistema (Acesso por Recursos).

## 1. Mapeamento de Perfis e Recursos (MVP)
Para simplificar a primeira iteração, trabalharemos com 3 perfis base. O acesso a um recurso garante permissão total (CRUD) dentro daquele domínio.

| Perfil | Descrição | Recursos Liberados |
| :--- | :--- | :--- |
| **Admin** | Acesso irrestrito a todo o Tenant. | `GestaoUsuarios`, `Produtos`, `Clientes`, `Vendas`, `Estoque`, `Financeiro` |
| **Gerente** | Gestão do negócio físico, sem mexer em configurações do sistema ou equipe. | `Produtos`, `Clientes`, `Vendas`, `Estoque`, `Financeiro` |
| **Operador** | Foco na operação diária (frente de caixa/atendimento). | `Clientes`, `Vendas` |

---

## 2. Etapas de Implementação - Back-end (API)

### 2.1. Ajustes no Domínio e Infraestrutura
- [ ] Atualizar o `JwtTokenGenerator` para incluir no *Payload* do JWT os claims de Perfil (`ClaimTypes.Role`) e do Tenant (`TenantId`).
- [ ] Garantir que o repositório de Usuários retorne os dados com a tabela `Perfis` inclusa (Include no EF Core).

### 2.2. Application (Use Cases)
Criar os seguintes casos de uso com seus respectivos DTOs:
- [ ] `RegisterUserUseCase`: Valida se o e-mail já existe, gera senha temporária, vincula ao Tenant atual e atribui o Perfil.
- [ ] `ListUsersByTenantUseCase`: Retorna a lista de usuários da empresa para a tabela de gestão.
- [ ] `UpdateUserProfileUseCase`: Permite alterar o perfil de um usuário existente.
- [ ] `DeactivateUserUseCase`: Soft delete ou inativação do usuário (para não perder histórico de auditoria em um ERP).

### 2.3. Configuração de Autorização (Program.cs e Controllers)
- [ ] Configurar a injeção do `AddAuthorization` no `Program.cs`.
- [ ] Criar o `UsersController` aplicando a restrição `[Authorize(Roles = "Admin")]` (Apenas admins podem gerenciar a equipe).
- [ ] Implementar a validação de Isolamento de Tenant (garantir que um Admin não possa listar ou deletar um usuário de outra empresa).

### 2.4. Testes Unitários da API (xUnit + Moq)
- [ ] Testar falha ao tentar cadastrar e-mail duplicado.
- [ ] Testar sucesso na criação e vinculação do `TenantId` do usuário logado.
- [ ] Testar retorno correto do JWT com as *Roles* embutidas na autenticação.

---

## 3. Etapas de Implementação - Front-end (Angular)

### 3.1. Infraestrutura de AuthZ no Angular
- [ ] Criar serviço genérico `AuthorizationService` para decodificar o JWT e extrair o `role` do usuário.
- [ ] Criar um **Guard** de Rota (`role.guard.ts`) para impedir acessos diretos via URL a módulos não permitidos.
- [ ] Criar uma **Diretiva Estrutural Customizada** (ex: `*appHasRole="['Admin']"`) para ocultar ou exibir botões e itens de menu HTML de forma declarativa.

### 3.2. Telas (Pages e Components)
- [ ] **Menu Lateral (Sidebar):** Atualizar o layout do Dashboard para renderizar os itens de menu dinamicamente baseados no perfil do usuário.
- [ ] **Tabela de Usuários (`/users`):** Tela listando a equipe, perfil e status.
- [ ] **Formulário de Cadastro (`/users/new`):** Formulário com Nome, E-mail e um `select` de Perfis.

### 3.3. Testes Unitários do Front-end (Vitest)
- [ ] Testar o `RoleGuard` (deve retornar `false` e redirecionar se o usuário não tiver o perfil exigido).
- [ ] Testar o componente de Tabela de Usuários (garantir que carrega os dados do `UserService`).
- [ ] Testar a injeção de estado do `AuthorizationService`.

---

## 4. Regras de Negócio e Segurança Adicionais
- Um usuário logado só pode visualizar/alterar usuários que possuam o mesmo `TenantId` extraído de seu próprio JWT.
- O e-mail de um usuário deve ser único globalmente na base para permitir login centralizado.
- Um usuário não pode desativar a si mesmo ou remover seu próprio acesso de Admin.