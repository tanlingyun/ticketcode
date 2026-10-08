using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace HxSDK.TicketCode
{
    public class SdkOption : IOptions<SdkOption>
    {
        public SdkOption()
        {
            ApiUrl = "http://47.100.97.19:9066/";
        }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiUrl { get; set; }

        public string AppId { get; set; }

        public string AppKey { get; set; }


        public SdkOption Value => this;
    }
}
