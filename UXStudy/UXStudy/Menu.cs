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
        private List<IGameControl> wanted_controls;

        public MenuType Type { get; }
        public List<SubMenu> Menus { get; }

        public Menu(MenuType type, List<SubMenu> menus, List<IGameControl> wanted, ResultLogger log)
        {
            logger = log;
            wanted_controls = wanted;

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
            logger.logResult(click.Control.ControlID, click.Control.Correct, click.Time);

            //if all controls have been correctly answered, log the finish and update the main game
            //so it can generate the next menu (if the answered control is not in the desired category and the 
            //menu was previoiusly incomplete there is no need to check again)
            if (wanted_controls.Contains(click.Control) && checkMenuFinished())
            {
                logger.logMenuFinished(Type, click.Time);
                MenuFinished?.Invoke(this, new EventArgs());
            }
        }

        //checks to see if all needed controls have been correctly answered
        private bool checkMenuFinished()
        {
            foreach (var control in wanted_controls)
            {
                if (!control.Correct) { return false; }
            }
            return true;
        }

        public event EventHandler MenuFinished;
    }

    //this menu has a set of tabs that the user can click between
    public class TabbedMenu : Menu
    {
        //this will eb bound to the menu and will indicate the controls on the currently selected tab
        private ObservableCollection<IGameControl> current_controls;
        public ObservableCollection<IGameControl> CurrentControls
        {
            get { return current_controls; }
            set { SetProperty(ref current_controls, value); }
        }

        //the id of the currently selected submenu
        private int selected_id;
        public int SelectedId
        {
            get { return selected_id; }
            set
            {
                //when the user selects a different tab, the following is triggered
                if (!checkNewValue(value)) { return; }
                handleTabSelectionChange(value);
                selected_id = value;
            }
        }

        public TabbedMenu(List<SubMenu> submenus, List<IGameControl> wanted, ResultLogger logger)
            :base(MenuType.TAB, submenus, wanted, logger)
        {
            CurrentControls = new ObservableCollection<IGameControl>();
        }

        private void handleTabSelectionChange(int new_val)
        {
            CurrentControls.Clear();
            //get the correct sub_menu using its id
            var menu = Menus.Where(s => (s as TabbedSubMenu)?.MenuId == new_val).FirstOrDefault();
            foreach (var control in menu.Controls)
            {
                CurrentControls.Add(control);
            }
        }

        //makes sure the newly selected tab is different from the current selection and is a valid id
        private bool checkNewValue(int new_val)
        {
            var menu = Menus.Where(s => (s as TabbedSubMenu)?.MenuId == new_val).FirstOrDefault();
            return (selected_id != new_val && menu != null);
        }
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
            logger.logControlSelected(click.Control.ControlID, click.Time);
        }

        //when the value of a control changes, log it and fire the ControlUpdated event so the 
        //main menu can check whether the game is "won"
        private void handleControlAnswered(object sender, ClickEvent click)
        {
            logger.logResult(click.Control.ControlID, click.Control.Correct, click.Time);
            ControlUpdated?.Invoke(this, click);
        }

        public event EventHandler<ClickEvent> ControlUpdated;
    }
    
    public class TabbedSubMenu : SubMenu
    {
        public int MenuId { get; }

        public TabbedSubMenu(ResultLogger log, string title, List<IGameControl> controls, int id)
            :base(log, title, controls)
        {
            MenuId = id;
        }
    }
}
