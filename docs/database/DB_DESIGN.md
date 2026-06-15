# Database Design

## Users

Stores user account information.
Includes both customers and system staff.

| Column       | Type         | Description                             |
| ------------ | ------------ | --------------------------------------- |
| Id           | uuid         | Primary key                             |
| Email        | varchar(255) | Login email                             |
| PasswordHash | varchar(500) | Hashed password                         |
| FullName     | varchar(255) | User full name                          |
| PhoneNumber  | varchar(20)  | Phone number                            |
| Address      | text         | Shipping address                        |
| RoleId       | uuid         | Assigned role                           |
| IsActive     | boolean      | Indicates whether the account is active |
| CreatedAt    | timestamp    | Account creation timestamp              |
| UpdatedAt    | timestamp    | Last update timestamp                   |

---

## Roles

Stores system roles.

| Column    | Type         | Description           |
| --------- | ------------ | --------------------- |
| Id        | uuid         | Primary key           |
| Name      | varchar(100) | Role name             |
| CreatedAt | timestamp    | Creation timestamp    |
| UpdatedAt | timestamp    | Last update timestamp |

---

## Permissions

Stores system permissions.

| Column    | Type         | Description           |
| --------- | ------------ | --------------------- |
| Id        | uuid         | Primary key           |
| Name      | varchar(255) | Permission code       |
| CreatedAt | timestamp    | Creation timestamp    |
| UpdatedAt | timestamp    | Last update timestamp |

---

## RolePermissions

Maps roles to permissions.

| Column       | Type | Description           |
| ------------ | ---- | --------------------- |
| RoleId       | uuid | Role identifier       |
| PermissionId | uuid | Permission identifier |

---

## RefreshTokens

Stores refresh tokens used for JWT authentication.


| Column     | Type         | Description                        |
| ---------- | ------------ | ---------------------------------- |
| Id         | uuid         | Primary key                        |
| UserId     | uuid         | Token owner                        |
| Token      | varchar(500) | Hashed refresh token               |
| ExpiresAt  | timestamp    | Expiration timestamp               |
| RevokedAt  | timestamp    | Revocation timestamp               |
| DeviceName | varchar(255) | Device name                        |
| IpAddress  | varchar(100) | Login IP address                   |
| UserAgent  | text         | Browser or application information |
| CreatedAt  | timestamp    | Token creation timestamp           |

---

## Brands

Stores mobile phone brand information.

| Column      | Type          | Description                           |
| ----------- | ------------- | ------------------------------------- |
| Id          | uuid          | Primary key                           |
| Name        | varchar(100)  | Brand name                            |
| Slug        | varchar(100)  | SEO-friendly URL slug                 |
| LogoUrl     | varchar(1000) | Brand logo URL                        |
| Description | text          | Brand description                     |
| IsActive    | boolean       | Indicates whether the brand is active |
| CreatedAt   | timestamp     | Creation timestamp                    |
| UpdatedAt   | timestamp     | Last update timestamp                 |

---

## Products

Stores product master information.

| Column      | Type         | Description                                         |
| ----------- | ------------ | --------------------------------------------------- |
| Id          | uuid         | Primary key                                         |
| BrandId     | uuid         | Associated brand                                    |
| Name        | varchar(255) | Product name                                        |
| Slug        | varchar(255) | SEO-friendly URL slug                               |
| Description | text         | Detailed product description                        |
| IsActive    | boolean      | Indicates whether the product is available for sale |
| CreatedAt   | timestamp    | Creation timestamp                                  |
| UpdatedAt   | timestamp    | Last update timestamp                               |

---

## ProductVariants

Represents sellable product variants managed independently for inventory.

| Column         | Type          | Description                                         |
| -------------- | ------------- | --------------------------------------------------- |
| Id             | uuid          | Variant primary key                                 |
| ProductId      | uuid          | Parent product                                      |
| Sku            | varchar(100)  | Internal SKU                                        |
| Color          | varchar(100)  | Product color                                       |
| StorageGb      | int           | Storage capacity                                    |
| ThumbnailUrl   | varchar(1000) | Variant thumbnail image                             |
| Price          | bigint        | Current selling price (stored as amount ×100)       |
| CompareAtPrice | bigint        | Original/list price (stored as amount ×100)         |
| IsActive       | boolean       | Indicates whether the variant is available for sale |

---

## Inventories

Stores inventory information for each product variant.

| Column            | Type      | Description                          |
| ----------------- | --------- | ------------------------------------ |
| VariantId         | uuid      | Inventory-managed variant            |
| AvailableQuantity | int       | Quantity available for sale          |
| ReservedQuantity  | int       | Quantity reserved for pending orders |
| UpdatedAt         | timestamp | Last inventory update timestamp      |

---

## Orders

Stores order summary information.

| Column         | Type        | Description                                           |
| -------------- | ----------- | ----------------------------------------------------- |
| Id             | uuid        | Primary key                                           |
| OrderNumber    | varchar(50) | Customer-facing order number                          |
| UserId         | uuid        | Customer placing the order                            |
| CouponId       | uuid        | Applied coupon                                        |
| Status         | varchar(50) | Current order status                                  |
| DeliveryMethod | varchar(50) | Home delivery or store pickup                         |
| Subtotal       | bigint      | Total amount before discounts (stored as amount ×100) |
| DiscountAmount | bigint      | Discount amount (stored as amount ×100)               |
| ShippingFee    | bigint      | Shipping fee (stored as amount ×100)                  |
| TotalAmount    | bigint      | Final payable amount (stored as amount ×100)          |
| CreatedAt      | timestamp   | Order creation timestamp                              |
| UpdatedAt      | timestamp   | Last update timestamp                                 |

