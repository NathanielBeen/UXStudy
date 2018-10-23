using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    //the type of menu currently being tested
    public enum MenuType
    {
        ICON = 0,
        TAB = 1,
        ALPHA = 2,
        GROUPED = 3
    }

    //this is the model class that will represent each menu
    public class Menu
    {
        private ResultLogger logger;
        private Dictionary<int, bool> wanted_controls;

        public MenuType Type { get; }
        public List<IGameControl> Controls { get; }

        public Menu(MenuType type, List<IGameControl> controls, Dictionary<int, bool> wanted_controls, ResultLogger log)
        {
            logger = log;

            Type = type;
            Controls = controls;
        }

        public void startMenu()
        {
            logger.logMenuStarted(Type, DateTime.Now);
        }

        private void hookControls()
        {
            foreach (IGameControl control in Controls)
            {
                control.ControlChangeStarted += handleControlSelected;
                control.ControlChanged += handleControlAnswered;
            }
        }

        private void handleControlSelected(object sender, int id)
        {
            logger.logControlSelected(id, DateTime.Now);
        }

        private void handleControlAnswered(object sender, ClickEvent click)
        {
            DateTime time = DateTime.Now;
            logger.logResult(click.Id, click.Correct, time);
            updateWantedControls(click);

            if (checkMenuFinished())
            {
                logger.logMenuFinished(Type, time);
                MenuFinished?.Invoke(this, new EventArgs());
            }
        }

        private void updateWantedControls(ClickEvent click)
        {
            if (wanted_controls.ContainsKey(click.Id))
            {
                wanted_controls[click.Id] = click.Correct;
            }
        }

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
}
