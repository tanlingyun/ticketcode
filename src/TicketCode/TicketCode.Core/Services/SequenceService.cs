using System;

namespace TicketCode.Core.Services
{
    public class SequenceService
    {
        protected readonly static int MAXID = 9999;
        protected static object _lock = null;
        protected static int CurID = 1;

        static SequenceService()
        {
            _lock = new object();
            CurID = 0;
        }

        public static string GetId(int iPrefixCode)
        {
            //iPrefixCode+yyyyMMddHHmmss+业务流水号(4位)
            lock (_lock)
            {
                CurID++;
                return $"{iPrefixCode}{DateTime.Now.ToString("yyMMddHHmmss")}{CurID.ToString().PadLeft(4, '0')}";
            }
        }
    }
}
