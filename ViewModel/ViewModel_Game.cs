using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataStructures;
using Model.Reversi;
using Model.MainMenu;
using Cells;
using System.Windows.Media;
using System.Windows;

namespace ViewModel
{
    public class BoardViewModel : WorkspaceViewModel
    {
        Cell<ReversiGame> game;

        public Cell<ReversiGame> Game
        {
            get { return game; }
            set
            {
                game = value;
            }
        }
        
        public BoardViewModel(Cell<GameInformation> info)
        {
            Game = Cell.Create<ReversiGame>(new ReversiGame(info.Value.Width, info.Value.Height));
            Debug.WriteLine(info.ToString());
            ReversiBoard board = Game.Value.Board;
            List<BoardRowViewModel> rows = new List<BoardRowViewModel>();
            P1Name = info.Value.Player_One.Name;
            P2Name = info.Value.Player_Two.Name;
            for (int i = 0; i != board.Height; ++i)
            {
                List<BoardSquareViewModel> squares = new List<BoardSquareViewModel>();
                for(int j = 0; j != board.Width; ++j)
                {
                    squares.Add(new BoardSquareViewModel(i, j,Game, info.Value.Player_One.Color, info.Value.Player_Two.Color));
                }
                rows.Add(new BoardRowViewModel(squares));
            }
            this.Rows = rows;
            this.BlackCount = this.game.Derive(g => g.Board.CountStones(Player.BLACK));
            this.WhiteCount = this.game.Derive(g => g.Board.CountStones(Player.WHITE));
            this.CurrentPlayer = this.game.Derive(g => g.CurrentPlayer);
        }

        public string P1Name { get; }

        public string P2Name { get; }
        public Cell<int> WhiteCount
        {
            get;
        }
        public Cell<int> BlackCount
        {
            get;
        }

        public IEnumerable<BoardRowViewModel> Rows
        {
            get;
        }
        
        public Cell<Player> CurrentPlayer
        {
            get;
        }

    }
    public class BoardRowViewModel
    {
        public BoardRowViewModel(List<BoardSquareViewModel> squares)
        {
            Squares = squares;
        }

        public IEnumerable<BoardSquareViewModel> Squares
        {
            get;
        }
        
    }
    public class BoardSquareViewModel

    {
        public Color color, c1, c2;
        public Cell<ReversiGame> game;

        public Cell<Player> Owner
        {
            get;
        }

        public Color GetColor
        {
            get
            {
                return color;
            }
        }

        public Vector2D Pos { get; }
        private ICommand putStone;

        public BoardSquareViewModel(int y, int x, Cell<ReversiGame> game, Color p1, Color p2)
        {
            Pos = new Vector2D(x, y);
            this.game = game;
            Owner = this.game.Derive(g =>  g.Board[Pos]);
            this.putStone = new PutStone(this);
        }
        public ICommand PutStone
        {
            get { return putStone; }
            set
            {
                putStone = value;
            }
        }
        public void PlacePiece()
        {
            game.Value = game.Value.PutStone(this.Pos);
        }
    }
    public class PutStone : ICommand
    {
        private BoardSquareViewModel square;

        public PutStone(BoardSquareViewModel square)
        {
            this.square = square;
            this.square.game.ValueChanged += () => CanExecuteChanged(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return square.game.Value.IsValidMove(square.Pos);
        }

        public void Execute(object parameter)
        {
            Debug.WriteLine($"at {square.Pos} placed a piece");
            square.PlacePiece();
        }
    }
}
