using System;
using System.Collections.Generic;
using System.Text;

namespace TicketCode.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionResult : Exception
    {
        public ExceptionResult(string message) : base(message)
        {

        }

        public ExceptionResult(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
