using HxSDK.TicketCode.Extensions;
using HxSDK.TicketCode.Helpers;
using HxSDK.TicketCode.Models;
using Microsoft.Extensions.Options;
using System;

namespace HxSDK.TicketCode
{
    public class HxTicketCode: IHxTicketCode
    {
        protected readonly string apiUrl;

        protected readonly string appId;

        protected readonly string appKey;


        public HxTicketCode(IOptions<SdkOption> options)
        {
            this.apiUrl = options?.Value?.ApiUrl;
            this.appId = options?.Value?.AppId;
            this.appKey = options?.Value?.AppKey;
        }

        /// <summary>
        /// 获取取票码
        /// </summary>
        /// <param name="sOrderNo">订单号</param>
        /// <param name="iNumber">数量</param>
        /// <param name="iGroupId">分组</param>
        /// <param name="tExpireTime">过期时间</param>
        /// <returns></returns>
        public TicketCodeResponse<GetCodeResponse> GetCode(string sOrderNo, int iNumber, int iGroupId, DateTime tExpireTime)
        {
            return GetData<GetCodeResponse>("api/code/get", iGroupId, new GetCodeRequest()
            {
                sOuterNo = sOrderNo,
                iNumber = iNumber,
                tExpireTime = tExpireTime.ToString("yyyy/MM/dd HH:mm:ss"),
                sMemo = ""
            });
        }

        private TicketCodeResponse<T> GetData<T>(string api, int group, object data)
        {
            long timestamp = DateTime.Now.Timestamp(0);
            string reqno = Guid.NewGuid().ToString().Replace("-", "");
            //appid+group+reqno+timestamp+key
            string sign = $"{appId}{group}{reqno}{timestamp}{appKey}".ToMd5().ToLower();
            string url = $"{apiUrl}{api}?appid={appId}&group={group}&timestamp={timestamp}&sign={sign}&reqno={reqno}";
            Logger<HxTicketCode>.LogInformation("请求的地址：" + url);

            var postdata = data.ToJsonString();
            Logger<HxTicketCode>.LogInformation("请求的数据：" + postdata);

            string result, error;
            var header = new System.Net.WebHeaderCollection();
            header.Add("Content-Type", "application/json");
            var status = HttpHelper.Post(url, postdata, header, out result, out error);
            if (status != System.Net.HttpStatusCode.OK)
            {
                Logger<HxTicketCode>.LogError("请求失败：" + status.ToString());
                return TicketCodeResponse.Fail<T>(error);
            }
            Logger<HxTicketCode>.LogInformation("请求返回:" + result);
            return result.JsonToObject<TicketCodeResponse<T>>();
        }
    }
}
