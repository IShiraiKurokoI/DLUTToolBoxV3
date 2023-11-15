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
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "大工网盘", "ms-appx:///Assets/AppIcons/Other/CloudDisk.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402468190c56f80f865ae449fe2ddfb88b/cas?filter=app&from=rj", 0));
        }

        private void Mail_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "大工邮箱", "ms-appx:///Assets/AppIcons/Other/Mail.png", "", "https://mail.dlut.edu.cn/", 14));
        }

        private void XGXT_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "学工系统", "ms-appx:///Assets/AppIcons/Other/XGXT.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b657940247c0d160bfd4d9742bf58b43d95aecb2487/cas?filter=app&from=rj", 0));
        }

        private void YJS_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "研究生系统", "ms-appx:///Assets/AppIcons/Other/YJS.png", "", "https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940247c0d161fef4d9742bf58b43d95aecb24b7/pyxx/LoginCAS.aspx?a=1&filter=app&from=rj", 0));
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "统一缴费平台", "ms-appx:///Assets/AppIcons/Other/Pay.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b657940247d1b030af8139257a749f42cdfbf89322e5e910d7a339f3b/?filter=app&from=rj", 0));
        }

        private void NetHelper_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "网络自助系统", "ms-appx:///Assets/AppIcons/Other/NetHelper.png", "", "https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940246817100cfd0fdd4aa659ee7694bf9069de28/sso/sso_zzxt.jsp?filter=app&from=rj", 0));
        }

        private void Market_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "跳蚤市场", "ms-appx:///Assets/AppIcons/Other/Pay.png", "", "https://res.dlut.edu.cn/tp_cgp/view?m=cgp", -2));
        }

        private void TeenHome_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "青年之家", "ms-appx:///Assets/AppIcons/Other/TeenHome.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b657940247a1a1149b2079f5bbe02ff3c84f58629/?filter=app&from=rj", 0));
        }

        private void DUTer_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "DUTer建言", "ms-appx:///Assets/AppIcons/Other/Duter.png", "", "https://ehall.dlut.edu.cn/fp/s/QZMKMV?from=rj", 0));
        }

        private void Mooc_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "大工慕课", "ms-appx:///Assets/AppIcons/Other/Mooc.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402471150d17ff4d9742bf58b43d95aecb249a/sso/dlut?filter=app&from=rj", 0));
        }

        private void Advice_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "大工就业网", "ms-appx:///Assets/AppIcons/Other/Advice.png", "", "https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2Fjob.dlut.edu.cn%2FautoLogin", 500));
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "自助打印系统", "ms-appx:///Assets/AppIcons/Other/Print.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b657940247d081017f305dd4aa659ee7694bf90694722/sso/login.jsp?filter=app&from=rj", 0));
        }
    }
}
