// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Windowing;
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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinUICommunity;
using System.Diagnostics;
using DLUTToolBoxV3.Configurations;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Text.RegularExpressions;
using DLUTToolBoxV3.Helpers;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExplorerCustomizationWindow : Window
    {
        public NLog.Logger logger;
        private AppWindow m_AppWindow;

        [DllImport("user32.dll")]
        static extern int GetDpiForWindow(IntPtr hwnd);
        public ExplorerCustomizationWindow()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            this.InitializeComponent();
            int dpi = GetDpiForWindow(WindowHelper.GetWindowHandleForCurrentWindow(this));
            m_AppWindow = this.AppWindow;
            m_AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            m_AppWindow.Resize(new SizeInt32((int)(1570 * (double)((double)dpi / (double)120)), (int)(900 * (double)((double)dpi / (double)120))));
            m_AppWindow.SetIcon("ms-appx:///Assets/logo.ico");
            this.Title = "DLUTToolBoxV3-文件资源管理器自定义";
            SetTitleBar(AppTitleBar);
            OverlappedPresenter overlappedPresenter = AppWindow.Presenter as OverlappedPresenter ?? Microsoft.UI.Windowing.OverlappedPresenter.Create();
            overlappedPresenter.IsResizable = false;
            logger.Info("文件浏览器自定义窗口激活成功");
        }

        private async void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            var hwnd = WindowHelper.GetWindowHandleForCurrentWindow(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".png");
            picker.CommitButtonText = "选择图片";
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string filePath = file.Path;
                // 使用 filePath 变量进行后续处理
                ApplicationConfig.SaveSettings("str1", filePath);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(filePath);
                xtb.Source = bitmapImage;
            }
        }

        private async void Grid_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            var hwnd = WindowHelper.GetWindowHandleForCurrentWindow(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".png");
            picker.CommitButtonText = "选择图片";
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string filePath = file.Path;
                ApplicationConfig.SaveSettings("str2", filePath);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(filePath);
                lb.Source = bitmapImage;
            }
        }

        private async void Grid_PointerPressed_2(object sender, PointerRoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            var hwnd = WindowHelper.GetWindowHandleForCurrentWindow(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".png");
            picker.CommitButtonText = "选择图片";
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string filePath = file.Path;
                ApplicationConfig.SaveSettings("str3", filePath);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(filePath);
                pp.Source = bitmapImage;
            }
        }

        private async void Grid_PointerPressed_3(object sender, PointerRoutedEventArgs e)
        {

            FileOpenPicker picker = new FileOpenPicker();
            var hwnd = WindowHelper.GetWindowHandleForCurrentWindow(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".png");
            picker.CommitButtonText = "选择图片";
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string filePath = file.Path;
                ApplicationConfig.SaveSettings("str4", filePath);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(filePath);
                zdtb.Source = bitmapImage;
            }
        }

        private async void Grid_PointerPressed_4(object sender, PointerRoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            var hwnd = WindowHelper.GetWindowHandleForCurrentWindow(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".png");
            picker.CommitButtonText = "选择图片";
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string filePath = file.Path;
                ApplicationConfig.SaveSettings("str5", filePath);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(filePath);
                xxst.Source = bitmapImage;
            }
        }

        private async void Grid_PointerPressed_5(object sender, PointerRoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            var hwnd = WindowHelper.GetWindowHandleForCurrentWindow(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
            picker.FileTypeFilter.Clear();
            picker.FileTypeFilter.Add(".png");
            picker.CommitButtonText = "选择图片";
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                string filePath = file.Path;
                ApplicationConfig.SaveSettings("str6", filePath);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(filePath);
                nrst.Source = bitmapImage;
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if(ApplicationConfig.GetSettings("str1")!=null && File.Exists(ApplicationConfig.GetSettings("str1")))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(ApplicationConfig.GetSettings("str1"));
                xtb.Source = bitmapImage;
            }
            if(ApplicationConfig.GetSettings("str2")!=null && File.Exists(ApplicationConfig.GetSettings("str2")))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(ApplicationConfig.GetSettings("str2"));
                lb.Source = bitmapImage;
            }
            if(ApplicationConfig.GetSettings("str3")!=null && File.Exists(ApplicationConfig.GetSettings("str3")))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(ApplicationConfig.GetSettings("str3"));
                pp.Source = bitmapImage;
            }
            if(ApplicationConfig.GetSettings("str4")!=null && File.Exists(ApplicationConfig.GetSettings("str4")))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(ApplicationConfig.GetSettings("str4"));
                zdtb.Source = bitmapImage;
            }
            if(ApplicationConfig.GetSettings("str5")!=null && File.Exists(ApplicationConfig.GetSettings("str5")))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(ApplicationConfig.GetSettings("str5"));
                xxst.Source = bitmapImage;
            }
            if(ApplicationConfig.GetSettings("str6")!=null && File.Exists(ApplicationConfig.GetSettings("str6")))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(ApplicationConfig.GetSettings("str6"));
                nrst.Source = bitmapImage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string directory = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            Directory.CreateDirectory(directory + "\\VisualCore\\");
            FileStream fs = new FileStream(directory + "\\VisualCore\\Background.config", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(ApplicationConfig.GetSettings("str1"));
            sw.WriteLine(ApplicationConfig.GetSettings("str2"));
            sw.WriteLine(ApplicationConfig.GetSettings("str3"));
            sw.WriteLine(ApplicationConfig.GetSettings("str4"));
            sw.WriteLine(ApplicationConfig.GetSettings("str5"));
            sw.WriteLine(ApplicationConfig.GetSettings("str6"));
            sw.Close();
            ActionHelper.SendMessageToVisualCore(directory + "\\VisualCore\\Background.config", (o, e) =>
            {

            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ActionHelper.SendMessageToVisualCore("reset", (o, e) =>
            {

            });
        }
    }
}
