using System;
using System.Collections.Generic;
using System.Text;

namespace TicketCode.Core.Dto
{
    public class DtoGetCodeResponse
    {
        /// <summary>
        /// 取码单号
        /// </summary>
        public string sTcNo { get; set; }

        public string sOuterNo { get; set; }


        public int iPrefixCode { get; set; }

        /// <summary>
        /// 取票码长度
        /// </summary>
        public int iLength { get; set; }


        public IEnumerable<int> aNumbers { get; set; }

        public string tExpireTime { get; set; }
    }
}
