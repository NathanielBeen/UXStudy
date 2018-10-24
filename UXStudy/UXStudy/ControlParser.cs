using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    //creates a control from a line in a config/text file
    public abstract class ControlParser
    {
        protected int id;
        protected ControlType type;
        protected string title;
        protected bool must_answer;

        public ControlParser(int id, string extra)
        {
            this.id = id;
            processExtra(extra);
        }

        //creates the base attributes for any control
        //format: type|title|must_answer|extra <- the contents of "extra" will change based on what type of control it is
        private void processExtra(string extra)
        {
            string[] parts = extra.Split('|');
            if (parts.Length != 4 || ControlTypeExtensions.getMenuTypeFromString(parts[0]) == ControlType.NONE ||
                !Boolean.TryParse(parts[2], out bool must))
            {
                throw new ArgumentException("must be formatted as type|title|must_answer|extras, where type is a valid controltype and must_answer" +
                    "is a boolean");
            }
            else
            {
                type = ControlTypeExtensions.getMenuTypeFromString(parts[0]);
                title = parts[1];
                must_answer = must;
            }
        }

        //will create the needed view so we can easily add it to a list
        public abstract IGameControl createControl();
    }

    //creates a single switch view.
    //format: type|title|must_answer|correct,init <- how line looks in txt file
    public class SwitchParser : ControlParser
    {
        private bool correct;
        private bool init;

        public SwitchParser(int id, string line)
            : base(id, line)
        {
            processExtra(line);
        }

        //turn correct,init into their actual values
        private void processExtra(string line)
        {
            string extra = line.Split('|')[3];
            string[] parts = extra.Split(',');
            if (parts.Length != 2 || !Boolean.TryParse(parts[0], out bool first) || !Boolean.TryParse(parts[1], out bool second))
            {
                throw new ArgumentException(id + ": extras must be formatted true,true or false,true ect.");
            }
            else
            {
                correct = first;
                init = second;
            }
        }

        public override IGameControl createControl()
        {
            return new SingleSwitchControl(id, title, must_answer, correct, init);
        }
    }
}
