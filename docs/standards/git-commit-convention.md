# Git Commit Convention (TPhoneShop Project)

This document defines the commit message convention used across the project (Backend .NET Core, NestJS API, and Angular Frontend).

---

## 📌 Commit Message Format

```
<type>(<scope>): <short summary>
```

### Example:

```
feat(auth): add JWT login flow
fix(api): resolve user validation bug
chore(db): update database schema
```

---

## 📌 Commit Types

| Type     | Description                                               |
| -------- | --------------------------------------------------------- |
| feat     | A new feature                                             |
| fix      | A bug fix                                                 |
| chore    | Maintenance tasks (dependencies, config, gitignore, etc.) |
| refactor | Code changes that do not affect behavior                  |
| perf     | Performance improvements                                  |
| test     | Adding or updating tests                                  |
| docs     | Documentation changes                                     |
| style    | Code formatting, whitespace, missing semicolons, etc.     |
| build    | Build system or external dependencies changes             |
| ci       | CI/CD pipeline changes                                    |

---

## 📌 Scope Guidelines

Scope defines the area of the change.

### Backend (.NET / NestJS)

```
feat(auth): ...
feat(user): ...
chore(api): ...
chore(db): ...
```

### Frontend (Angular)

```
feat(ui): ...
feat(dashboard): ...
fix(ui): ...
```

### Infrastructure / DevOps

```
chore(ci): ...
chore(docker): ...
chore(infra): ...
```

---

## 📌 Good Commit Examples

### Simple commits

```
feat(auth): add login endpoint
fix(api): handle null reference exception
chore(api): update dependencies
```

### Multiple related changes (still same domain)

```
chore(api): update dependencies and improve configuration
```

---

## 📌 Bad Commit Examples

```
chore: update dependency TPhoneShop.API & update database design & add .gitignore
```

Avoid:

- Mixing multiple unrelated tasks
- Using "&", "and", or long sentences
- Vague messages like "fix stuff" or "update code"

---

## 📌 Better Way (Split Commits)

```
chore(api): update dependencies
chore(db): revise database design
chore(git): add .gitignore
```

---

## 📌 Commit Message Rules

- Use **present tense**: add, fix, update, remove
- Keep summary under 72 characters when possible
- One commit = one logical change
- Be clear and descriptive
- Avoid combining unrelated changes

---

## 📌 Recommended Project Scopes

### Backend

- auth
- user
- api
- db

### Frontend

- ui
- dashboard
- auth

### Platform

- ci
- docker
- infra

---

## 🚀 Summary

Good commits help:

- Improve team collaboration
- Make history readable
- Simplify debugging
- Support clean architecture
