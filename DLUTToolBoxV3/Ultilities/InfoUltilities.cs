using DLUTToolBoxV3.Entities;
using Newtonsoft.Json;
using System;
using DLUTToolBoxV3.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using System.IO;
using DLUTToolBoxV3.Configurations;
using Castle.Core.Internal;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Diagnostics;
using System.Net;

namespace DLUTToolBoxV3.Ultilities
{
    public class InfoUltilities
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static DrcomStatus GetEDANetworkOnlineInfo()
        {
            using (WebClientPro client = new WebClientPro())
            {
                try
                {
                    string result = client.DownloadString("http://172.20.30.1/drcom/chkstatus?callback=");
                    string data = result.Split(new[] { "(" }, StringSplitOptions.None)[1].Split(new[] { ")" }, StringSplitOptions.None)[0];
                    DrcomStatus drcomStatus = JsonConvert.DeserializeObject<DrcomStatus>(data);
                    return drcomStatus;
                }
                catch (System.Net.WebException e)
                {
                    logger.Error(e);
                    return null;
                }
            }
        }

        public static void GetElectricityInfo(InfoBar infoBar, Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue)
        {
            if (int.Parse(DateTime.Now.Hour.ToString()) <= 1 || int.Parse(DateTime.Now.Hour.ToString()) >= 23)
            {
                dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                {
                    infoBar.Message = "当前不在查询时间！";
                });
            }
            else
            {
                if (ApplicationConfig.GetSettings("Uid").IsNullOrEmpty())
                {
                    return;
                }
                try
                {
                    string Response = PostWebRequest("https://card.m.dlut.edu.cn/business/queryResEleByIdserial?openid=DLUTToolBox&connect_redirect=1", "{\"idserial\":\"" + ApplicationConfig.GetSettings("Uid") + "\",\"xqh\":\"01\"}", Encoding.UTF8);
                    if (Response.Contains("无绑定"))
                    {
                        Response = PostWebRequest("https://card.m.dlut.edu.cn/business/queryResEleByIdserial?openid=DLUTToolBox&connect_redirect=1", "{\"idserial\":\"" + ApplicationConfig.GetSettings("Uid") + "\",\"xqh\":\"02\"}", Encoding.UTF8);
                    }
                    if (Response.Contains("无绑定"))
                    {
                        Response = PostWebRequest("https://card.m.dlut.edu.cn/business/queryResEleByIdserial?openid=DLUTToolBox&connect_redirect=1", "{\"idserial\":\"" + ApplicationConfig.GetSettings("Uid") + "\",\"xqh\":\"03\"}", Encoding.UTF8);
                    }
                    if (Response.Contains("无绑定"))
                    {
                        dispatcherQueue.TryEnqueue(() =>
                        {
                            infoBar.Message = "未查询到绑定的宿舍信息";
                        });
                    }
                    else
                    {
                        ElectricityStatus electricityStatus = JsonConvert.DeserializeObject<ElectricityStatus>(Response);
                        dispatcherQueue.TryEnqueue(() =>
                        {
                            if (electricityStatus.resultData == null)
                            {
                                infoBar.Message = "电费余额查询失败";
                                return;
                            }
                            if (electricityStatus.resultData.sydl == null)
                            {
                                infoBar.Message = "电费余额查询失败";
                                return;
                            }
                            infoBar.Message = "您寝室电费余额为" + electricityStatus.resultData.sydl.Substring(0, electricityStatus.resultData.sydl.Length - 2) + "度";
                            if (double.Parse(electricityStatus.resultData.sydl) < 10)
                            {
                                infoBar.Message += "\n⚠您电费余额不足10度，请及时充值！⚠";
                            }
                        });
                    }
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    dispatcherQueue.TryEnqueue(() =>
                    {
                        var builder = new AppNotificationBuilder()
                            .AddText("电费查询失败：" + e.Message);
                        var notificationManager = AppNotificationManager.Default;
                        notificationManager.Show(builder.BuildNotification());
                    });
                }
            }
        }
        public static string GetWebRequest(string url, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                int statusCode = (int)response.StatusCode;
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
            return ret;
        }
        public static string CommonGetWebRequest(string url, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.5249.91 Safari/537.36";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                int statusCode = (int)response.StatusCode;
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                logger.Error(ex);
            }
            return ret;
        }

        private static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            byte[] byteArray = dataEncode.GetBytes(paramData); //转化
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
            webReq.Method = "POST";
            webReq.ContentType = "application/json";
            webReq.UserAgent = "weishao";
            webReq.ContentLength = byteArray.Length;
            Stream newStream = webReq.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);//写入参数
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            ret = sr.ReadToEnd();
            sr.Close();
            response.Close();
            newStream.Close();
            return ret;
        }

        public static void StarupLogin()
        {
            DrcomStatus drcomStatus = InfoUltilities.GetEDANetworkOnlineInfo();
            if (drcomStatus != null)
            {
                if (drcomStatus.result != 1)
                {
                    string LoginURL = "https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2F172.20.30.2%3A8080%2FSelf%2Fsso_login%3Fwlan_user_ip%3D" + drcomStatus.v46ip + "%26authex_enable%3D%26type%3D1";
                    WebView2 loginweb = new WebView2();
                    loginweb.CoreWebView2Initialized += (sender, args) =>
                    {
                        loginweb.NavigationCompleted += (sender1, args1) =>
                        {
                            if (loginweb.Source.AbsoluteUri.IndexOf("https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2F172.20.30.2%3A8080%2FSelf%2Fsso_login") != -1)
                            {
                                logger.Info("执行sso登录注入");
                                string jscode = "un.value='" + ApplicationConfig.GetSettings("Uid") + "'";
                                string jscode1 = "pd.value='" + ApplicationConfig.GetSettings("Password") + "'";
                                string rm = "rememberName.checked='checked'";
                                loginweb.CoreWebView2.ExecuteScriptAsync(rm);
                                loginweb.CoreWebView2.ExecuteScriptAsync(jscode);
                                loginweb.CoreWebView2.ExecuteScriptAsync(jscode1);
                                string jscode2 = "login()";
                                loginweb.CoreWebView2.ExecuteScriptAsync(jscode2);
                            }
                            else if (loginweb.Source.AbsoluteUri.IndexOf("http://172.20.30.2:8080/Self/dashboard;jsessionid=") != -1)
                            {
                                drcomStatus = InfoUltilities.GetEDANetworkOnlineInfo();
                                double fee = drcomStatus.fee;
                                fee /= 10000;
                                string V4IP = drcomStatus.v4ip;
                                string flowused = FormatFlow(drcomStatus.flow);
                                var builder = new AppNotificationBuilder()
                                    .AddText("校园网已连接！\n校园网余额：" + fee + "\n本机校园网已用流量：\n" + flowused + "\nIPV4地址：" + V4IP + "\n网卡MAC：" + drcomStatus.olmac);
                                var notificationManager = AppNotificationManager.Default;
                                notificationManager.Show(builder.BuildNotification());
                                logger.Info("登陆成功");
                                loginweb.CoreWebView2.CookieManager.DeleteCookiesWithDomainAndPath("JSESSIONID", "172.20.30.2", "/Self");
                                loginweb.CoreWebView2.CookieManager.DeleteCookiesWithDomainAndPath("JSESSIONID", "172.20.30.2", "/");
                            }
                        };
                        loginweb.Source = new Uri(LoginURL);
                    };
                    loginweb.EnsureCoreWebView2Async();
                }
                else
                {
                    double fee = drcomStatus.fee;
                    fee /= 10000;
                    string V4IP = drcomStatus.v4ip;
                    string flowused = FormatFlow(drcomStatus.flow);
                    var builder = new AppNotificationBuilder()
                        .AddText("校园网已连接！\n校园网余额：" + fee + "\n本机校园网已用流量：\n" + flowused + "\nIPV4地址：" + V4IP + "\n网卡MAC：" + drcomStatus.olmac);
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                    logger.Info("无需登录");
                }
                return;
            }

            string FormatFlow(long num)
            {
                double temp = num;
                string re = "";
                if (temp > 1048576)
                {
                    temp /= (double)(1024 * 1024);
                    re = temp.ToString() + "GB";
                }
                else if (temp > 1024)
                {
                    temp /= (double)(1024);
                    re = temp.ToString() + "MB";
                }
                else
                {
                    re = temp + "KB";
                }
                return re;
            }
        }
    }
}
