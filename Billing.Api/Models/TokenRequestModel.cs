using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.Api.Models
{
    public class TokenRequestModel
    {
        public string ApiKey { get; set; }      // R2lnaVNjaG9vbA==
        public string Signature { get; set; }   // gC0xdV8gLD2cU0lzeDxFGZoZhxd78iz+6KojPZR5Wh4=
    }
}