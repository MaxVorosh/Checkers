using GameClasses;
using static NUnit.Framework.Assert;

namespace GameClassesTests;

public class BoardTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ArrangeCheckersTest()
    {
        var board = new Board(8, 12);
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                That(board.GetChecker(new Tuple<int, int>(i, j)).IsMissis(), Is.False);
                if ((i + j) % 2 == 0 || i == 4 || i == 3)
                {
                    That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists(), Is.False);
                }
                else if (i < 3)
                {
                    That(board.GetChecker(new Tuple<int, int>(i, j)).IsWhite(), Is.False);
                    That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists(), Is.True);
                }
                else if (i > 4)
                {
                    That(board.GetChecker(new Tuple<int, int>(i, j)).IsWhite(), Is.True);
                    That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists(), Is.True);
                }
            }
        }
    }

    [Test]
    public void GetTileTest()
    {
        var board = new Board(8, 12);
        That(board.GetTile(0, 0), Is.EqualTo(new Tuple<int, int>(0, 0)));
        That(board.GetTile(1, 49), Is.EqualTo(new Tuple<int, int>(0, 0)));
        That(board.GetTile(399, 399), Is.EqualTo(new Tuple<int, int>(7, 7)));
        That(board.GetTile(210, 40), Is.EqualTo(new Tuple<int, int>(4, 0)));
        That(board.GetTile(50, 100), Is.EqualTo(new Tuple<int, int>(1, 2)));
    }

    [Test]
    public void GetCheckerTest()
    {
        var board = new Board(8, 12);
        board.GetChecker(new Tuple<int, int>(0, 1)).BecomeMissis();
        board.GetChecker(new Tuple<int, int>(7, 6)).BecomeMissis();
        That(board.GetChecker(new Tuple<int, int>(7, 6)).IsExists, Is.True);
        That(board.GetChecker(new Tuple<int, int>(7, 6)).IsMissis, Is.True);
        That(board.GetChecker(new Tuple<int, int>(7, 6)).IsWhite, Is.True);
        That(board.GetChecker(new Tuple<int, int>(0, 1)).IsExists, Is.True);
        That(board.GetChecker(new Tuple<int, int>(0, 1)).IsMissis, Is.True);
        That(board.GetChecker(new Tuple<int, int>(0, 1)).IsWhite, Is.False);
        That(board.GetChecker(new Tuple<int, int>(7, 4)).IsExists, Is.True);
        That(board.GetChecker(new Tuple<int, int>(7, 4)).IsMissis, Is.False);
        That(board.GetChecker(new Tuple<int, int>(7, 4)).IsWhite, Is.True);
        That(board.GetChecker(new Tuple<int, int>(0, 3)).IsExists, Is.True);
        That(board.GetChecker(new Tuple<int, int>(0, 3)).IsMissis, Is.False);
        That(board.GetChecker(new Tuple<int, int>(0, 3)).IsWhite, Is.False);
        That(board.GetChecker(new Tuple<int, int>(7, 7)).IsExists, Is.False);
        That(board.GetChecker(new Tuple<int, int>(4, 5)).IsExists, Is.False);
    }

    [Test]
    public void DeleteCheckerTest()
    {
        var board = new Board(8, 12);
        board.DeleteChecker(new Tuple<int, int>(0, 1));
        board.DeleteChecker(new Tuple<int, int>(7, 6));
        board.DeleteChecker(new Tuple<int, int>(4, 4));
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i + j) % 2 == 0 || i == 3 || i == 4 || (i == 7 && j == 6) || (i == 0 && j == 1))
                {
                    That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.False);
                }
                else
                {
                    That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.True);
                }
            }
        }
    }

    [Test]
    public void MoveCheckerTest()
    {
        var board = new Board(8, 12);
        board.MoveChecker(new Tuple<int, int>(0, 0), new Tuple<int, int>(4, 4));
        board.MoveChecker(new Tuple<int, int>(0, 1), new Tuple<int, int>(4, 5));
        board.MoveChecker(new Tuple<int, int>(7, 6), new Tuple<int, int>(3, 4));
        board.MoveChecker(new Tuple<int, int>(3, 4), new Tuple<int, int>(7, 6));
        board.GetChecker(new Tuple<int, int>(7, 6)).BecomeMissis();
        board.MoveChecker(new Tuple<int, int>(7, 6), new Tuple<int, int>(3, 2));
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                if ((i + j) % 2 == 0 || i == 3 || i == 4)
                {
                    if (i == 4 && j == 5)
                    {
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.True);
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsWhite, Is.False);
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsMissis, Is.False);
                    }
                    else if (i == 3 && j == 2)
                    {
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.True);
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsWhite, Is.True);
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsMissis, Is.True);
                    }
                    else
                    {
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.False);
                    }
                }
                else if (i < 3)
                {
                    if (i == 0 && j == 1)
                    {
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.False);
                    }
                    else
                    {
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.True);
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsWhite, Is.False);
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsMissis, Is.False);
                    }
                }
                else
                {
                    if (i == 7 && j == 6)
                    {
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.False);
                    }
                    else
                    {
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsExists, Is.True);
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsWhite, Is.True);
                        That(board.GetChecker(new Tuple<int, int>(i, j)).IsMissis, Is.False);
                    }
                }
            }
        }
    }

    [Test]
    public void CopyTest()
    {
        var board = new Board(8, 12);
        board.MoveChecker(new Tuple<int, int>(7, 2), new Tuple<int, int>(4, 5));
        var copyBoard = board.Copy();
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                Checker oldChecker = board.GetChecker(new Tuple<int, int>(i, j));
                Checker newChecker = copyBoard.GetChecker(new Tuple<int, int>(i, j));
                That(oldChecker.IsExists, Is.EqualTo(newChecker.IsExists()));
                That(oldChecker.IsMissis, Is.EqualTo(newChecker.IsMissis()));
                That(oldChecker.IsWhite, Is.EqualTo(newChecker.IsWhite()));
            }
        }
    }
}