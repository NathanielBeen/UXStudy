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

        private StreamWriter info_writer;
        private StreamWriter answer_writer;
        private StreamWriter position_writer;

        public ResultLogger(string info_path, string answer_path, string position_path)
        {
            info_file_path = info_path;
            answer_file_path = answer_path;
            position_file_path = position_path;

            info_writer = new StreamWriter(info_file_path);
            answer_writer = new StreamWriter(answer_file_path);
            position_writer = new StreamWriter(position_file_path);
        }

        //specifies which user took the test
        public void logUserInfo(string name)
        {
            info_writer.WriteLine("User name: " + name);
        }

        //specifies which controls were created
        public void logMenuInfo(MenuType type, List<IGameControl> controls)
        {
            info_writer.WriteLine("Controls for the Menu Type: " + type.getTypeString());
        }

        //tracks when a control is selected but an answer is not yet selected (such as when a textbox is focused)
        public void logControlSelected(int id, DateTime time)
        {
            answer_writer.WriteLine("Selected | " + id + " | " + time.ToShortTimeString());
        }

        //tracks when something is clicked/entered/changed on a control
        public void logResult(int id, bool correct, DateTime time)
        {
            answer_writer.WriteLine("Answered | " + id + " | " + time.ToShortTimeString());
        }

        //when the test begins
        public void logMenuStarted(MenuType type, DateTime time)
        {
            answer_writer.WriteLine("Started | "+type.getTypeString()+" | "+time.ToShortTimeString());
        }

        //when the test ends
        public void logMenuFinished(MenuType type, DateTime time)
        {
            answer_writer.WriteLine("Ended | " + type.getTypeString() + " | " + time.ToShortTimeString());
        }

        //when the mouse moves
        public void logMouseMovement(int x, int y, DateTime time)
        {
            position_writer.WriteLine(x + "|" + y + "|" + time.ToShortTimeString());
        }

        public void testComplete()
        {
            answer_writer.Close();
            position_writer.Close();
        }
    }
}
