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
    public sealed partial class ExamPage : Page
    {
        public NLog.Logger logger;
        public ExamPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("�򿪿��Խ���ҳ��");
            this.InitializeComponent();
        }

        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "������ҳ", "ms-appx:///Assets/AppIcons/Exam/JXGL.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402472000514b2079f5bbe02ff3c84f58629/student/ucas-sso/login?filter=app&from=rj", 0));
        }

        private void SchoolCalender_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "ѧУУ��", "ms-appx:///Assets/AppIcons/Exam/SchoolCalender.png", "", "https://api.m.dlut.edu.cn/login?client_id=9qXqHnRQuhhViycC&redirect_uri=https%3a%2f%2flightapp.m.dlut.edu.cn%2fcheck%2fschcalendar&response_type=code&scope=base_api&state=dlut", 0));
        }

        private void CourseSelect_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "ѡ��ϵͳ", "ms-appx:///Assets/AppIcons/Exam/CourseSelect.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402472000514b2079f5bbe02ff3c84f58629/student/ucas-sso/login?filter=app&from=rj", 3));
        }

        private void CourseEvaluate_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "����ϵͳ", "ms-appx:///Assets/AppIcons/Exam/CourseEvaluate.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402472000514b2079f5bbe02ff3c84f58629/student/ucas-sso/login?filter=app&from=rj", 4));
        }

        private void ExamArrangement_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "���԰���", "ms-appx:///Assets/AppIcons/Exam/ExamArrangement.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402472000514b2079f5bbe02ff3c84f58629/student/ucas-sso/login?filter=app&from=rj", 5));
        }

        private void ExamDelay_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "����ϵͳ", "ms-appx:///Assets/AppIcons/Exam/ExamDelay.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402472000514b2079f5bbe02ff3c84f58629/student/ucas-sso/login?filter=app&from=rj", 6));
        }

        private void Score_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "�ɼ���Ϣ", "ms-appx:///Assets/AppIcons/Exam/Score.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402472000514b2079f5bbe02ff3c84f58629/student/ucas-sso/login?filter=app&from=rj", 7));
        }

        private void CourseSelectSwitch_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "��ͻѡ������", "ms-appx:///Assets/AppIcons/Exam/CourseSelectSwitch.png", "", "https://webvpn.dlut.edu.cn/http/57787a7876706e323032336b6579402472000514b2079f5bbe02ff3c84f58629/student/ucas-sso/login?filter=app&from=rj", 8));
        }
    }
}
