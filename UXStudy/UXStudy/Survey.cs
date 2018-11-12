using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class Survey : BaseViewModel
    {
        private ResultLogger logger;

        public MenuType Type { get; }
        public bool Alpha { get; }
        public List<SurveyRadio> Questions { get; private set; }

        private bool knew_alpha;
        public bool KnewAlpha
        {
            get { return knew_alpha; }
            set { SetProperty(ref knew_alpha, value); }
        }

        public RelayCommand SubmitCommand { get; private set; }

        public event EventHandler SurveySubmitted;

        public Survey(ResultLogger logger, MenuType type)
        {
            this.logger = logger;
            Type = type;
            Alpha = (type == MenuType.ALPHA);
            KnewAlpha = false;

            initSurveys();
            initCommands();
        }

        private void initSurveys()
        {
            Questions = new List<SurveyRadio>
            {
                new SurveyRadio("This menu was easy to navigate", "Easy Navigation"),
                new SurveyRadio("This menu was frustrating to navigate", "Frustating Navigation"),
                new SurveyRadio("settings were easy to find", "Finding Easy"),
                new SurveyRadio("I would recommend using this menu", "Recommend Using")
            };
        }

        private void initCommands()
        {
            SubmitCommand = new RelayCommand(handleSubmitted);
        }

        private void handleSubmitted()
        {
            logger.logSurveyCompleted(this);
            SurveySubmitted?.Invoke(this, new EventArgs());
        }
    }

    public class SurveyRadio : BaseViewModel
    {
        private string short_question;

        public string Question { get; }
        public List<SurveyEntry> Options { get; }

        public SurveyRadio(string question, string short_ques)
        {
            short_question = short_ques;
            Question = question;
            Options = genEntries();
        }

        public string getSelected()
        {
            var selected = Options.Where(o => o.Selected == true).FirstOrDefault();
            return (selected == null) ? "no answer" : selected.Value;
        }

        private List<SurveyEntry> genEntries()
        {
            List<SurveyEntry> options = new List<SurveyEntry>();
            options.Add(new SurveyEntry("Strongly Agree"));
            options.Add(new SurveyEntry("Agree"));
            options.Add(new SurveyEntry("Slightly Agree"));
            options.Add(new SurveyEntry("Neutral"));
            options.Add(new SurveyEntry("Slightly Disagree"));
            options.Add(new SurveyEntry("Disagree"));
            options.Add(new SurveyEntry("Strongly Disagree"));

            foreach (SurveyEntry entry in options)
            {
                entry.EntrySelected += handleSelectionChanged;
            }
            return options;
        }

        private void handleSelectionChanged(object sender, SurveyEntry entry)
        {
            Options.ForEach(o => o.Selected = o.Equals(entry));
        }
    }

    public class SurveyEntry : BaseViewModel
    {
        public string Value { get; }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { handleSelected(value); }
        }

        public SurveyEntry(string value)
        {
            Value = value;
            Selected = false;
        }

        public event EventHandler<SurveyEntry> EntrySelected;

        private void handleSelected(bool value)
        {
            bool set = SetProperty(ref selected, value);
            if (set && value)
            {
                EntrySelected?.Invoke(this, this);
            }
        }
    }
}
