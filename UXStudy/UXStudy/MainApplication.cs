﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UXStudy
{
    //determines where in the study the application currently is
    //Initial: enter user's name
    //ready: show button prompting start of phase
    //test: shows the actual menu
    //complete: show end screen
    public enum StudyState
    {
        INITIAL,
        READY,
        TEST,
        COMPLETE
    }

    public class MainApplication : BaseViewModel
    {
        //location of files to write to/read from
        public const string INPUT_FILE = "../../Files/input.txt";
        public const string INFO_OUTPUT = "../../Files/info_out.txt";
        public const string ANSWER_OUTPUT = "../../Files/answer_out.txt";
        public const string POSITION_OUTPUT = "../../Files/position_out.txt";
        public const string TOOL_IMAGES = "../../Files/";

        private GameState state;
        private ResultLogger logger;
        private CompletionTracker completion;
        private MenuFactory factory;
        private ImageReader reader;

        private StudyState current_state;
        public StudyState CurrentState
        {
            get { return current_state; }
            set { SetProperty(ref current_state, value); }
        }

        //name of the current user
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

        //if the name was not entered, this will display an error
        private string error;
        public string Error
        {
            get { return error; }
            set { SetProperty(ref error, value); }
        }

        public Menu CurrentMenu { get; private set; }
        public MenuType CurrentType { get; private set; }

        public Instructions Instructions { get; private set; }
        public Toolkit Toolkit { get; private set; }

        //Buttons on the main screen will bind to these commands
        public ICommand SubmitInfoCommand { get; private set; }
        public ICommand StartPhase { get; private set; }
        public ICommand ExitProgram { get; private set; }

        public MainApplication()
        {
            state = new GameState(Tool.NONE);
            logger = new ResultLogger(INFO_OUTPUT, ANSWER_OUTPUT, POSITION_OUTPUT);
            completion = new CompletionTracker();
            factory = new MenuFactory(new MenuParser(logger, state, INPUT_FILE), logger);
            reader = new ImageReader(TOOL_IMAGES);

            CurrentState = StudyState.INITIAL;
            Name = String.Empty;
            Error = String.Empty;
            CurrentMenu = null;
            CurrentType = MenuType.NONE;

            Instructions = new Instructions();
            Toolkit = initToolkit();

            initCommands();
        }

        //links the commands to methods tha will fire when they are called
        //(when a user presses the button assigned to a command, it will
        //directly call whatevewr method is passed into it)
        private void initCommands()
        {
            SubmitInfoCommand = new RelayCommand(handleSubmitInfo);
            StartPhase = new RelayCommand(handleStartPhase);
            ExitProgram = new RelayCommand(handleExitProgram);
        }

        private Toolkit initToolkit()
        {
            List<ToolView> tools = new List<ToolView>();
            var tool_dict = reader.getToolImages();

            foreach (var entry in tool_dict)
            {
                tools.Add(new ToolView(entry.Key, entry.Value));
            }

            return new Toolkit(state, tools);
        }

        //called when the user enters their name and presses the confirm button
        private void handleSubmitInfo()
        {
            //check if there is no entered name
            if (Name == String.Empty) { Error = "must enter a valid name"; }
            else
            {
                Error = String.Empty;
                CurrentState = StudyState.READY;
                logger.logUserInfo(Name);
            }
        }

        //called when the user presses the "start test" button
        private void handleStartPhase()
        {
            //generates the next menu
            CurrentType = completion.getNextType();
            CurrentMenu = factory.getNextMenu(CurrentType);
            CurrentMenu.MenuFinished += handleMenuFinished;

            Instructions.setInstructions(CurrentMenu.WantedControls);
            Instructions.launchInstructions();
            CurrentState = StudyState.TEST;
            CurrentMenu.startMenu();
        }

        //called when the MenuFinished event of the CurrentMenu is triggered
        private void handleEndPhase()
        {
            //mark the current type as having been completed, then check if all stages have been complete
            completion.typeCompleted(CurrentType);
            Instructions.closeInstructions();

            if (completion.testComplete())
            {
                CurrentState = StudyState.COMPLETE;
            }
            else { CurrentState = StudyState.READY; }
        }

        private void handleExitProgram()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void handleMenuFinished(object sender, EventArgs args) { handleEndPhase(); }
    }

    //keeps track of which menu types have been completed and which still need to be done
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

        //marks a specific type as having been compelted
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

        //randomly generates the next menutype to complete
        public MenuType getNextType()
        {
            //add all the incomplete types to a list
            List<MenuType> incomplete = new List<MenuType>();
            if (!random_complete) { incomplete.Add(MenuType.RANDOM); }
            if (!alpha_complete) { incomplete.Add(MenuType.ALPHA); }
            if (!grouped_complete) { incomplete.Add(MenuType.GROUPED); }
            if (!tabbed_complete) { incomplete.Add(MenuType.TAB); }

            //return a random member of the created list
            if (incomplete.Count == 0) { return MenuType.NONE; }
            return incomplete[random.Next(incomplete.Count)];
        }

        //check if all phases are complete
        public bool testComplete()
        {
            return (random_complete && alpha_complete && grouped_complete && tabbed_complete);
        }
    }
}
