// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public NLog.Logger logger;

        public string Version = string.Format("版本：{0}.{1}.{2}.{3}",
                        Package.Current.Id.Version.Major,
                        Package.Current.Id.Version.Minor,
                        Package.Current.Id.Version.Build,
                        Package.Current.Id.Version.Revision);
        public AboutPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开关于软件页面");
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
