using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UXStudy
{
    public class WireGroup : BaseViewModel
    {
        private GameState state;

        private ObservableCollection<Wire> wires;
        public ObservableCollection<Wire> Wires
        {
            get { return wires; }
            set { SetProperty(ref wires, value); }
        }

        public List<Connection> TopConnections { get; private set; }
        public List<Connection> BottomConnections { get; private set; }
        public int Size { get; }

        public WireGroup(GameState st, int size, List<WireGuide> wire_list)
        {
            state = st;
            Size = size;
            initConnections();
            initWires(wire_list);
        }

        private void initConnections()
        {
            List<Connection> top_list = new List<Connection>();
            List<Connection> bottom_list = new List<Connection>();

            char label = 'A';
            for (int index = 0; index < Size; index++)
            {
                Connection top = new Connection(index.ToString());
                Connection bottom = new Connection(label.ToString());

                top.ConnSelected += handleConnectionSelected;
                top.ConnDeselected += handleConnectionDeselected;
                bottom.ConnSelected += handleConnectionSelected;
                bottom.ConnDeselected += handleConnectionDeselected;

                top_list.Add(top);
                bottom_list.Add(bottom);

                label++;
            }

            TopConnections = top_list;
            BottomConnections = bottom_list;
        }

        private void initWires(List<WireGuide> wire_list)
        {
            Wires = new ObservableCollection<Wire>();

            foreach (var wire_guide in wire_list)
            {
                Connection top = TopConnections[wire_guide.Top];
                Connection bottom = BottomConnections[wire_guide.Bottom];

                Wire wire = new Wire(top, bottom, wire_guide.Color);
                wire.Clicked += handleWireClicked;

                Wires.Add(wire);
            }
        }

        private void handleConnectionSelected(object sender, Connection selected)
        {
            bool conn_is_top = TopConnections.Contains(selected);

            if (conn_is_top) { TopConnections.ForEach(c => c.Selected = c.Equals(selected)); }
            else { BottomConnections.ForEach(c => c.Selected = c.Equals(selected)); }

            checkForWireCreation();
        }

        private void handleConnectionDeselected(object sender, Connection deselected)
        {
            deselected.Selected = false;
        }

        private void handleWireClicked(object sender, Wire wire)
        {
            if (state.ToolState == Tool.WIRE_CUTTER) { Wires.Remove(wire); }
        }

        private void checkForWireCreation()
        {
            Connection top_selected = TopConnections.Where(c => c.Selected = true).FirstOrDefault();
            Connection bottom_selected = BottomConnections.Where(c => c.Selected = true).FirstOrDefault();

            if (top_selected != null && bottom_selected != null)
            {
                if (state.ToolState.isWire()) { createWire(top_selected, bottom_selected); }
                else { resetConnections(); }
            }
        }

        private void resetConnections()
        {
            TopConnections.ForEach(c => c.Selected = false);
            BottomConnections.ForEach(c => c.Selected = false);
        }

        private void createWire(Connection top, Connection bottom)
        {
            Wire prev_wire = Wires.Where(w => w.TopConnect == top && w.BottomConnect == bottom).FirstOrDefault();
            if (prev_wire != null) { Wires.Remove(prev_wire); }

            Wires.Add(new Wire(top, bottom, state.ToolState.getWireColor()));
        }
    }

    public class WireGuide
    {
        public int Top { get; }
        public int Bottom { get; }
        public Color Color { get; }

        public WireGuide(int top, int bottom, Color color)
        {
            Top = top;
            Bottom = bottom;
            Color = color;
        }
    }

    public class Wire
    {
        public Connection TopConnect { get; }
        public Connection BottomConnect { get; }
        public Color Color { get; }

        public RelayCommand ClickCommand { get; private set; }

        public Wire(Connection top, Connection bottom, Color color)
        {
            TopConnect = top;
            BottomConnect = bottom;
            Color = color;

            initCommands();
        }

        private void initCommands()
        {
            ClickCommand = new RelayCommand(handleClick);
        }

        private void handleClick()
        {
            Clicked?.Invoke(this, this);
        }

        public event EventHandler<Wire> Clicked;
    }
    
    public class Connection : BaseViewModel
    {
        public string Title { get; private set; }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        public RelayCommand SelectCommand { get; private set; }
        public RelayCommand DeselectCommand { get; private set; }

        public Connection(string title)
        {
            Selected = false;
            initCommands();
        }

        private void initCommands()
        {
            SelectCommand = new RelayCommand(mouseSelect);
            DeselectCommand = new RelayCommand(mouseDeselect);
        }

        private void mouseSelect()
        {
            Selected = true;
            ConnSelected?.Invoke(this, this);
        }

        private void mouseDeselect()
        {
            Selected = false;
            ConnDeselected?.Invoke(this, this);
        }

        public event EventHandler<Connection> ConnSelected;
        public event EventHandler<Connection> ConnDeselected;
    }
}
