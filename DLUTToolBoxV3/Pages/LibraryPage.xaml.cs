// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using DLUTToolBoxV3.Entities;
using DLUTToolBoxV3.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "万方查重", "ms-appx:///Assets/AppIcons/Library/WFCC.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b657940246e1d1011fa1add59ab42fc399fbc81268a6c4637b8b16a7834/?filter=app&from=rj", 0));
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
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书馆入馆培训", "ms-appx:///Assets/AppIcons/Library/LibraryPreEducation.png", "", "https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940246c0a0311f24d9f47a802fe3484afcb22ec20bb291d/?filter=app&from=rj", 0));
        }

        private void BookLent_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "图书借阅", "ms-appx:///Assets/AppIcons/Library/BookLent.png", "", "https://sso.dlut.edu.cn/cas/login?service=https%3A%2F%2Fopac.lib.dlut.edu.cn%3A443%2Fmeta-local%2Fopac%2Fcas%2Frosetta&from=rj", 0));
        }

        private void resource_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "资源预约", "ms-appx:///Assets/AppIcons/Library/Res.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b657940247f1f1801b2079f5bbe02ff3c84f58629/InteAuth/Account/IntegratedAuth", 0));
        }
        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "毕业生档案服务", "ms-appx:///Assets/AppIcons/Library/Profile.png", "", "https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940247c190856f80f865ae449fe2ddfb88b/jsp/archivesweb/custom/dlut/sso/index.jsp?filter=app", 0));
        }
    }
}
