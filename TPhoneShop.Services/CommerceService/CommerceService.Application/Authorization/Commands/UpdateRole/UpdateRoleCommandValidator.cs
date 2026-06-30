namespace CommerceService.Application.Authorization.Commands.UpdateRole
{
    internal class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithName("Tên vai trò");
        }
    }
}
