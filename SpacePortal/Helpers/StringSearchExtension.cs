using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacePortal.Helpers;

public static class StringExtensions
{
    public static string NormalizeSearch(this string text)
    {
        return new string(text
            .Normalize(NormalizationForm.FormD)
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .ToArray())
            .ToLowerInvariant();
    }
}
