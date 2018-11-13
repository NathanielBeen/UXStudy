using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class TextboxControl : BaseViewModel, IGameControl
    {
        private string correct;

        public int ControlID { get; }
        public ControlType ControlType { get { return ControlType.TEXTBOX; } }
        public string Title { get; }
        public string Instructions { get; }
        public bool Correct { get { return correct.Equals(Entered); } }

        public string entered;
        public string Entered
        {
            get { return entered; }
            set
            {
                bool prev_correct = Correct;
                SetProperty(ref entered, value);
                bool after_correct = Correct;

                if (prev_correct != after_correct)
                {
                    ControlChanged?.Invoke(this, new ClickEvent(this, value, DateTime.Now));
                }
            }
        }

        public TextboxControl(int id, string title, string instructions, string correct)
        {
            this.correct = correct;
            ControlID = id;
            Title = title;
            Instructions = instructions;
            Entered = String.Empty;
        }

        public event EventHandler<ClickEvent> ControlChangeStarted;
        public event EventHandler<ClickEvent> ControlChanged;

        public void reset()
        {
            Entered = String.Empty;
        }
    }
}
