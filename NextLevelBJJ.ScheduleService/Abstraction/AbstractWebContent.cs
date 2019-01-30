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

            try
            {
                HtmlDocument = web.LoadFromWebAsync(url).Result;
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania grafiku zajęć ze strony. Dodatkowa informacja: " + ex.Message);
            }
        }
    }
}
