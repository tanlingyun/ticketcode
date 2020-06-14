using System;
using System.Collections.Generic;
using System.Text;
using TicketCode.Infrastructure.Models;

namespace TicketCode.Core.Models
{
    public class TcGroupInAccount : EntityBase
    {
        public long iGroupId { get; set; }

        public long iAccountId { get; set; }

        public virtual TcGroups TcGroup { get; set; }

        public virtual TcAccounts TcAccount { get; set; }
    }
}
