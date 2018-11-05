using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class GameState
    {
        public Tool ToolState { get; set; }

        public GameState(Tool tool_state) { ToolState = tool_state; }
    }
}
