using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.WebContentServices.Abstraction
{
    public interface IWebHtmlLoadHelper
    {
        HtmlDocument LoadContentFromUrl(string url);
    }
}
