using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Mode SelectedMode;
        private Difficult SelectedDifficult;
        private Gameplay selectedGameplay;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            CheckersWindow checkersWindow = new CheckersWindow(SelectedMode, SelectedDifficult, selectedGameplay);
            checkersWindow.Show();
            this.Close();
        }

        private void ChangeDifficult(string newDifficult)
        {
            SelectedDifficult = (Difficult)Enum.Parse(typeof(Difficult), newDifficult);
        }

        private void ChangeGameplay(string newGameplay)
        {
            if (newGameplay == "Русские")
            {
                selectedGameplay = Gameplay.Russian;
            }
            else if (newGameplay == "Поддавки")
            {
                selectedGameplay = Gameplay.Giveaway;
            }
            else
            {
                selectedGameplay = Gameplay.International;
            }
        }

        private void ChangeMode(string newMode)
        {
            if (newMode == "2 players")
            {
                SelectedMode = Mode.Multiplayer;
                DifficultStackPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                SelectedMode = Mode.SinglePlayer;
                DifficultStackPanel.Visibility = Visibility.Visible;
            }
        }

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            if (pressed.GroupName == "Difficult")
            {
                ChangeDifficult(pressed.Content.ToString());
            }
            else if (pressed.GroupName == "Gameplay")
            {
                ChangeGameplay(pressed.Content.ToString());
            }
            else
            {
                ChangeMode(pressed.Content.ToString());
            }
        }
    }

    /*public static void Main(string[] args)
    {
        CheckersBoard board = new CheckersBoard(8, 12);
        board.Print();
        string s = Console.ReadLine()!;
        while (s != "end")
        {
            int a = int.Parse(s.Split(' ')[0]);
            int b = int.Parse(s.Split(' ')[1]);
            board.NewTile(new Tuple<int, int>(a, b));
            board.Print();
            s = Console.ReadLine()!;
        }
    }*/
}