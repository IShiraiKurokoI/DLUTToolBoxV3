// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using DLUTToolBoxV3.Entities;
using DLUTToolBoxV3.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OtherPage : Page
    {
        public NLog.Logger logger;
        public OtherPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开其他系统页面");
            this.InitializeComponent();
        }

        private void CloudDisk_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "大工网盘", "ms-appx:///Assets/AppIcons/Other/CloudDisk.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421e0f64fd2233c7d44300d8db9d6562d/cas?filter=app&from=rj", 0));
        }

        private void Mail_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "大工邮箱", "ms-appx:///Assets/AppIcons/Other/Mail.png", "", "https://mail.dlut.edu.cn/", 14));
        }

        private void XGXT_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "学工系统", "ms-appx:///Assets/AppIcons/Other/XGXT.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421f4e2558f267e6c5c6b1cc7a99c406d36b7/cas?filter=app&from=rj", 0));
        }

        private void YJS_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "研究生系统", "ms-appx:///Assets/AppIcons/Other/YJS.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421a2a713d27661301e285dc7fdca06/pyxx/LoginCAS.aspx?a=1&filter=app&from=rj", 0));
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "统一缴费平台", "ms-appx:///Assets/AppIcons/Other/Pay.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421f5f4408e23206949730d87b8d6512f209640763a21f75b0c/?filter=app&from=rj", 0));
        }

        private void NetHelper_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "网络自助系统", "ms-appx:///Assets/AppIcons/Other/NetHelper.png", "", "https://webvpn.dlut.edu.cn/https/77726476706e69737468656265737421e0f85388263c2654721d9de29d51367b3449/sso/sso_zzxt.jsp?filter=app&from=rj", 0));
        }

        private void Market_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "跳蚤市场", "ms-appx:///Assets/AppIcons/Other/Pay.png", "", "https://res.dlut.edu.cn/tp_cgp/view?m=cgp", -2));
        }

        private void TeenHome_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "跳蚤市场", "ms-appx:///Assets/AppIcons/Other/TeenHome.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421f2f552cd693464456a468ca88d1b203b/?filter=app&from=rj", 0));
        }

        private void DUTer_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "DUTer之家", "ms-appx:///Assets/AppIcons/Other/Duter.png", "", "https://ehall.dlut.edu.cn/fp/s/QZMKMV?from=rj", 0));
        }

        private void Mooc_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "大工慕课", "ms-appx:///Assets/AppIcons/Other/Mooc.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421f9fa4e93247e6c5c6b1cc7a99c406d3642/sso/dlut?filter=app&from=rj", 0));
        }

        private void Advice_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "网上意见箱", "ms-appx:///Assets/AppIcons/Other/Advice.png", "", "https://sso.dlut.edu.cn/cas/login?from=rj&service=https%3A%2F%2Fsso.dlut.edu.cn%2Fcas%2Flogin%3Fservice%3Dhttps%253A%252F%252Fehall.dlut.edu.cn%252Ffp%252Fview%253Fm%253Dfp%2523from%253Dhall%2526serveID%253D0d9c4d08-6828-4507-80b8-f989d3a1904c%2526act%253Dfp%252Fserveapply", 0));
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "自助打印系统", "ms-appx:///Assets/AppIcons/Other/Print.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421f5e7539328362654721d9de29d51367bd857/sso/login.jsp?filter=app&from=rj", 0));
        }
    }
}
