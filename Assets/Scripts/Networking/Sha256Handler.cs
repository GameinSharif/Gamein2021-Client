using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class Sha256Handler
{
    public static List<string> Sha256Encrypt(string phrase, string username)
    {
        var salt = CreateSalt();
        var saltAndPwd = string.Concat(phrase, salt);
        var hashedPwd = GetHashSha256(saltAndPwd);
        var passAndSalt = new List<string> {hashedPwd, salt};
        return passAndSalt;
    }

    private static string ByteArrayToString(byte[] inputArray)
    {
        var output = new StringBuilder("");
        foreach (var input in inputArray)
        {
            output.Append(input.ToString("X2"));
        }

        return output.ToString();
    }

    private static string CreateSalt()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 20)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string GetHashSha256(string text)
    {
        var bytes = Encoding.ASCII.GetBytes(text);
        var hashString = new SHA256Managed();
        var hash = hashString.ComputeHash(bytes);

        return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
    }
}