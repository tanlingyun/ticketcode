using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TicketCode.Infrastructure.Models;

namespace TicketCode.Core.Models
{
    public class TcAccounts : EntityBase
    {
        [Required]
        [MaxLength(50)]
        public string sAppName { get; set; }

        [Required]
        [MaxLength(50)]
        public string sAppId { get; set; }

        [Required]
        [MaxLength(50)]
        public string sAppSecret { get; set; }

        [Required]
        public DateTime tCreateTime { get; set; }

        [Required]
        public bool bDisable { get; set; }

        public virtual IEnumerable<TcGroupInAccount> TcGroupInAccounts { get; set; }
    }
}
