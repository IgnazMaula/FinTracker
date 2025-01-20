using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FinTracker.Utilities.Helper
{
    public static class YearMonthFormatValidator
    {
        public static (bool IsValid, int Year) ValidateAndExtractYear(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);

            if (!Regex.IsMatch(name, @"^\d{6}$"))
            {
                return (false, 0);
            }

            if (int.TryParse(name.Substring(0, 4), out int year) &&
                int.TryParse(name.Substring(4, 2), out int month) &&
                year >= 1900 && year <= 2100 && month >= 1 && month <= 12)
            {
                return (true, year);
            }

            return (false, 0);
        }
    }
}
