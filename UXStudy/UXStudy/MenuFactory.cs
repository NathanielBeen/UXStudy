using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class MenuFactory
    {
        private MenuParser parser;
        private ResultLogger logger;

        public MenuFactory(MenuParser parse, ResultLogger log)
        {
            parser = parse;
            logger = log;
        }

        public Menu createRandomMenu()
        {
            List<IGameControl> randomized_controls = parser.getAllControls();
            Random random = new Random();
            int index = randomized_controls.Count - 1;

            while (index > 0)
            {
                int next = random.Next(index + 1);
                IGameControl temp = randomized_controls[next];
                randomized_controls[next] = randomized_controls[index];
                randomized_controls[index] = temp;
                index--;
            }

            SubMenu sub = new SubMenu(logger, String.Empty, randomized_controls);
            List<IGameControl> wanted = parser.getWantedControls((int)MenuType.RANDOM);

            return new Menu(MenuType.RANDOM, new List<SubMenu>() { sub }, wanted, logger);
        }

        public Menu createAlphaMenu()
        {
            List<IGameControl> alpha_controls = parser.getAllControls().OrderBy(c => c.Title).ToList();
            SubMenu sub = new SubMenu(logger, String.Empty, alpha_controls);
            List<IGameControl> wanted = parser.getWantedControls((int)MenuType.ALPHA);

            return new Menu(MenuType.ALPHA, new List<SubMenu>() { sub }, wanted, logger);
        }

        public Menu createGroupedMenu()
        {
            List<SubMenu> menus = new List<SubMenu>();
            var groups = parser.getGroupedControls();
            foreach (var entry in groups)
            {
                menus.Add(new SubMenu(logger, entry.Key, entry.Value));
            }

            List<IGameControl> wanted = parser.getWantedControls((int)MenuType.GROUPED);

            return new Menu(MenuType.GROUPED, menus, wanted, logger);
        }

        public Menu createTabbedMenu()
        {
            List<TabbedSubMenu> menus = new List<TabbedSubMenu>();
            var groups = parser.getGroupedControls();

            int id = 0;
            foreach (var entry in groups)
            {
                menus.Add(new TabbedSubMenu(logger, entry.Key, entry.Value, id));
                id++;
            }

            List<IGameControl> wanted = parser.getWantedControls((int)MenuType.TAB);

            return new Menu(MenuType.TAB, menus, wanted, logger);
        }
    }
}
