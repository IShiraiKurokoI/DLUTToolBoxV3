using DLUTToolBoxV3.Pages;
using System;
using System.Collections.Generic;
using WinUICommunity;

namespace DLUTToolBoxV3
{
    public class ShellPageService : PageServiceEx
    {
        public ShellPageService()
        {
            _pageKeyToTypeMap = new Dictionary<string, Type>
            {
                { "GeneralPage", typeof(GeneralPage) },
                { "NetworkPage", typeof(NetworkPage) },
                { "ChargePage", typeof(ChargePage) },
                { "ExamPage", typeof(ExamPage) },
                { "StudyPage", typeof(StudyPage) },
                { "EhallPage", typeof(EhallPage) },
                { "LibraryPage", typeof(LibraryPage) },
                { "OtherPage", typeof(OtherPage) },
                { "SystemPage", typeof(SystemPage) },
                { "SettingsPage", typeof(SettingsPage) },
                { "AboutPage", typeof(AboutPage) },
            };
        }
    }
}
