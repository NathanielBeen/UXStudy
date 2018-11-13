using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    class SingleSwitchControl : BaseViewModel, IGameControl
    {
        private bool correct_answer;
        private bool init;

        public int ControlID { get; }
        public ControlType ControlType { get { return ControlType.SWITCH; } }
        public string Title { get; }
        public string Instructions { get; }

        public bool Correct { get { return correct_answer == switched; } }

        private bool switched;
        public bool Switched
        {
            get { return switched; }
            set
            {
                bool set = SetProperty(ref switched, value);
                if (set) { ControlChanged?.Invoke(this, new ClickEvent(this, value.ToString(), DateTime.Now)); }
            }
        }

        public SingleSwitchControl(int id, string title, string instructions, bool correct_ans, bool init)
        {
            correct_answer = correct_ans;
            this.init = init;

            ControlID = id;
            Title = title;
            Instructions = instructions;
            Switched = init;
        }

        public void reset()
        {
            Switched = init;
        }

        public event EventHandler<ClickEvent> ControlChangeStarted;
        public event EventHandler<ClickEvent> ControlChanged;
    }
}
