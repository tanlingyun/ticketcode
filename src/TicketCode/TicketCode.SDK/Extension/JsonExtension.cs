using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HxSDK.TicketCode.Extensions
{
    internal static class JsonExtension
    {
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj, string encoding = "utf-8")
        {
            //序列化
            string jsondata = "";
            var serializer = new DataContractJsonSerializer(obj.GetType(), new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true,
                DateTimeFormat = new DateTimeFormat("yyyy/MM/dd HH:mm:ss"),
                IgnoreExtensionDataObject = true,
                SerializeReadOnlyTypes = true
            });
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                ms.Position = 0;
                using (var reader = new StreamReader(ms, Encoding.GetEncoding(encoding)))
                {
                    jsondata = reader.ReadToEnd();
                }
            }
            return jsondata;
        }

        public static T JsonToObject<T>(this string str, string encoding = "utf-8")
        {
            var serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true,
                DateTimeFormat = new DateTimeFormat("yyyy/MM/dd HH:mm:ss"),
                IgnoreExtensionDataObject = true,
                SerializeReadOnlyTypes = false
            });
            using (var ms = new MemoryStream(Encoding.GetEncoding(encoding).GetBytes(str)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
