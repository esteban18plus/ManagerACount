using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerACount.Utilities
{
    public static class Extensions
    {
        public static string Base64Encode(this string userPassword)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(userPassword);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
