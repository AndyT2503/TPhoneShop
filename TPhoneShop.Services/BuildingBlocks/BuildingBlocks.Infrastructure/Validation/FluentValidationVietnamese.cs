using FluentValidation.Resources;

namespace BuildingBlocks.Infrastructure.Validation
{
    public class CustomLanguageManager : LanguageManager
    {
        public CustomLanguageManager()
        {
            AddTranslation("vi", "NotNullValidator", "{PropertyName} không được để trống");
            AddTranslation("vi", "NotEmptyValidator", "{PropertyName} không được để trống");
            AddTranslation("vi", "EmailValidator", "Địa chỉ email không hợp lệ");
            AddTranslation("vi", "MinimumLengthValidator", "{PropertyName} phải có ít nhất {MinLength} ký tự");
            AddTranslation("vi", "MaximumLengthValidator", "{PropertyName} không được vượt quá {MaxLength} ký tự");
            AddTranslation("vi", "MatchesValidator", "{PropertyName} không đúng định dạng");

        }
    }
}
