using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UXStudy
{
    public class ImageReader
    {
        private string tool_img_loc;

        public ImageReader(string tool_loc)
        {
            tool_img_loc = tool_loc;
        }

        public Dictionary<Tool, ImageBrush> getToolImages()
        {
            Dictionary<Tool, ImageBrush> img_dict = new Dictionary<Tool, ImageBrush>();
            foreach (Tool tool in Enum.GetValues(typeof(Tool)).Cast<Tool>())
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = getImageFromLocation(tool_img_loc + tool.getToolImageLocation());
                img_dict.Add(tool, brush);
            }

            return img_dict;
        }

        private BitmapImage getImageFromLocation(string loc)
        {
            BitmapImage bitmap;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(loc))
            {
                bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
            }
            return bitmap;
        }
    }
}
