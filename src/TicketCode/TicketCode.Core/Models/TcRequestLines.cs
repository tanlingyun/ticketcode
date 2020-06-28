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

        /// <summary>
        /// 取票码
        /// </summary>
        [Required]
        public long iCode { get; set; }

        /// <summary>
        /// 完整取票码
        /// </summary>
        [Required]
        public long iFullCode { get; set; }

        /// <summary>
        /// 是否核销
        /// </summary>
        [Required]
        public bool bConsume { get; set; }

        /// <summary>
        /// 核销时间
        /// </summary>
        public DateTime? tConsumeTime { get; set; }

        public virtual TcRequsets TcRequset { get; set; }

        public virtual TcConsume TcConsume { get; set; }
    }
}
