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
        ControlType ControlType { get; }
        int ControlID { get; }
        string Title { get; }
        //whether the user must set this control to pass
        bool MustAnswer { get; }

        //when a user has clicked on a textbox or combobox, ect.
        event EventHandler<ClickEvent> ControlChangeStarted;

        //when a user has selected an option or exited the control
        event EventHandler<ClickEvent> ControlChanged;
    }

    //allows the id of a control and whether a change was correct to be passed as needed
    public class ClickEvent : EventArgs
    {
        public int Id { get; }
        public DateTime Time { get; }
        public bool Correct { get; }

        public ClickEvent(int id, DateTime time, bool correct)
        {
            Id = id;
            Time = time;
            Correct = correct;
        }
    }
}
