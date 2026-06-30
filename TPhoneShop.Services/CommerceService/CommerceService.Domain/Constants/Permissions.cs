namespace CommerceService.Domain.Constants
{
    public static class Permissions
    {
        public const string RolesRead = "roles.read";
        public const string RolesCreate = "roles.create";
        public const string RolesDelete = "roles.delete";
        public const string RolesUpdate = "roles.update";
        public const string RolesAssignPermissions = "roles.assign-permissions";

        public const string PermissionsRead = "permissions.read";

        public const string BrandsCreate = "brands.create";
        public const string BrandsUpdate = "brands.update";
        public const string BrandsDelete = "brands.delete";
        public const string BrandsRead = "brands.read";

        public const string ProductsCreate = "products.create";
        public const string ProductsUpdate = "products.update";
        public const string ProductsDelete = "products.delete";
        public const string ProductsRead = "products.read";

        public const string CategoriesCreate = "categories.create";
        public const string CategoriesUpdate = "categories.update";
        public const string CategoriesDelete = "categories.delete";
        public const string CategoriesRead = "categories.read";
    }
}
