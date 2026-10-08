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
        /// 分组前缀代码;
        /// 2022.6.17 增加冗余字段
        /// </summary>
        [Required]
        public int iPrefixCode { get; set; }

        /// <summary>
        /// 是否核销
        /// </summary>
        [Required]
        public bool bConsume { get; set; }

        /// <summary>
        /// 过期时间(冗余字段)
        /// </summary>
        [Required]
        public DateTime tExpireTime { get; set; }

        /// <summary>
        /// 核销时间
        /// </summary>
        public DateTime? tConsumeTime { get; set; }

        /// <summary>
        /// 核销类型；
        /// 0-游客核销，1-系统回收
        /// </summary>
        [Required]
        public int iConsumeType { get; set; }

        public virtual TcRequsets TcRequset { get; set; }

        public virtual TcConsume TcConsume { get; set; }
    }
}
