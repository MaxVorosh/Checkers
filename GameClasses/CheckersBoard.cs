namespace GameClasses;

public class CheckersBoard
{
    // Class, that responsible for compliance game rules

    private readonly int _size; // count of board's rows and columns 
    private bool _isWhiteTurn;
    public int Rule15; // Count of missis's moves without captures
    private Board _board; // Checkers position
    private Tuple<int, int> _currentTile; // Tile with a selected checker
    private bool _isMoveStarted; // True, if checker capture something and can capture something else
    private Result _result; // Game status
    private Gameplay _gameplay;
    private Mode _mode;
    private Difficult _difficult;
    private int _cntCheckersForOne;

    public CheckersBoard(int size, int cnt, Mode mode, Difficult difficult, Gameplay gameplay)
    {
        _size = size;
        _isWhiteTurn = (gameplay != Gameplay.PoolCheckers);
        Rule15 = 0;
        _cntCheckersForOne = cnt;
        _board = new Board(size, cnt);
        _currentTile = new Tuple<int, int>(-1, -1); // This value shows, that no checker is selected
        _isMoveStarted = false; // Should player continue his turn if he already moved a checker
        _result = Result.NotEnd;
        _gameplay = gameplay;
        _mode = mode;
        _difficult = difficult;
        if (!_isWhiteTurn && _mode == Mode.SinglePlayer)
        {
            MakeComputerMove();
        }
    }

    public Tuple<int, int> GetMultipliers(Tuple<int, int> from, Tuple<int, int> to)
    {
        // Returns direction, in which checker should go from start tile to last
        // E.g. (1, -1) - checker should go to axis X and facing axis Y

        int xMultiplier = (from.Item1 < to.Item1) ? 1 : -1;
        int yMultiplier = (from.Item2 < to.Item2) ? 1 : -1;
        return new Tuple<int, int>(xMultiplier, yMultiplier);
    }

