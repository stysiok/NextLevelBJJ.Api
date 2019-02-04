using HtmlAgilityPack;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;

namespace NextLevelBJJ.WebContentServices.Helpers
{
    internal class WebHtmlLoadHelper : IWebHtmlLoadHelper
    {
        public HtmlDocument LoadContentFromUrl(string url)
        {
            var web = new HtmlWeb();

            try
            {
                return web.LoadFromWebAsync(url).Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania grafiku zajęć ze strony. Dodatkowa informacja: " + ex.Message);
            }
        }
    }
}
