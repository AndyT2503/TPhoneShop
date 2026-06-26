namespace CommerceService.Application.Common.Abstractions
{
    public interface ISlugGenerator
    {
        string Generate(string value);
    }
}
