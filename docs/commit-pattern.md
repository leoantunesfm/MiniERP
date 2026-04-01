# Padrao de Commits - Conventional Commits

Este documento define o padrao de mensagens de commit adotado no projeto, baseado na especificacao Conventional Commits.

O objetivo e manter um historico organizado, legivel e facil de automatizar.

## Estrutura

```text
<tipo>(escopo opcional): descricao curta

[corpo opcional]

[rodape opcional]
```

## Tipos de commit

| Tipo | Descricao |
| --- | --- |
| `feat` | Nova funcionalidade |
| `fix` | Correcao de bug |
| `docs` | Alteracoes na documentacao |
| `style` | Formatacao sem mudanca de logica |
| `refactor` | Refatoracao de codigo |
| `test` | Adicao ou ajuste de testes |
| `chore` | Tarefas internas, build e configuracao |

## Escopo opcional

O escopo ajuda a indicar a area afetada.

Exemplos:

```bash
feat(auth): adiciona login com Google
fix(api): corrige validacao de token
refactor(user): reorganiza service
```

Escopos comuns no projeto:

- `auth`
- `api`
- `ui`
- `database`
- `infra`
- `integration`

## Exemplos

```bash
feat(payment): adiciona suporte a PIX
fix(api): corrige erro ao validar token JWT
refactor(order): melhora organizacao do service
docs(readme): adiciona instrucoes de instalacao
test(auth): adiciona testes para login
chore(ci): ajusta pipeline de build
```

## Breaking changes

Forma curta:

```bash
feat(api)!: altera estrutura de resposta
```

Forma com rodape:

```bash
feat(api): altera estrutura de resposta

BREAKING CHANGE: campo "data" foi removido
```

## Boas praticas

- use descricao curta e objetiva
- escreva no imperativo
- evite mensagens genericas como `ajustes`
- separe commits por responsabilidade
- use escopo quando fizer sentido

## Impacto em versionamento

- `feat`: incrementa versao minor
- `fix`: incrementa versao patch
- `BREAKING CHANGE`: incrementa versao major

## Atalho recomendado para o dia a dia

```bash
feat: ...
fix: ...
refactor: ...
docs: ...
```
