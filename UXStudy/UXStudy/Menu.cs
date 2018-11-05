using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    //this is the model class that will represent each menu
    public class Menu : BaseViewModel
    {
        private ResultLogger logger;
        //a list of ids and current status of controls that need to be changed to "win"
        public List<IGameControl> WantedControls { get; private set; }

        public MenuType Type { get; }
        public List<SubMenu> Menus { get; }

        public Menu(MenuType type, List<SubMenu> menus, List<IGameControl> wanted, ResultLogger log)
        {
            logger = log;
            WantedControls = wanted;

            Type = type;
            Menus = menus;

            hookMenus();
        }

        public void startMenu()
        {
            logger.logMenuStarted(Type, DateTime.Now);
        }

        private void hookMenus()
        {
            foreach (SubMenu menu in Menus)
            {
                //when a control is changed in a sub-menu, it is routed to the main menu
                menu.ControlUpdated += handleControlAnswered;
                menu.hookControls();
            }
        }

        private void unhookMenus()
        {
            foreach (SubMenu menu in Menus)
            {
                menu.ControlUpdated -= handleControlAnswered;
                menu.unhookControls();
            }
        }

        private void handleControlAnswered(object sender, ClickEvent click)
        {
            bool correct = (click.Control.Correct && WantedControls.Contains(click.Control));
            logger.logResult(click.Control.ControlID, correct, click.Time);

            //if all controls have been correctly answered, log the finish and update the main game
            //so it can generate the next menu (if the answered control is not in the desired category and the 
            //menu was previoiusly incomplete there is no need to check again)
            if (WantedControls.Contains(click.Control) && checkMenuFinished())
            {
                logger.logMenuFinished(Type, click.Time);
                unhookMenus();
                MenuFinished?.Invoke(this, new EventArgs());
            }
        }

        //checks to see if all needed controls have been correctly answered
        private bool checkMenuFinished()
        {
            foreach (var control in WantedControls)
            {
                if (!control.Correct) { return false; }
            }
            return true;
        }

        public event EventHandler MenuFinished;
    }

    //a single sub-menu grouping for a menu. Allows controls to be grouped into different tabs. A single-page menu
    //will only have one sub-menu, while the tabbed one will have 3 or 4.
    public class SubMenu
    {
        private ResultLogger logger;

        public string Title { get; }
        public List<IGameControl> Controls { get; }

        public SubMenu(ResultLogger log, string title, List<IGameControl> controls)
        {
            logger = log;

            Title = title;
            Controls = controls;
        }

        public void hookControls()
        {
            foreach (IGameControl control in Controls)
            {
                control.ControlChangeStarted += handleControlSelected;
                control.ControlChanged += handleControlAnswered;
            }
        }

        public void unhookControls()
        {
            foreach (IGameControl control in Controls)
            {
                control.ControlChangeStarted -= handleControlSelected;
                control.ControlChanged -= handleControlAnswered;
            }
        }

        private void handleControlSelected(object sender, ClickEvent click)
        {
            logger.logControlSelected(click.Control.ControlID, click.Time);
        }

        //when the value of a control changes, log it and fire the ControlUpdated event so the 
        //main menu can check whether the game is "won"
        private void handleControlAnswered(object sender, ClickEvent click)
        {
            ControlUpdated?.Invoke(this, click);
        }

        public event EventHandler<ClickEvent> ControlUpdated;
    }
}
