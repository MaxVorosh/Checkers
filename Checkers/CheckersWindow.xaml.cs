using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GameClasses;
using Style = GameClasses.Style;

namespace Checkers;

public partial class CheckersWindow : Window
{
    // Window, that shows game
    private Mode _gameMode;
    private Difficult _gameDifficult;
    private Gameplay _gameplay;
    private Style _gameStyle;
    private CheckerSprite?[,] _sprites; // Checkers images, that consists of ellipses
    private CheckersBoard _board; // Board, that responsible for business-logic
    private Border _selectedChecker; // Border around selected checker
    private List<Border> _canMoveTiles;
    private bool _gameEnd;
    private SolidColorBrush whiteColor;
    private SolidColorBrush blackColor;
    private SolidColorBrush selectedColor;
    private SolidColorBrush canMoveColor;

    private void BoardClick(object sender, MouseButtonEventArgs e)
    {
        // Event of clicking on the board

        if (_gameEnd)
            return;

        // Calculating cursor coordinates
        var p = e.GetPosition(this);
        p = GameWindow.TranslatePoint(p, BoardBorder);
        var x = Convert.ToInt32(p.X - 2);
        var y = Convert.ToInt32(p.Y - 2);
        if (x < 0 || y < 0 || x >= 400 || y >= 400)
            return;

        // Give coords to the board
        _board.NewCoords(y, x);
        
        // Update interface
        UpdateSelectedLighting(x, y);
        UpdateCanMoveLighting();
        SetIndicatorText();
        UpdateSprites();
        UpdateButtons();
        if (_gameEnd)
        {
            ChangeButtonsAfterGame();
        }
    }

    private void UpdateSelectedLighting(int xCoord, int yCoord)
    {
        // Update lighting area near selected checker
        
        Tuple<int, int> tile = _board.GetTile(yCoord, xCoord);
        if (_board.IsSelectedChecker() && !_board.GetSelectedTile().Equals(tile))
        {
            return; // If there is selected checker, that captures more than one time, we shouldn't remove border
        }

        if (_selectedChecker != new Border())
        {
            Board.Children.Remove(_selectedChecker);
        }
        
        _selectedChecker = new Border();
        
        if (!_board.IsSelectedChecker() || _gameEnd)
            return;

        _selectedChecker.BorderBrush = selectedColor;
        _selectedChecker.BorderThickness = new Thickness(3);
        Board.Children.Add(_selectedChecker);
        Grid.SetColumn(_selectedChecker, tile.Item2);
        Grid.SetRow(_selectedChecker, tile.Item1);
    }

