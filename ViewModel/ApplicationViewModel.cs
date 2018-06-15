using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace ViewModel
{
    public class ApplicationViewModel: ObservableObject
    {
        public ICommand NewGameMenu { get; }

        public ICommand NewGame { get; }

        public MenuViewModel Menu { get; set; }

        public BoardViewModel Board { get; set; }

        private WorkspaceViewModel currentWorkSpace;
        public WorkspaceViewModel CurrentWorkSpace
        {
            get { return currentWorkSpace; }
            set
            {
                if (currentWorkSpace != value)
                {
                    currentWorkSpace = value;
                    OnPropertyChanged();
                }
            }
        }

        public ApplicationViewModel()
        {
            NewGameMenu = new RelayCommand((p) => CreateNewGame());
            NewGame = new RelayCommand((p) => NewGameExecute(), () => CanStartNewGame());
        }

        public void CreateNewGame()
        {
            Menu = new MenuViewModel();
            CurrentWorkSpace = Menu;
        }

        public bool CanStartNewGame()
        {
            return Menu.GameInformation.Value.CompleteInformation();
        }

        public void NewGameExecute()
        {
            Debug.WriteLine("NewGame from AppVM");
            Board = new BoardViewModel(Menu.GameInformation);
            CurrentWorkSpace = Board;
            Prefs.PlayerBlack =  Menu.PlayerOne.Color.Value;
            Prefs.PlayerWhite = Menu.PlayerTwo.Color.Value;
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute,null)
        { }

        public RelayCommand(Action<object> execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = execute;
            this.canExecute = canExecute;
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }

    public abstract class WorkspaceViewModel : ObservableObject { }

    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
