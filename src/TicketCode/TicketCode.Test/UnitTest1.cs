using NUnit.Framework;
using System;

namespace TicketCode.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var sdk = new HxSDK.TicketCode.HxTicketCode(new HxSDK.TicketCode.SdkOption()
            {
                ApiUrl = "http://47.100.97.19:9066/",
                AppId = "HXWXWEB",
                AppKey = "debed64fb5ff11ea98285404a66171ec"
            });
            var c = sdk.GetCode("1233333333", 1, 10, DateTime.Now.AddDays(1));

            Assert.Pass();
        }
    }
}