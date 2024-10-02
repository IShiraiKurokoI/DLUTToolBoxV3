// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using DLUTToolBoxV3.Entities;
using DLUTToolBoxV3.Helpers;
using DLUTToolBoxV3.Ultilities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChargePage : Page
    {
        public NLog.Logger logger;
        public ChargePage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开日常缴费页面");
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadElectricityInfo();
        }

        public void LoadElectricityInfo()
        {
            logger.Info("加载电费信息");
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            Task.Run(() =>
            {
                InfoUltilities.GetElectricityInfo(ElectrcityInfo, dispatcherQueue);
            });
        }

        private void ElectricityCharge_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "电费充值", "ms-appx:///Assets/AppIcons/Charge/Pay.png", "", "https://sso.dlut.edu.cn/cas/login?from=rj&service=https%3A%2F%2Fsso.dlut.edu.cn%2Fcas%2Flogin%3Fservice%3Dhttps%253A%252F%252Fehall.dlut.edu.cn%252Ffp%252FvisitService%253Fservice_id%253Dca2b52e6-1145-4b63-9ea9-e443b376da0d", 0));
        }

        private void Ecard_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "玉兰卡", "ms-appx:///Assets/AppIcons/Charge/Ecard.png", "", "about:blank", 2));
        }

        private void ElectricityStatusRefresh_Click(object sender, RoutedEventArgs e)
        {
            ElectrcityInfo.Message = "正在加载信息。。。";
            LoadElectricityInfo();
        }
    }
}
