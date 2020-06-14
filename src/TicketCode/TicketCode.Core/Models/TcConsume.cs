using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TicketCode.Infrastructure.Models;

namespace TicketCode.Core.Models
{
    /// <summary>
    /// 核销记录
    /// </summary>
    public class TcConsume : EntityBase
    {
        [Required]
        public long iRequestLineId { get; set; }

        /// <summary>
        /// 取票码
        /// </summary>
        [Required]
        public int iFullCode { get; set; }

        /// <summary>
        /// 核销时间
        /// </summary>
        [Required]
        public DateTime tConsumeTime { get; set; }

        [Required]
        public long iGroupId { get; set; }

        [Required]
        public long iAccountId { get; set; }

        public virtual TcRequestLines TcRequestLine { get; set; }

        public virtual TcGroups TcGroup { get; set; }

        public virtual TcAccounts TcAccount { get; set; }
    }
}
