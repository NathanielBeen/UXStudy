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

        public int ControlID { get; }
        public ControlType ControlType { get { return ControlType.SWITCH; } }
        public string Title { get; }

        public bool Correct { get { return correct_answer == switched; } }

        private bool switched;
        public bool Switched
        {
            get { return switched; }
            set { switchChanged(value); }
        }

        public SingleSwitchControl(int id, string title, bool correct_ans, bool init)
        {
            correct_answer = correct_ans;

            ControlID = id;
            Title = title;
            

            Switched = init;
        }

        public event EventHandler<ClickEvent> ControlChangeStarted;
        public event EventHandler<ClickEvent> ControlChanged;

        private void switchChanged(bool value)
        {
            bool set = SetProperty(ref switched, value);
            if (set)
            {
                ControlChanged?.Invoke(this, new ClickEvent(this, DateTime.Now));
            }
        }
    }
}
