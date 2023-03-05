// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

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

        }

        private void Mail_Click(object sender, RoutedEventArgs e)
        {

        }

        private void XGXT_Click(object sender, RoutedEventArgs e)
        {

        }

        private void YJS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NetHelper_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Market_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TeenHome_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DUTer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Mooc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Advice_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
