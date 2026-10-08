using System.Runtime.Serialization;

namespace HxSDK.TicketCode.Models
{
    [DataContract]
    public class TicketCodeResponse<TValue> : TicketCodeResponse
    {
        [DataMember]
        public TValue data { get; set; }

        public TicketCodeResponse(TValue data, int code, string message)
            : base(code, message)
        {
            this.data = data;
        }
    }
}
