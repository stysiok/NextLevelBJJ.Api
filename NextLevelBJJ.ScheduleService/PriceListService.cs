using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace NextLevelBJJ.WebContentServices
{
    public class PriceListService : AbstractWebContent, IPriceListService
    {
        public PriceListService() : base(@"https://www.nextlevelbjj.pl/cennik/")
        {

        }
    }
}
