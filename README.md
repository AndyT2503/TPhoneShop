# TPhoneShop Setup Guide

## Requirements

- MongoDB
- PostgreSQL
- Node.js / Yarn
- .NET 10 SDK
- Docker (for Redis, MinIO, RabbitMQ)

## Backend Setup

### 1. Run infrastructure services

From the repository root:

```bash
docker compose up -d
```

This starts:

- Redis
- MinIO
- RabbitMQ

### 2. Configure Firebase for Identity Service

In `TPhoneShop.Services\IdentityService\IdentityService.Infrastructure\Securities\Firebase`, copy:

- `tphoneshop-firebase.example.json`

to:

- `tphoneshop-firebase.json`

Then update the file with your Firebase configuration values.

> Note: `tphoneshop-firebase.json` must be created in the same folder as the example file.

### 3. Run database migrations

Follow the EF Core Migration Guide in `docs\database\ef-core-migration-guide.md`.

Run migrations for:

- `TPhoneShop.Services\TPhoneShop.Services\IdentityService\IdentityService.Persistence`
- `TPhoneShop.Services\TPhoneShop.Services\CommerceService\CommerceService.Persistence`
- `TPhoneShop.Services\TPhoneShop.Services\FileService\FileService.Persistence`

Example:

```bash
cd TPhoneShop.Services\IdentityService\IdentityService.Persistence

dotnet ef migrations add InitialIdentityMigration --startup-project ..\IdentityService.API

dotnet ef database update --startup-project ..\IdentityService.API
```

Repeat similarly for CommerceService and FileService.

### 4. Initialize default data with SQL script

If you need the first admin user and default commerce role data, run the SQL script from `database\*.sql` against your PostgreSQL database.

Example using `psql`:

```bash
psql "host=localhost port=5432 dbname=your_db user=postgres password=your_password" -f database/bootstrap.sql
```

Adjust the connection settings to match your PostgreSQL instance.

## Frontend Setup

### 1. Install dependencies

From the workspace root:

```bash
yarn install
```

### 2. Configure frontend apps

Update `env-config.json` for each app before starting:

- `TPhoneShop.Web\apps\auth\public\config\env-config.json`
- `TPhoneShop.Web\apps\commerce\public\config\env-config.json`
- `TPhoneShop.Web\apps\admin\public\config\env-config.json`
- `TPhoneShop.Web\apps\shell\public\config\env-config.json`

Each file should contain the correct backend service URLs, for example:

```json
{
  "identityService": "http://localhost:550/identity",
  "commerceService": "http://localhost:550/commerce",
  "notificationService": "http://localhost:550/notification",
  "firebase": {
    "apiKey": "",
    "authDomain": "",
    "projectId": "",
    "storageBucket": "",
    "messagingSenderId": "",
    "appId": ""
  }
}
```

### 3. Start the frontend

From the repo root:

```bash
yarn start
```

This should launch the NX workspace and run the micro frontend apps.

## Notes

- Ensure MongoDB and PostgreSQL are running before starting backend services.
- Use the root `docker compose up -d` command to start Redis, MinIO, and RabbitMQ.
- Update Firebase config in `tphoneshop-firebase.json` before running Identity Service.
- Verify each app's `env-config.json` file is correct before starting the frontend.
