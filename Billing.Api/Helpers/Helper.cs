using Billing.Database;
using System;
using System.Reflection;
using System.Text;
using System.Web;

namespace Billing.Api.Helpers
{
    public static class Helper
    {
        public static int StatusCount { get { return Enum.GetValues(typeof(Status)).Length; } }
        public static int RegionCount { get { return Enum.GetValues(typeof(Region)).Length; } }

        public static string Signature(string Secret, string AppId)
        {
            byte[] secret = Convert.FromBase64String(Secret);
            byte[] appId = Convert.FromBase64String(AppId);

            var provider = new System.Security.Cryptography.HMACSHA256(secret);
            string key = System.Text.Encoding.Default.GetString(appId);
            var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(key));

            return Convert.ToBase64String(hash);
        }
    }
}