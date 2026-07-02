# Database Design

## Identity Service

### users

Stores application users for authentication and identity management.

| Column       | Type         | Description                          |
| ------------ | ------------ | ------------------------------------ |
| Id           | uuid         | Primary key                          |
| Email        | varchar(255) | User email                           |
| PasswordHash | varchar(500) | Hashed password (nullable for SSO)   |
| FullName     | varchar(255) | User full name                       |
| IsActive     | boolean      | Indicates whether the user is active |
| CreatedAt    | timestamp    | Record creation timestamp            |
| UpdatedAt    | timestamp    | Record update timestamp              |

---

### refresh_tokens

Stores refresh tokens issued to authenticated users.

| Column     | Type          | Description                 |
| ---------- | ------------- | --------------------------- |
| Id         | uuid          | Primary key                 |
| UserId     | uuid          | References user             |
| Token      | varchar(500)  | Refresh token               |
| ExpiresAt  | timestamp     | Token expiration timestamp  |
| RevokedAt  | timestamp     | Token revocation timestamp  |
| DeviceName | varchar(200)  | Device name for the session |
| IpAddress  | varchar(100)  | Origin IP address           |
| UserAgent  | varchar(1000) | Client user agent           |
| CreatedAt  | timestamp     | Record creation timestamp   |
| UpdatedAt  | timestamp     | Record update timestamp     |

---

### user_security_logs

Tracks security and authentication-related actions.

| Column        | Type          | Description                            |
| ------------- | ------------- | -------------------------------------- |
| Id            | uuid          | Primary key                            |
| UserId        | uuid          | References user                        |
| Action        | varchar(255)  | Security action performed              |
| IsSuccess     | boolean       | Indicates whether the action succeeded |
| FailureReason | varchar(500)  | Failure reason when action fails       |
| IpAddress     | varchar(100)  | Request origin IP address              |
| UserAgent     | varchar(1000) | Client user agent                      |
| DeviceName    | varchar(200)  | Device identification                  |
| CreatedAt     | timestamp     | Record creation timestamp              |
| UpdatedAt     | timestamp     | Record update timestamp                |

---

### signing_keys

Stores signing keys for token issuance and rotation.

| Column      | Type         | Description                         |
| ----------- | ------------ | ----------------------------------- |
| Id          | uuid         | Primary key                         |
| Kid         | varchar(100) | Key identifier                      |
| PrivateKey  | text         | Private signing key                 |
| PublicKey   | text         | Public signing key                  |
| IsActive    | boolean      | Indicates whether the key is active |
| RevokedAt   | timestamp    | Key revocation timestamp            |
| ActivatedAt | timestamp    | Key activation timestamp            |
| CreatedAt   | timestamp    | Record creation timestamp           |
| UpdatedAt   | timestamp    | Record update timestamp             |

---

### reset_password_tokens

Stores password reset tokens.

| Column    | Type         | Description                               |
| --------- | ------------ | ----------------------------------------- |
| Id        | uuid         | Primary key                               |
| UserId    | uuid         | References user                           |
| Token     | varchar(256) | Reset token                               |
| ExpiredAt | timestamp    | Token expiration timestamp                |
| IsUsed    | boolean      | Indicates whether the token has been used |
| CreatedAt | timestamp    | Record creation timestamp                 |
| UpdatedAt | timestamp    | Record update timestamp                   |

---

## Commerce Service

### permissions

Stores authorization permissions.

| Column    | Type         | Description               |
| --------- | ------------ | ------------------------- |
| Id        | uuid         | Primary key               |
| Name      | varchar(200) | Permission code           |
| CreatedAt | timestamp    | Record creation timestamp |
| UpdatedAt | timestamp    | Record update timestamp   |

---

### roles

Stores authorization roles.

| Column    | Type         | Description               |
| --------- | ------------ | ------------------------- |
| Id        | uuid         | Primary key               |
| Name      | varchar(100) | Role name                 |
| CreatedAt | timestamp    | Record creation timestamp |
| UpdatedAt | timestamp    | Record update timestamp   |

---

### role_permissions

Maps roles to permissions.

| Column       | Type | Description           |
| ------------ | ---- | --------------------- |
| RoleId       | uuid | Role identifier       |
| PermissionId | uuid | Permission identifier |

---

### user_roles

Maps users to roles with uniqueness constraints.

| Column    | Type      | Description               |
| --------- | --------- | ------------------------- |
| Id        | uuid      | Primary key               |
| UserId    | uuid      | References user           |
| RoleId    | uuid      | References role           |
| CreatedAt | timestamp | Record creation timestamp |
| UpdatedAt | timestamp | Record update timestamp   |

---

### brands

Stores product brand metadata.

| Column      | Type          | Description                           |
| ----------- | ------------- | ------------------------------------- |
| Id          | uuid          | Primary key                           |
| Name        | varchar(100)  | Brand name                            |
| Slug        | varchar(100)  | SEO-friendly URL slug                 |
| LogoUrl     | varchar(1000) | Brand logo URL                        |
| Description | text          | Brand description                     |
| IsActive    | boolean       | Indicates whether the brand is active |
| CreatedAt   | timestamp     | Record creation timestamp             |
| UpdatedAt   | timestamp     | Record update timestamp               |

