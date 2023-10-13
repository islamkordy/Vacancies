using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public static class GenerateString
    {
        public static string GenerateRandomString()
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            return "WD" + new string(Enumerable.Repeat(allowedChars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
