using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class MenuParser
    {
        public static int CurrentId { get; set; }
    }

    public abstract class ControlParser
    {
        protected int id;
        protected ControlType type;
        protected string title;
        private bool must_answer;
        
        public ControlParser(int id, ControlType type, string title, bool must_answer)
        {
            this.id = id;
            this.type = type;
            this.title = title;
            this.must_answer = must_answer;
        }

        public abstract IGameControl createControl();
    }

    public class SwitchParser : ControlParser
    {
        private bool correct;
        private bool init;

        public SwitchParser(int id, ControlType type, string title, bool must_answer, string extra)
            :base(id, type, title, must_answer)
        {

        }

        private void processExtra(string extra)
        {
            string[] parts = extra.Split(',');
            if (parts.Length != 2 || !Boolean.TryParse(parts[0], out bool first) || !Boolean.TryParse(parts[1], out bool second))
            {
                throw new ArgumentException(id+": extras must be formatted true,true or false,true ect.");
            }
            else
            {
                correct = first;
                init = second;
            }
        }
    }
}
