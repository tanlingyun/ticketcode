using System.Runtime.Serialization;

namespace HxSDK.TicketCode.Models
{
    [DataContract]
    public class TicketCodeResponse
    {
        [DataMember]
        public int code { get; set; }

        [DataMember]
        public string message { get; set; }

        protected TicketCodeResponse(int code, string error)
        {
            this.code = code;
            message = error;
        }

        public static TicketCodeResponse Fail(string error)
        {
            return new TicketCodeResponse(-1, error);
        }

        public static TicketCodeResponse Fail(int code, string error)
        {
            return new TicketCodeResponse(code, error);
        }

        public static TicketCodeResponse Ok()
        {
            return new TicketCodeResponse(0, null);
        }

        public static TicketCodeResponse<TValue> Ok<TValue>(TValue value)
        {
            return new TicketCodeResponse<TValue>(value, 0, null);
        }

        public static TicketCodeResponse<TValue> Ok<TValue>(TValue value,string message)
        {
            return new TicketCodeResponse<TValue>(value, 0, message);
        }

        public static TicketCodeResponse<TValue> Fail<TValue>(string error)
        {
            return TicketCodeResponse.Fail<TValue>(-1, default(TValue), error);
        }

        public static TicketCodeResponse<TValue> Fail<TValue>(TValue value, string error)
        {
            return TicketCodeResponse.Fail<TValue>(-1,value,error);
        }

        public static TicketCodeResponse<TValue> Fail<TValue>(int code, TValue value, string error)
        {
            return new TicketCodeResponse<TValue>(value, code, error);
        }
    }
}
