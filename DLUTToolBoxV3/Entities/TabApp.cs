using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLUTToolBoxV3.Entities
{
    public class TabApp
    {
        public AppDataItem appDataItem;
        public TabViewItem tabViewItem;
        public TabApp(AppDataItem appDataItem,TabViewItem tabViewItem) {
            this.appDataItem = appDataItem;
            this.tabViewItem = tabViewItem;
        }
    }
}
