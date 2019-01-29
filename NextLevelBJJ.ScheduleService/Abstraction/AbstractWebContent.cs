using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.WebContentServices.Abstraction
{
    public abstract class AbstractWebContent
    {
        public HtmlDocument HtmlDocument { get; set; }

        public AbstractWebContent(string url)
        {
            var web = new HtmlWeb();

            HtmlDocument = web.LoadFromWebAsync(url).Result;
        }
    }
}
