using System;

namespace Checkers;

public class CheckersBoard
{
    // Class, that responsible for compliance game rules

    private readonly int _size; // count of board's rows and columns 
    private bool _isWhiteTurn;
    private int _rule15; // Count of missis's moves without captures
    private readonly Board _board; // Checkers position
    private Tuple<int, int> _currentTile; // Tile with a selected checker
    private bool _isMoveStarted; // True, if checker capture something and can capture something else
    private Result _result; // Game status

    public CheckersBoard(int size, int cnt)
    {
        _size = size;
        _isWhiteTurn = true;
        _rule15 = 0;
        _board = new Board(size, cnt);
        _currentTile = new Tuple<int, int>(-1, -1); // This value shows, that no checker is selected
        _isMoveStarted = false; // Should player continue his turn if he already moved a checker
        _result = Result.NotEnd;
    }

    private Tuple<int, int> GetMultipliers(Tuple<int, int> from, Tuple<int, int> to)
    {
        // Returns direction, in which checker should go from start tile to last
        // E.g. (1, -1) - checker should go to axis X and facing axis Y

        int xMultiplier = (from.Item1 < to.Item1) ? 1 : -1;
        int yMultiplier = (from.Item2 < to.Item2) ? 1 : -1;
        return new Tuple<int, int>(xMultiplier, yMultiplier);
    }

