using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    //the type of menu currently being tested
    public enum MenuType
    {
        NONE = 0,
        TAB = 1,
        ALPHA = 2,
        GROUPED = 3,
        RANDOM = 4,
        ICON = 5
    }

    public static class MenuTypeExtensions
    {
        public static MenuType getMenuTypeFromString(string value)
        {
            switch (value)
            {
                case "Icon":
                    return MenuType.ICON;
                case "Tab":
                    return MenuType.TAB;
                case "Alpha":
                    return MenuType.ALPHA;
                case "Grouped":
                    return MenuType.GROUPED;
                case "Random":
                    return MenuType.RANDOM;
                default:
                    return MenuType.NONE;
            }
        }
    }

    //the type of control being created
    public enum ControlType
    {
        NONE = 0,
        SWITCH = 1
    }

    public static class ControlTypeExtensions
    {
        //allow us to represent ControlTypes as strings
        public static ControlType getMenuTypeFromString(string value)
        {
            switch (value)
            {
                case "Switch":
                    return ControlType.SWITCH;
                default:
                    return ControlType.NONE;
            }
        }
    }
}
