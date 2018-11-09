using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXStudy
{
    public class WireGroupControl : BaseViewModel, IGameControl
    {
        public WireGroup WireView { get; }

        public int ControlID { get; }
        public ControlType ControlType { get { return ControlType.WIRE; } }
        public string Title { get; }
        public string Instructions { get; }

        public bool Correct { get; private set; }

        public WireGroupControl(WireGroup wire)
        {
            WireView = wire;
            wire.Completed += handleCompleted;
            wire.NotCompleted += handleIncomplete;
        }

        public event EventHandler<ClickEvent> ControlChangeStarted;
        public event EventHandler<ClickEvent> ControlChanged;

        public void reset() { WireView.resetWires(); }

        private void handleCompleted(object sender, EventArgs args)
        {
            Correct = true;
            ControlChanged?.Invoke(this, new ClickEvent(this, DateTime.Now));
        }

        private void handleIncomplete(object sender, EventArgs args)
        {
            Correct = false;
            ControlChanged?.Invoke(this, new ClickEvent(this, DateTime.Now));
        }
    }
}
