using Cells;
using Model.MainMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ViewModel
{

    public class MenuViewModel: WorkspaceViewModel
    {
        private Cell<GameInformation> gameInfo;
        public Cell<GameInformation> GameInformation
        {
            get { return gameInfo; }
            set { gameInfo = value; }
        }

        public IEnumerable<int> DimentionOptions
        {
            get;
        }

        public Cell<int> GetWidth { get; }

        public Cell<int> GetHeight { get; }

        public Cell<PlayerInfo> GetPlayerOne { get; }
        public PlayerInformationViewModel PlayerOne { get; }

        public Cell<PlayerInfo> GetPlayerTwo { get; }
        public PlayerInformationViewModel PlayerTwo { get; }

        public IEnumerable<ColorPickerViewModel> ColorsP1 { get; }
        public IEnumerable<ColorPickerViewModel> ColorsP2 { get; }

        public MenuViewModel()
        {

            gameInfo = Cell.Create<GameInformation>(new GameInformation());

            PlayerOne = new PlayerInformationViewModel(gameInfo, 1);
            PlayerTwo = new PlayerInformationViewModel(gameInfo, 2);

            List<ColorPickerViewModel> colors = new List<ColorPickerViewModel>();
            for (int i = 0; i != gameInfo.Value.Colors.Count; ++i)
            {
                colors.Add(new ColorPickerViewModel(gameInfo, i, 1, this));
            }
            ColorsP1 = colors;
            List<ColorPickerViewModel> colors2 = new List<ColorPickerViewModel>();
            for (int i = 0; i != gameInfo.Value.Colors.Count; ++i)
            {
                colors2.Add(new ColorPickerViewModel(gameInfo, i, 2, this));
            }
            ColorsP2 = colors2;

            this.GetHeight = this.gameInfo.Derive(h => h.Height);
            this.GetWidth = this.gameInfo.Derive(w => w.Width);
            this.GetPlayerOne = this.gameInfo.Derive(p1 => p1.Player_One);
            this.GetPlayerTwo = this.gameInfo.Derive(p2 => p2.Player_Two);

            List<int> options = new List<int>();
            for (int i = 2; i <= 20; i += 2)
            {
                
                options.Add(i);
            }
            DimentionOptions = options;

        }

        
    }
    public class PlayerInformationViewModel
    {
        public Cell<GameInformation> info;

        public Cell<string> Name { get; set; }

        public Cell<Color> Color
        {
            get; set;
        }

        private int PlayerNumber;

        public PlayerInformationViewModel(Cell<GameInformation> info, int index)
        {
            this.info = info;
            PlayerNumber = index;
            Color = this.info.Derive(c => c.GetPlayerInfo(PlayerNumber).Color);
            Name = this.info.Derive(n => n.GetPlayerInfo(PlayerNumber).Name);
        }

    }

    public class ColorPickerViewModel
    {
        public Cell<GameInformation> info;
        public int playerIndex;
        public int colorIndex;
        private ICommand pickColor;
        public MenuViewModel parent;

        public Cell<Color> Color { get; }

        public ICommand PickColor
        {
            get { return pickColor; }
            set { pickColor = value; }
        }

        public ColorPickerViewModel(Cell<GameInformation> info, int index, int pI, MenuViewModel parent)
        {
            this.colorIndex = index;
            this.info = info;
            this.playerIndex = pI;
            this.Color = info.Derive(c => c.GetColor(colorIndex));

            this.parent = parent;
            this.PickColor = new ColorButton(this);
        }

        public void ChooseColor()
        {
            info.Value = info.Value.SetColor(playerIndex, colorIndex);
        }
    }

    public class ColorButton : ICommand
    {
        private ColorPickerViewModel color;

        public event EventHandler CanExecuteChanged;

        public ColorButton(ColorPickerViewModel colorPicker)
        {
            color = colorPicker;
            color.parent.GameInformation.ValueChanged += () => CanExecuteChanged(this, EventArgs.Empty);
        }



        public bool CanExecute(object parameter)
        {
            //return true;
            return !color.info.Value.IsAlreadyChosen(color.colorIndex);
        }

        public void Execute(object parameter)
        {
            color.ChooseColor();
        }
    }
}
