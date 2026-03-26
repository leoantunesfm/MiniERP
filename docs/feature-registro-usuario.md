# Feature: Registro de Novo Usuário (Onboarding Multitenant)

## Descrição
Fluxo inicial onde um novo cliente acessa a plataforma, cria a sua conta de acesso e, simultaneamente, registra a sua empresa (*Tenant*). Este será o usuário administrador daquela instância, recebendo o perfil de 'Admin' por padrão. Posteriormente, dentro da plataforma, será possível atribuir múltiplos perfis de acesso a um mesmo usuário (ex: Vendedor e Financeiro).

## 1. Banco de Dados (Tabelas Principais)
Usaremos o Entity Framework Core com a abordagem *Code-First*. Para suportar múltiplos perfis por usuário, utilizaremos um relacionamento N:N.

* **Table `Empresa` (Tenant)**
    * `Id` (UUID/Guid) - *Primary Key*
    * `RazaoSocial` (string)
    * `NomeFantasia` (string)
    * `CNPJ` (string)
    * `DataCadastro` (DateTime)

* **Table `Usuario` (User)**
    * `Id` (UUID/Guid) - *Primary Key*
    * `EmpresaId` (UUID/Guid) - *Foreign Key* vinculando ao Tenant
    * `Nome` (string)
    * `Email` (string) - Único
    * `SenhaHash` (string)
    * `Ativo` (bool)

* **Table `Perfil` (Role)**
    * `Id` (UUID/Guid) - *Primary Key*
    * `Nome` (string) - Ex: Admin, Vendedor, Financeiro, Estoque
    * `Descricao` (string)

* **Table `UsuarioPerfil` (UserRole)** *(Tabela de Junção)*
    * `UsuarioId` (UUID/Guid) - *Foreign Key*
    * `PerfilId` (UUID/Guid) - *Foreign Key*

## 2. API Back-end (Estrutura DDD)

### Camadas (*Layers*)
* **Domain:** Entidades (`Empresa`, `Usuario`, `Perfil`), *Value Objects* (ex: `Cnpj`, `Email`), e *Interfaces* de *Repository* (`IEmpresaRepository`, `IUsuarioRepository`, `IPerfilRepository`).
* **Application:** *Use Cases* (ex: `RegisterTenantUseCase`), *DTOs* (Data Transfer Objects) para entrada e saída de dados.
* **Infrastructure:** Implementação dos repositórios usando o `AppDbContext` do EF Core, configuração do relacionamento N:N via *Fluent API* (`HasMany().WithMany()`) e migrações.
* **Presentation:** `TenantsController` expondo o endpoint de registro.

### Endpoints
* `POST /api/tenants/register`
    * **Payload (Request):** Dados da empresa (Razão Social, CNPJ) e dados do usuário (Nome, Email, Senha).
    * **Response:** `201 Created` com os dados básicos gerados ou `400 Bad Request`.
    * *Regra de Negócio Central:* O *Use Case* de registro deve automaticamente criar o *Tenant*, o *User* e vincular o *Role* de 'Admin' a este primeiro usuário criado.

## 3. Front-end (Angular)

### Telas (*Components*)
* **Register Component:** Um formulário para o *Onboarding*.
    * *Step 1:* Dados da Empresa.
    * *Step 2:* Dados do Usuário Administrador.
* **Services:** `TenantService` para fazer as chamadas HTTP (`HttpClient`) para a API.

### Validações Visuais
* Bloquear o botão de "Criar Conta" caso os campos obrigatórios não estejam preenchidos.
* Exibir mensagens de erro amigáveis retornadas pela API (ex: CNPJ inválido ou E-mail já em uso).