# EF Core Migration Guide

## Connection String (appsettings.json)

```
{
  "ConnectionStrings": {
    "<db_key>": "Host=localhost;Port=5432;Database=identity_db;Username=postgres;Password=<your_password>"
  }
}
```

---

## Create Migration

```
cd <*>.Persistence

dotnet ef migrations add <MigrationName> --startup-project <*>.API
```

---

## Update Database

```
cd <*>.Persistence

dotnet ef database update --startup-project <*>.API
```

---

## Remove Last Migration

```
cd <*>.Persistence

dotnet ef migrations remove --startup-project <*>.API
```