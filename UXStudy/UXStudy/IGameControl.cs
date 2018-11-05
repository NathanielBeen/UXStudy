using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{ 
    //the view models for custom controls will inherit from this
    public interface IGameControl
    {
        int ControlID { get; }
        ControlType ControlType { get; }
        string Title { get; }
        string Instructions { get; }
        //whether the user must set this control to pass
        bool Correct { get; }

        //used in between menus
        void reset();

        //when a user has clicked on a textbox or combobox, ect.
        event EventHandler<ClickEvent> ControlChangeStarted;

        //when a user has selected an option or exited the control
        event EventHandler<ClickEvent> ControlChanged;
    }

    //allows the id of a control and whether a change was correct to be passed as needed
    public class ClickEvent : EventArgs
    {
        public IGameControl Control { get; }
        public DateTime Time { get; }

        public ClickEvent(IGameControl control, DateTime time)
        {
            Control = control;
            Time = time;
        }
    }
}
