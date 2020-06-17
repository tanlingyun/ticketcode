using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TicketCode.Infrastructure.Models;

namespace TicketCode.Core.Dto
{
    public class DtoGetCodeRequest
    {
        [Required(ErrorMessage = "外部订单号不能为空")]
        [MaxLength(50,ErrorMessage = "外部订单号长度不能超过50个字符")]
        public string sOuterNo { get; set; }

        [Required(ErrorMessage = "取票码数量不能为空")]
        [Range(1,100,ErrorMessage = "单次取票仅支持1-100个码")]
        public int iNumber { get; set; }

        [Required(ErrorMessage = "失效时间不能为空")]
        public DateTime tExpireTime { get; set; }

        [MaxLength(200,ErrorMessage = "备注不能超过200个字符")]
        public string sMemo { get; set; }
    }
}
