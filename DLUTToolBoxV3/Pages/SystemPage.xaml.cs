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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SystemPage : Page
    {
        public SystemPage()
        {
            this.InitializeComponent();
        }

        bool StatusInitialized = false;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey hkrm = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\ReserveManager", false);
            ReservedSpace.IsOn = (hkrm.GetValue("ShippedWithReserves").ToString() == "1");

            RegistryKey hkmm = hklm.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management", false);
            if (hkmm.GetValue("FeatureSettingsOverride") == null)
            {
                MeltDownPatch.IsOn = true;
            }
            else
            {
                MeltDownPatch.IsOn = (hkmm.GetValue("FeatureSettingsOverride").ToString() == "0");
            }

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

            RegistryKey hksk = hklm.OpenSubKey(@"System\CurrentControlSet\Control\Session Manager\kernel", false);
            if (hksk.GetValue("DisableTsx") == null)
            {
                TSX.IsOn = false;
            }
            else
            {
                TSX.IsOn = !(hksk.GetValue("DisableTsx").ToString() == "1");
            }

            RegistryKey hkws = hklm.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System", false);
            if (hkws.GetValue("EnableActivityFeed") == null)
            {
                TimeLine.IsOn = true;
            }
            else
            {
                TimeLine.IsOn = (hkws.GetValue("EnableActivityFeed").ToString() == "1");
            }

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
            StatusInitialized = true;
        }
        bool RLF = false;
        private void ReservedSpace_Toggled(object sender, RoutedEventArgs e)
        {
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
            ActionHelper.SendMessageToUserCore("3", (o, e) =>
            {

            });
        }

        private void RestartFileExpolerer_Click(object sender, RoutedEventArgs e)
        {
            ActionHelper.SendMessageToSystemCore("6", (o, e) =>
            {

            });
        }

        private void SystemFix_Click(object sender, RoutedEventArgs e)
        {
            ActionHelper.LaunchSysAutoFix();
        }

        private void SystemAdvanceSettings_Click(object sender, RoutedEventArgs e)
        {
            Process P = new Process();
            P.StartInfo.UseShellExecute = true;
            P.StartInfo.Verb = "runas";
            P.StartInfo.FileName = @"C:\Windows\System32\SystemPropertiesAdvanced.exe";
            P.Start();
        }

        private void Performance_Click(object sender, RoutedEventArgs e)
        {
            Process P = new Process();
            P.StartInfo.UseShellExecute = true;
            P.StartInfo.Verb = "runas";
            P.StartInfo.FileName = @"C:\Windows\System32\SystemPropertiesPerformance.exe";
            P.Start();
        }

        private void DesktopIcon_Click(object sender, RoutedEventArgs e)
        {
            Process P = new Process();
            P.StartInfo.UseShellExecute = true;
            P.StartInfo.Verb = "runas";
            P.StartInfo.FileName = @"C:\Windows\System32\rundll32.exe";
            P.StartInfo.Arguments = "shell32.dll,Control_RunDLL desk.cpl,,0";
            P.Start();
        }

        private void QQClean_Click(object sender, RoutedEventArgs e)
        {
            ActionHelper.SendMessageToUserCore("1", (o, e) =>
            {

            });
        }

        private void SystemClean_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("C:\\windows\\System32\\cleanmgr.exe", "");
        }

        private void DriveAnalyzer_Click(object sender, RoutedEventArgs e)
        {
            ActionHelper.LaunchSpaceSniffer();
        }

        private void DriveManagement_Click(object sender, RoutedEventArgs e)
        {
            Process P = new Process();
            P.StartInfo.UseShellExecute = true;
            P.StartInfo.Verb = "runas";
            P.StartInfo.FileName = @"C:\Windows\System32\diskmgmt.msc";
            P.Start();
        }

        private void HistoryPoint_Click(object sender, RoutedEventArgs e)
        {
            Process P = new Process();
            P.StartInfo.UseShellExecute = true;
            P.StartInfo.Verb = "runas";
            P.StartInfo.FileName = @"C:\Windows\System32\SystemPropertiesProtection.exe";
            P.Start();
        }

        private void CCleaner_Click(object sender, RoutedEventArgs e)
        {
            ActionHelper.LaunchCCleaner();
        }

        private void FileExplorerCustomization_Click(object sender, RoutedEventArgs e)
        {
            var builder = new AppNotificationBuilder()
                .AddText("…–Œ¥ µœ÷£°");
            var notificationManager = AppNotificationManager.Default;
            notificationManager.Show(builder.BuildNotification());
        }
    }
}
