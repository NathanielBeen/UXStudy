using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UXStudy
{
    public enum StudyState
    {
        INITIAL,
        READY,
        TEST,
        COMPLETE
    }

    public class MainApplication : BaseViewModel
    {
        public const string INPUT_FILE = "";
        public const string INFO_OUTPUT = "";
        public const string ANSWER_OUTPUT = "";
        public const string POSITION_OUTPUT = "";

        private ResultLogger logger;
        private CompletionTracker completion;
        private MenuFactory factory;

        private StudyState current_state;
        public StudyState CurrentState
        {
            get { return current_state; }
            set { SetProperty(ref current_state, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
                if (value == String.Empty) { Error = "must enter a valid name"; }
                else { Error = String.Empty; }
            }
        }

        private string error;
        public string Error
        {
            get { return error; }
            set { SetProperty(ref error, value); }
        }

        public Menu CurrentMenu { get; private set; }
        public MenuType CurrentType { get; private set; }

        public ICommand SubmitInfoCommand { get; private set; }
        public ICommand StartPhase { get; private set; }
        public ICommand EndPhase { get; private set; }

        public MainApplication()
        {
            logger = new ResultLogger(INFO_OUTPUT, ANSWER_OUTPUT, POSITION_OUTPUT);
            completion = new CompletionTracker();
            factory = new MenuFactory(new MenuParser(logger, INPUT_FILE), logger);

            CurrentState = StudyState.INITIAL;
            Name = String.Empty;
            Error = String.Empty;
            CurrentMenu = null;
            CurrentType = MenuType.NONE;

            initCommands();
        }

        private void initCommands()
        {
            SubmitInfoCommand = new RelayCommand(handleSubmitInfo);
            StartPhase = new RelayCommand(handleStartPhase);
            EndPhase = new RelayCommand(handleEndPhase);
        }

        private void handleSubmitInfo()
        {
            if (Name == String.Empty) { Error = "must enter a valid name"; }
            else
            {
                Error = String.Empty;
                CurrentState = StudyState.READY;
                logger.logUserInfo(Name);
            }
        }

        private void handleStartPhase()
        {
                CurrentType = completion.getNextType();
                CurrentMenu = factory.getNextMenu(CurrentType);
                CurrentMenu.MenuFinished += handleMenuFinished;

                CurrentState = StudyState.TEST;
        }

        private void handleEndPhase()
        {
            completion.typeCompleted(CurrentType);
            if (completion.testComplete()) { CurrentState = StudyState.COMPLETE; }
            else { CurrentState = StudyState.READY; }
        }

        private void handleMenuFinished(object sender, EventArgs args) { handleEndPhase(); }
    }

    public class CompletionTracker
    {
        private Random random;
        private bool random_complete;
        private bool alpha_complete;
        private bool grouped_complete;
        private bool tabbed_complete;

        public CompletionTracker()
        {
            random = new Random();
            random_complete = false;
            alpha_complete = false;
            grouped_complete = false;
            tabbed_complete = false;
        }

        public void typeCompleted(MenuType type)
        {
            switch (type)
            {
                case MenuType.RANDOM:
                    random_complete = true;
                    break;
                case MenuType.ALPHA:
                    alpha_complete = true;
                    break;
                case MenuType.GROUPED:
                    grouped_complete = true;
                    break;
                case MenuType.TAB:
                    tabbed_complete = true;
                    break;
            }
        }

        public MenuType getNextType()
        {
            List<MenuType> incomplete = new List<MenuType>();
            if (!random_complete) { incomplete.Add(MenuType.RANDOM); }
            if (!alpha_complete) { incomplete.Add(MenuType.ALPHA); }
            if (!grouped_complete) { incomplete.Add(MenuType.GROUPED); }
            if (!tabbed_complete) { incomplete.Add(MenuType.TAB); }

            if (incomplete.Count == 0) { return MenuType.NONE; }
            return incomplete[random.Next(incomplete.Count)];
        }

        public bool testComplete()
        {
            return (random_complete && alpha_complete && grouped_complete && tabbed_complete);
        }
    }
}