    private void UpdateCanMoveLighting()
    {
        // Remove old and set new borders for tiles, where selected checker can go
        foreach (var border in _canMoveTiles)
        {
            Board.Children.Remove(border);
        }

        if (!_board.IsSelectedChecker())
        {
            return;
        }

        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if (_board.ShouldLight(new Tuple<int, int>(i, j)))
                {
                    Border border = GetCanMoveLighting();
                    _canMoveTiles.Add(border);
                    Board.Children.Add(border);
                    Grid.SetColumn(border, j);
                    Grid.SetRow(border, i);
                }
            }
        }
    }
    
    private Border GetCanMoveLighting()
    {
        var border = new Border();
        border.BorderBrush = canMoveColor;
        border.BorderThickness = new Thickness(2);
        return border;
    }

    private void AddSprite(int x, int y, bool isWhite, bool isMissis)
    {
        // Add a checker's image on the board
        
        var checker = new CheckerSprite(isWhite, isMissis);
        _sprites[x, y] = checker;
        
        Board.Children.Add(checker.MainShape);
        Grid.SetRow(checker.MainShape, x);
        Grid.SetColumn(checker.MainShape, y);
        
        Board.Children.Add(checker.MissisShape);
        Grid.SetRow(checker.MissisShape, x);
        Grid.SetColumn(checker.MissisShape, y);
    }

    private void DeleteAllCheckers()
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if (_sprites[i, j] != null)
                {
                    DeleteSprite(i, j);
                }
            }
        }
    }

    private void DeleteSprite(int x, int y)
    {
        // Delete checker's image from the board
        Board.Children.Remove(_sprites[x, y].MainShape);
        Board.Children.Remove(_sprites[x, y].MissisShape);
    }

    private void UpdateSprites()
    {
        // Redrawing all checkers on the board in relation to position on the board
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                // Delete not empty image
                if (_sprites[i, j] != null)
                {
                    DeleteSprite(i, j);
                }

                // Add new image, if it's necessary
                var checker = _board.GetChecker(new Tuple<int, int>(i, j));
                if (checker.IsExists())
                {
                    AddSprite(i, j, checker.IsWhite(), checker.IsMissis());
                }
            }
        }
    }

    private void SetIndicatorText()
    {
        // Set text on the TextBlock in relation to game status
        Indicator.Text = (_board.IsWhiteTurn) ? "White to move" : "Black to move";
        if (_board.GetResult != Result.NotEnd)
        {
            _gameEnd = true;
            Indicator.Text = _board.GetResult switch
            {
                Result.Draw => "Draw",
                Result.BlackWin => "Black wins",
                _ => "White wins"
            };
        }
    }

    private void AddNotationTextBlock(int row, int column, string text, bool isLeft)
    {
        var textBlock = new TextBlock();
        textBlock.Text = text;
        Board.Children.Add(textBlock);
        Grid.SetColumn(textBlock, column);
        Grid.SetRow(textBlock, row);
        if (isLeft)
        {
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.VerticalAlignment = VerticalAlignment.Top;
        }
        else
        {
            textBlock.HorizontalAlignment = HorizontalAlignment.Right;
            textBlock.VerticalAlignment = VerticalAlignment.Bottom;
        }
        textBlock.FontWeight = FontWeights.Bold;
        textBlock.FontSize = 10;
    }

    private void DrawNotation()
    {
        for (int i = 0; i < 8; ++i)
        {
            Char letter = Convert.ToChar('a' + i);
            AddNotationTextBlock(7, i, letter.ToString(), false);
            AddNotationTextBlock(i, 0, (8 - i).ToString(), true);
        }
    }

    private void SetDefaultParams()
    {
        _sprites = new CheckerSprite[8, 8];
        _board = new CheckersBoard(8, 12, _gameMode, _gameDifficult, _gameplay);
        _gameEnd = false;
        _selectedChecker = new Border();
        _canMoveTiles = new List<Border>();
        UpdateSprites();
        SetIndicatorText();
        UpdateButtons();
    }

    private void SetColors()
    {
        if (_gameStyle == GameClasses.Style.GreenStyle)
        {
            blackColor = new SolidColorBrush(Color.FromRgb(118, 150, 86));
            whiteColor = new SolidColorBrush(Color.FromRgb(238, 238, 210));
            selectedColor = new SolidColorBrush(Color.FromRgb(10, 69, 0));
            canMoveColor = new SolidColorBrush(Color.FromRgb(124, 252, 0));
        }
        else if (_gameStyle == GameClasses.Style.BlueStyle)
        {
            blackColor = new SolidColorBrush(Color.FromRgb(137, 154, 162));
            whiteColor = new SolidColorBrush(Color.FromRgb(254, 254, 252));
            selectedColor = new SolidColorBrush(Color.FromRgb(63, 73, 160));
            canMoveColor = new SolidColorBrush(Color.FromRgb(110, 227, 255));
        }
        else
        {
            blackColor = new SolidColorBrush(Color.FromRgb(181, 110, 68));
            whiteColor = new SolidColorBrush(Color.FromRgb(237, 212, 171));
            selectedColor = new SolidColorBrush(Color.FromRgb(92, 58, 0));
            canMoveColor = new SolidColorBrush(Color.FromRgb(97, 0, 0));
        }
    }

    public void SetGrid()
    {
        BoardBorder.Background = whiteColor;
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i + j) % 2 == 1)
                {
                    var tile = new Border();
                    tile.Background = blackColor;
                    Board.Children.Add(tile);
                    Grid.SetColumn(tile, i);
                    Grid.SetRow(tile, j);
                }
            }
        }
    }

    public CheckersWindow(Mode mode, Difficult difficult, Gameplay gameplay, Style style)
    {
        _gameMode = mode;
        _gameDifficult = difficult;
        _gameplay = gameplay;
        _gameStyle = style;
        InitializeComponent();
        SetColors();
        SetGrid();
        SetDefaultParams();
        DrawNotation();
    }

    private void Resign(object sender, RoutedEventArgs e)
    {
        // Function ends a game, when someone click on "Resign" button
        var btn = (Button)e.Source;
        if (btn.Name == WhiteResign.Name)
        {
            _board.SetResult(Result.BlackWin);
        }
        else
        {
            _board.SetResult(Result.WhiteWin);
        }
        SetIndicatorText();
        ChangeButtonsAfterGame();
    }

    private void Draw(object sender, RoutedEventArgs e)
    {
        // When both CheckBoxes are clicked, than ir's draw
        if (WhiteDraw.IsChecked == true && BlackDraw.IsChecked == true)
        {
            _board.SetResult(Result.Draw);
            SetIndicatorText();
            ChangeButtonsAfterGame();
        }
    }

    private void ChangeButtonsAfterGame()
    {
        WhiteDraw.Visibility = Visibility.Hidden;
        BlackDraw.Visibility = Visibility.Hidden;
        WhiteResign.Visibility = Visibility.Hidden;
        BlackResign.Visibility = Visibility.Hidden;
        WhiteDraw.IsChecked = false;
        BlackDraw.IsChecked = false;
        ShowAfterGameButtons();
    }

    private void UpdateButtons()
    {
        // Hide and show "Resign" and "Draw" buttons
        if (_board.IsWhiteTurn)
        {
            if (_gameMode == Mode.Multiplayer)
            {
                WhiteDraw.Visibility = Visibility.Visible;
            }
            else
            {
                WhiteDraw.Visibility = Visibility.Hidden;
            }

            WhiteResign.Visibility = Visibility.Visible;
            BlackDraw.Visibility = Visibility.Hidden;
            BlackResign.Visibility = Visibility.Hidden;
        }
        else
        {
            WhiteDraw.Visibility = Visibility.Hidden;
            WhiteResign.Visibility = Visibility.Hidden;
            BlackDraw.Visibility = Visibility.Visible;
            BlackResign.Visibility = Visibility.Visible;
        }
    }

    private void ShowAfterGameButtons()
    {
        ToMenuButton.Visibility = Visibility.Visible;
        RematchButton.Visibility = Visibility.Visible;
    }

    private void HideAfterGameButtons()
    {
        ToMenuButton.Visibility = Visibility.Hidden;
        RematchButton.Visibility = Visibility.Hidden;
    }

    private void BackToMenu(object sender, RoutedEventArgs e)
    {
        var menu = new MainWindow();
        menu.Show();
        this.Close();
    }

    private void Rematch(object sender, RoutedEventArgs e)
    {
        // Creates new values (start values) and update form
        DeleteAllCheckers();
        SetDefaultParams();
        HideAfterGameButtons();
    }
}