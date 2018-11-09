using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    //will create a menu
    public class MenuParser
    {
        private GameState state;
        private string location;
        private ResultLogger logger;
        private int current_id;

        private Dictionary<IGameControl, string> control_groupings;
        private Dictionary<int, List<IGameControl>> wanted_controls;

        public MenuParser(ResultLogger log, GameState st, string input_location)
        {
            state = st;
            location = input_location;
            logger = log;
            current_id = 0;

            control_groupings = new Dictionary<IGameControl, string>();
            wanted_controls = new Dictionary<int, List<IGameControl>>();

            readInControls(location);
        }

        private void readInControls(string input_location)
        {
            using (var reader = new StreamReader(input_location))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    createControl(line);
                }
            }
        }

        private void createControl(string line)
        {
            var parser = ControlParserFactory.createParser(current_id, line);
            var control = parser.createControl(state);

            control_groupings.Add(control, parser.Grouping);
            foreach (int type in parser.MustAnswer)
            {
                if (wanted_controls.ContainsKey(type))
                {
                    wanted_controls[type].Add(control);
                }
                else
                {
                    wanted_controls[type] = new List<IGameControl>() { control };
                }
            }

            current_id += 1;
        }

        public void resetAllControls()
        {
            foreach (var control in control_groupings.Keys)
            {
                control.reset();
            }
        }

        public List<IGameControl> getAllControls()
        {
            List<IGameControl> controls = new List<IGameControl>();
            foreach (var pair in control_groupings)
            {
                controls.Add(pair.Key);
            }

            return controls;
        }

        public Dictionary<string, List<IGameControl>> getGroupedControls()
        {
            var controls = new Dictionary<string, List<IGameControl>>();
            foreach (var pair in control_groupings)
            {
                if (controls.ContainsKey(pair.Value))
                {
                    controls[pair.Value].Add(pair.Key);
                }
                else { controls[pair.Value] = new List<IGameControl>() { pair.Key }; }
            }
            return controls;
        }

        public List<IGameControl> getWantedControls(int round) { return wanted_controls[round]; }
    }
}
