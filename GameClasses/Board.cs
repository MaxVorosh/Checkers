using System;

namespace GameClasses;

public class Board
{
    // Class, that responsible for actual movement of checkers
    
    private readonly int _size; // Count rows and columns at the board
    private readonly int _tileSize; // size of every tile in pixels
    private readonly Checker[,] _checkers; // Array of position of checkers
    private readonly int _cntCheckersForOne;

    public Board(int size, int cnt)
    {
        _size = size;
        _cntCheckersForOne = cnt;
        _checkers = new Checker[_size, _size];
        _tileSize = 50;
        for (int i = 0; i < _size; ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                _checkers[i, j] = new Checker(true, false);
            }
        }

        ArrangeCheckers();
    }

    private void ArrangeCheckers()
    {
        // Get start position of checkers
        for (int i = 0; i < _cntCheckersForOne / (_size / 2); ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                if ((i + j) % 2 == 1)
                {
                    _checkers[i, j] = new Checker(false, false, true);
                    _checkers[_size - i - 1, _size - j - 1] = new Checker(true, false, true);
                }
            }
        }
    }

    public Tuple<int, int> GetTile(int xMouse, int yMouse)
    {
        // Get coords, that refers to the board. Returns tile with this coords
        return new Tuple<int, int>(xMouse / _tileSize, yMouse / _tileSize);
    }

    public Checker GetChecker(Tuple<int, int> tile)
    {
        return _checkers[tile.Item1, tile.Item2];
    }

    public void DeleteChecker(Tuple<int, int> tile)
    {
        _checkers[tile.Item1, tile.Item2].Delete();
    }

    public void MoveChecker(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
    {
        // Actual movement of a checker. Can violate game rules
        _checkers[tileTo.Item1, tileTo.Item2] = _checkers[tileFrom.Item1, tileFrom.Item2].Copy();
        _checkers[tileFrom.Item1, tileFrom.Item2].Delete();
    }

    public Board Copy()
    {
        var board = new Board(_size, _cntCheckersForOne);
        for (int i = 0; i < _size; ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                board._checkers[i, j] = _checkers[i, j].Copy();
            }
        }
        return board;
    }
    
    public static void Main(string[] args)
    {
    }
}