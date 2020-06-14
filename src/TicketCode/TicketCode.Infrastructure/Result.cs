namespace TicketCode.Infrastructure
{
    public class Result
    {
        public int code { get; }

        public string message { get; }

        protected Result(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public static Result Fail(string error)
        {
            return new Result(-1, error);
        }

        public static Result Ok()
        {
            return new Result(0, "");
        }

        public static Result<TValue> Ok<TValue>(TValue value)
        {
            return new Result<TValue>(value, 0, null);
        }

        public static Result<TValue> Fail<TValue>(string error)
        {
            return new Result<TValue>(default(TValue), -1, error);
        }
    }
}
