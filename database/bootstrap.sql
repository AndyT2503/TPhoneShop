-- Identity Database
-- Create default admin user
INSERT INTO public.users
(
    "Id",
    "Email",
    "PasswordHash",
    "FullName",
    "IsActive",
    "CreatedAt",
    "UpdatedAt"
)
VALUES
(
    '00000000-0000-4000-8000-000000000001',
    'dev@gmail.com',
    '$2a$11$wBg3eqNI5n5J7nDvIRpHLelrsRf81oZBUtotUz0qvak7DcrRl58ai',
    'Developer',
    TRUE,
    NOW(),
    NOW()
);

-- commerce Database
-- Create default role
INSERT INTO auth.roles
(
    "Id",
    "Name",
    "CreatedAt",
    "UpdatedAt"
)
VALUES
(
    '00000000-0000-4000-8000-000000000002',
    'Super Admin',
    NOW(),
    NOW()
)
ON CONFLICT ("Name") DO NOTHING;

-- Assign all permissions to super admin
INSERT INTO auth.role_permissions ("RoleId", "PermissionId")
SELECT
    r."Id",
    p."Id"
FROM auth.roles r
CROSS JOIN auth.permissions p
WHERE r."Name" = 'Super Admin'
ON CONFLICT ("RoleId", "PermissionId") DO NOTHING;

-- Assign the default admin user to the Super Admin role
INSERT INTO auth.user_roles
(
    "Id",
    "UserId",
    "RoleId",
    "CreatedAt",
    "UpdatedAt"
)
SELECT
    '00000000-0000-4000-8000-000000000003',
    '00000000-0000-4000-8000-000000000001',
    r."Id",
    NOW(),
    NOW()
FROM auth.roles r
WHERE r."Name" = 'Super Admin'
ON CONFLICT ("UserId", "RoleId") DO NOTHING;