using System.Globalization;
using System.Windows.Controls;

namespace CapriciousUI.Wpf.Sample;

public class IntegerValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is not string str)
        {
            return new(false, "入力値が不正です。");
        }

        if (int.TryParse(str, out var _))
        {
            return new(true, null);
        }
        else
        {
            return new(false, "入力値が不正です。");
        }
    }
}
