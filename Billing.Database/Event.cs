using System;

namespace Billing.Database
{
    public class Event : Basic
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }

        public virtual Invoice Invoice { get; set; }
    }
}