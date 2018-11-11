using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class RadioButtonControl : BaseViewModel, IGameControl
    {
        private RadioEntry correct;
        private RadioEntry init;

        public int ControlID { get; }
        public ControlType ControlType { get { return ControlType.RADIO; } }
        public string Title { get; }
        public string Instructions { get; }

        public bool Correct { get { return correct.Selected == true; } }
        public List<RadioEntry> Options { get; private set; }

        public RadioButtonControl(int id, string title, string instructions, string correct, string init, List<string> options)
        {
            ControlID = id;
            Title = title;
            Instructions = instructions;
            initEntries(correct, init, options);
        }

        public event EventHandler<ClickEvent> ControlChangeStarted;
        public event EventHandler<ClickEvent> ControlChanged;

        public void reset()
        {
            Options.ForEach(o => o.Selected = o.Equals(init));
        }

        private void initEntries(string correct, string init, List<string> options)
        {
            Options = new List<RadioEntry>();
            foreach (string option in options)
            {
                var entry = new RadioEntry(option, option.Equals(init));
                entry.EntrySelected += handleSelectedChanged;
                Options.Add(entry);
            }

            this.correct = Options.Where(o => o.Value.Equals(correct)).FirstOrDefault();
            this.init = Options.Where(o => o.Value.Equals(init)).FirstOrDefault();
        }

        private void handleSelectedChanged(object sender, RadioEntry entry)
        {
            Options.ForEach(o => o.Selected = o.Equals(entry));
            ControlChanged?.Invoke(this, new ClickEvent(this, DateTime.Now));
        }
    }

    public class RadioEntry : BaseViewModel
    {
        public string Value { get; }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { selectedChanged(value); }
        }

        public RadioEntry(string value, bool selected)
        {
            Value = value;
            Selected = selected;
        }

        public event EventHandler<RadioEntry> EntrySelected;

        private void selectedChanged(bool value)
        {
            bool set = SetProperty(ref selected, value);
            if (set && value)
            {
                EntrySelected?.Invoke(this, this);
            }
        }
    }
}
