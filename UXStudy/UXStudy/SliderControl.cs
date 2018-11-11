using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class SliderControl : BaseViewModel, IGameControl
    {
        private int correct;
        private int init;

        public int ControlID { get; }
        public ControlType ControlType { get { return ControlType.SLIDER; } }
        public string Title { get; }
        public string Instructions { get; }
        public bool Correct { get { return correct == Current; } }

        public int Min { get; }
        public int Max { get; }

        private int current;
        public int Current
        {
            get { return current; }
            set { currentChanged(value); }
        }

        public SliderControl(int id, string title, string instructions, int correct, int init, int min, int max)
        {
            this.correct = correct;
            this.init = init;

            ControlID = id;
            Title = title;
            Instructions = instructions;

            Min = min;
            Max = max;
        }

        public event EventHandler<ClickEvent> ControlChangeStarted;
        public event EventHandler<ClickEvent> ControlChanged;

        public void reset()
        {
            Current = init;
        }

        private void currentChanged(int value)
        {
            bool set = SetProperty(ref current, value);
            if (set)
            {
                ControlChanged?.Invoke(this, new ClickEvent(this, DateTime.Now));
            }
        }
    }
}
