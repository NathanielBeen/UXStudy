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
        public string Title { get; }
        public bool MustAnswer { get; }

        private bool switched;
        public bool Switched
        {
            get { return switched; }
            set { switchChanged(value); }
        }

        public SingleSwitchControl(int id, string title, bool must_answer, bool correct, bool init)
        {
            correct_answer = correct;

            ControlID = id;
            Title = title;
            MustAnswer = must_answer;

            Switched = init;
        }

        public event EventHandler<int> ControlChangeStarted;
        public event EventHandler<ClickEvent> ControlChanged;

        private void switchChanged(bool value)
        {
            bool set = SetProperty(ref switched, value);
            if (set)
            {
                ControlChanged?.Invoke(this, new ClickEvent(ControlID, (correct_answer == switched)));
            }
        }
    }
}
