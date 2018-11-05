using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UXStudy
{
    public class Instructions : BaseViewModel
    {
        private Visibility visibility;
        public Visibility Visibility
        {
            get { return visibility; }
            set { SetProperty(ref visibility, value); }
        }

        private string instructions;
        public string InstructionString
        {
            get { return instructions; }
            set { SetProperty(ref instructions, value); }
        }

        public string Preamble { get; private set; }

        public Instructions()
        {
            Visibility = Visibility.Hidden;
            InstructionString = String.Empty;
            Preamble = createInstructionPreamble();
        }

        public void setInstructions(List<IGameControl> wanted)
        {
            StringBuilder sb = new StringBuilder();

            randomizeControls(wanted);
            foreach (IGameControl control in wanted)
            {
                sb.AppendLine("- "+control.Instructions);
            }

            InstructionString = sb.ToString();
        }

        private string createInstructionPreamble()
        {
            return "Instructions \n \n" +
                    "Complete all of the following tasks to proceed \n \n";
        }

        private void randomizeControls(List<IGameControl> wanted)
        {
            Random random = new Random();
            int index = wanted.Count - 1;

            while (index > 0)
            {
                int next = random.Next(index + 1);
                IGameControl temp = wanted[next];
                wanted[next] = wanted[index];
                wanted[index] = temp;
                index--;
            }
        }

        public void launchInstructions() { Visibility = Visibility.Visible; }

        public void closeInstructions() { Visibility = Visibility.Hidden; }
    }
}
