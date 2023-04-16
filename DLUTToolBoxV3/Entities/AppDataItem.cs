using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLUTToolBoxV3.Entities
{
    public class AppDataItem
    {
        public AppDataItem(string uniqueId, string title, string imagePath, string description, string uri,int handleId)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Uri = new Uri(uri);
            this.HandleId = handleId;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public Uri Uri { get; private set; }
        public int HandleId { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
