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
using GameClasses;
using Style = GameClasses.Style;

namespace Checkers
{
    /// <summary>
    /// Game parameter selection window
    /// Processes change game parameters. Give this parameters to CheckersWindow
    /// </summary>

    public partial class MainWindow : Window
    {
        private Mode _selectedMode;
        private Difficult _selectedDifficult;
        private Gameplay _selectedGameplay;
        private Style _selectedStyle;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // Function, that starts the game after button click
            var checkersWindow = new CheckersWindow(_selectedMode, _selectedDifficult, _selectedGameplay, _selectedStyle);
            checkersWindow.Show();
            this.Close();
        }

        private void ChangeDifficult(string newDifficult)
        {
            _selectedDifficult = (Difficult)Enum.Parse(typeof(Difficult), newDifficult);
        }

        private void ChangeStyle(string newStyle)
        {
            _selectedStyle = newStyle switch
            {
                "Green Style" => GameClasses.Style.GreenStyle,
                "Brown Style" => GameClasses.Style.BrownStyle,
                _ => GameClasses.Style.BlueStyle
            };
        }

        private void ChangeGameplay(string newGameplay)
        {
            if (newGameplay == "Pool checkers")
            {
                _selectedGameplay = Gameplay.PoolCheckers;
            }
            else
            {
                _selectedGameplay = (Gameplay)Enum.Parse(typeof(Gameplay), newGameplay);
            }
        }

        private void ChangeMode(string newMode)
        {
            if (newMode == "2 players")
            {
                _selectedMode = Mode.Multiplayer;
                DifficultStackPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                _selectedMode = Mode.SinglePlayer;
                DifficultStackPanel.Visibility = Visibility.Visible;
            }
        }

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            // Function chooses action depends on pressed radioButton group
            RadioButton pressed = (RadioButton)sender;
            switch (pressed.GroupName)
            {
                case "Difficult":
                    ChangeDifficult(pressed.Content.ToString());
                    break;
                case "Gameplay":
                    ChangeGameplay(pressed.Content.ToString());
                    break;
                case "Styles":
                    ChangeStyle(pressed.Content.ToString());
                    break;
                default:
                    ChangeMode(pressed.Content.ToString());
                    break;
            }
        }
    }
}