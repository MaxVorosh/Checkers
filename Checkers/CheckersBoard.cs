using System;

namespace Checkers;

public class CheckersBoard
{
    private int _size;
    private bool isWhiteTurn;
    private int rule15;
    private Board board;
    private Tuple<int, int> _currentTile;
    private bool isMoveStarted;
    private int result;
    private int _whiteCheckers;
    private int _blackCheckers;

    public CheckersBoard(int size, int cnt)
    {
        _size = size;
        isWhiteTurn = true;
        rule15 = 0;
        board = new Board(size, cnt);
        _currentTile = new Tuple<int, int>(-1, -1);
        isMoveStarted = false;
        _whiteCheckers = cnt;
        _blackCheckers = cnt;
        result = -1; // 0 - white win, 1 - draw, 2 - black win
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
            tileFrom.Item1 == tileTo.Item1 || board.GetChecker(tileTo).IsExists())
        {
            return false;
        }

        var currentChecker = board.GetChecker(tileFrom);
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
                if (board.GetChecker(currentTile).IsExists())
                {
                    if (board.GetChecker(currentTile).IsWhite() == currentChecker.IsWhite())
                    {
                        return false;
                    }

                    canTake++;
                }
            }

            if (isMoveStarted || mustCapture)
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

            if ((currentChecker.IsWhite() && tileTo.Item1 > tileFrom.Item1) ||
                (!currentChecker.IsWhite() && tileTo.Item1 < tileFrom.Item1))
            {
                return !board.GetChecker(tileTo).IsExists();
            }

            return false;
        }

        var middleTile = new Tuple<int, int>((tileFrom.Item1 + tileTo.Item1) / 2,
            (tileFrom.Item2 + tileTo.Item2) / 2);
        return (board.GetChecker(middleTile).IsExists() &&
                board.GetChecker(middleTile).IsWhite() != currentChecker.IsWhite());
    }

    public void Move(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
    {
        if (!CanMove(tileFrom, tileTo, false))
        {
            return;
        }

        board.MoveChecker(tileFrom, tileTo);
        var multipliers = GetMultipliers(tileFrom, tileTo);
        int xMultiplier = multipliers.Item1;
        int yMultiplier = multipliers.Item2;
        int length = Math.Abs(tileFrom.Item1 - tileTo.Item1);
        for (int i = 1; i < length; ++i)
        {
            int currentX = tileFrom.Item1 + i * xMultiplier;
            int currentY = tileFrom.Item2 + i * yMultiplier;
            var currentTile = new Tuple<int, int>(currentX, currentY);
            if (board.GetChecker(currentTile).IsWhite())
            {
                _whiteCheckers--;
            }
            else
            {
                _blackCheckers--;
            }

            board.DeleteChecker(currentTile);
        }

        if (tileTo.Item2 == 0 || tileTo.Item2 == _size - 1)
        {
            board.GetChecker(tileTo).BecomeMisis();
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
            if (board.GetChecker(new Tuple<int, int>(currentX, currentY)).IsExists())
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

    public bool CanCaptureColor(Tuple<int, int> tile)
    {
        for (int i = 2; i < _size; ++i)
        {
            Tuple<int, int> upLeft = new Tuple<int, int>(tile.Item1 + i, tile.Item2 - i);
            Tuple<int, int> upRight = new Tuple<int, int>(tile.Item1 + i, tile.Item2 + i);
            Tuple<int, int> downLeft = new Tuple<int, int>(tile.Item1 - i, tile.Item2 - i);
            Tuple<int, int> downRight = new Tuple<int, int>(tile.Item1 - i, tile.Item2 + i);
            Tuple<int, int>[] moves = { upLeft, upRight, downLeft, downRight };
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
                Checker currentChecker = board.GetChecker(new Tuple<int, int>(i, j));
                if (currentChecker.IsExists() && currentChecker.IsWhite() == color &&
                    CanCaptureColor(new Tuple<int, int>(i, j)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void Turn(Tuple<int, int> tileTo)
    {
        if (!CanMove(_currentTile, tileTo, CanCaptureSmth(isWhiteTurn)))
        {
            //Console.WriteLine("Can't move");
            if (!isMoveStarted)
            {
                RejectCurrentTile();
            }

            return;
        }

        isMoveStarted = true;
        bool shouldCheckCaptures = CanCaptureColor(tileTo);
        if (board.GetChecker(_currentTile).IsMissis())
        {
            rule15++;
        }
        else
        {
            rule15 = 0;
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
        isMoveStarted = false;
        isWhiteTurn = !isWhiteTurn;
        if (rule15 == 30)
        {
            result = 1;
        }
        else if (_whiteCheckers == 0)
        {
            result = 2;
        }
        else
        {
            result = 0;
        }
        //If someone can't move, he lose. Check it 
    }

    public void NewTile(Tuple<int, int> tile)
    {
        if (_currentTile.Equals(new Tuple<int, int>(-1, -1)))
        {
            //Console.WriteLine(Convert.ToString(isWhiteTurn == board.GetChecker(tile).IsWhite()));
            if (isWhiteTurn == board.GetChecker(tile).IsWhite() && board.GetChecker(tile).IsExists())
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
        NewTile(board.GetTile(coordX, coordY));
    }

    /*public void Print()
    {
        board.Print();
        Console.WriteLine(Convert.ToString(isMoveStarted) + ' ' + Convert.ToString(isWhiteTurn)
                          + ' ' + Convert.ToString(rule15) + ' ' + Convert.ToString(_currentTile.Item1) +
                          ' ' + Convert.ToString(_currentTile.Item2) + ' ' + Convert.ToString(result));
    }*/
}