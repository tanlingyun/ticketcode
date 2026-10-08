using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HxSDK.TicketCode.Models
{
    [DataContract]
    public class GetCodeResponse
    {
        /// <summary>
        /// 系统为本次取码分配的单号
        /// </summary>
        [DataMember]
        public string sTcNo { get; set; }

        /// <summary>
        /// 外部订单号
        /// </summary>
        [DataMember]
        public string sOuterNo { get; set; }

        /// <summary>
        /// 取票码分组
        /// </summary>
        [DataMember]
        public int iPrefixCode { get; set; }

        /// <summary>
        /// 取票码长度
        /// </summary>
        [DataMember]
        public int iLength { get; set; }

        /// <summary>
        /// 取票码数组
        /// </summary>
        [DataMember]
        public long[] aNumbers { get; set; }

        /// <summary>
        /// 取票码失效时间
        /// </summary>
        [DataMember]
        public DateTime tExpireTime { get; set; }
    }
}
