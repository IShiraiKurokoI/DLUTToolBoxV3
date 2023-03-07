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
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "万方查重", "ms-appx:///Assets/AppIcons/Library/WFCC.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421a1af13d27e6226022e5dc7fecc01/dljc/?filter=app&from=rj", 0));
        }

        private void WFSS_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "万方搜索", "ms-appx:///Assets/AppIcons/Library/WFSS.png", "", "https://webvpn.dlut.edu.cn/https/77726476706e69737468656265737421f7b9569d2936695e790c88b8991b203a18454272/index.html?filter=app&from=rj", 0));
        }

        private void ZWSS_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "知网搜索", "ms-appx:///Assets/AppIcons/Library/ZWSS.png", "", "https://webvpn.dlut.edu.cn/https/77726476706e69737468656265737421f5e7549e6933665b774687a98c/kns/brief/result.aspx?filter=app&from=rj", 0));
        }

        private void Resources_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书馆资源列表", "ms-appx:///Assets/AppIcons/Library/Resources.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421e7e056d22b396a1e7a049cb8d65027204e7199/sjkdhejlbyAtoZ/ALL.htm?filter=app&from=rj", 0));
        }

        private void LibraryPreEducation_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书馆入馆培训", "ms-appx:///Assets/AppIcons/Library/LibraryPreEducation.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421f7e24898693c6152300c85b98c1b2631a4d2be57/site/login.html?filter=app&from=rj", 0));
        }

        private void BookLent_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书借阅", "ms-appx:///Assets/AppIcons/Library/BookLent.png", "", "https://api.m.dlut.edu.cn/login?client_id=9qXqHnRQuhhViycC&redirect_uri=https%3a%2f%2flightapp.m.dlut.edu.cn%2fcheck%2flibrary&response_type=code&scope=base_api&state=dlut", 0));
        }
    }
}
