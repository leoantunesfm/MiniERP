# Feature: Autenticacao e Autorizacao

## Descricao

O modulo de autenticacao ja esta implementado no back-end e no front-end para permitir acesso autenticado ao sistema por meio de JWT.

## Back-end

### Endpoint

- `POST /api/Auth/login`

### Comportamento atual

- recebe `email` e `senha`
- busca o usuario pelo e-mail
- valida a senha com `PasswordHasher` baseado em BCrypt
- impede login de usuario inativo
- impede login se a empresa ainda estiver em `AguardandoConfirmacaoEmail`
- carrega os perfis do usuario
- gera JWT com `JwtTokenGenerator`

### Resposta atual

O login retorna:

- `token`
- `usuarioId`
- `empresaId`
- `nome`
- `tenantStatus`

## Front-end

### Tela de login

A rota `/login` usa Reactive Forms para capturar credenciais e tratar estados de loading/erro.

### Persistencia local

O front-end salva no `localStorage`:

- `minierp_token`
- `minierp_user_nome`
- `minierp_empresa_id`
- `minierp_tenant_status`

### Interceptor

O `jwt-interceptor.ts` ja injeta o cabecalho `Authorization: Bearer <token>` nas requisicoes autenticadas.

### Guards

- `guestGuard`: bloqueia acesso a rotas publicas se ja houver token
- `authGuard`: protege rotas autenticadas e redireciona conforme o `tenantStatus`

## Regras de status do tenant

- `AguardandoConfirmacaoEmail`: login bloqueado
- `AguardandoDadosCompletos`: login permitido, mas o usuario e redirecionado para `/complete-registration`
- `Ativo`: acesso normal ao `/dashboard`

## Observacoes atuais

- o controller de autenticacao exposto hoje possui apenas login
- a documentacao anterior tratava autenticacao como "em breve", mas isso nao corresponde mais ao estado da codebase
- a autorizacao ainda depende fortemente do estado salvo no `localStorage`, entao ha espaco para endurecer o fluxo com mais validacoes no back-end
