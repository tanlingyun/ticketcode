using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TicketCode.Core.Dto
{
    public class DtoConsumeCodeRequest
    {
        [Required(ErrorMessage = "sNo不能为空")]
        public string sNo { get; set; }

        [Required(ErrorMessage = "取票码无效")]
        public long[] aCodes { get; set; }
    }
}
