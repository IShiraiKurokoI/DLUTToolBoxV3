// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.UserControls
{
    public sealed partial class AppButton : Button
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string ImagePath { get; set; }
        public int ButtonWidth { get; set; }
        public AppButton()
        {
            this.InitializeComponent();
        }


    }
}