---

## OrderItems

Stores a snapshot of purchased products at the time of order placement.
Historical data must remain unchanged even if product information changes later.

| Column              | Type         | Description                                        |
| ------------------- | ------------ | -------------------------------------------------- |
| Id                  | uuid         | Primary key                                        |
| OrderId             | uuid         | Associated order                                   |
| VariantId           | uuid         | Purchased variant                                  |
| ProductNameSnapshot | varchar(255) | Product name at purchase time                      |
| VariantSnapshot     | varchar(255) | Example: Black - 256GB                             |
| UnitPrice           | bigint       | Purchase price at checkout (stored as amount ×100) |
| Quantity            | int          | Purchased quantity                                 |
| LineTotal           | bigint       | Line item total (stored as amount ×100)            |

---

## Payments

Stores payment transaction history.
An order may have multiple payment attempts.

| Column          | Type         | Description                              |
| --------------- | ------------ | ---------------------------------------- |
| Id              | uuid         | Primary key                              |
| OrderId         | uuid         | Associated order                         |
| Provider        | varchar(50)  | Payment gateway                          |
| TransactionId   | varchar(255) | Gateway transaction identifier           |
| Amount          | bigint       | Payment amount (stored as amount ×100)   |
| Status          | varchar(50)  | Payment status                           |
| ResponsePayload | jsonb        | Raw gateway response for troubleshooting |
| CreatedAt       | timestamp    | Transaction timestamp                    |

---

## Coupons

Stores promotional coupons and discount campaigns.

| Column                | Type         | Description                                             |
| --------------------- | ------------ | ------------------------------------------------------- |
| Id                    | uuid         | Primary key                                             |
| Code                  | varchar(100) | Coupon code                                             |
| Name                  | varchar(255) | Coupon name                                             |
| Description           | text         | Coupon description                                      |
| DiscountType          | varchar(50)  | Percentage or fixed amount                              |
| DiscountValue         | bigint       | Discount value (stored as amount ×100)                  |
| MinimumOrderAmount    | bigint       | Minimum order value required (stored as amount ×100)    |
| MaximumDiscountAmount | bigint       | Maximum discount amount allowed (stored as amount ×100) |
| UsageLimit            | int          | Maximum number of times the coupon can be used          |
| UsedCount             | int          | Number of times the coupon has been used                |
| StartAt               | timestamp    | Coupon activation date                                  |
| EndAt                 | timestamp    | Coupon expiration date                                  |
| IsActive              | boolean      | Indicates whether the coupon is active                  |
| CreatedAt             | timestamp    | Creation timestamp                                      |
| UpdatedAt             | timestamp    | Last update timestamp                                   |

---

## CouponUsages

Tracks coupon usage history.
Used to prevent duplicate coupon usage when business rules require restrictions.

| Column         | Type      | Description                                     |
| -------------- | --------- | ----------------------------------------------- |
| Id             | uuid      | Primary key                                     |
| CouponId       | uuid      | Associated coupon                               |
| UserId         | uuid      | User who used the coupon                        |
| OrderId        | uuid      | Associated order                                |
| DiscountAmount | bigint    | Applied discount amount (stored as amount ×100) |
| CreatedAt      | timestamp | Usage timestamp                                 |

---

## ProductComments

Stores product discussion comments.
Supports nested replies.

| Column          | Type      | Description                       |
| --------------- | --------- | --------------------------------- |
| Id              | uuid      | Primary key                       |
| ProductId       | uuid      | Associated product                |
| UserId          | uuid      | Comment author                    |
| ParentCommentId | uuid      | Parent comment for nested replies |
| Content         | text      | Comment content                   |
| IsDeleted       | boolean   | Soft delete flag                  |
| CreatedAt       | timestamp | Creation timestamp                |
| UpdatedAt       | timestamp | Last update timestamp             |

---

## ProductReviews

Stores customer product ratings and reviews.

Only customers who have purchased the product may submit reviews.

| Column    | Type      | Description               |
| --------- | --------- | ------------------------- |
| Id        | uuid      | Primary key               |
| ProductId | uuid      | Reviewed product          |
| UserId    | uuid      | Reviewer                  |
| OrderId   | uuid      | Associated purchase order |
| Rating    | int       | Rating score (1–5)        |
| Content   | text      | Review content            |
| CreatedAt | timestamp | Creation timestamp        |
| UpdatedAt | timestamp | Last update timestamp     |

---

## Notifications

Stores user notifications.


| Column    | Type         | Description                                      |
| --------- | ------------ | ------------------------------------------------ |
| Id        | uuid         | Primary key                                      |
| UserId    | uuid         | Notification recipient                           |
| Type      | varchar(50)  | Notification type                                |
| Title     | varchar(255) | Notification title                               |
| Metadata  | jsonb        | Notification metadata                            |
| Content   | text         | Notification content                             |
| IsRead    | boolean      | Indicates whether the notification has been read |
| ReadAt    | timestamp    | Read timestamp                                   |
| CreatedAt | timestamp    | Creation timestamp                               |


---

## OutboxEvents

Stores domain events waiting to be published.

Implements the Outbox Pattern to guarantee consistency between database transactions and message broker delivery.

| Column      | Type         | Description                                    |
| ----------- | ------------ | ---------------------------------------------- |
| Id          | uuid         | Primary key                                    |
| EventType   | varchar(255) | Event name                                     |
| Payload     | jsonb        | Serialized event payload                       |
| IsPublished | boolean      | Indicates whether the event has been published |
| PublishedAt | timestamp    | Publish timestamp                              |
| CreatedAt   | timestamp    | Event creation timestamp                       |

---

