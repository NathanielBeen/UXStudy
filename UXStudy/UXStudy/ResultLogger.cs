using System;
using System.Collections.Generic;
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

        public ResultLogger(string info_path, string answer_path, string position_path)
        {
            answer_file_path = answer_path;
            position_file_path = position_path;
        }

        //specifies which controls were created
        public void logMenuInfo(MenuType type, List<IGameControl> controls)
        {

        }

        //tracks when a control is selected but an answer is not yet selected (such as when a textbox is focused)
        public void logControlSelected(int id, DateTime time)
        {

        }

        //tracks when something is clicked/entered/changed on a control
        public void logResult(int id, bool correct, DateTime time)
        {

        }

        //when the test begins
        public void logMenuStarted(MenuType type, DateTime time)
        {

        }

        //when the test ends
        public void logMenuFinished(MenuType type, DateTime time)
        {

        }

        //when the mouse moves
        public void logMouseMovement(int x, int y, DateTime time)
        {

        }
    }
}
