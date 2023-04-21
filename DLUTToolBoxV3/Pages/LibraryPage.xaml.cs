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
    public sealed partial class LibraryPage : Page
    {
        public NLog.Logger logger;
        public LibraryPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开图书馆页面");
            this.InitializeComponent();
        }

        private void SeatReserve_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书馆座位预约", "ms-appx:///Assets/AppIcons/Library/SeatReserve.png", "", "https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2Fseat.lib.dlut.edu.cn%2Fyanxiujian%2Fclient%2Flogin.php%3Fredirect%3DareaSelectSeat.php", 13));
        }

        private void WFCC_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "万方查重", "ms-appx:///Assets/AppIcons/Library/WFCC.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402429405056a551dd1cfa19b46ac5ef/dljc/?filter=app&from=rj", 0));
        }

        private void WFSS_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "万方搜索", "ms-appx:///Assets/AppIcons/Library/WFSS.png", "", "https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940247f561519f2059240ad48fb2c90f5862810867b93/index.html?filter=app&from=rj", 0));
        }

        private void ZWSS_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "知网搜索", "ms-appx:///Assets/AppIcons/Library/ZWSS.png", "", "https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940246f0f1556ff0d9847e442ff2c/?filter=app&from=rj", 0));
        }

        private void Resources_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书馆系统", "ms-appx:///Assets/AppIcons/Library/Resources.png", "", "https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940247708031bb20f9a4ce448f62d85f580230796e3c6/meta-local/opac/cas/rosetta?filter=app&from=rj", 0));
        }

        private void LibraryPreEducation_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书馆入馆培训", "ms-appx:///Assets/AppIcons/Library/LibraryPreEducation.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b657940247f0d0b1cb20f9a4ce448f62d85f5802345904e3a/site/login.html?filter=app&from=rj", 0));
        }

        private void BookLent_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书借阅", "ms-appx:///Assets/AppIcons/Library/BookLent.png", "", "https://api.m.dlut.edu.cn/login?client_id=9qXqHnRQuhhViycC&redirect_uri=https%3a%2f%2flightapp.m.dlut.edu.cn%2fcheck%2flibrary&response_type=code&scope=base_api&state=dlut", 0));
        }
    }
}
