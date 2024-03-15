// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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
using Windows.Networking.Connectivity;
using DLUTToolBoxV3.Pages;
using DLUTToolBoxV3.Configurations;
using Castle.Core.Internal;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using WinUICommunity;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public static ShellPage Instance { get; private set; }
        public NavigationManager navigationManager { get; set; }
        public ShellPage()
        {
            this.InitializeComponent();
            navigationManager = new NavigationManager(navigationView, new NavigationViewOptions
            {
                DefaultPage = typeof(GeneralPage)
            }, shellFrame);

            if(String.IsNullOrEmpty(ApplicationConfig.GetSettings("Uid")) || String.IsNullOrEmpty(ApplicationConfig.GetSettings("Password")))
            {
                var builder = new AppNotificationBuilder()
                    .AddText("请先在参数配置界面设置学工号和统一认证密码!\n设置完成后⚠重启本程序⚠方可正常使用所有功能！");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                shellFrame.Navigate(typeof(SettingsPage));
            }
        }

        private void autoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            List<string> list3 = new List<string>();
            List<NavigationViewItem> source = navigationView.MenuItems.OfType<NavigationViewItem>().ToList();
            string[] querySplit = autoSuggestBox.Text.Split(' ');
            foreach (NavigationViewItem item4 in source.Where(delegate (NavigationViewItem item)
            {
                bool result = true;
                string[] array = querySplit;
                foreach (string value in array)
                {
                    if (item.Content.ToString()!.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) < 0)
                    {
                        result = false;
                    }
                }

                return result;
            }))
            {
                list3.Add(item4.Content.ToString());
            }

            if (list3.Count > 0)
            {
                autoSuggestBox.ItemsSource = list3;
                return;
            }

            autoSuggestBox.ItemsSource = new string[1] { "没有匹配的结果" };
        }

        private void autoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion == null)
            {
                return;
            }
            string item = args.ChosenSuggestion as string;
            navigationView.SelectedItem = navigationView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault((NavigationViewItem menuItem) => (string)menuItem.Content == item);
            Type pageType = ((NavigationViewItem)navigationView.SelectedItem).GetValue(NavHelper.NavigateToProperty) as Type;
            shellFrame.Navigate(pageType);
        }
    }
}
