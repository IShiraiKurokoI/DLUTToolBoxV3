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

namespace DLUTToolBoxV3.Helpers
{
    public class ActionHelper
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        public async static Task SendMessageToUserCore(string msg)
        {
            await Task.Run(() =>
            {
                logger.Info("尝试向UserCore发送信息:" + msg);
                Process P = new Process();
                P.StartInfo.UseShellExecute = true;
                P.StartInfo.Verb = "runas";
                P.StartInfo.FileName = ApplicationHelper.GetFullPathToExe() + "\\Win64\\UserCore\\ToolBox.User.Core.exe";
                P.StartInfo.WorkingDirectory = ApplicationHelper.GetFullPathToExe() + "\\Win64\\UserCore\\";
                P.StartInfo.Arguments = msg;
                P.Start();
            });
        }
    }
}
