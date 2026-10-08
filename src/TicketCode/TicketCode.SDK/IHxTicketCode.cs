using HxSDK.TicketCode.Models;
using System;

namespace HxSDK.TicketCode
{
    public interface IHxTicketCode
    {
        TicketCodeResponse<GetCodeResponse> GetCode(string sOrderNo, int iNumber, int iGroupId, DateTime tExpireTime);
    }
}
