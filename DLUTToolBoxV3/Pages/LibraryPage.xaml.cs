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

        }

        private void WFCC_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WFSS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ZWSS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Resources_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LibraryPreEducation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BookLent_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
