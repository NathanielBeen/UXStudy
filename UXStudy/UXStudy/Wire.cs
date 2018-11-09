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
        private List<Wire> correct;
        private List<Wire> init;
        private bool complete;

        private ObservableCollection<Wire> wires;
        public ObservableCollection<Wire> Wires
        {
            get { return wires; }
            set { SetProperty(ref wires, value); }
        }

        public List<Connection> TopConnections { get; private set; }
        public List<Connection> BottomConnections { get; private set; }
        public int Size { get; }

        public WireGroup(GameState st, int size, List<WireGuide> correct_list, List<WireGuide> init_list)
        {
            state = st;
            Size = size;

            initConnections();
            initCorrectWires(correct_list);
            initStartWires(init_list);
        }

        private void initConnections()
        {
            List<Connection> top_list = new List<Connection>();
            List<Connection> bottom_list = new List<Connection>();

            char label = 'A';
            for (int index = 0; index < Size; index++)
            {
                Connection top = new Connection(index.ToString(), true, index);
                Connection bottom = new Connection(label.ToString(), false, index);

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

        private void initCorrectWires(List<WireGuide> guide)
        {
            correct = new List<Wire>();

            foreach (var wire_guide in guide)
            {
                Connection top = TopConnections[wire_guide.Top];
                Connection bottom = BottomConnections[wire_guide.Bottom];

                Wire correct_wire = new Wire(top, bottom, wire_guide.Color);
                correct.Add(correct_wire);
            }
        }

        private void initStartWires(List<WireGuide> guide)
        {
            Wires = new ObservableCollection<Wire>();
            init = new List<Wire>();

            foreach (var wire_guide in guide)
            {
                Connection top = TopConnections[wire_guide.Top];
                Connection bottom = BottomConnections[wire_guide.Bottom];

                Wire init_wire = new Wire(top, bottom, wire_guide.Color);
                init.Add(init_wire);

                Wire wire = new Wire(top, bottom, wire_guide.Color);
                wire.Clicked += handleWireClicked;

                Wires.Add(wire);
            }

            checkIfGroupComplete();
        }

        public void resetWires()
        {
            foreach (var wire in init)
            {
                wire.Clicked -= handleWireClicked;
            }
            Wires.Clear();

            foreach (var wire_guide in init)
            {
                Wire wire = new Wire(wire_guide.TopConnect, wire_guide.BottomConnect, wire_guide.Color);
                wire.Clicked += handleWireClicked;

                Wires.Add(wire);
            }

            checkIfGroupComplete();
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
            if (state.ToolState == Tool.WIRE_CUTTER)
            {
                Wires.Remove(wire);
                checkIfGroupComplete();
            }
        }

        private void checkForWireCreation()
        {
            Connection top_selected = TopConnections.Where(c => c.Selected == true).FirstOrDefault();
            Connection bottom_selected = BottomConnections.Where(c => c.Selected == true).FirstOrDefault();

            if (top_selected != null && bottom_selected != null)
            {
                if (state.ToolState.isWire())
                {
                    createWire(top_selected, bottom_selected);
                    checkIfGroupComplete();
                }
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

        private void checkIfGroupComplete()
        {
            foreach (Wire wire in correct)
            {
                Wire matching = Wires.Where(w => w.TopConnect.Equals(wire.TopConnect) && w.BottomConnect.Equals(wire.BottomConnect)
                    && colorMatches(w.Color, wire.Color)).FirstOrDefault();
                
                if (matching == null)
                {
                    if (complete == true) { NotCompleted?.Invoke(this, new EventArgs()); }
                    complete = false;
                    return;
                }
            }

            Completed?.Invoke(this, new EventArgs());
        }

        private bool colorMatches(Color first, Color second)
        {
            return (first.R == second.R && first.G == second.G && first.B == second.B);
        }

        public event EventHandler Completed;
        public event EventHandler NotCompleted;
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
        public string Title { get;}
        public bool IsTop { get; }

        public int Top { get; private set; }
        public int Left { get; private set; }

        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        public RelayCommand SelectCommand { get; private set; }

        public Connection(string title, bool top, int index)
        {
            Title = title;
            IsTop = top;

            Selected = false;
            initCommands();
            genCoords(top, index);
        }

        private void initCommands()
        {
            SelectCommand = new RelayCommand(mouseSelect);
        }

        private void genCoords(bool top, int index)
        {
            Top = (top) ? 0 : 100;
            Left = index * 15;
        }

        private void mouseSelect()
        {
            if (Selected)
            {
                Selected = false;
                ConnDeselected?.Invoke(this, this);
            }
            else
            {
                Selected = true;
                ConnSelected?.Invoke(this, this);
            }
        }

        public event EventHandler<Connection> ConnSelected;
        public event EventHandler<Connection> ConnDeselected;
    }
}
