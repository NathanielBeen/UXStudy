using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public static class ControlParserFactory
    {
        public static ControlParser createParser(int id, string line)
        {
            ControlType type = ControlTypeExtensions.getMenuTypeFromString(line.Split('|')[ControlParser.TYPE]);
            switch (type)
            {
                case ControlType.SWITCH:
                    return new SwitchParser(id, line);
                default:
                    return null;
            }
        }
    }
}
