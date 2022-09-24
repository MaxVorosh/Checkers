using System;

namespace Checkers;

public class CheckersBoard
{
    private readonly int _size;
    private bool _isWhiteTurn;
    private int _rule15;
    private Board _board;
    private Tuple<int, int> _currentTile;
    private bool _isMoveStarted;
    private Result _result;
    private int _whiteCheckers;
    private int _blackCheckers;

    public CheckersBoard(int size, int cnt)
    {
        _size = size;
        _isWhiteTurn = true;
        _rule15 = 0;
        _board = new Board(size, cnt);
        _currentTile = new Tuple<int, int>(-1, -1);
        _isMoveStarted = false;
        _whiteCheckers = cnt;
        _blackCheckers = cnt;
        _result = Result.NotEnd;
    }

    Tuple<int, int> GetMultipliers(Tuple<int, int> from, Tuple<int, int> to)
    {
        int xMultiplier = (from.Item1 < to.Item1) ? 1 : -1;
        int yMultiplier = (from.Item2 < to.Item2) ? 1 : -1;
        return new Tuple<int, int>(xMultiplier, yMultiplier);
    }

    public bool CanMove(Tuple<int, int> tileFrom, Tuple<int, int> tileTo, bool mustCapture)
    {
        if (Math.Abs(tileFrom.Item1 - tileTo.Item1) != Math.Abs(tileFrom.Item2 - tileTo.Item2) ||
            tileFrom.Item1 == tileTo.Item1 || _board.GetChecker(tileTo).IsExists())
        {
            return false;
        }

        var currentChecker = _board.GetChecker(tileFrom);
        int length = Math.Abs(tileFrom.Item1 - tileTo.Item1);

        if (currentChecker.IsMissis())
        {
            var multipliers = GetMultipliers(tileFrom, tileTo);
            int xMultiplier = multipliers.Item1;
            int yMultiplier = multipliers.Item2;
            int canTake = 0;
            for (int i = 1; i <= length; ++i)
            {
                int currentX = tileFrom.Item1 + i * xMultiplier;
                int currentY = tileFrom.Item2 + i * yMultiplier;
                var currentTile = new Tuple<int, int>(currentX, currentY);
                if (_board.GetChecker(currentTile).IsExists())
                {
                    if (_board.GetChecker(currentTile).IsWhite() == currentChecker.IsWhite())
                    {
                        return false;
                    }

                    canTake++;
                }
            }

            if (mustCapture)
            {
                return (canTake == 1);
            }

            return (canTake <= 1);
        }

        if (length > 2)
        {
            return false;
        }

        if (length == 1)
        {
            if (mustCapture)
            {
                return false;
            }

            if ((currentChecker.IsWhite() && tileTo.Item1 < tileFrom.Item1) ||
                (!currentChecker.IsWhite() && tileTo.Item1 > tileFrom.Item1))
            {
                return !_board.GetChecker(tileTo).IsExists();
            }

            return false;
        }

        var middleTile = new Tuple<int, int>((tileFrom.Item1 + tileTo.Item1) / 2,
            (tileFrom.Item2 + tileTo.Item2) / 2);
        return (_board.GetChecker(middleTile).IsExists() &&
                _board.GetChecker(middleTile).IsWhite() != currentChecker.IsWhite());
    }

