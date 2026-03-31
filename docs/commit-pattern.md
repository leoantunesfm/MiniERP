# 📘 Padrão de Commits — Conventional Commits

Este documento define o padrão de mensagens de commit adotado no projeto, baseado na especificação **Conventional Commits**.

O objetivo é manter um histórico de commits organizado, legível e automatizável.

---

## 🧠 Estrutura do Commit

```
<tipo>(escopo opcional): descrição curta

[corpo opcional]

[rodapé opcional]
```

---

## 🧩 Tipos de Commit

| Tipo     | Descrição                             |
| -------- | ------------------------------------- |
| feat     | Nova funcionalidade                   |
| fix      | Correção de bug                       |
| docs     | Alterações na documentação            |
| style    | Formatação (sem alteração de lógica)  |
| refactor | Refatoração de código                 |
| test     | Adição ou alteração de testes         |
| chore    | Tarefas internas (build, config, etc) |

---

## 🧠 Escopo (Opcional)

O escopo indica a área do sistema afetada.

### Exemplos:

```bash
feat(auth): adiciona login com Google
fix(api): corrige validação de token
refactor(user): reorganiza service
```

### Sugestões de escopo para o projeto:

* auth
* api
* ui
* database
* infra
* integration

---

## ✍️ Exemplos de Commits

### ✅ Nova funcionalidade

```bash
feat(payment): adiciona suporte a PIX
```

### 🐛 Correção de bug

```bash
fix(api): corrige erro ao validar token JWT
```

### ♻️ Refatoração

```bash
refactor(order): melhora organização do service
```

### 📄 Documentação

```bash
docs(readme): adiciona instruções de instalação
```

### 🧪 Testes

```bash
test(auth): adiciona testes para login
```

### ⚙️ Tarefas internas

```bash
chore(ci): ajusta pipeline de build
```

---

## ⚠️ Breaking Changes

Utilizado quando há mudanças que quebram compatibilidade.

### Forma 1 — Usando "!"

```bash
feat(api)!: altera estrutura de resposta
```

### Forma 2 — Rodapé

```bash
feat(api): altera estrutura de resposta

BREAKING CHANGE: campo "data" foi removido
```

---

## 🧱 Corpo do Commit (Opcional)

Utilize para detalhar o contexto da alteração.

```bash
fix(auth): corrige erro no refresh token

O token não estava sendo validado corretamente
quando expirava após 15 minutos.
```

---

## 🔚 Rodapé (Opcional)

Utilizado para referenciar issues.

```bash
Closes #123
```

---

## 🚀 Versionamento Semântico (SemVer)

Os tipos de commit podem ser utilizados para versionamento automático:

| Tipo            | Impacto                          |
| --------------- | -------------------------------- |
| feat            | Incrementa MINOR (1.2.0 → 1.3.0) |
| fix             | Incrementa PATCH (1.2.0 → 1.2.1) |
| BREAKING CHANGE | Incrementa MAJOR (1.2.0 → 2.0.0) |

---

## 🛠️ Boas Práticas

* Use descrição curta e objetiva
* Escreva no imperativo ("adiciona", "corrige", "remove")
* Evite mensagens genéricas como "ajustes" ou "update"
* Separe commits por responsabilidade
* Utilize escopo sempre que possível

---

## ✅ Padrão Simplificado (Recomendado para uso diário)

Se quiser simplificar:

```bash
feat: ...
fix: ...
refactor: ...
```

---

## 📌 Conclusão

O uso do padrão Conventional Commits melhora a organização do projeto, facilita o entendimento do histórico e permite automações como geração de changelog e versionamento automático.
