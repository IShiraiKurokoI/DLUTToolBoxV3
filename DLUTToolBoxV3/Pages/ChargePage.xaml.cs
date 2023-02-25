// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using System.Security.Cryptography;
using System.Text;
using System;
using Windows.Foundation.Metadata;
using Castle.Core.Configuration;
using System.Threading.Tasks;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using Castle.Core.Internal;
using System.Diagnostics;
using WinUICommunity.Common.Extensions;
using Windows.ApplicationModel.DataTransfer;
using DLUTToolBoxV3.Configurations;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChargePage : Page
    {
        public ChargePage()
        {
            this.InitializeComponent();
        }
    }
}
