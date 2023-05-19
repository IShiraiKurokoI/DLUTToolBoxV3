// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using WinUICommunity;
using NLog;
using System.Diagnostics;
using DLUTToolBoxV3.Helpers;
using System.Runtime.InteropServices;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public NLog.Logger logger;
        private AppWindow m_AppWindow;

        [DllImport("user32.dll")]
        static extern int GetDpiForWindow(IntPtr hwnd);

        public MainWindow()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            this.InitializeComponent();
            int dpi = GetDpiForWindow(WindowHelper.GetWindowHandleForCurrentWindow(this));
            m_AppWindow = this.AppWindow;
            m_AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            m_AppWindow.Resize(new SizeInt32((int)(1570 * (double)((double)dpi / (double)120)), (int)(800 * (double)((double)dpi / (double)120))));
            m_AppWindow.SetIcon("ms-appx:///Assets/logo.ico");
            this.Title = "DLUTToolBoxV3";
            SetTitleBar(AppTitleBar);
            OverlappedPresenter overlappedPresenter = AppWindow.Presenter as OverlappedPresenter ?? Microsoft.UI.Windowing.OverlappedPresenter.Create();
            overlappedPresenter.IsResizable = false;
            logger.Info("主窗口激活成功");
        }
    }
}
