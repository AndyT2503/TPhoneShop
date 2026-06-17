# 📌 TPhoneShop - System Architecture Overview

## 🧩 1. High-Level Architecture

TPhoneShop is a distributed microservices-based e-commerce platform designed for scalability, maintainability, and high performance.

The system follows a **Clean Architecture + CQRS + MediatR pattern** on backend services and a **modern Angular 22 reactive frontend using Signals**.

---

## 2. Technology Stack

### Frontend

- Angular 22
- Signals
- TailwindCSS

### Backend

- .NET 10
- MediatR
- FluentValidation
- Entity Framework Core
- NestJS
- TypeORM
- Mongoose

### Databases

- PostgreSQL (Primary Transactional Database)
- MongoDB (Read Model & Reporting)

### Search

- Elasticsearch

### Security

- JWT Authentication
- Refresh Token
- RSA (RS256)
- JWKS Endpoint
- Key Rotation

### Infrastructure

- Docker
- Redis (caching)
- RabbitMQ / Kafka (future event-driven architecture)

---

## 3. System Architecture

```text
┌──────────────────────────────────────────────┐
│                 Angular 22                   │
│                  Frontend                    │
└────────────────────┬─────────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────────┐
│                 API Gateway                  │
│                                              │
│ • Authentication                             │
│ • Authorization                              │
│ • Routing                                    │
│ • Rate Limiting                              │
│ • Request Aggregation                        │
│ • Logging / Tracing                          │
└───────────┬────────────────────────┬─────────┘
            │                        │
            │                        │
            ▼                        ▼
┌──────────────────────┐   ┌──────────────────────┐
│   IdentityService    │   │   CommerceService    │
│       .NET 10        │   │       .NET 10        │
├──────────────────────┤   ├──────────────────────┤
│ Authentication       │   │ Business Logic       │
│ User Management      │   │ Orders               │
│ Key Rotation         │   │ Products             │
│ JWKS Endpoint        │   │ Inventory            │
│                      │   │ Cart                 │
└──────────┬───────────┘   └──────────┬───────────┘
           │                          │
           ▼                          ▼
┌──────────────────────┐   ┌──────────────────────┐
│ PostgreSQL           │   │ PostgreSQL           │
│                      │   │ Redis                │
└──────────┬───────────┘   └──────────┬───────────┘
           │                          │
           │                          │
           │         ┌────────────────┼─────────────────┐
           │         │                │                 │
           ▼         ▼                ▼                 ▼
     ┌──────────────────────┐ ┌───────────────┐ ┌───────────────┐
     │ RabbitMQ             │ │ Elasticsearch │ │ MongoDB       │
     └────────┬─────────────┘ └───────────────┘ └───────────────┘
              │
              ▼
    ┌──────────────────────┐
    │ NotificationService  │
    │       NestJS         │
    ├──────────────────────┤
    │ Email                │
    │ Push Notification    │
    │ Template Engine      │
    │ Retry Processing     │
    └───────────┬──────────┘
                │
                ▼
    ┌──────────────────────┐
    │ MongoDB              │
    │ Resend               │
    │ Redis                │
    └──────────────────────┘
                                                        
```

---
