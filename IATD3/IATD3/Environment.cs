using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IATD3
{
    public class Environment
    {
        private const int _boardSizeBeginning = 4;
        private const float _percentageRateMonster = 10.0f;
        private const float _percentageRateAbyss = 10.0f;

        private int boardSize;
        private Cell[,] board;
        private int playerPosX;
        private int playerPosY;

        internal Cell[,] Board { get => board; set => board = value; }
        public int BoardSize { get => boardSize; set => boardSize = value; }

        public Environment()
        {
            boardSize = _boardSizeBeginning;
            board = new Cell[boardSize, boardSize];
            InitializeEnvironment();
        }

        private void InitializeEnvironment()
        {
            Random rng = new Random();
            for (int line = 0; line < boardSize; line++)
            {
                for (int column = 0; column < boardSize; column++)
                {

                    bool hasMonster = rng.Next(0, 100) <= _percentageRateMonster;
                    bool hasAbyss = rng.Next(0, 100) <= _percentageRateAbyss;

                    board[line, column] = new Cell(line, column, hasAbyss, hasMonster);
                    AdaptToNeighboursProperties(line, column);
                    AdaptNeighboursProperties(line, column, hasAbyss, hasMonster);
                }
            }
        }

        private void AdaptToNeighboursProperties(int line, int column)
        {
            if (CheckNeighboursMonsters(line, column))
            {
                board[line, column].HasOdour = true;
            }
            if (CheckNeighboursAbysses(line, column))
            {
                board[line, column].HasWind = true;
            }
        }

        private bool CheckNeighboursMonsters(int line, int column)
        {
            bool hasMonsterUp = (
                (line - 1) >= 0 &&
                board[line - 1, column].HasMonster
            );
            bool hasMonsterLeft = (
                (column - 1) >= 0 &&
                board[line, column - 1].HasMonster
            );
            return hasMonsterUp || hasMonsterLeft;
        }

        private bool CheckNeighboursAbysses(int line, int column)
        {
            bool hasAbyssUp = (
                (line - 1) >= 0 &&
                board[line - 1, column].HasAbyss
            );
            bool hasAbyssLeft = (
                (column - 1) >= 0 &&
                board[line, column - 1].HasAbyss
            );
            return hasAbyssUp || hasAbyssLeft;
        }

        private void AdaptNeighboursProperties(int line, int column, bool hasAbyss, bool hasMonster)
        {
            if (hasAbyss)
            {
                AdaptNeighboursWind(line, column);
            }
            if (hasMonster)
            {
                AdaptNeighboursMonster(line, column);
            }
        }

        private void AdaptNeighboursWind(int line, int column)
        {
            if ((line - 1) >= 0)
            {
                board[line - 1, column].HasWind = true;
            }
            if ((column - 1) >= 0)
            {
                board[line, column - 1].HasWind = true;
            }
        }

        private void AdaptNeighboursMonster(int line, int column)
        {
            if ((line - 1) >= 0)
            {
                board[line - 1, column].HasOdour = true;
            }
            if ((column - 1) >= 0)
            {
                board[line, column - 1].HasOdour = true;
            }
        }
    }
}
