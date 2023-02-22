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
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {

        public string Version = string.Format("版本：{0}.{1}.{2}.{3}",
                        Package.Current.Id.Version.Major,
                        Package.Current.Id.Version.Minor,
                        Package.Current.Id.Version.Build,
                        Package.Current.Id.Version.Revision);
        public AboutPage()
        {
            this.InitializeComponent();
        }

        private void HyperlinkButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText("https://github.com/IShiraiKurokoI/DLUTToolBoxV3");
            Clipboard.SetContent(dataPackage);
            var builder = new AppNotificationBuilder()
                .AddText("复制成功！");
            var notificationManager = AppNotificationManager.Default;
            notificationManager.Show(builder.BuildNotification());
        }

        private void SettingsCard_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {

            Windows.System.Launcher.LaunchUriAsync(new System.Uri("https://github.com/IShiraiKurokoI/DLUTToolBoxV3"));
        }
    }
}
