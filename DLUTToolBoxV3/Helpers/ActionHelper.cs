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
using WinUICommunity;
using System.Threading;
using DLUTToolBoxV3.Configurations;
using System.Drawing;
using System.Runtime.InteropServices;

namespace DLUTToolBoxV3.Helpers
{
    public class ActionHelper
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        public async static Task SendMessageToUserCore(string msg, EventHandler ExitHandler)
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
                    P.EnableRaisingEvents = true;
                    P.Exited += ExitHandler;
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"UserCore启动失败：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                    ExitHandler(null, null);
                }
            });
        }
        
        public async static Task SendMessageToSystemCore(string msg, EventHandler ExitHandler)
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
                    P.EnableRaisingEvents = true;
                    P.Exited+= ExitHandler;
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"SystemCore启动失败：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                    ExitHandler(null,null);
                }
            });
        }
        public async static Task SendMessageToVisualCore(string msg, EventHandler ExitHandler)
        {
            await Task.Run(async () =>
            {
                try
                {
                    string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                    DirectoryCopy(ApplicationHelper.GetFullPathToExe() + "\\Win64\\VisualCore", directory + "\\VisualCore", true);
                    logger.Info("VisualCore：" + directory + "\\VisualCore\\ToolBox.Visual.Core.exe");
                    logger.Info("VisualCore:" + msg);
                    Process P = new Process();
                    P.StartInfo.UseShellExecute = true;
                    P.StartInfo.Verb = "runas";
                    P.StartInfo.FileName = directory + "\\VisualCore\\ToolBox.Visual.Core.exe";
                    P.StartInfo.WorkingDirectory = directory + "\\VisualCore\\";
                    P.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    P.StartInfo.Arguments = msg;
                    P.EnableRaisingEvents = true;
                    P.Exited+= ExitHandler;
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"VisualCore：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                    ExitHandler(null,null);
                }
            });
        }
        
        public async static Task LaunchSpaceSniffer()
        {
            await Task.Run(async () =>
            {
                try
                {
                    string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                    await File.WriteAllBytesAsync(directory + "\\SpaceSniffer.exe", File.ReadAllBytes(ApplicationHelper.GetFullPathToExe() + "\\Win64\\ThirdParty\\SpaceSniffer\\SpaceSniffer.exe"));
                    logger.Info("SpaceSniffer位置：" + directory + "\\SpaceSniffer.exe");
                    Process P = new Process();
                    P.StartInfo.UseShellExecute = true;
                    P.StartInfo.Verb = "runas";
                    P.StartInfo.FileName = directory + "\\SpaceSniffer.exe";
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"SpaceSniffer启动失败：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                }
            });
        }
        
        public async static Task LaunchSysAutoFix()
        {
            await Task.Run(async () =>
            {
                try
                {
                    string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                    await File.WriteAllBytesAsync(directory + "\\SysAutoFix.exe", File.ReadAllBytes(ApplicationHelper.GetFullPathToExe() + "\\Win64\\SysAutoFix.exe"));
                    logger.Info("SysAutoFix位置：" + directory + "\\SysAutoFix.exe");
                    Process P = new Process();
                    P.StartInfo.UseShellExecute = true;
                    P.StartInfo.Verb = "runas";
                    P.StartInfo.FileName = directory + "\\SysAutoFix.exe";
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"SysAutoFix启动失败：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                }
            });
        }
        
        public async static Task LaunchCCleaner()
        {
            await Task.Run(async () =>
            {
                string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                DirectoryCopy(ApplicationHelper.GetFullPathToExe() + "\\Win64\\ThirdParty\\CCleaner", directory + "\\CCleaner", true);
                try
                {
                    Process P = new Process();
                    P.StartInfo.UseShellExecute = true;
                    P.StartInfo.Verb = "runas";
                    P.StartInfo.FileName = directory + "\\CCleaner\\CCleaner64.exe";
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"CCleaner启动失败：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                }
            });
        }
        
        public async static Task SetupEDALoginModule(string msg,EventHandler eventHandler)
        {
            await Task.Run(async () =>
            {
                string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                try
                {
                    if (!File.Exists(directory + "\\EDALoginModule\\EDALoginModule.exe"))
                    {
                        DirectoryCopy(ApplicationHelper.GetFullPathToExe() + "\\Win64\\EDALoginModule", directory + "\\EDALoginModule", true);
                    }
                    else
                    {
                        var ExistingModule = File.ReadAllBytes(directory + "\\EDALoginModule\\EDALoginModule.exe");
                        var PackageModule = File.ReadAllBytes(ApplicationHelper.GetFullPathToExe() + "\\Win64\\EDALoginModule\\EDALoginModule.exe");
                        if (ExistingModule != PackageModule)
                        {
                            DirectoryCopy(ApplicationHelper.GetFullPathToExe() + "\\Win64\\EDALoginModule", directory + "\\EDALoginModule", true);
                        }
                    }
                    FileStream fs = new FileStream(directory + "\\EDALoginModule\\Account.config", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(ApplicationConfig.GetSettings("Uid"));
                    sw.WriteLine(ApplicationConfig.GetSettings("Password"));
                    sw.Close();
                }
                catch(Exception e)
                {
                    logger.Error(e);
                }
                try
                {
                    Process P = new Process();
                    P.StartInfo.UseShellExecute = true;
                    P.StartInfo.Verb = "runas";
                    P.StartInfo.FileName = directory + "\\EDALoginModule\\EDALoginModule.exe";
                    P.StartInfo.Arguments = "Setup " + msg;
                    P.EnableRaisingEvents= true;
                    P.Exited += eventHandler;
                    P.Start();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                    var builder = new AppNotificationBuilder()
                        .AddText($"EDALoginModule启动失败：\n{ex.Message}");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                    eventHandler(null, null);
                }
            });
        }
        
        public async static Task CheckAccountData()
        {
            await Task.Run(async () =>
            {
                try
                {
                    string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
                    FileStream fs = new FileStream(directory + "\\EDALoginModule\\Account.config", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(ApplicationConfig.GetSettings("Uid"));
                    sw.WriteLine(ApplicationConfig.GetSettings("Password"));
                    sw.Close();
                }catch(Exception ex)
                {
                    logger.Error(ex);
                }
            });
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found:"
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
