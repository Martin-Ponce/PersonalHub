namespace PersonalHub.API.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var chars = value
                .Normalize(System.Text.NormalizationForm.FormD)
                .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                .ToArray();
            var noAccents = new string(chars);

            var parts = noAccents.Split([' '], StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                return string.Empty;

            var result = new System.Text.StringBuilder(parts[0].ToLowerInvariant());
            for (int i = 1; i < parts.Length; i++)
            {
                if (parts[i].Length == 0) continue;
                result.Append(char.ToUpperInvariant(parts[i][0]))
                      .Append(parts[i][1..].ToLowerInvariant());
            }
            return result.ToString();
        }

        public static string ToSnakeCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var result = new System.Text.StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (char.IsUpper(c) && i > 0)
                    result.Append('_');
                result.Append(char.ToLowerInvariant(c));
            }
            return result.ToString();
        }

        public static string FromPascal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var result = new System.Text.StringBuilder();
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (char.IsUpper(c) && i > 0)
                    result.Append(' ');
                result.Append(c);
            }
            return result.ToString();
        }

        public static string ToUrlWithSlash(this string? url, Exception exceptionWhenInvalid)
        {
            if (string.IsNullOrEmpty(url)) throw exceptionWhenInvalid;
            if (!url.EndsWith('/'))
                url += "/";
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                throw exceptionWhenInvalid;
            return url;
        }

        public static string ToUrlWithoutSlash(this string? url, Exception exceptionWhenInvalid)
        {
            if (string.IsNullOrEmpty(url)) throw exceptionWhenInvalid;
            if (url.EndsWith('/'))
                url = url.TrimEnd('/');
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                throw exceptionWhenInvalid;
            return url;
        }

        public static int ToPort(this string? value, Exception exceptionWhenInvalid)
        {
            if (string.IsNullOrEmpty(value)) throw exceptionWhenInvalid;
            if (!int.TryParse(value, out var port) || port < 0 || port > 65535)
                throw exceptionWhenInvalid;
            return port;
        }

        public static bool ToBool(this string? value, Exception exceptionWhenInvalid)
        {
            if (string.IsNullOrEmpty(value)) throw exceptionWhenInvalid;
            if (!bool.TryParse(value, out var result))
                throw exceptionWhenInvalid;
            return result;
        }
    }
}
