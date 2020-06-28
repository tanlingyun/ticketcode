using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TicketCode.Infrastructure.Models;

namespace TicketCode.Core.Models
{
    public class TcGroups : EntityBase
    {
        /// <summary>
        /// 取票码前缀
        /// </summary>
        [Required]
        public int iPrefixCode { get; set; }

        [Required]
        [MaxLength(10)]
        public string sName { get; set; }

        /// <summary>
        /// 取票码长度，不含前缀
        /// </summary>
        [Required]
        public int iLength { get; set; }

        /// <summary>
        /// 已使用的数量
        /// </summary>
        [Required]
        public long iUsedNumber { get; set; }

        /// <summary>
        /// 每次递增的数量
        /// </summary>
        [Required]
        public long iIncrNumber { get; set; }

        /// <summary>
        /// 低值预警
        /// </summary>
        public long iMinNumber { get; set; }

        [Required]
        public DateTime tCreateTime { get; set; }

        /// <summary>
        /// 当前可用数量
        /// </summary>
        [Required]
        public long iCurrAvaNumber { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? tUpdateTime { get; set; }


        [Required]
        public bool bDisable { get; set; }

        [Required]
        public bool bDelete { get; set; }

        public virtual IEnumerable<TcGroupInAccount> TcGroupInAccounts { get; set; }
    }
}
