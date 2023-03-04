using System.Security.Cryptography;
using System.Text;

namespace GraduateThesis.ApplicationCore.Hash;

public class HashFunctions
{
    public static string GetSHA256(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            StringBuilder hashSb = new StringBuilder();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte b in hash)
            {
                hashSb.Append(b.ToString("x2"));
            }
            return hashSb.ToString();
        }
    }

    public static string GetSHA384(string input)
    {
        using (SHA384 sha384 = SHA384.Create())
        {
            StringBuilder hashSb = new StringBuilder();
            byte[] hash = sha384.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte b in hash)
            {
                hashSb.Append(b.ToString("x2"));
            }
            return hashSb.ToString();
        }
    }

    public static string GetSHA512(string input)
    {
        using (SHA512 sha512 = SHA512.Create())
        {
            StringBuilder hashSb = new StringBuilder();
            byte[] hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte b in hash)
            {
                hashSb.Append(b.ToString("x2"));
            }
            return hashSb.ToString();
        }
    }

    public static string GetMD5(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder hashSb = new StringBuilder();
            foreach (byte b in hash)
            {
                hashSb.Append(b.ToString("X2"));
            }
            return hashSb.ToString();
        };
    }
}
