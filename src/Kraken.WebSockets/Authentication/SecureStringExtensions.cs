using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Kraken.WebSockets.Authentication
{
    public static class SecureStringExtensions
    {
        public static string ToPlainString(this SecureString secureString)
        {
#pragma warning disable S1854 // Unused assignments should be removed
            IntPtr unmanagedString = IntPtr.Zero;
#pragma warning restore S1854 // Unused assignments should be removed
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString ToSecureString(this string value)
        {
            var secureString = new SecureString();
            secureString.Clear();

            foreach (var character in value)
            {
                secureString.AppendChar(character);
            }
            secureString.MakeReadOnly();

            return secureString;
        }
    }
}
