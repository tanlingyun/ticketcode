using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TicketCode.Infrastructure.Models;

namespace TicketCode.Core.Models
{
    public class TcRequsets : EntityBase
    {
        [Required]
        public string sRequestNo { get; set; }

        /// <summary>
        /// 外部单号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string sOuterNo { get; set; }

        /// <summary>
        /// 取票码数量
        /// </summary>
        [Required]
        public int iNumber { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime tCreateTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Required]
        public DateTime tExpireTime { get; set; }
        
        /// <summary>
        /// 分组编号
        /// </summary>
        [Required]
        public long iGroupId { get; set; }
        
        /// <summary>
        /// 账户编号
        /// </summary>
        [Required]
        public long iAccountId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(250)]
        public string sMemo { get; set; }

        public virtual TcAccounts TcAccount { get; set; }

        public virtual TcGroups TcGroup { get; set; }

        public virtual IEnumerable<TcRequestLines> TcRequestLines { get; set; }
    }
}