    private int GetLength(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
    {
        // How many tiles should checker go (tiles are on the one diagonal)
        return Math.Abs(tileFrom.Item1 - tileTo.Item1);
    }

    private bool CanMissisMove(Tuple<int, int> tileFrom, Tuple<int, int> tileTo, bool mustCapture)
    {
        // Part of CanMove function, when checker is missis
        var currentChecker = _board.GetChecker(tileFrom);
        int length = GetLength(tileFrom, tileTo);

        // Get a direction
        var multipliers = GetMultipliers(tileFrom, tileTo);
        int xMultiplier = multipliers.Item1;
        int yMultiplier = multipliers.Item2;

        int canTake = 0; // How many not empty tiles on the checker's way

        // Checking the checker's way
        for (int i = 1; i <= length; ++i)
        {
            int currentX = tileFrom.Item1 + i * xMultiplier;
            int currentY = tileFrom.Item2 + i * yMultiplier;
            var currentTile = new Tuple<int, int>(currentX, currentY); // tile on the way

            if (_board.GetChecker(currentTile).IsExists())
            {
                if (_board.GetChecker(currentTile).IsWhite() == currentChecker.IsWhite())
                    return false; // The same colored checker on the way - we can't move like this
                
                canTake++; // The other colored checker on the way - we must capture it
            }
        } 
        // We can't eat more than one checker by checker's move
        if (mustCapture)
            return canTake == 1;
        
        return canTake <= 1;
    }

    private bool CanMove(Tuple<int, int> tileFrom, Tuple<int, int> tileTo, bool mustCapture)
    {
        // Can checker move from start tile to last. If mustCapture is true, than checker should capture something

        if (Math.Abs(tileFrom.Item1 - tileTo.Item1) != Math.Abs(tileFrom.Item2 - tileTo.Item2) ||
            tileFrom.Item1 == tileTo.Item1 || _board.GetChecker(tileTo).IsExists())
        {
            // If tileTo has a checker or tileFrom == tileTo or there is no diagonal move, checker can't move
            return false;
        }

        var currentChecker = _board.GetChecker(tileFrom);
        int length = GetLength(tileFrom, tileTo);

        if (currentChecker.IsMissis())
        {
            return CanMissisMove(tileFrom, tileTo, mustCapture);
        }

        // Now our checker isn't missis

        if (length > 2)
            return false; // Regular checker can't move more than 2 tiles

        if (length == 1)
        {
            // Checker can't move one tile and capture something
            // Check that checkers can't move back. Last tile is empty, as we check earlier 
            return ((currentChecker.IsWhite() && tileTo.Item1 < tileFrom.Item1) ||
                   (!currentChecker.IsWhite() && tileTo.Item1 > tileFrom.Item1)) && !mustCapture;
        }

        // We should capture a checker on the middle of our way
        var middleTile = new Tuple<int, int>((tileFrom.Item1 + tileTo.Item1) / 2,
            (tileFrom.Item2 + tileTo.Item2) / 2);
        return _board.GetChecker(middleTile).IsExists() &&
               _board.GetChecker(middleTile).IsWhite() != currentChecker.IsWhite();
    }

    private void Move(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
    {
        // Moves checker if it's possible, and delete captured checkers 
        
        if (!CanMove(tileFrom, tileTo, false))
            return;

        _board.MoveChecker(tileFrom, tileTo); // Actually move the checker
        
        var multipliers = GetMultipliers(tileFrom, tileTo); // Get direction
        int xMultiplier = multipliers.Item1;
        int yMultiplier = multipliers.Item2;

        int length = GetLength(tileFrom, tileTo);
        
        for (int i = 1; i < length; ++i)
        {
            // Delete checkers on the way
            int currentX = tileFrom.Item1 + i * xMultiplier;
            int currentY = tileFrom.Item2 + i * yMultiplier;
            var currentTile = new Tuple<int, int>(currentX, currentY);
            _board.DeleteChecker(currentTile);
        }

        // If the checker in the other side of the board, it should become missis
        if ((tileTo.Item1 == 0 && _board.GetChecker(tileTo).IsWhite())
            || (tileTo.Item1 == _size - 1 && !_board.GetChecker(tileTo).IsWhite()))
        {
            _board.GetChecker(tileTo).BecomeMissis();
        }
    }

    private bool CanCaptureTile(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
    {
        //??
        // Is checker capture something on the way it goes
        if (!CanMove(tileFrom, tileTo, false))
            return false;

        var multipliers = GetMultipliers(tileFrom, tileTo); // Get direction
        int xMultiplier = multipliers.Item1;
        int yMultiplier = multipliers.Item2;
        
        int length = GetLength(tileFrom, tileTo);
        
        for (int i = 1; i < length; ++i)
        {
            int currentX = tileFrom.Item1 + xMultiplier * i;
            int currentY = tileFrom.Item2 + yMultiplier * i;
            if (_board.GetChecker(new Tuple<int, int>(currentX, currentY)).IsExists())
            {
                return true;
            }
        }
        return false;
    }

    private void RejectCurrentTile()
    {
        _currentTile = new Tuple<int, int>(-1, -1);
    }

    private bool IsValidTile(Tuple<int, int> tile)
    {
        // Check is tile's coordinates in range
        return (0 <= tile.Item1 && 0 <= tile.Item2 && tile.Item1 < _size && tile.Item2 < _size);
    }

    private Tuple<int, int>[] GetDiagonalMoves(Tuple<int, int> tile, int dist)
    {
        // returns array with tiles, that can be visit by one diagonal move, on the distance dist
        var upLeft = new Tuple<int, int>(tile.Item1 + dist, tile.Item2 - dist);
        var upRight = new Tuple<int, int>(tile.Item1 + dist, tile.Item2 + dist);
        var downLeft = new Tuple<int, int>(tile.Item1 - dist, tile.Item2 - dist);
        var downRight = new Tuple<int, int>(tile.Item1 - dist, tile.Item2 + dist);
        Tuple<int, int>[] moves = { upLeft, upRight, downLeft, downRight };
        return moves;
    }

    private bool CanCaptureColor(Tuple<int, int> tile)
    {
        // Can this checker make move, that capture something
        for (int i = 2; i <= _size; ++i)
        {
            var moves = GetDiagonalMoves(tile, i);
            foreach (var newTile in moves)
            {
                if (IsValidTile(newTile) && CanCaptureTile(tile, newTile))
                    return true;
            }
        }
        return false;
    }

    private bool CanCaptureSmth(bool color)
    {
        // Can someone checker of this color make move, that capture something
        for (int i = 0; i < _size; ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                var currentChecker = _board.GetChecker(new Tuple<int, int>(i, j));
                if (currentChecker.IsExists() && currentChecker.IsWhite() == color &&
                    CanCaptureColor(new Tuple<int, int>(i, j)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool CanCheckerMove(Tuple<int, int> tile)
    {
        // Can checker move somewhere
        for (int i = 1; i <= _size; ++i)
        {
            var moves = GetDiagonalMoves(tile, i);
            foreach (var newTile in moves)
            {
                if (IsValidTile(newTile) && CanMove(tile, newTile, false))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CanColorMove(bool isWhite)
    {
        // Can someone checker of this color move somewhere
        bool canMove = false;
        for (int i = 0; i < _size; ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                var tile = new Tuple<int, int>(i, j);
                if (_board.GetChecker(tile).IsExists() && isWhite == _board.GetChecker(tile).IsWhite())
                {
                    canMove |= CanCheckerMove(tile);
                }
            }
        }

        return canMove;
    }

    private void UpdateResult()
    {
        if (_rule15 == 30)
        {
            _result = Result.Draw;
        }
        if (_isWhiteTurn && !CanColorMove(_isWhiteTurn))
        {
            _result = Result.BlackWin;
        }
        else if (!_isWhiteTurn && !CanColorMove(_isWhiteTurn))
        {
            _result = Result.WhiteWin;
        }
    }

    private void Turn(Tuple<int, int> tileTo)
    {
        // Makes turn by 2 tiles - current and new
        
        // If move is illegal (we can't move or we should move something else to capture), forget current tile
        if (!CanMove(_currentTile, tileTo, CanCaptureSmth(_isWhiteTurn)))
        {
            if (!_isMoveStarted)
                RejectCurrentTile();
            return;
        }

        _isMoveStarted = true;
        bool isCaptured = CanCaptureTile(_currentTile, tileTo); // When checker moves, was it captured something

        if (_board.GetChecker(_currentTile).IsMissis() && !isCaptured)
            _rule15++;
        else
            _rule15 = 0;

        Move(_currentTile, tileTo);
        _currentTile = tileTo;
        
        // If checker captured something, we check, can it capture something else
        if (isCaptured)
        {
            if (CanCaptureColor(_currentTile))
                return;
        }

        // When our turn is end
        RejectCurrentTile();
        _isMoveStarted = false;
        _isWhiteTurn = !_isWhiteTurn;
        UpdateResult();
    }

    private void NewTile(Tuple<int, int> tile)
    {
        // Gets a new tile. Remember it or make turn
        if (_currentTile.Equals(new Tuple<int, int>(-1, -1)))
        {
            if (_isWhiteTurn == _board.GetChecker(tile).IsWhite() && _board.GetChecker(tile).IsExists())
            {
                _currentTile = tile;
            }
        }
        else
        {
            Turn(tile);
        }
    }

    public void NewCoords(int coordX, int coordY)
    {
        // Get coords from window class. Processing they
        NewTile(_board.GetTile(coordX, coordY));
    }

    public Checker GetChecker(Tuple<int, int> tile)
    {
        return _board.GetChecker(tile);
    }

    public bool IsWhiteTurn => _isWhiteTurn;

    public Result GetResult => _result;
}