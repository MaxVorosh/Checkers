using System;


namespace Checkers;

public class Board
    {
        private int _size;
        private int tileSize;
        private int xBoard;
        private int yBoard;
        private Checker[,] checkers;
        private int _cntCheckersForOne;

        public Board(int size, int cnt)
        {
            _size = size;
            _cntCheckersForOne = cnt;
            checkers = new Checker[_size, _size];
            for (int i = 0; i < _size; ++i)
            {
                for (int j = 0; j < _size; ++j)
                {
                    checkers[i, j] = new Checker(true, false, false);
                }
            }
            ArrangeCheckers();
        }

        private void ArrangeCheckers()
        {
            for (int i = 0; i < _cntCheckersForOne / (_size / 2); ++i)
            {
                for (int j = 0; j < _size; ++j)
                {
                    if ((i + j) % 2 == 0)
                    {
                        checkers[i, j] = new Checker(true, false, true);
                        checkers[_size - i - 1, _size - j - 1] = new Checker(false, false, true);
                    }
                }
            }
        }

        public Tuple<int, int> GetTile(int xMouse, int yMouse)
        {
            return new Tuple<int, int>((xMouse - xBoard) / tileSize, (yMouse - yBoard) / tileSize);
        }

        public Checker GetChecker(Tuple<int, int> tile)
        {
            return checkers[tile.Item1, tile.Item2];
        }

        public void DeleteChecker(Tuple<int, int> tile)
        {
            checkers[tile.Item1, tile.Item2].Delete();
        }

        public void MoveChecker(Tuple<int, int> tileFrom, Tuple<int, int> tileTo)
        {
            checkers[tileTo.Item1, tileTo.Item2] = checkers[tileFrom.Item1, tileFrom.Item2].Copy();
            checkers[tileFrom.Item1, tileFrom.Item2].Delete();
        }

        /*public void Print()
        {
            Console.Write(' ');
            for (int i = 0; i < _size; ++i)
            {
                Console.Write(Convert.ToString(i));
            }
            Console.Write('\n');

            for (int i = _size - 1; i >= 0; --i)
            {
                Console.Write(Convert.ToString(i));
                for (int j = 0; j < _size; ++j)
                {
                    if (checkers[i, j].IsExists())
                    {
                        Console.Write((checkers[i, j].IsWhite()) ? "W" : "B");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.Write('\n');
            }
        }*/
    }