using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class ComboBoxControl : BaseViewModel, IGameControl
    {
        private string correct;
        private string init;

        public int ControlID { get; }
        public ControlType ControlType { get { return ControlType.COMBOBOX; } }
        public string Title { get; }
        public string Instructions { get; }

        public bool Correct { get { return correct.Equals(Selected); } }
        public List<string> Options { get; }

        private string selected;
        public string Selected
        {
            get { return selected; }
            set { selectedChanged(value); }
        }

        public ComboBoxControl(int id, string title, string instructions, string correct, string init, List<string> options)
        {
            this.correct = correct;
            this.init = init;
            ControlID = id;
            Title = title;
            Instructions = instructions;
            Options = options;
        }

        public event EventHandler<ClickEvent> ControlChangeStarted;
        public event EventHandler<ClickEvent> ControlChanged;

        public void reset()
        {
            Selected = init;
        }

        private void selectedChanged(string value)
        {
            bool set = SetProperty(ref selected, value);
            if (set)
            {
                ControlChanged?.Invoke(this, new ClickEvent(this, DateTime.Now));
            }
        }
    }
}
