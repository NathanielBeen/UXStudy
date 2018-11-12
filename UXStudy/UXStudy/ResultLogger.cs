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
        private string answer_file_path;
        private string survey_file_path;

        private StringBuilder answer_string;
        private StringBuilder survey_string;

        public ResultLogger(string answer_path, string survey_path)
        {
            answer_file_path = answer_path;
            survey_file_path = survey_path;

            answer_string = new StringBuilder();
            survey_string = new StringBuilder();
        }

        //specifies which user took the test
        public void logUserInfo(string name)
        {
            answer_string.AppendLine("User name: " + name);
            answer_string.AppendLine("");
        }

        //tracks when a control is selected but an answer is not yet selected (such as when a textbox is focused)
        public void logControlSelected(int id, DateTime time)
        {
            answer_string.AppendLine("Selected | " + id + " | " + time.ToString("HH:mm:ss"));
        }

        //tracks when something is clicked/entered/changed on a control
        public void logResult(IGameControl control, string answer, DateTime time)
        {
            answer_string.AppendLine("Answered | " + control.Title + " ("+control.ControlType.ToString()+") "+" | " 
                + answer + " (" + control.Correct + ") " + "|" + time.ToString("HH:mm:ss"));
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
            answer_string.AppendLine("");

            using (var sw = new StreamWriter(answer_file_path, true))
            {
                sw.Write(answer_string.ToString());
            }
            using (var sw = new StreamWriter(survey_file_path, true))
            {
                sw.Write(survey_string.ToString());
            }

            answer_string.Clear();
            survey_string.Clear();
        }

        public void logSurveyCompleted(Survey survey)
        {
            survey_string.AppendLine("Survey | " + survey.Type.getTypeString());
            if (survey.Alpha) { survey_string.AppendLine("Knew Alpha: " + survey.KnewAlpha.ToString()); }
            foreach (var answer in survey.Questions)
            {
                survey_string.AppendLine(answer.Question + ": " + answer.getSelected());
            }
            survey_string.AppendLine("");
        }
    }
}
