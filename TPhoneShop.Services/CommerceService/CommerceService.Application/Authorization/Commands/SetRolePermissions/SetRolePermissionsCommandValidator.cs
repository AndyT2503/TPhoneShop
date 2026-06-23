namespace CommerceService.Application.Authorization.Commands.SetRolePermissions
{
    internal class SetRolePermissionsCommandValidator : AbstractValidator<SetRolePermissionsCommand>
    {
        public SetRolePermissionsCommandValidator()
        {
            RuleFor(x => x.RoleId).NotNull().WithName("Vai tro");
        }
    }
}