    public void Move(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
    {
        if (!CanMove(tileFrom, tileTo, false))
        {
            return;
        }

        _board.MoveChecker(tileFrom, tileTo);
        var multipliers = GetMultipliers(tileFrom, tileTo);
        int xMultiplier = multipliers.Item1;
        int yMultiplier = multipliers.Item2;
        int length = Math.Abs(tileFrom.Item1 - tileTo.Item1);
        for (int i = 1; i < length; ++i)
        {
            int currentX = tileFrom.Item1 + i * xMultiplier;
            int currentY = tileFrom.Item2 + i * yMultiplier;
            var currentTile = new Tuple<int, int>(currentX, currentY);
            if (_board.GetChecker(currentTile).IsExists() && _board.GetChecker(currentTile).IsWhite())
            {
                _whiteCheckers--;
            }
            else if (_board.GetChecker(currentTile).IsExists())
            {
                _blackCheckers--;
            }

            _board.DeleteChecker(currentTile);
        }

        if ((tileTo.Item1 == 0 && _board.GetChecker(tileTo).IsWhite())
            || (tileTo.Item1 == _size - 1 && !_board.GetChecker(tileTo).IsWhite()))
        {
            _board.GetChecker(tileTo).BecomeMisis();
        }
    }

    public bool CanCaptureTile(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
    {
        if (!CanMove(tileFrom, tileTo, false))
        {
            return false;
        }

        var multipliers = GetMultipliers(tileFrom, tileTo);
        int xMultiplier = multipliers.Item1;
        int yMultiplier = multipliers.Item2;
        int length = Math.Abs(tileFrom.Item1 - tileTo.Item1);
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

    public void RejectCurrentTile()
    {
        _currentTile = new Tuple<int, int>(-1, -1);
    }

    public bool IsValidTile(Tuple<int, int> tile)
    {
        return (0 <= tile.Item1 && 0 <= tile.Item2 && tile.Item1 < _size && tile.Item2 < _size);
    }

    public Tuple<int, int>[] GetDiagonalMoves(Tuple<int, int> tile, int i)
    {
        var upLeft = new Tuple<int, int>(tile.Item1 + i, tile.Item2 - i);
        var upRight = new Tuple<int, int>(tile.Item1 + i, tile.Item2 + i);
        var downLeft = new Tuple<int, int>(tile.Item1 - i, tile.Item2 - i);
        var downRight = new Tuple<int, int>(tile.Item1 - i, tile.Item2 + i);
        Tuple<int, int>[] moves = { upLeft, upRight, downLeft, downRight };
        return moves;
    }

    public bool CanCaptureColor(Tuple<int, int> tile)
    {
        for (int i = 2; i <= _size; ++i)
        {
            var moves = GetDiagonalMoves(tile, i);
            foreach (var newTile in moves)
            {
                if (IsValidTile(newTile) && CanCaptureTile(tile, newTile))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool CanCaptureSmth(bool color)
    {
        for (int i = 0; i < _size; ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                Checker currentChecker = _board.GetChecker(new Tuple<int, int>(i, j));
                if (currentChecker.IsExists() && currentChecker.IsWhite() == color &&
                    CanCaptureColor(new Tuple<int, int>(i, j)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool CanCheckerMove(Tuple<int, int> tile)
    {
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

    public bool CanColorMove(bool isWhite)
    {
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

    public void Turn(Tuple<int, int> tileTo)
    {
        if (!CanMove(_currentTile, tileTo, CanCaptureSmth(_isWhiteTurn)))
        {
            //Console.WriteLine("Can't move");
            if (!_isMoveStarted)
            {
                RejectCurrentTile();
            }

            return;
        }

        _isMoveStarted = true;
        bool isCaptured = CanCaptureTile(_currentTile, tileTo);
        _board.MoveChecker(_currentTile, tileTo);
        bool shouldCheckCaptures = CanCaptureColor(tileTo) && isCaptured;
        _board.MoveChecker(tileTo, _currentTile);
        if (_board.GetChecker(_currentTile).IsMissis() && (!isCaptured))
        {
            _rule15++;
        }
        else
        {
            _rule15 = 0;
        }

        //Console.WriteLine("Get to move");
        Move(_currentTile, tileTo);
        //Console.WriteLine("Moved");
        _currentTile = tileTo;
        if (shouldCheckCaptures)
        {
            for (int i = 0; i < _size; ++i)
            {
                for (int j = 0; j < _size; ++j)
                {
                    if (CanCaptureTile(_currentTile, new Tuple<int, int>(i, j)))
                    {
                        return;
                    }
                }
            }
        }

        RejectCurrentTile();
        _isMoveStarted = false;
        _isWhiteTurn = !_isWhiteTurn;
        if (_rule15 == 30)
        {
            _result = Result.Draw;
        }
        else if (_isWhiteTurn && !CanColorMove(_isWhiteTurn))
        {
            _result = Result.BlackWin;
        }
        else if (!_isWhiteTurn && !CanColorMove(_isWhiteTurn))
        {
            _result = Result.WhiteWin;
        }
    }

    public void NewTile(Tuple<int, int> tile)
    {
        if (_currentTile.Equals(new Tuple<int, int>(-1, -1)))
        {
            //Console.WriteLine(Convert.ToString(isWhiteTurn == board.GetChecker(tile).IsWhite()));
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
        // Good coords
        NewTile(_board.GetTile(coordX, coordY));
    }

    public Checker GetChecker(Tuple<int, int> tile)
    {
        return _board.GetChecker(tile);
    }

    public bool IsWhiteTurn => _isWhiteTurn;

    public Result GetResult => _result;

    /*public void Print()
    {
        board.Print();
        Console.WriteLine(Convert.ToString(isMoveStarted) + ' ' + Convert.ToString(isWhiteTurn)
                          + ' ' + Convert.ToString(rule15) + ' ' + Convert.ToString(_currentTile.Item1) +
                          ' ' + Convert.ToString(_currentTile.Item2) + ' ' + Convert.ToString(result));
    }*/
}