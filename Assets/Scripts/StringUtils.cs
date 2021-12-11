using System.Globalization;
using System.Text;

public class StringUtils
{
    public static string FormatPrice(float price, int separateNumber = 3, char separator = ',')
    {
        var sb = new StringBuilder(price.ToString("0.00",CultureInfo.GetCultureInfo("en-US")));

        for (int i = 2 + separateNumber; i < sb.Length; i+= separateNumber)
        {
            var index = sb.Length - 1 - i;

            if (index > 0)
            {
                sb.Insert(index, separator);
                i++;
            }
        }

        return sb.ToString();
    }
}