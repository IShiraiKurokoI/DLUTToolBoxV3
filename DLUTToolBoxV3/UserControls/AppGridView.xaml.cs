// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using DLUTToolBoxV3.Entities;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.UserControls
{
    public sealed partial class AppGridView : UserControl
    {
        public ObservableCollection<AppDataItem> Items { get; set; }
        public AppGridView()
        {
            this.InitializeComponent();
        }
    }
}
