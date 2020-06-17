namespace TicketCode.Infrastructure
{
    public class Result
    {
        public int code { get; }

        public string reqno { get; set; }

        public string message { get; }

        protected Result(int code, string message, string reqno)
        {
            this.code = code;
            this.message = message;
            this.reqno = reqno;
        }

        public static Result Fail(string error, string reqno)
        {
            return new Result(-1, error,reqno);
        }

        public static Result Ok(string reqno)
        {
            return new Result(0, "", reqno);
        }

        public static Result<TValue> Ok<TValue>(TValue value, string reqno)
        {
            return new Result<TValue>(value, 0, null,reqno);
        }

        public static Result<TValue> Fail<TValue>(string error, string reqno)
        {
            return new Result<TValue>(default(TValue), -1, error, reqno);
        }
    }
}
