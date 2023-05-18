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
using Windows.UI.WebUI;

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
                if (String.IsNullOrEmpty(ApplicationConfig.GetSettings("Uid")))
                {
                    dispatcherQueue.TryEnqueue(() =>
                    {
                         infoBar.Message = "电费余额查询失败\n账号密码为空";
                    });
                    return;
                }
                try
                {
                    bool APILoginTried = false;
                    dispatcherQueue.TryEnqueue(() => {
                        WebView2 webView2 = new WebView2();
                        webView2.CoreWebView2Initialized += (sender, args) =>
                        {
                            webView2.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (Linux; Android 10; EBG-AN00 Build/HUAWEIEBG-AN00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/83.0.4103.106 Mobile Safari/537.36 weishao(3.2.2.74616)";
                            webView2.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = true;
                            webView2.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
                            webView2.CoreWebView2.Settings.IsZoomControlEnabled = true;
                            webView2.CoreWebView2.Settings.IsGeneralAutofillEnabled = true;
                            webView2.CoreWebView2.Settings.IsWebMessageEnabled = true;
                            webView2.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = true;
                            webView2.CoreWebView2.NewWindowRequested += (sender, args) =>
                            {
                                webView2.CoreWebView2.ExecuteScriptAsync("window.location.href='" + args.Uri.ToString() + "'");
                                args.Handled = true;
                            };
                            webView2.CoreWebView2.NavigationCompleted += async (sender, args) =>
                            {
                                if (webView2.Source.AbsoluteUri.StartsWith("https://api.m.dlut.edu.cn/login"))
                                {
                                    if (APILoginTried)
                                    {
                                        dispatcherQueue.TryEnqueue(() =>
                                        {
                                            logger.Info("电费余额查询失败\n账号密码错误");
                                            infoBar.Message = "电费余额查询失败\n账号密码错误";
                                        });
                                    }
                                    else
                                    {
                                        APILoginTried = true;
                                        if (String.IsNullOrEmpty(ApplicationConfig.GetSettings("Uid")) || String.IsNullOrEmpty(ApplicationConfig.GetSettings("Password")))
                                        {
                                            dispatcherQueue.TryEnqueue(() =>
                                            {
                                                logger.Info("电费余额查询失败\n账号密码错误");
                                                infoBar.Message = "电费余额查询失败\n账号密码为空";
                                            });
                                        }
                                        else
                                        {
                                            logger.Info("执行api登录注入");
                                            string jscode = "username.value='" + ApplicationConfig.GetSettings("Uid") + "'";
                                            string jscode1 = "password.value='" + ApplicationConfig.GetSettings("Password") + "'";
                                            await webView2.CoreWebView2.ExecuteScriptAsync(jscode);
                                            await webView2.CoreWebView2.ExecuteScriptAsync(jscode1);
                                            string jscode2 = "$(\"#formpc\").submit()";
                                            await webView2.CoreWebView2.ExecuteScriptAsync(jscode2);
                                        }
                                        return;
                                    }
                                }
                                if (webView2.Source.AbsoluteUri.IndexOf("homerj") != -1 && webView2.Source.AbsoluteUri.IndexOf("api") == -1)
                                {
                                    await webView2.CoreWebView2.ExecuteScriptAsync("window.location.href='https://card.m.dlut.edu.cn/elepay/openElePay?openid='+openid[0].value+'&displayflag=1&id=30'");
                                }
                                if (webView2.Source.AbsoluteUri.IndexOf("openElePay") != -1)
                                {
                                    await webView2.CoreWebView2.ExecuteScriptAsync(Properties.Resources.Send);
                                    await webView2.CoreWebView2.ExecuteScriptAsync(Properties.Resources.Eleget);
                                }
                            };
                            webView2.CoreWebView2.WebMessageReceived += (sender, e) =>
                            {
                                string data = e.WebMessageAsJson.ToString().Split(new[] { "\"" }, StringSplitOptions.None)[1].Split(new[] { "：" }, StringSplitOptions.None)[1];
                                if (data != "0.00")
                                {
                                    dispatcherQueue.TryEnqueue(() =>
                                    {
                                        logger.Info("您寝室剩余电量为" + data + "度");
                                        infoBar.Message = "您寝室剩余电量为" + data + "度";
                                        if (double.Parse(data) <= 10)
                                        {
                                            infoBar.Message += "\n⚠余额不足10度，请及时充值！⚠";
                                        }
                                        webView2.CoreWebView2.Stop();
                                        webView2.Close();
                                    });
                                }
                                else
                                {
                                    dispatcherQueue.TryEnqueue(() =>
                                    {
                                        logger.Info("电费查询服务器返回错误");
                                        infoBar.Message = "服务器返回错误\n请稍后再试";
                                        webView2.CoreWebView2.Stop();
                                        webView2.Close();
                                    });
                                }
                            };
                        };
                        webView2.Source = new Uri("https://api.m.dlut.edu.cn/oauth/authorize?client_id=19b32196decf419a&redirect_uri=https%3A%2F%2Fcard.m.dlut.edu.cn%2Fhomerj%2FopenRjOAuthPage&response_type=code&scope=base_api&state=weishao");
                        webView2.EnsureCoreWebView2Async();
                    });
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
    }
}
