using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UXStudy
{
    public enum Tool
    {
        NONE,
        WIRE_CUTTER,
        BLUE_WIRE,
        RED_WIRE,
        GREEN_WIRE
    }

    public static class ToolExtensions
    {
        public static string getToolName(this Tool tool)
        {
            switch (tool)
            {
                case Tool.BLUE_WIRE:
                    return "Blue Wire";
                case Tool.GREEN_WIRE:
                    return "Green Wire";
                case Tool.RED_WIRE:
                    return "Red Wire";
                case Tool.WIRE_CUTTER:
                    return "Wire Cutter";
                default:
                    return "No Selection";
            }
        }

        public static string getToolImageLocation(this Tool tool)
        {
            switch (tool)
            {
                case Tool.BLUE_WIRE:
                    return "Blue_Wire.png";
                case Tool.GREEN_WIRE:
                    return "Green_Wire.png";
                case Tool.RED_WIRE:
                    return "Red_Wire.png";
                case Tool.WIRE_CUTTER:
                    return "Wire_Cutter.png";
                default:
                    return "None.png";
            }
        }
    }

    public class Toolkit : BaseViewModel
    {
        private GameState state;

        public List<ToolView> Tools { get; private set; }

        public Toolkit(GameState st, List<ToolView> tools)
        {
            state = st;
            initTools(tools);
        }

        private void initTools(List<ToolView> tools)
        {
            Tools = tools;
            ToolView none_tool = Tools.Where(t => t.Tool == Tool.NONE).FirstOrDefault();
            if (none_tool != null) { none_tool.Selected = true; }

            foreach (ToolView tool in Tools)
            {
                tool.ToolSelected += handleSelectedChanged;
            }
        }

        private void handleSelectedChanged(object sender, ToolView tool)
        {
            Tools.Where(t => !t.Equals(tool)).ToList()
                .ForEach(t => t.Selected = false);

            state.ToolState = tool.Tool;
        }
    }

    public class ToolView : BaseViewModel
    {
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set
            {
                bool old = selected;
                SetProperty(ref selected, value);
                changeSelected(old, value);
            }
        }

        public string ToolName { get; }
        public Tool Tool { get; }
        public ImageBrush Image { get; }

        public ToolView(Tool tool, ImageBrush image)
        {
            ToolName = tool.getToolName();
            Tool = tool;
            Image = image;
        }

        private void changeSelected(bool old_selected, bool new_selected)
        {
            if (!old_selected && new_selected) { ToolSelected?.Invoke(this, this); }
        }

        public event EventHandler<ToolView> ToolSelected;
    }
}
