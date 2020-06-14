using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TicketCode.Infrastructure.Models;

namespace TicketCode.Core.Models
{
    public class TcRequestLines : EntityBase
    {
        [Required]
        public long iRequestId { get; set; }

        [Required]
        public int iCode { get; set; }

        public virtual TcRequsets TcRequset { get; set; }
    }
}
