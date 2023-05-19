// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using DLUTToolBoxV3.Helpers;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using NLog;
using DLUTToolBoxV3.Configurations;
using WinUICommunity;
using DLUTToolBoxV3.Entities;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SystemPage : Page
    {
        public NLog.Logger logger;
        public SystemPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开系统工具页面");
            this.InitializeComponent();
        }

        bool StatusInitialized = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            try
            {
                RegistryKey hkrm = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", false);
                ReservedSpace.IsOn = (hkrm.GetValue("ShippedWithReserves").ToString() == "1");
            }
            catch(Exception ee) { 
                logger.Error(ee);
            }

            try
            {
                RegistryKey hkmm = hklm.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", false);
                if (hkmm.GetValue("FeatureSettingsOverride") == null)
                {
                    MeltDownPatch.IsOn = true;
                }
                else
                {
                    MeltDownPatch.IsOn = (hkmm.GetValue("FeatureSettingsOverride").ToString() == "0");
                }
            }
            catch (Exception ee)
            {
                logger.Error(ee);
            }

            try
            {
                RegistryKey hkcd = hklm.OpenSubKey(@"System\CurrentControlSet\Control\DeviceGuard", false);
                if (hkcd == null)
                {
                    VBS.IsOn = true;
                }
                else
                {
                    if (hkcd.GetValue("EnableVirtualizationBasedSecurity") == null)
                    {
                        VBS.IsOn = true;
                    }
                    else
                    {
                        VBS.IsOn = (hkcd.GetValue("EnableVirtualizationBasedSecurity").ToString() == "1");
                    }
                }
            }
            catch (Exception ee)
            {
                logger.Error(ee);
            }

            try
            {
                RegistryKey hksk = hklm.OpenSubKey(@"System\CurrentControlSet\Control\Session Manager\kernel", false);
                if (hksk.GetValue("DisableTsx") == null)
                {
                    TSX.IsOn = false;
                }
                else
                {
                    TSX.IsOn = !(hksk.GetValue("DisableTsx").ToString() == "1");
                }
            }
            catch (Exception ee)
            {
                logger.Error(ee);
            }

            try
            {
                RegistryKey hkws = hklm.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System", false);
                if (hkws.GetValue("EnableActivityFeed") == null)
                {
                    TimeLine.IsOn = true;
                }
                else
                {
                    TimeLine.IsOn = (hkws.GetValue("EnableActivityFeed").ToString() == "1");
                }
            }
            catch (Exception ee)
            {
                logger.Error(ee);
            }

            try
            {
                RegistryKey hkcu = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                RegistryKey hkcd1 = hkcu.OpenSubKey(@"Control Panel\Desktop", false);
                if (hkcd1.GetValue("JPEGImportQuality") == null)
                {
                    HighQualityWallpaper.IsOn = false;
                }
                else
                {
                    HighQualityWallpaper.IsOn = (hkcd1.GetValue("JPEGImportQuality").ToString() == "100");
                }
            }
            catch (Exception ee)
            {
                logger.Error(ee);
            }
            StatusInitialized = true;
            logger.Info("页面状态初始化完成");
        }
        bool RLF = false;
        private void ReservedSpace_Toggled(object sender, RoutedEventArgs e)
        {
            logger.Info("切换预留空间");
            if (StatusInitialized&&!RLF)
            {
                var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
                ActionHelper.SendMessageToSystemCore("1", (o, e) =>
                {
                    if(o==null)
                    {
                        RLF = true;
                    }
                    RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey hkrm = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", false);
                    dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                    {
                        ReservedSpace.IsOn = (hkrm.GetValue("ShippedWithReserves").ToString() == "1");
                        RLF = false;
                    });
                });
            }
        }
        bool MLF = false;
        private void MeltDownPatch_Toggled(object sender, RoutedEventArgs e)
        {
            logger.Info("切换熔断补丁");
            if (StatusInitialized&&!MLF)
            {
                var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
                ActionHelper.SendMessageToSystemCore("2", (o, e) =>
                {
                    if (o == null)
                    {
                        MLF = true;
                    }
                    RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey hkmm = hklm.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", false);
                    if (hkmm.GetValue("FeatureSettingsOverride") == null)
                    {
                        dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                        {
                            MeltDownPatch.IsOn = true;
                            MLF = false;
                        });
                    }
                    else
                    {
                        dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                        {
                            MeltDownPatch.IsOn = (hkmm.GetValue("FeatureSettingsOverride").ToString() == "0");
                            MLF = false;
                        });
                    }
                });
            }
        }

        bool VLF = false;
        private void VBS_Toggled(object sender, RoutedEventArgs e)
        {
            logger.Info("切换虚拟安全性");
            if (StatusInitialized&&!VLF)
            {
                var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
                ActionHelper.SendMessageToSystemCore("3", (o, e) =>
                {
                    if(o == null)
                    {
                        VLF = true;
                    }
                    RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey hkcd = hklm.OpenSubKey(@"System\CurrentControlSet\Control\DeviceGuard", false);
                    dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                    {
                        if (hkcd == null)
                        {
                            VBS.IsOn = true;
                        }
                        else
                        {
                            if (hkcd.GetValue("EnableVirtualizationBasedSecurity") == null)
                            {
                                VBS.IsOn = true;
                            }
                            else
                            {
                                VBS.IsOn = (hkcd.GetValue("EnableVirtualizationBasedSecurity").ToString() == "1");
                            }
                        }
                        VLF = false;
                    });
                });
            }
        }

        bool TLF = false;
        private void TSX_Toggled(object sender, RoutedEventArgs e)
        {
            logger.Info("切换TSX");
            if (StatusInitialized&&!TLF)
            {
                var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
                ActionHelper.SendMessageToSystemCore("4", (o, e) =>
                {
                    if (o == null)
                    {
                        TLF = true;
                    }
                    RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey hksk = hklm.OpenSubKey(@"System\CurrentControlSet\Control\Session Manager\kernel", false);
                    dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                    {
                        if (hksk.GetValue("DisableTsx") == null)
                        {
                            TSX.IsOn = false;
                        }
                        else
                        {
                            TSX.IsOn = !(hksk.GetValue("DisableTsx").ToString() == "1");
                        }
                        TLF= false;
                    });
                });
            }
        }

        bool TLLF = false;

        private void TimeLine_Toggled(object sender, RoutedEventArgs e)
        {
            logger.Info("切换时间线");
            if (StatusInitialized&&!TLLF)
            {
                var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
                ActionHelper.SendMessageToSystemCore("5", (o, e) =>
                {
                    if (o == null)
                    {
                        TLLF = true;
                    }
                    RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey hkws = hklm.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System", false);
                    dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                    {
                        if (hkws.GetValue("EnableActivityFeed") == null)
                        {
                            TimeLine.IsOn = true;
                        }
                        else
                        {
                            TimeLine.IsOn = (hkws.GetValue("EnableActivityFeed").ToString() == "1");
                        }
                        TLLF = false;
                    });
                });
            }
        }

        bool HLF  = false;
        private void HighQualityWallpaper_Toggled(object sender, RoutedEventArgs e)
        {
            logger.Info("切换壁纸清晰度");
            if (StatusInitialized&&!HLF)
            {
                var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
                ActionHelper.SendMessageToUserCore("4", (o, e) =>
                {
                    if (o == null)
                    {
                        HLF = true;
                    }
                    RegistryKey hkcu = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                    RegistryKey hkcd = hkcu.OpenSubKey(@"Control Panel\Desktop", false);
                    dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                    {
                        if (hkcd.GetValue("JPEGImportQuality") == null)
                        {
                            HighQualityWallpaper.IsOn = false;
                        }
                        else
                        {
                            HighQualityWallpaper.IsOn = (hkcd.GetValue("JPEGImportQuality").ToString() == "100");
                        }
                        HLF= false;
                    });
                });
            }
        }

        private void ResetScreenshotNumber_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("重置截图编号");
            ActionHelper.SendMessageToUserCore("3", (o, e) =>
            {

            });
        }

        private void RestartFileExpolerer_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("重启文件资源管理器");
            ActionHelper.SendMessageToSystemCore("6", (o, e) =>
            {

            });
        }

        private void SystemFix_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("执行系统修复");
            ActionHelper.LaunchSysAutoFix();
        }

        private void SystemAdvanceSettings_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("打开高级系统设置");
            try
            {
                Process P = new Process();
                P.StartInfo.UseShellExecute = true;
                P.StartInfo.Verb = "runas";
                P.StartInfo.FileName = @"C:\Windows\System32\SystemPropertiesAdvanced.exe";
                P.Start();
            }catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void Performance_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("打开性能选项");
            try
            {
                Process P = new Process();
                P.StartInfo.UseShellExecute = true;
                P.StartInfo.Verb = "runas";
                P.StartInfo.FileName = @"C:\Windows\System32\SystemPropertiesPerformance.exe";
                P.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void DesktopIcon_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("打开桌面图标管理");
            try
            {
                Process P = new Process();
                P.StartInfo.UseShellExecute = true;
                P.StartInfo.Verb = "runas";
                P.StartInfo.FileName = @"C:\Windows\System32\rundll32.exe";
                P.StartInfo.Arguments = "shell32.dll,Control_RunDLL desk.cpl,,0";
                P.Start();
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void QQClean_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("清理QQ");
            ActionHelper.SendMessageToUserCore("1", (o, e) =>
            {

            });
        }

        private void SystemClean_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("启动系统文件清理");
            Process.Start("C:\\windows\\System32\\cleanmgr.exe", "");
        }

        private void DriveAnalyzer_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("启动磁盘分析器");
            ActionHelper.LaunchSpaceSniffer();
        }

        private void DriveManagement_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("启动磁盘管理");
            try
            {
                Process P = new Process();
                P.StartInfo.UseShellExecute = true;
                P.StartInfo.Verb = "runas";
                P.StartInfo.FileName = @"C:\Windows\System32\diskmgmt.msc";
                P.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void HistoryPoint_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("打开还原点界面");
            try
            {
                Process P = new Process();
                P.StartInfo.UseShellExecute = true;
                P.StartInfo.Verb = "runas";
                P.StartInfo.FileName = @"C:\Windows\System32\SystemPropertiesProtection.exe";
                P.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void CCleaner_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("打开CCleaner");
            ActionHelper.LaunchCCleaner();
        }
        public static ThemeManager themeManager { get; private set; }

        private void FileExplorerCustomization_Click(object sender, RoutedEventArgs e)
        {
            logger.Info("打开文件资源管理器背景自定义界面");
            ElementTheme SettingsTheme = ElementTheme.Default;
            if (ApplicationConfig.GetSettings("Theme") != null)
            {
                if (ApplicationConfig.GetSettings("Theme") == "Light")
                {
                    SettingsTheme = ElementTheme.Light;
                }
                if (ApplicationConfig.GetSettings("Theme") == "Dark")
                {
                    SettingsTheme = ElementTheme.Dark;
                }
            }
            else
            {
                ApplicationConfig.SaveSettings("Theme", "Default");
            }
            ExplorerCustomizationWindow explorerCustomizationWindow = new ExplorerCustomizationWindow();
            try
            {
                themeManager = ThemeManager.GetCurrent()
                                            .UseWindow(explorerCustomizationWindow)
                                            .UseThemeOptions(new ThemeOptions
                                            {
                                                BackdropType = BackdropType.DesktopAcrylic,
                                                ElementTheme = SettingsTheme,
                                                ForceBackdrop = true,
                                                ForceElementTheme = true,
                                                UseBuiltInSettings = true,
                                                TitleBarCustomization = new TitleBarCustomization
                                                {
                                                    TitleBarType = TitleBarType.AppWindow
                                                }
                                            })
                                            .Build();
            }
            catch (Exception)
            {

            }
            //To Be Fixed
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(explorerCustomizationWindow);
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            if (appWindow is not null)
            {
                Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                if (displayArea is not null)
                {
                    var CenteredPosition = appWindow.Position;
                    CenteredPosition.X = ((displayArea.WorkArea.Width - appWindow.Size.Width) / 2);
                    CenteredPosition.Y = ((displayArea.WorkArea.Height - appWindow.Size.Height) / 2);
                    appWindow.Move(CenteredPosition);
                }
            }
            explorerCustomizationWindow.Activate();
        }
    }
}
