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
    public sealed partial class StudyPage : Page
    {
        public NLog.Logger logger;
        public StudyPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开学习生活页面");
            this.InitializeComponent();
        }

        private void WeekTimeTable_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "本周课表", "ms-appx:///Assets/AppIcons/Study/WeekTimeTable.png", "", "https://api.m.dlut.edu.cn/login?client_id=9qXqHnRQuhhViycC&redirect_uri=https%3a%2f%2flightapp.m.dlut.edu.cn%2fcheck%2fcourseschedule&response_type=code", -1));
        }

        private void MyTimeTable_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "我的课表", "ms-appx:///Assets/AppIcons/Study/MyTimeTable.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421faef4690693464456a468ca88d1b203b/student/ucas-sso/login?filter=app&from=rj", 9));
        }

        private void ClassTimeTable_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "班级课表", "ms-appx:///Assets/AppIcons/Study/ClassTimeTable.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421faef4690693464456a468ca88d1b203b/student/ucas-sso/login?filter=app&from=rj", 10));
        }

        private void ProgramCompletion_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "培养方案", "ms-appx:///Assets/AppIcons/Study/ProgramCompletion.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421faef4690693464456a468ca88d1b203b/student/ucas-sso/login?filter=app&from=rj", 11));
        }

        private void PublicNotice_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "公共通知", "ms-appx:///Assets/AppIcons/Study/PublicNotice.png", "", "https://api.m.dlut.edu.cn/login?client_id=9qXqHnRQuhhViycC&redirect_uri=https%3a%2f%2flightapp.m.dlut.edu.cn%2fcheck%2fnotice&response_type=code&scope=base_api&state=dlut", 0));
        }

        private void CourseSearch_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "开课查询", "ms-appx:///Assets/AppIcons/Study/CourseSearch.png", "", "https://webvpn.dlut.edu.cn/http/77726476706e69737468656265737421faef4690693464456a468ca88d1b203b/student/ucas-sso/login?filter=app&from=rj", 12));
        }

        private void SpareClassroom_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "空闲教室", "ms-appx:///Assets/AppIcons/Study/SpareClassroom.png", "", "https://api.m.dlut.edu.cn/login?client_id=9qXqHnRQuhhViycC&redirect_uri=https%3a%2f%2flightapp.m.dlut.edu.cn%2fcheck%2femptyclassroom&response_type=code&scope=base_api&state=dlut", 0));
        }

        private void LeaveSchool_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "校园进出及请销假管理", "ms-appx:///Assets/AppIcons/Study/LeaveSchool.png", "", "https://sso.dlut.edu.cn/cas/login?service=https%3A%2F%2Fehall.dlut.edu.cn%2Ffp%2Fview%3Fm%3Dfp#act=fp/formProcess/graduate_student", 0));
        }
    }
}
