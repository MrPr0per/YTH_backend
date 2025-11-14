using System.Globalization;
using System.Net.Mail;

namespace YTH_backend.Infrastructure.Email;

public static class EmailHelper
{
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        
        email = email.Trim();
        
        if (email.Contains(' '))
            return false;

        var parts = email.Split('@');
        if (parts.Length != 2)
            return false;

        var localPart = parts[0];
        var domainPart = parts[1];

        if (string.IsNullOrWhiteSpace(localPart) || string.IsNullOrWhiteSpace(domainPart))
            return false;
        
        try
        {
            var idn = new IdnMapping();
            domainPart = idn.GetAscii(domainPart);
        }
        catch
        {
            return false;
        }

        if (localPart.Length > 64)
            return false;

        if (domainPart.Length > 255)
            return false;
        
        try
        {
            new MailAddress($"{localPart}@{domainPart}");
            return true;
        }
        catch
        {
            return false;
        }
    }
}