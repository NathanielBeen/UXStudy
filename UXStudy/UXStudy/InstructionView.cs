using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UXStudy
{
    public class InstructionView
    {
        public static string INSTRUCTION_LOC = "../../Files/Instructions.png";
        public static string MENU_LOC = "../../Files/Menu.png";

        public ImageBrush InstructionImage { get; private set; }
        public ImageBrush MenuImage { get; private set; }

        public ICommand ContinueCommand { get; private set; }

        public event EventHandler ContinueSelected;

        public InstructionView()
        {
            InstructionImage = new ImageBrush();
            InstructionImage.ImageSource = getImageFromLocation(INSTRUCTION_LOC);
            MenuImage = new ImageBrush();
            MenuImage.ImageSource = getImageFromLocation(MENU_LOC);

            initCommands();
        }

        private void initCommands()
        {
            ContinueCommand = new RelayCommand(handleContinue);
        }

        private BitmapImage getImageFromLocation(string loc)
        {
            return new BitmapImage(new Uri(loc, UriKind.Relative));
        }

        private void handleContinue() { ContinueSelected?.Invoke(this, new EventArgs()); }
    }
}
