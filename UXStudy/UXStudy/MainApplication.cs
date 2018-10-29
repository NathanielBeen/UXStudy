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
            }
        }

        private void handleStartPhase()
        {

        }

        private void handleEndPhase()
        {

        }
    }
}
