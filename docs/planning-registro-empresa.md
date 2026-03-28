# Planning: Novo Fluxo de Registro de Empresa (Tenant)

Este documento detalha as etapas de implementação do novo fluxo de *onboarding* de clientes no MiniERP, contemplando processamento assíncrono, integração com APIs externas e armazenamento de arquivos.

## Arquitetura e Stack
* **Mensageria:** RabbitMQ (Docker)
* **Mock de E-mail:** Mailpit (Docker)
* **Object Storage:** MinIO S3 Compatible (Docker)
* **Enriquecimento de Dados:** ReceitaWS (API Pública HTTP)

---

## Fases de Implementação

### Fase 1: Infraestrutura Base (Docker)
- [ ] Atualizar o arquivo `docker-compose.yml` para incluir os serviços:
  - `rabbitmq` (com painel de gerencimento)
  - `mailpit` (servidor SMTP de testes + Web UI)
  - `minio` (armazenamento de arquivos + Web UI)
- [ ] Validar se os containers sobem corretamente e se as portas de UI estão acessíveis.

### Fase 2: Serviços Compartilhados (Back-end)
- [ ] **Email Service (Infra):**
  - Criar interface `IEmailService`.
  - Criar integração com servidor SMTP (apontando para o Mailpit no *appsettings.json*).
- [ ] **Message Bus (Application/Infra):**
  - Configurar publicador de mensagens para o RabbitMQ (ex: `EmailConfirmationMessage`).
  - Criar um `BackgroundService` (Worker) para consumir a fila do RabbitMQ e disparar o e-mail usando o `IEmailService`.
- [ ] **Storage Service (Infra):**
  - Criar interface `IStorageService` (métodos: `UploadAsync`, `GetFileUrlAsync`).
  - Implementar `MinioStorageService` utilizando o SDK do Amazon S3 (AWSSDK.S3) configurado para o MinIO local.

### Fase 3: Refatoração do Cadastro Inicial (Back-end)
- [ ] **Application (Use Cases & DTOs):**
  - Limpar `RegisterTenantRequestDto`: manter apenas `Cnpj`, `Nome` (Admin), `Email` e `Senha`.
  - Atualizar o `RegisterTenantUseCase`:
    - Remover lógica de `RazaoSocial`, `NomeFantasia` e endereço.
    - Gerar um token/hash único de confirmação de e-mail.
    - Publicar evento na fila do RabbitMQ solicitando envio de e-mail.
- [ ] **API (Endpoints):**
  - Ajustar `/api/tenants/register` para o novo DTO.
  - Criar endpoint estático `/api/tenants/confirm-email?token=xyz` que atualiza o `Status` da empresa e ativa o acesso.

### Fase 4: Integração ReceitaWS (Back-end)
- [ ] **Infrastructure (External Services):**
  - Criar `IReceitaWsClient`.
  - Implementar chamada HTTP via `HttpClientFactory` para `https://receitaws.com.br/v1/cnpj/{cnpj}`.
  - Criar DTOs internos para mapear o retorno do JSON da ReceitaWS (filtrando apenas: nome, fantasia, logradouro, numero, complemento, municipio, bairro, uf, cep, telefone).

### Fase 5: Endpoint de "Completar Cadastro" (Back-end)
- [ ] **Application:**
  - Criar `CompleteRegistrationRequestDto` (dados de endereço, razao social, telefone) recebendo arquivos (`IFormFile` no ASP.NET).
  - Criar `GetCompanyDataByCnpjUseCase` (que chama a API da ReceitaWS e retorna os dados para a tela).
  - Criar `CompleteRegistrationUseCase`:
    1. Recebe os dados validados pelo usuário.
    2. Faz o upload do(s) documento(s) usando o `IStorageService`.
    3. Registra os caminhos retornados pelo MinIO na entidade `DocumentoEmpresa`.
    4. Chama `empresa.CompletarCadastro(...)`.
    5. Atualiza `Status` do Tenant para `Ativo`.
- [ ] **API:**
  - Criar endpoint `GET /api/tenants/cnpj-data` (aciona o ReceitaWS).
  - Criar endpoint `POST /api/tenants/complete-registration` (recebe *multipart/form-data* devido ao upload de arquivos).

### Fase 6: Front-end (Angular)
- [ ] **Página Inicial de Registro:**
  - Remover campos extras do formulário, deixando apenas CNPJ, Nome, E-mail e Senha.
  - Exibir tela de sucesso informando: "Verifique sua caixa de entrada para confirmar o e-mail".
- [ ] **Página de Confirmação:**
  - Criar rota que recebe o token do e-mail e chama a API para validar.
- [ ] **Página "Completar Cadastro" (Onboarding Pós-Login):**
  - Se o usuário logar e o status for "Aguardando Cadastro", redirecionar para essa tela.
  - Fazer chamada automática para buscar dados na ReceitaWS usando o CNPJ do usuário logado e preencher o formulário automaticamente.
  - Permitir edição dos dados.
  - Adicionar componente de Upload de Arquivos (Drag & Drop ou botão de seleção).
  - Bloquear acesso ao Dashboard até que esta etapa seja concluída.