namespace TicketCode.Infrastructure
{
    public class Result<TValue> : Result
    {
        public TValue data { get; set; }

        protected internal Result(TValue value, int code, string message, string reqno)
            : base(code, message, reqno)
        {
            data = value;
        }
    }
}
