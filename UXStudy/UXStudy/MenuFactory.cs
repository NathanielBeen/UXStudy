using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class MenuFactory
    {
        private MenuParser parser;

        public MenuFactory(MenuParser parse)
        {
            parser = parse;
        }

        public Menu createRandomMenu()
        {
            return null;
        }
    }
}
