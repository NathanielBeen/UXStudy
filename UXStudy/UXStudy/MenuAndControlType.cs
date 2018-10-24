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
        ICON = 1,
        TAB = 2,
        ALPHA = 3,
        GROUPED = 4,
        RANDOM = 5
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
