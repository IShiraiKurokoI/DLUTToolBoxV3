using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using WinUICommunity.Common.Helpers;
using System.Threading;

namespace DLUTToolBoxV3.Helpers
{
    public class ActionHelper
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        public async static Task SendMessageToUserCore(string msg)
        {
            //1.清理QQ文件
            //2.网络优化
            //3.重置截图序号
            //4.切换桌面壁纸高画质
            await Task.Run(async () =>
            {
                try
                {
                    string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                    await File.WriteAllBytesAsync(directory + "\\ToolBox.User.Core.exe", File.ReadAllBytes(ApplicationHelper.GetFullPathToExe() + "\\Win64\\UserCore\\ToolBox.User.Core.exe"));
                    logger.Info("UserCore位置：" + directory + "\\ToolBox.User.Core.exe");
                    logger.Info("尝试向UserCore发送信息:" + msg);
                    Process P = new Process();
                    P.StartInfo.UseShellExecute = true;
                    P.StartInfo.Verb = "runas";
                    P.StartInfo.FileName = directory + "\\ToolBox.User.Core.exe";
                    P.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    P.StartInfo.Arguments = msg;
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"UserCore启动失败：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                }
            });
        }
        
        public async static Task SendMessageToSystemCore(string msg)
        {
            //1.切换系统预留空间
            //2.切换幽灵熔断补丁
            //3.切换基于虚拟化的安全性
            //4.切换TSX多事务管理
            //5.切换时间线开关
            //6.重启资源管理器
            await Task.Run(async () =>
            {
                try
                {
                    string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                    await File.WriteAllBytesAsync(directory + "\\ToolBox.System.Core.exe", File.ReadAllBytes(ApplicationHelper.GetFullPathToExe() + "\\Win64\\SystemCore\\ToolBox.System.Core.exe"));
                    logger.Info("SystemCore位置：" + directory + "\\ToolBox.System.Core.exe");
                    logger.Info("尝试向SystemCore发送信息:" + msg);
                    Process P = new Process();
                    P.StartInfo.UseShellExecute = true;
                    P.StartInfo.Verb = "runas";
                    P.StartInfo.FileName = directory + "\\ToolBox.System.Core.exe";
                    P.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    P.StartInfo.Arguments = msg;
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"SystemCore启动失败：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                }
            });
        }
    }
}
