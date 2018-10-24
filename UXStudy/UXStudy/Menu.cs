using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    //this is the model class that will represent each menu
    public class Menu
    {
        private ResultLogger logger;
        //a list of ids and current status of controls that need to be changed to "win"
        private Dictionary<int, bool> wanted_controls;

        public MenuType Type { get; }
        public List<SubMenu> Menus { get; }

        public Menu(MenuType type, List<SubMenu> menus, Dictionary<int, bool> wanted_controls, ResultLogger log)
        {
            logger = log;

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
            }
        }

        private void handleControlAnswered(object sender, ClickEvent click)
        {
            updateWantedControls(click);

            //if all controls have been correctly answered, log the finish and update the main game
            //so it can generate the next menu
            if (checkMenuFinished())
            {
                logger.logMenuFinished(Type, click.Time);
                MenuFinished?.Invoke(this, new EventArgs());
            }
        }

        //checks to see if the updated control was one of the needed controls to complete the game
        private void updateWantedControls(ClickEvent click)
        {
            if (wanted_controls.ContainsKey(click.Id))
            {
                wanted_controls[click.Id] = click.Correct;
            }
        }

        //checks to see if all needed controls have been correctly answered
        private bool checkMenuFinished()
        {
            foreach (bool valid in wanted_controls.Values)
            {
                if (!valid) { return false; }
            }
            return true;
        }

        event EventHandler MenuFinished;
    }

    //a single sub-menu grouping for a menu. Allows controls to be grouped into different tabs. A single-page menu
    //will only have one sub-menu, while the tabbed one will have 3 or 4.
    public class SubMenu
    {
        private ResultLogger logger;
        public List<IGameControl> Controls { get; }

        public SubMenu(ResultLogger log, List<IGameControl> controls)
        {
            logger = log;
            Controls = controls;
            hookControls();
        }

        private void hookControls()
        {
            foreach (IGameControl control in Controls)
            {
                control.ControlChangeStarted += handleControlSelected;
                control.ControlChanged += handleControlAnswered;
            }
        }

        private void handleControlSelected(object sender, ClickEvent click)
        {
            logger.logControlSelected(click.Id, click.Time);
        }

        //when the value of a control changes, log it and fire the ControlUpdated event so the 
        //main menu can check whether the game is "won"
        private void handleControlAnswered(object sender, ClickEvent click)
        {
            logger.logResult(click.Id, click.Correct, click.Time);
            ControlUpdated?.Invoke(this, click);
        }

        public event EventHandler<ClickEvent> ControlUpdated;
    }
}
