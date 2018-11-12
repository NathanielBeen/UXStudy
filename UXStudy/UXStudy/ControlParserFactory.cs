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
                case ControlType.COMBOBOX:
                    return new ComboBoxParser(id, line);
                case ControlType.RADIO:
                    return new RadioParser(id, line);
                case ControlType.TEXTBOX:
                    return new TextboxParser(id, line);
                case ControlType.SLIDER:
                    return new SliderParser(id, line);
                default:
                    return null;
            }
        }
    }
}
