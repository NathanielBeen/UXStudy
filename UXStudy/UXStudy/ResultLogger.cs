using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    //tracks the results of the study
    public class ResultLogger
    {
        //results will be stored in these files
        private string info_file_path;
        private string answer_file_path;
        private string position_file_path;

        private StringBuilder info_string;
        private StringBuilder answer_string;
        private StringBuilder position_string;

        public ResultLogger(string info_path, string answer_path, string position_path)
        {
            info_file_path = info_path;
            answer_file_path = answer_path;
            position_file_path = position_path;

            info_string = new StringBuilder();
            answer_string = new StringBuilder();
            position_string = new StringBuilder();
        }

        //specifies which user took the test
        public void logUserInfo(string name)
        {
            answer_string.AppendLine("User name: " + name);
        }

        //specifies which controls were created
        public void logMenuInfo(MenuType type, List<IGameControl> controls)
        {
            info_string.AppendLine("Controls for the Menu Type: " + type.getTypeString());
        }

        //tracks when a control is selected but an answer is not yet selected (such as when a textbox is focused)
        public void logControlSelected(int id, DateTime time)
        {
            answer_string.AppendLine("Selected | " + id + " | " + time.ToString("HH:mm:ss"));
        }

        //tracks when something is clicked/entered/changed on a control
        public void logResult(int id, bool correct, DateTime time)
        {
            answer_string.AppendLine("Answered | " + id + " | " + correct + "|" + time.ToString("HH:mm:ss"));
        }

        //when the test begins
        public void logMenuStarted(MenuType type, DateTime time)
        {
            answer_string.AppendLine("Started | "+type.getTypeString()+" | "+time.ToString("HH:mm:ss"));
        }

        //when the test ends
        public void logMenuFinished(MenuType type, DateTime time)
        {
            answer_string.AppendLine("Ended | " + type.getTypeString() + " | " + time.ToString("HH:mm:ss"));

            using (var sw = new StreamWriter(info_file_path, true))
            {
                sw.Write(info_string.ToString());
            }
            using (var sw = new StreamWriter(answer_file_path, true))
            {
                sw.Write(answer_string.ToString());
            }
            using (var sw = new StreamWriter(position_file_path, true))
            {
                sw.Write(position_string.ToString());
            }

            info_string.Clear();
            answer_string.Clear();
            position_string.Clear();
        }

        //when the mouse moves
        public void logMouseMovement(int x, int y, DateTime time)
        {
            position_string.AppendLine(x + "|" + y + "|" + time.ToString("HH:mm:ss"));
        }
    }
}
