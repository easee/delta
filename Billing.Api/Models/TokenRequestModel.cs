using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class TokenRequestModel
    {
        public string ApiKey { get; set; }      // RGVsdGEtQmlsbGluZw==
        public string Signature { get; set; }   // CaByDlSnGLx0gOtlDClK6L94Jb/cwJoJAdUDlhtNpnI=
        public string Remember { get; set; }
    }
}