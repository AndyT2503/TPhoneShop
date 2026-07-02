using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BuildingBlocks.Application.Slug
{
    public class SlugGenerator : ISlugGenerator
    {
        public string Generate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            value = value.Trim().ToLowerInvariant();

            var normalized = value.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            value = builder.ToString()
                .Normalize(NormalizationForm.FormC);
            value = value.Replace('đ', 'd');
            value = Regex.Replace(value, @"[^a-z0-9]+", "-");
            value = Regex.Replace(value, @"-+", "-");

            return value.Trim('-');
        }
    }
}