---

### categories

Stores product categories.

| Column      | Type         | Description                              |
| ----------- | ------------ | ---------------------------------------- |
| Id          | uuid         | Primary key                              |
| ParentId    | uuid         | Parent category identifier (nullable)    |
| Name        | varchar(100) | Category name                            |
| Slug        | varchar(100) | SEO-friendly URL slug                    |
| Description | text         | Category description                     |
| IsActive    | boolean      | Indicates whether the category is active |
| CreatedAt   | timestamp    | Record creation timestamp                |
| UpdatedAt   | timestamp    | Record update timestamp                  |

---

### product_groups

Stores optional product grouping metadata.

| Column    | Type         | Description                           |
| --------- | ------------ | ------------------------------------- |
| Id        | uuid         | Primary key                           |
| Name      | varchar(255) | Group name                            |
| Slug      | varchar(255) | SEO-friendly URL slug                 |
| IsActive  | boolean      | Indicates whether the group is active |
| CreatedAt | timestamp    | Record creation timestamp             |
| UpdatedAt | timestamp    | Record update timestamp               |

---

### products

Stores master product data.

| Column         | Type         | Description                             |
| -------------- | ------------ | --------------------------------------- |
| Id             | uuid         | Primary key                             |
| ProductGroupId | uuid         | Optional product group identifier       |
| CategoryId     | uuid         | References category                     |
| BrandId        | uuid         | References brand                        |
| Name           | varchar(255) | Product name                            |
| Slug           | varchar(255) | SEO-friendly URL slug                   |
| Description    | text         | Product description                     |
| Attributes     | jsonb        | Product attributes serialized as JSONB  |
| IsActive       | boolean      | Indicates whether the product is active |
| CreatedAt      | timestamp    | Record creation timestamp               |
| UpdatedAt      | timestamp    | Record update timestamp                 |

---

### product_variants

Stores variant-level product data.

| Column         | Type          | Description                                             |
| -------------- | ------------- | ------------------------------------------------------- |
| Id             | uuid          | Primary key                                             |
| ProductId      | uuid          | References parent product                               |
| Name           | varchar(255)  | Variant name                                            |
| Sku            | varchar(100)  | Unique SKU                                              |
| ThumbnailUrl   | varchar(1000) | Variant image URL                                       |
| Price          | bigint        | Price stored as minor units (amount ×100)               |
| CompareAtPrice | bigint        | Original/list price stored as minor units (amount ×100) |
| StockQuantity  | int           | Available stock quantity                                |
| IsActive       | boolean       | Indicates whether the variant is active                 |
| CreatedAt      | timestamp     | Record creation timestamp                               |
| UpdatedAt      | timestamp     | Record update timestamp                                 |

---

### outbox_messages

Stores messages for the outbox pattern used by services.

| Column      | Type         | Description                       |
| ----------- | ------------ | --------------------------------- |
| Id          | uuid         | Primary key                       |
| Type        | varchar(255) | Message type                      |
| Payload     | jsonb        | Serialized message payload        |
| Status      | varchar(50)  | Delivery status                   |
| RetryCount  | int          | Number of delivery retry attempts |
| ProcessedAt | timestamp    | Last processed timestamp          |
| ExpiresAt   | timestamp    | Message expiration timestamp      |
| Error       | text         | Processing error message          |
| CreatedAt   | timestamp    | Record creation timestamp         |
| UpdatedAt   | timestamp    | Record update timestamp           |

---

## File Service

### medias

Stores uploaded media metadata.

| Column       | Type         | Description                    |
| ------------ | ------------ | ------------------------------ |
| Id           | uuid         | Primary key                    |
| Key          | varchar(255) | Storage key / object reference |
| ReferrenceId | varchar(255) | Related entity reference       |
| Size         | bigint       | File size in bytes             |
| ContentType  | varchar(255) | MIME type                      |
| CreatedAt    | timestamp    | Record creation timestamp      |
| UpdatedAt    | timestamp    | Record update timestamp        |

---

## Notification Service

### notification_logs

Stores notification requests and delivery status.

| Column       | Type         | Description                                |
| ------------ | ------------ | ------------------------------------------ |
| Id           | uuid         | Primary key                                |
| RecipientId  | varchar(255) | Target recipient identifier                |
| Event        | varchar(255) | Notification event key                     |
| Channel      | varchar(50)  | Delivery channel (email, sms, push)        |
| Payload      | jsonb        | Notification payload                       |
| Status       | varchar(50)  | Delivery status (pending, success, failed) |
| SentAt       | timestamp    | Delivery timestamp                         |
| ErrorMessage | text         | Failure reason message                     |
| CreatedAt    | timestamp    | Record creation timestamp                  |
| UpdatedAt    | timestamp    | Record update timestamp                    |
