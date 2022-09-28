using GameClasses;
using static NUnit.Framework.Assert;

namespace GameClassesTests;

public class CheckersBoardTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetMultipliersTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        var result1 = checkersBoard.GetMultipliers(new Tuple<int, int>(1, 1), new Tuple<int, int>(2, 2));
        var result2 = checkersBoard.GetMultipliers(new Tuple<int, int>(1, 1), new Tuple<int, int>(0, 2));
        var result3 = checkersBoard.GetMultipliers(new Tuple<int, int>(4, 4), new Tuple<int, int>(6, 2));
        var result4 = checkersBoard.GetMultipliers(new Tuple<int, int>(5, 5), new Tuple<int, int>(1, 1));
        That(result1, Is.EqualTo(new Tuple<int, int>(1, 1)));
        That(result2, Is.EqualTo(new Tuple<int, int>(-1, 1)));
        That(result3, Is.EqualTo(new Tuple<int, int>(1, -1)));
        That(result4, Is.EqualTo(new Tuple<int, int>(-1, -1)));
    }

    [Test]
    public void GetLengthTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        That(checkersBoard.GetLength(new Tuple<int, int>(1, 1), new Tuple<int, int>(1, 1)), Is.EqualTo(0));
        That(checkersBoard.GetLength(new Tuple<int, int>(1, 1), new Tuple<int, int>(3, 3)), Is.EqualTo(2));
        That(checkersBoard.GetLength(new Tuple<int, int>(7, 7), new Tuple<int, int>(2, 2)), Is.EqualTo(5));
        That(checkersBoard.GetLength(new Tuple<int, int>(3, 4), new Tuple<int, int>(2, 5)), Is.EqualTo(1));
        That(checkersBoard.GetLength(new Tuple<int, int>(2, 4), new Tuple<int, int>(5, 1)), Is.EqualTo(3));
    }

    [Test]
    public void CanMoveTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i == 1 && j == 6) || (i == 7 && (j == 0 || j == 2)) || (i == 2 && j == 3))
                {
                    continue;
                }

                if (i == 0 && j == 7)
                {
                    checkersBoard.GetChecker(new Tuple<int, int>(i, j)).White = true;
                }
                else if (i == 6 && j == 1)
                {
                    checkersBoard.GetChecker(new Tuple<int, int>(i, j)).White = false;
                }
                else if (i == 5 && j == 6)
                {
                    checkersBoard.GetChecker(new Tuple<int, int>(i, j)).BecomeMissis();
                }
                else
                {
                    checkersBoard.DeleteChecker(new Tuple<int, int>(i, j));
                }
            }
        }
        That(checkersBoard.CanMove(new Tuple<int, int>(0, 7), new Tuple<int, int>(2, 5), true), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(0, 7), new Tuple<int, int>(2, 5), false), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(7, 0), new Tuple<int, int>(6, 1), false), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(7, 0), new Tuple<int, int>(6, 1), false), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(7, 0), new Tuple<int, int>(7, 1), false), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(7, 0), new Tuple<int, int>(5, 2), true), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(7, 0), new Tuple<int, int>(5, 2), false), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(7, 0), new Tuple<int, int>(4, 3), false), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(7, 2), new Tuple<int, int>(6, 3), true), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(7, 2), new Tuple<int, int>(6, 3), false), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(2, 3), new Tuple<int, int>(1, 2), false), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(2, 3), new Tuple<int, int>(4, 5), false), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(5, 6), new Tuple<int, int>(6, 7), false), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(5, 6), new Tuple<int, int>(4, 5), true), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(5, 6), new Tuple<int, int>(3, 4), true), Is.False);
        That(checkersBoard.CanMove(new Tuple<int, int>(5, 6), new Tuple<int, int>(3, 4), false), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(5, 6), new Tuple<int, int>(4, 5), false), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(5, 6), new Tuple<int, int>(1, 2), true), Is.True);
        That(checkersBoard.CanMove(new Tuple<int, int>(5, 6), new Tuple<int, int>(0, 1), false), Is.True);
    }

    [Test]
    public void ImpossibleMoveTest()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        var checkersBoard2 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        checkersBoard1.Move(new Tuple<int, int>(0, 1), new Tuple<int, int>(1, 2));
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker1 = checkersBoard1.GetChecker(new Tuple<int, int>(i, j));
                var checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(i, j));
                That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
            }
        }
    }

    [Test]
    public void RegularCheckerMoveTest()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        var checkersBoard2 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        checkersBoard1.Move(new Tuple<int, int>(2, 1), new Tuple<int, int>(3, 2));
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker1 = checkersBoard1.GetChecker(new Tuple<int, int>(i, j));
                var checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(i, j));
                if (i == 2 && j == 1)
                {
                    That(checker1.IsExists, Is.False);
                }
                else if (i == 3 && j == 2)
                {
                    checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(2, 1));
                    That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                    That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                    That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
                }
                else
                {
                    That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                    That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                    That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
                }
            }
        }
    }

    [Test]
    public void MissisCaptureMoveTest()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        var checkersBoard2 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        checkersBoard1.GetChecker(new Tuple<int, int>(5, 2)).BecomeMissis();
        checkersBoard2.GetChecker(new Tuple<int, int>(5, 2)).BecomeMissis();
        checkersBoard1.DeleteChecker(new Tuple<int, int>(1, 6));
        checkersBoard1.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(1, 6));
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker1 = checkersBoard1.GetChecker(new Tuple<int, int>(i, j));
                var checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(i, j));
                if ((i == 5 && j == 2) || (i == 2 && j == 5))
                {
                    That(checker1.IsExists, Is.False);
                }
                else if (i == 1 && j == 6)
                {
                    checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(5, 2));
                    That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                    That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                    That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
                }
                else
                {
                    That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                    That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                    That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
                }
            }
        }
    }

    [Test]
    public void ImpossibleCanCaptureTile()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        bool result = checkersBoard1.CanCaptureTile(new Tuple<int, int>(0, 1), new Tuple<int, int>(3, 4));
        That(result, Is.False);
    }

    [Test]
    public void TrueCanCaptureTile()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        checkersBoard1.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(4, 3));
        checkersBoard1.Move(new Tuple<int, int>(2, 5), new Tuple<int, int>(3, 4));
        bool result = checkersBoard1.CanCaptureTile(new Tuple<int, int>(4, 3), new Tuple<int, int>(2, 5));
        That(result, Is.True);
    }

    [Test]
    public void FalseCanCaptureTile()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        bool result = checkersBoard1.CanCaptureTile(new Tuple<int, int>(5, 0), new Tuple<int, int>(4, 1));
        That(result, Is.False);
    }

    [Test]
    public void IsValidTileTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        That(checkersBoard.IsValidTile(new Tuple<int, int>(1, 3)), Is.True);
        That(checkersBoard.IsValidTile(new Tuple<int, int>(0, 7)), Is.True);
        That(checkersBoard.IsValidTile(new Tuple<int, int>(-1, 6)), Is.False);
        That(checkersBoard.IsValidTile(new Tuple<int, int>(5, 8)), Is.False);
    }

    [Test]
    public void CanCaptureColorTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        checkersBoard.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(4, 3));
        checkersBoard.Move(new Tuple<int, int>(2, 5), new Tuple<int, int>(3, 4));
        checkersBoard.DeleteChecker(new Tuple<int, int>(2, 3));
        checkersBoard.GetChecker(new Tuple<int, int>(5, 6)).BecomeMissis();
        That(checkersBoard.CanCaptureColor(new Tuple<int, int>(4, 3)), Is.True);
        That(checkersBoard.CanCaptureColor(new Tuple<int, int>(5, 0)), Is.False);
        That(checkersBoard.CanCaptureColor(new Tuple<int, int>(5, 6)), Is.True);
    }

    [Test]
    public void CanCaptureSmthTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        That(checkersBoard.CanCaptureSmth(true), Is.False);
        checkersBoard.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(4, 3));
        checkersBoard.Move(new Tuple<int, int>(2, 5), new Tuple<int, int>(3, 4));
        That(checkersBoard.CanCaptureSmth(false), Is.True);
        checkersBoard.Move(new Tuple<int, int>(1, 6), new Tuple<int, int>(2, 5));
        checkersBoard.DeleteChecker(new Tuple<int, int>(2, 3));
        checkersBoard.GetChecker(new Tuple<int, int>(5, 6)).BecomeMissis();
        That(checkersBoard.CanCaptureSmth(true), Is.True);
    }

    [Test]
    public void CanCheckerMoveTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        That(checkersBoard.CanCheckerMove(new Tuple<int, int>(0, 1)), Is.False);
        That(checkersBoard.CanCheckerMove(new Tuple<int, int>(2, 1)), Is.True);
        checkersBoard.GetChecker(new Tuple<int, int>(5, 6)).BecomeMissis();
        checkersBoard.Move(new Tuple<int, int>(5, 6), new Tuple<int, int>(3, 4));
        That(checkersBoard.CanCheckerMove(new Tuple<int, int>(3, 4)), Is.True);
        checkersBoard.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(4, 3));
        checkersBoard.Move(new Tuple<int, int>(5, 4), new Tuple<int, int>(4, 5));
        That(checkersBoard.CanCheckerMove(new Tuple<int, int>(3, 4)), Is.False);
        checkersBoard.DeleteChecker(new Tuple<int, int>(1, 6));
        That(checkersBoard.CanCheckerMove(new Tuple<int, int>(3, 4)), Is.True);
    }

    [Test]
    public void CanColorMoveTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Giveaway);
        That(checkersBoard.CanColorMove(true), Is.True);
        That(checkersBoard.CanColorMove(false), Is.True);
        for (int j = 0; j < 8; ++j)
        {
            if (checkersBoard.GetChecker(new Tuple<int, int>(5, j)).IsExists())
            {
                checkersBoard.Move(new Tuple<int, int>(5, j), new Tuple<int, int>(4, j + 1));
                checkersBoard.Move(new Tuple<int, int>(4, j + 1), new Tuple<int, int>(3, j));
            }
            checkersBoard.DeleteChecker(new Tuple<int, int>(6, j));
            checkersBoard.DeleteChecker(new Tuple<int, int>(7, j));
        }
        That(checkersBoard.CanColorMove(true), Is.False);
        That(checkersBoard.CanColorMove(false), Is.True);
    }

    [Test]
    public void UpdateResultToWhiteWinTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.PoolCheckers);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i == 1 && j == 0) || (i == 2 && j == 1) || (i == 5 && j == 0))
                {
                    continue;
                }
                checkersBoard.DeleteChecker(new Tuple<int, int>(i, j));
            }
        }
        checkersBoard.Move(new Tuple<int, int>(5, 0), new Tuple<int, int>(4, 1));
        checkersBoard.Move(new Tuple<int, int>(4, 1), new Tuple<int, int>(3, 2));
        checkersBoard.UpdateResult();
        That(checkersBoard.GetResult, Is.EqualTo(Result.NotEnd));
        checkersBoard.GetChecker(new Tuple<int, int>(2, 1)).White = true;
        checkersBoard.UpdateResult();
        That(checkersBoard.GetResult, Is.EqualTo(Result.WhiteWin));
        checkersBoard.SetResult(Result.NotEnd);
        checkersBoard.DeleteChecker(new Tuple<int, int>(1, 0));
        checkersBoard.UpdateResult();
        That(checkersBoard.GetResult, Is.EqualTo(Result.WhiteWin));
    }

    [Test]
    public void UpdateResultToBlackWinTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i == 6 && j == 7) || (i == 5 && j == 6) || (i == 2 && j == 3))
                {
                    continue;
                }
                checkersBoard.DeleteChecker(new Tuple<int, int>(i, j));
            }
        }
        checkersBoard.Move(new Tuple<int, int>(2, 3), new Tuple<int, int>(3, 4));
        checkersBoard.Move(new Tuple<int, int>(3, 4), new Tuple<int, int>(4, 5));
        checkersBoard.UpdateResult();
        That(checkersBoard.GetResult, Is.EqualTo(Result.NotEnd));
        checkersBoard.GetChecker(new Tuple<int, int>(5, 6)).White = false;
        checkersBoard.UpdateResult();
        That(checkersBoard.GetResult, Is.EqualTo(Result.BlackWin));
        checkersBoard.SetResult(Result.NotEnd);
        checkersBoard.DeleteChecker(new Tuple<int, int>(6, 7));
        checkersBoard.UpdateResult();
        That(checkersBoard.GetResult, Is.EqualTo(Result.BlackWin));
    }

    [Test]
    public void UpdateResultDrawTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        checkersBoard.Rule15 = 30;
        checkersBoard.UpdateResult();
        That(checkersBoard.GetResult, Is.EqualTo(Result.Draw));
    }

    [Test]
    public void ImpossibleMoveTurnTest()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        var checkersBoard2 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        var checkersBoard3 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        checkersBoard1.NewTile(new Tuple<int, int>(5, 0));
        checkersBoard3.NewTile(new Tuple<int, int>(5, 0));
        bool fl1 = checkersBoard1.Turn(new Tuple<int, int>(4, 0));
        bool fl3 = checkersBoard1.Turn(new Tuple<int, int>(5, 0));
        That(fl1, Is.False);
        That(fl3, Is.False);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker1 = checkersBoard1.GetChecker(new Tuple<int, int>(i, j));
                var checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(i, j));
                var checker3 = checkersBoard3.GetChecker(new Tuple<int, int>(i, j));
                That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
                That(checker3.IsExists, Is.EqualTo(checker2.IsExists()));
                That(checker3.IsWhite, Is.EqualTo(checker2.IsWhite()));
                That(checker3.IsMissis, Is.EqualTo(checker2.IsMissis()));
            }
        }
    }

    [Test]
    public void ShouldCaptureTurnTest()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        var checkersBoard2 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        checkersBoard1.Move(new Tuple<int, int>(2, 3), new Tuple<int, int>(3, 4));
        checkersBoard1.Move(new Tuple<int, int>(3, 4), new Tuple<int, int>(4, 3));
        checkersBoard2.Move(new Tuple<int, int>(2, 3), new Tuple<int, int>(3, 4));
        checkersBoard2.Move(new Tuple<int, int>(3, 4), new Tuple<int, int>(4, 3));
        checkersBoard1.NewTile(new Tuple<int, int>(5, 2));
        bool fl = checkersBoard1.Turn(new Tuple<int, int>(4, 1));
        That(fl, Is.False);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker1 = checkersBoard1.GetChecker(new Tuple<int, int>(i, j));
                var checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(i, j));
                That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
            }
        }
        checkersBoard1.NewTile(new Tuple<int, int>(5, 2));
        fl = checkersBoard1.Turn(new Tuple<int, int>(3, 4));
        That(fl, Is.True);
        checkersBoard2.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(3, 4));
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker1 = checkersBoard1.GetChecker(new Tuple<int, int>(i, j));
                var checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(i, j));
                That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
            }
        }
    }

    [Test]
    public void RegularTurnTest()
    {
        var checkersBoard1 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        var checkersBoard2 = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        checkersBoard1.NewTile(new Tuple<int, int>(5, 2));
        checkersBoard2.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(4, 3));
        bool fl = checkersBoard1.Turn(new Tuple<int, int>(4, 3));
        That(fl, Is.True);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker1 = checkersBoard1.GetChecker(new Tuple<int, int>(i, j));
                var checker2 = checkersBoard2.GetChecker(new Tuple<int, int>(i, j));
                That(checker1.IsExists, Is.EqualTo(checker2.IsExists()));
                That(checker1.IsWhite, Is.EqualTo(checker2.IsWhite()));
                That(checker1.IsMissis, Is.EqualTo(checker2.IsMissis()));
            }
        }
    }

    [Test]
    public void ShouldIncreaseRule15Test()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        var board = new Board(8, 12);
        board.MoveChecker(new Tuple<int, int>(5, 2), new Tuple<int, int>(4, 3));
        That(checkersBoard.ShouldIncreaseRule15(board), Is.False);
        board.GetChecker(new Tuple<int, int>(4, 3)).BecomeMissis();
        checkersBoard.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(4, 3));
        checkersBoard.GetChecker(new Tuple<int, int>(4, 3)).BecomeMissis();
        board.MoveChecker(new Tuple<int, int>(4, 3), new Tuple<int, int>(5, 2));
        That(checkersBoard.ShouldIncreaseRule15(board), Is.True);
    }

    [Test]
    public void EvaluatePositionTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.Russian);
        var board = new Board(8, 12);
        That(checkersBoard.EvaluatePosition(board), Is.EqualTo(0));
        for (int i = 0; i < 8; ++i)
        {
            board.DeleteChecker(new Tuple<int, int>(0, i));
        }
        That(checkersBoard.EvaluatePosition(board), Is.EqualTo(4));
        for (int i = 0; i < 8; ++i)
        {
            board.GetChecker(new Tuple<int, int>(1, i)).BecomeMissis();
        }
        That(checkersBoard.EvaluatePosition(board), Is.EqualTo(-4));
    }

    [Test]
    public void MakeComputerCheckerMoveTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.PoolCheckers);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i == 6 && j == 1) || (i == 5 && j == 2) || (i == 6 && j == 7))
                {
                    checkersBoard.GetChecker(new Tuple<int, int>(i, j)).BecomeMissis();
                }
                else if ((i == 0 && j == 7) || (i == 5 && j == 6))
                {
                    continue;
                }
                else
                {
                    checkersBoard.DeleteChecker(new Tuple<int, int>(i, j));
                }
            }
        }
        checkersBoard.Move(new Tuple<int, int>(5, 6), new Tuple<int, int>(4, 7));
        checkersBoard.Move(new Tuple<int, int>(4, 7), new Tuple<int, int>(3, 6));
        checkersBoard.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(1, 6));
        checkersBoard.Move(new Tuple<int, int>(6, 1), new Tuple<int, int>(5, 2));
        checkersBoard.Move(new Tuple<int, int>(6, 7), new Tuple<int, int>(3, 4));
        var result = checkersBoard.MakeComputerCheckerMove(4, new Tuple<int, int>(0, 7));
        var board = result.Item1;
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker = board.GetChecker(new Tuple<int, int>(i, j));
                if (i == 6 && j == 1)
                {
                    That(checker.IsExists, Is.True);
                    That(checker.IsMissis, Is.False);
                    That(checker.IsWhite, Is.False);
                }
                else if (i == 3 && j == 6)
                {
                    That(checker.IsExists, Is.True);
                    That(checker.IsMissis, Is.False);
                    That(checker.IsWhite, Is.True);
                }
                else
                {
                    That(checker.IsExists, Is.False);
                }
            }
        }
    }

    [Test]
    public void MakeRealComputerMoveTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.PoolCheckers);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i == 6 && j == 1) || (i == 5 && j == 2) || (i == 6 && j == 7))
                {
                    checkersBoard.GetChecker(new Tuple<int, int>(i, j)).BecomeMissis();
                }
                else if ((i == 0 && j == 7) || (i == 5 && j == 6) || (i == 0 && j == 5) || (i == 2 && j == 3))
                {
                    continue;
                }
                else
                {
                    checkersBoard.DeleteChecker(new Tuple<int, int>(i, j));
                }
            }
        }
        checkersBoard.Move(new Tuple<int, int>(5, 6), new Tuple<int, int>(4, 7));
        checkersBoard.Move(new Tuple<int, int>(4, 7), new Tuple<int, int>(3, 6));
        checkersBoard.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(1, 6));
        checkersBoard.Move(new Tuple<int, int>(6, 1), new Tuple<int, int>(5, 2));
        checkersBoard.Move(new Tuple<int, int>(6, 7), new Tuple<int, int>(3, 4));
        var result = checkersBoard.MakeRealComputerMove(4);
        var board = result.Item1;
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker = board.GetChecker(new Tuple<int, int>(i, j));
                if ((i == 6 && j == 1) || (i == 0 && j == 5) || (i == 2 && j == 3))
                {
                    That(checker.IsExists, Is.True);
                    That(checker.IsMissis, Is.False);
                    That(checker.IsWhite, Is.False);
                }
                else if (i == 3 && j == 6)
                {
                    That(checker.IsExists, Is.True);
                    That(checker.IsMissis, Is.False);
                    That(checker.IsWhite, Is.True);
                }
                else
                {
                    That(checker.IsExists, Is.False);
                }
            }
        }
    }

    [Test]
    public void MakeComputerRandomMoveTest()
    {
        var checkersBoard = new CheckersBoard(8, 12, Mode.Multiplayer, Difficult.Easy, Gameplay.PoolCheckers);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i == 6 && j == 1) || (i == 5 && j == 2) || (i == 6 && j == 7))
                {
                    checkersBoard.GetChecker(new Tuple<int, int>(i, j)).BecomeMissis();
                }
                else if ((i == 0 && j == 7) || (i == 5 && j == 6) || (i == 0 && j == 1))
                {
                    continue;
                }
                else
                {
                    checkersBoard.DeleteChecker(new Tuple<int, int>(i, j));
                }
            }
        }
        checkersBoard.Move(new Tuple<int, int>(5, 6), new Tuple<int, int>(4, 7));
        checkersBoard.Move(new Tuple<int, int>(4, 7), new Tuple<int, int>(3, 6));
        checkersBoard.Move(new Tuple<int, int>(5, 2), new Tuple<int, int>(1, 6));
        checkersBoard.Move(new Tuple<int, int>(6, 1), new Tuple<int, int>(5, 2));
        checkersBoard.Move(new Tuple<int, int>(6, 7), new Tuple<int, int>(3, 4));
        var result = checkersBoard.MakeRealComputerMove(4);
        var board = result.Item1;
        bool case1 = true;
        bool case2 = true;
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                var checker = board.GetChecker(new Tuple<int, int>(i, j));
                if (i == 6 && j == 1)
                {
                    case1 &= checker.IsExists() & !checker.IsMissis() & !checker.IsWhite();
                    case2 &= !checker.IsExists();
                }
                else if (i == 3 && j == 6)
                {
                    case1 &= checker.IsExists() & !checker.IsMissis() & checker.IsWhite();
                    case2 &= !checker.IsExists();
                }
                else if (i == 0 && j == 1)
                {
                    case1 &= checker.IsExists() & !checker.IsMissis() & !checker.IsWhite();
                    case2 &= checker.IsExists() & !checker.IsMissis() & !checker.IsWhite();
                }
                else if ((i == 3 && j == 4) || (i == 5 && j == 2))
                {
                    case2 &= checker.IsExists() & checker.IsMissis() & checker.IsWhite();
                    case1 &= !checker.IsExists();
                }
                else if (i == 4 && j == 7)
                {
                    case2 &= checker.IsExists() & !checker.IsMissis() & !checker.IsWhite();
                    case1 &= !checker.IsExists();
                }
                else
                {
                    case1 &= !checker.IsExists();
                    case2 &= !checker.IsExists();
                }
            }
        }
        That(case1 || case2, Is.True);
    }
}