    public int GetLength(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
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

    public bool CanMove(Tuple<int, int> tileFrom, Tuple<int, int> tileTo, bool mustCapture)
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

    public void Move(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
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

    public bool CanCaptureTile(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
    {
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

    public bool IsValidTile(Tuple<int, int> tile)
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

    public bool CanCaptureColor(Tuple<int, int> tile)
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

    public bool CanCaptureSmth(bool color)
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

    public bool CanCheckerMove(Tuple<int, int> tile)
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

    public bool CanColorMove(bool isWhite)
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

    public void UpdateResult()
    {
        if (Rule15 == 30)
        {
            _result = Result.Draw;
        }

        if (_isWhiteTurn && !CanColorMove(_isWhiteTurn))
        {
            _result = _gameplay != Gameplay.Giveaway ? Result.BlackWin : Result.WhiteWin;
        }
        else if (!_isWhiteTurn && !CanColorMove(_isWhiteTurn))
        {
            _result = _gameplay != Gameplay.Giveaway ? Result.WhiteWin : Result.BlackWin;
        }
    }

    public bool Turn(Tuple<int, int> tileTo)
    {
        // Makes turn by 2 tiles - current and new. Returns true, if the move was maked

        // If move is illegal (we can't move or we should move something else to capture), forget current tile
        if (!CanMove(_currentTile, tileTo, CanCaptureSmth(_isWhiteTurn)))
        {
            if (!_isMoveStarted)
                RejectCurrentTile();
            return false;
        }

        _isMoveStarted = true;
        bool changeMissis = _board.GetChecker(_currentTile).IsMissis(); // Was checker changes from regular to missis
        bool isCaptured = CanCaptureTile(_currentTile, tileTo); // When checker moves, was it captured something

        if (_board.GetChecker(_currentTile).IsMissis() && !isCaptured)
            Rule15++;
        else
            Rule15 = 0;

        Move(_currentTile, tileTo);
        _currentTile = tileTo;
        changeMissis =
            (!changeMissis) && _board.GetChecker(_currentTile).IsMissis(); // true if it's changed false->true

        // If checker captured something, we check, can it capture something else
        // If it is pool checker and checker becomes missis, we should end turn
        if (isCaptured && (_gameplay != Gameplay.PoolCheckers || !changeMissis))
        {
            if (CanCaptureColor(_currentTile))
                return true;
        }

        // When our turn is end
        RejectCurrentTile();
        _isMoveStarted = false;
        _isWhiteTurn = !_isWhiteTurn;
        UpdateResult();
        return true;
    }

    public void NewTile(Tuple<int, int> tile)
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
            bool wasMoved = Turn(tile);
            if (wasMoved && !_isMoveStarted && _mode == Mode.SinglePlayer)
            {
                MakeComputerMove();
            }
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

    public void SetResult(Result newResult)
    {
        _result = newResult;
    }

    public Tuple<int, int> GetTile(int xCoord, int yCoord)
    {
        return _board.GetTile(xCoord, yCoord);
    }

    public bool IsSelectedChecker()
    {
        return !_currentTile.Equals(new Tuple<int, int>(-1, -1));
    }

    public Tuple<int, int> GetSelectedTile()
    {
        return _currentTile;
    }

    public bool ShouldLight(Tuple<int, int> tile)
    {
        // True if tile should be with a border (selected checker can go to it)
        return IsSelectedChecker() && CanMove(_currentTile, tile, CanCaptureSmth(_isWhiteTurn));
    }

    public bool ShouldIncreaseRule15(Board board)
    {
        // If we move missis and not capture any checker, we should increase rule15. This function check it
        bool isMovedMissis = false; // Becomes true if there was missis on the old version of board, not new version
        int deltaCheckers = 0; // Count checker of opposite color on the old board minus on the new board
        for (int i = 0; i < _size; ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                var lastChecker = _board.GetChecker(new Tuple<int, int>(i, j));
                var newChecker = board.GetChecker(new Tuple<int, int>(i, j));
                if (lastChecker.IsExists())
                {
                    if (lastChecker.IsWhite() != _isWhiteTurn)
                        deltaCheckers++;

                    if (!newChecker.IsExists())
                        isMovedMissis = lastChecker.IsMissis();
                }

                if (newChecker.IsExists())
                {
                    if (newChecker.IsWhite() != _isWhiteTurn)
                        deltaCheckers--;
                }
            }
        }

        return isMovedMissis && (deltaCheckers == 0); // True if we moved missis and don't capture anything
    }

    private void MakeComputerMove()
    {
        if (_difficult == Difficult.Easy)
        {
            MakeRandomComputerMove();
        }
        else
        {
            int depth = 2;
            if (_difficult == Difficult.Hard)
            {
                depth = 4;
            }

            var newPosition = MakeRealComputerMove(depth);
            if (ShouldIncreaseRule15(newPosition.Item1))
            {
                Rule15++;
            }
            else
            {
                Rule15 = 0;
            }

            _isWhiteTurn = !_isWhiteTurn;
            _board = newPosition.Item1.Copy();
        }

        UpdateResult();
    }

    public int EvaluatePosition(Board board)
    {
        // Evaluates position on the board depends on number of checkers. Regular costs 1 point, missis costs 3 points
        // Returns white score - black score
        if (_result == Result.Draw)
            return 0;
        if (_result == Result.BlackWin)
            return -3 * _size * _size;
        if (_result == Result.WhiteWin)
            return 3 * _size * _size;

        int myScore = 0;
        int opponentScore = 0;
        for (int i = 0; i < _size; ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                Checker checker = board.GetChecker(new Tuple<int, int>(i, j));
                if (checker.IsExists())
                {
                    int add = 1;
                    if (checker.IsMissis())
                        add = 3;
                    if (checker.IsWhite() && _gameplay != Gameplay.Giveaway)
                        myScore += add;
                    else
                        opponentScore += add;
                }
            }
        }
        return myScore - opponentScore;
    }

    public Tuple<Board, int> MakeComputerCheckerMove(int depth, Tuple<int, int> tile)
    {
        // Makes all possible move by checker and chooses the best one. Depth - How many moves program will simulate
        if (_result != Result.NotEnd)
            return new Tuple<Board, int>(_board, EvaluatePosition(_board)); // If game ends, we shouldn't look deeper
        
        int needEvaluate = 3 * _size * _size + 3; // Best evaluate
        if (_isWhiteTurn)
            needEvaluate = -3 * _size * _size - 3;

        var multipliers = GetAllMultipliers();
        Board bestBoard = _board.Copy(); // Best position after next move
        var board = _board.Copy(); // Current position
        for (int length = 0; length < _size; ++length)
        {
            foreach (var multiplier in multipliers)
            {
                int xCoord = tile.Item1 + length * multiplier.Item1;
                int yCoord = tile.Item2 + length * multiplier.Item2;
                var tileTo = new Tuple<int, int>(xCoord, yCoord);
                if (!IsValidTile(tileTo))
                    continue;
                // Current parameters of position
                int currentRule15 = Rule15;
                bool currentTurn = _isWhiteTurn;
                _currentTile = tile;
                bool fl = Turn(tileTo); // Try make turn
                if (fl)
                {
                    Tuple<Board, int> moveResult;
                    bool shouldResetBoard = !_isMoveStarted; // False if there's more than one moves in the turn
                    if (_isMoveStarted) // If checker can capture something else, we continue our turn
                        moveResult = MakeComputerCheckerMove(depth, tileTo); 
                    else // Else we should look all possible moves for other player
                        moveResult = MakeRealComputerMove(depth - 1);
                    int eval = moveResult.Item2;
                    if (tile.Equals(new Tuple<int, int>(7, 6)))
                    {
                    }

                    if ((eval > needEvaluate) ^ !currentTurn)
                    {
                        if (shouldResetBoard)
                        {
                            bestBoard = _board.Copy();
                        }
                        else
                        {
                            bestBoard = moveResult.Item1.Copy();
                        }

                        needEvaluate = eval;
                    }
                    // Set old values
                    _board = board.Copy();
                    Rule15 = currentRule15;
                    _isWhiteTurn = currentTurn;
                    _result = Result.NotEnd;
                }
            }
        }
        return new Tuple<Board, int>(bestBoard, needEvaluate);
    }

    public Tuple<Board, int> MakeRealComputerMove(int depth)
    {
        // Make all possible moves and choose the best one
        if (depth == 0)
            return new Tuple<Board, int>(_board, EvaluatePosition(_board));

        var bestBoard = _board; // Best position after next move
        int needEvaluate = 3 * _size * _size + 3; // Best evaluate after next move. Min for black, max for white
        if (_isWhiteTurn)
        {
            needEvaluate = -3 * _size * _size - 3;
        }
        bool mustCapture = CanCaptureSmth(_isWhiteTurn);
        for (int i = 0; i < _size; ++i)
        {
            for (int j = 0; j < _size; ++j)
            {
                var tile = new Tuple<int, int>(i, j);
                Checker currentChecker = _board.GetChecker(tile);
                if (!currentChecker.IsExists() || currentChecker.IsWhite() != _isWhiteTurn ||
                    !CanCheckerMove(tile) || (!CanCaptureColor(tile) && mustCapture))  
                {

                    continue; // We can't move this checker if it's not exist, bad color, can't move or must capture, but can't
                }
                var moveResult = MakeComputerCheckerMove(depth, new Tuple<int, int>(i, j));
                Board newBoard = moveResult.Item1;
                int eval = moveResult.Item2;
                if ((eval > needEvaluate) ^ !_isWhiteTurn)
                {
                    bestBoard = newBoard.Copy();
                    needEvaluate = eval;
                }
            }
        }
        return new Tuple<Board, int>(bestBoard, needEvaluate);
    }

    public void MakeRandomComputerMove()
    {
        // Make random move, on the easiest difficult

        var checkersQueue = GetCheckersQueue();
        checkersQueue = Shuffle(checkersQueue);
        var multipliers = GetAllMultipliers();
        var range = new List<Tuple<int, int>>(); // Possible length of move
        for (int i = 0; i < _size; ++i)
        {
            range.Add(new Tuple<int, int>(i, i));
        }

        foreach (var checkerPosition in checkersQueue)
        {
            multipliers = Shuffle(multipliers);
            range = Shuffle(range);
            foreach (var multiplier in multipliers)
            {
                foreach (var lentgh in range)
                {
                    _currentTile = checkerPosition;
                    int xCoord = checkerPosition.Item1 + multiplier.Item1 * lentgh.Item1;
                    int yCoord = checkerPosition.Item2 + multiplier.Item2 * lentgh.Item1;
                    var tileTo = new Tuple<int, int>(xCoord, yCoord); // Random tile. Trying to make move there
                    if (IsValidTile(tileTo) && ShouldLight(tileTo))
                    {
                        Turn(tileTo);
                        if (_isMoveStarted) // If computer's turn is not over, we should make one more move
                        {
                            MakeRandomComputerMove();
                        }

                        return;
                    }
                }
            }
        }
    }

    private List<Tuple<int, int>> GetCheckersQueue()
    {
        // Gets List of positions of checkers, that are same colored as person, whose turn
        var checkersQueue = new List<Tuple<int, int>>();
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                Checker currentChecker = _board.GetChecker(new Tuple<int, int>(i, j));
                if (currentChecker.IsExists() && _isWhiteTurn == currentChecker.IsWhite())
                {
                    checkersQueue.Add(new Tuple<int, int>(i, j));
                }
            }
        }

        return checkersQueue;
    }

    private List<Tuple<int, int>> Shuffle(List<Tuple<int, int>> list)
    {
        // Shuffle Tuples list. Called just on MakeRandomMove function
        var rnd = new Random();
        for (int i = 0; i < list.Count; ++i)
        {
            int pos = rnd.Next(i, list.Count);
            (list[i], list[pos]) = (list[pos], list[i]);
        }

        return list;
    }

    private List<Tuple<int, int>> GetAllMultipliers()
    {
        var multipliers = new List<Tuple<int, int>> // Possible directions of move
        {
            new Tuple<int, int>(1, 1), new Tuple<int, int>(1, -1), new Tuple<int, int>(-1, 1),
            new Tuple<int, int>(-1, -1)
        };
        return multipliers;
    }

    public void DeleteChecker(Tuple<int, int> tile)
    {
        // Deletes checker from tile. Needs for testing
        _board.DeleteChecker(tile);
    }
}