-- Identity Database
-- Create default admin user
-- Default password is 123456
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