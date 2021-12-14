using System;
using System.Text.RegularExpressions;

public class StringUtils
{
    public static bool IsAlphaNumeric(string str)
    {
        Regex r = new Regex("^[a-zA-Z0-9]*$");
        return r.IsMatch(str);
    }

    public static string Reverse(string str)
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}