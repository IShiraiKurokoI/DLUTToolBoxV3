using Microsoft.UI.Xaml.Controls;

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
