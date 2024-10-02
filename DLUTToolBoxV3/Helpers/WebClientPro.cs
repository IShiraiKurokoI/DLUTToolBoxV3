using System;
using System.Net;

namespace DLUTToolBoxV3.Helpers
{
    public class WebClientPro : WebClient
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public int Timeout { get; set; }

        public WebClientPro(int timeout = 5000)
        {//默认30秒
            Timeout = timeout;
        }

        /// <summary>
        /// 重写GetWebRequest,添加WebRequest对象超时时间
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {//WebClient里上传下载的方法很多，但最终应该都是调用了这个方法
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = Timeout;
            request.ReadWriteTimeout = Timeout;
            return request;
        }
    }
}
