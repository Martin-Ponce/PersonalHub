using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalHub.Domain.Validators
{
    public static class DomainValidators
    {
        public static string FormatDescription(this string description)
        {
            var trimmed = description.Trim();
            if (trimmed.Length == 0)
                throw new ArgumentException("Description cannot be empty or whitespace.");
            if (trimmed.Length == 1)
                return trimmed.ToUpper();
            return char.ToUpper(trimmed[0]) + trimmed.Substring(1);
        }

    }
}
