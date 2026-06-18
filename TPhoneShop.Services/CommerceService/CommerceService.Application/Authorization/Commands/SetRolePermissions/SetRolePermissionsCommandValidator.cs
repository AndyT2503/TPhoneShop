namespace CommerceService.Application.Authorization.Commands.SetRolePermissions
{
    public class SetRolePermissionsCommandValidator : AbstractValidator<SetRolePermissionsCommand>
    {
        public SetRolePermissionsCommandValidator()
        {
            RuleFor(x => x.RoleId).NotNull().WithName("Vai tro");
        }
    }
}
