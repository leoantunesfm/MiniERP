# Feature: Registro de Novo Usuario e Onboarding Multitenant

## Descricao

O fluxo de onboarding do MiniERP ja foi implementado em duas etapas:

1. registro inicial com criacao de tenant e usuario administrador
2. conclusao de cadastro apos confirmacao de e-mail

O fluxo e guiado pelo status do tenant.

## Status do tenant

- `AguardandoConfirmacaoEmail`: empresa criada, aguardando clique no link enviado
- `AguardandoDadosCompletos`: e-mail confirmado, mas cadastro empresarial ainda incompleto
- `Ativo`: empresa liberada para uso normal

## Etapa 1: Registro inicial

### Front-end

A rota `/onboarding` exibe formulario com:

- CNPJ
- nome do administrador
- e-mail
- senha

Ao concluir, a tela mostra mensagem orientando o usuario a verificar a caixa de entrada.

### Back-end

Endpoint atual:

- `POST /api/Tenants/register`

Comportamento implementado:

- valida duplicidade de CNPJ e e-mail
- busca o perfil `Admin`
- cria `Empresa` com token de confirmacao de e-mail
- cria `Usuario` administrador
- cria vinculacao `UsuarioPerfil`
- salva dados no banco
- publica mensagem na fila `email_queue`

## Etapa 2: Confirmacao de e-mail

### Front-end

A rota `/confirm-email` recebe `token` por query string e chama a API.

### Back-end

Endpoint atual:

- `GET /api/Tenants/confirm-email?token=...`

Comportamento implementado:

- busca empresa pelo token
- invalida o token
- atualiza status para `AguardandoDadosCompletos`

## Etapa 3: Completar cadastro

### Front-end

A rota `/complete-registration`:

- busca o tenant autenticado por `empresaId`
- consulta o CNPJ da empresa
- chama a ReceitaWS por meio da API para pre-preencher o formulario
- envia os dados finais com arquivos por `multipart/form-data`

### Back-end

Endpoints atuais:

- `GET /api/Tenants/{id}`
- `GET /api/Tenants/cnpj-data/{cnpj}`
- `POST /api/Tenants/complete-registration`

Comportamento implementado:

- consulta dados empresariais pela ReceitaWS
- recebe os dados finais e uma colecao de arquivos
- faz upload de documentos em storage S3-compatible
- registra `DocumentoEmpresa`
- ativa o tenant

## Divergencias e observacoes

- o planejamento original mencionava `email-validation-queue`, mas a fila implementada hoje e `email_queue`
- o endpoint de consulta por CNPJ esta publico na implementacao atual
- o endpoint de completar cadastro exige autenticacao, mas usa `EmpresaId` vindo do formulario
- o dashboard so e liberado depois da ativacao do tenant
