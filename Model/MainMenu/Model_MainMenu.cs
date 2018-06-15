using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Model.MainMenu
{

    public class DimentionOption
    {
        public int Value
        {
            get;
        }

        public DimentionOption(int value)
        {
            Value = value;
        }
    }


    public class GameInformation
    {
        private int h, w;
        public int Width
        {
            get
            { return w; }
            set
            {
                w = value;
            }
        }
        public int Height
        {
            get
            { return h; }
            set
            {
                h = value;
            }
        }

        private List<Color> colors;

        public List<Color> Colors
        {
            get { return colors; }

            set { colors = value; }
        }


        public PlayerInfo Player_One { get; set; }
        public PlayerInfo Player_Two { get; set; }

        public PlayerInfo GetPlayerInfo(int index)
        {
            switch (index)
            {
                case 1:
                    return Player_One;
                case 2:
                    return Player_Two;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public GameInformation() : this(new PlayerInfo("Player Uno", Color.FromArgb(255, 0, 0, 0)), new PlayerInfo("Player Dos", Color.FromArgb(255, 255, 255, 255)), -1, -1)
        {
            //NOP :p 
        }

        private GameInformation(PlayerInfo p1, PlayerInfo p2, int w, int h)
        {
            this.Player_One = p1;
            this.Player_Two = p2;
            Width = w;
            Height = h;

            Colors = new List<Color>
            {
                Color.FromRgb(0,0,0),
                Color.FromRgb(255,255,255),
                Color.FromRgb(255,0,0),
                Color.FromRgb(255,255,0),
                Color.FromRgb(0,255,0),
                Color.FromRgb(0,255,255),
                Color.FromRgb(0,0,255),
                Color.FromRgb(255,0,255)
            };
            Debug.WriteLine("w: " + w + ", h: " + h + ", C1: " + p1.Color + ", N1: " + p1.Name + ", C2: " + p2.Color + ", N2: " + p2.Name + "");
            Debug.WriteLine("_________________________________________________________________________________________________________________");

        }

        public GameInformation ChangeDimention(string type, int newValue)
        {
            switch (type)
            {
                case "height":
                    return new GameInformation(Player_One, Player_Two, w, newValue);
                case "width":
                    return new GameInformation(Player_One, Player_Two, newValue, h);
                default: return this;
            }
        }


        public bool IsAlreadyChosen(int i)
        {
            Color c = Colors[i];
            return Player_One.Color == c || Player_Two.Color == c;
        }

        public Color GetColor(int index)
        {
            return Colors[index];
        }

        public GameInformation SetColor(int i, int c)
        {
            PlayerInfo playerInfo;
            Color color = colors[c];
            switch (i)
            {
                case 1:
                    playerInfo = Player_One.ChangeColor(color);
                    return new GameInformation(playerInfo, Player_Two, w, h);
                case 2:
                    playerInfo = Player_Two.ChangeColor(color);
                    return new GameInformation(Player_One, playerInfo, w, h);
                default:
                    return this;

            }
        }

        public override string ToString()
        {
            return "Height: " + h + "\n"
                + "width: " + w + "\n"
                + "Player 1: [ Name: " + Player_One.Name + ", Color: "+Player_One.Color+ " ]\n"
                + "Player 2: [ Name: " + Player_Two.Name + ", Color: " + Player_Two.Color + " ]\n"
                + "####################################################";
        }

        public bool CompleteInformation()
        {
            return (h >= 2 && h <= 20
                && w >= 2 && w <= 20
                && Player_One.Name != "" && Player_One.Color != null
                && Player_Two.Name != "" && Player_Two.Color != null
                && Player_One.Name != Player_Two.Name && Player_One.Color != Player_Two.Color);
        }
    }

    public class PlayerInfo
    {
        private string name;
        public string Name { get { return name; } set { name = value; Debug.WriteLine("NAME CHANGED " + value); } }

        private Color color;

        public Color Color { get { return color; } set { color = value; Debug.WriteLine("ColorChanged: " + value); } }

        public PlayerInfo(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public PlayerInfo ChangeName(string name)
        {
            return new PlayerInfo(name, this.color);
        }
        public PlayerInfo ChangeColor(Color color)
        {
            return new PlayerInfo(this.name, color);
        }
    }
}
