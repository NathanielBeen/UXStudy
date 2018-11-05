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
        public const int TYPE = 0;
        public const int TITLE = 1;
        public const int GROUPING = 2;
        public const int MUST_ANSWER = 3;
        public const int EXTRA = 4;
        public const int INSTRUCTIONS = 5;

        protected int id;
        protected string title;
        protected string instructions;
        
        public string Grouping { get; private set; }
        public List<int> MustAnswer { get; private set; }

        public ControlParser(int id, string line)
        {
            this.id = id;
            processLine(line);
        }

        //creates the base attributes for any control
        //format: type|title|grouping|must_answer|extra|instructions <- the contents of "extra" will change based on what type of control it is
        private void processLine(string line)
        {
            string[] parts = line.Split('|');
            if (parts.Length != 6 || ControlTypeExtensions.getMenuTypeFromString(parts[TYPE]) == ControlType.NONE)
            {
                throw new ArgumentException("must be formatted as type|title|grouping|must_answer|extras|instructions, where type is a valid controltype and must_answer" +
                    "is a boolean");
            }
            else
            {
                title = parts[TITLE];
                instructions = parts[INSTRUCTIONS];
                Grouping = parts[GROUPING];
                MustAnswer = generateMustAnswer(parts[MUST_ANSWER]);
            }
        }

        //generate a list of integers that refer to when the user has to answer the control to pass
        //ex (0,3) means that the control must be answered in the first round and the fourth
        private List<int> generateMustAnswer(string must_answer)
        {
            List<int> ma_list = new List<int>();
            //if must answer is "none" then it is never used in any round
            if (must_answer == "none") { return ma_list; }

            string[] parts = must_answer.Split(',');
            foreach (string part in parts)
            {
                //if each part can be parsed as an int, add it too the overall list
                if (int.TryParse(part, out int conv)) { ma_list.Add(conv); }
                else { throw new ArgumentException("all grouping values must be integers (or set to none if not used)"); }
            }

            return ma_list;
        }

        //will create the needed view so we can easily add it to a list
        public abstract IGameControl createControl();
    }

    //creates a single switch view.
    //format: Switch|title|grouping|must_answer|correct,init <- how line looks in txt file
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
            string extra = line.Split('|')[EXTRA];
            string[] parts = extra.Split(',');
            //if either value is not present or either cannot be converted to a boolean throw an exception
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
            return new SingleSwitchControl(id, title, instructions, correct, init);
        }
    }
}
