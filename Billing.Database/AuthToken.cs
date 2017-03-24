﻿using System;

namespace Billing.Database
{
    public class AuthToken : Basic
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public virtual ApiUser ApiUser { get; set; }
    }
}