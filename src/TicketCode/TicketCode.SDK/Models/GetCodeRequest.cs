using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HxSDK.TicketCode.Models
{
    [DataContract]
    internal class GetCodeRequest
    {
        [DataMember]
        public string sOuterNo { get; set; }

        [DataMember]
        public int iNumber { get; set; }

        [DataMember]
        public string tExpireTime { get; set; }

        [DataMember]
        public string sMemo { get; set; }
    }
}
