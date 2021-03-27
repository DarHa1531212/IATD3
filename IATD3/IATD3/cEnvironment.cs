using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IATD3
{
    public class cEnvironment
    {
        // Generation
        private const int _boardSizeBeginning = 3;
        private const float _percentageRateMonster = 10.0f;
        private const float _percentageRateAbyss = 10.0f;

        // Costs
        private const int _deathCostPerCell = -10;
        private const int _portalCostPerCell = 10;
        private const int _movementCost = -1;
        private const int _rockCost = -10;


        private int boardSize;
        private cCell[,] board;
        private bool sizeToBeAdapted;

        private int agentPosX;
        private int agentPosY;

        private cAgent agent;

        public cAgent Agent { get => agent; set => agent = value; }

        internal cCell[,] Board { get => board; set => board = value; }
        public int BoardSize { get => boardSize; set => boardSize = value; }
        public bool SizeToBeAdapted { get => sizeToBeAdapted; set => sizeToBeAdapted = value; }

        public int AgentPosX { get => agentPosX; }
        public int AgentPosY { get => agentPosY; }

        public cEnvironment()
        {
            agentPosX = 0;
            agentPosY = 0;
            AdaptSize(_boardSizeBeginning);
            //agent = new cAgent(this);
        }

        private void AdaptSize(int size)
        {
            boardSize = size;
            board = new cCell[boardSize, boardSize];
            InitializeEnvironment();
            sizeToBeAdapted = true;
        }

        private void InitializeEnvironment()
        {
            Random rng = new Random();
            int portalPosX = rng.Next(0, boardSize);
            int portalPosY = rng.Next(0, boardSize);
            for (int line = 0; line < boardSize; line++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    if (line == portalPosY && column == portalPosX)
                    {
                        board[line, column] = new cCell(line, column, true);
                        AdaptToNeighboursProperties(line, column);
                    } else
                    {
                        bool hasMonster = rng.Next(0, 100) <= _percentageRateMonster;
                        bool hasAbyss = rng.Next(0, 100) <= _percentageRateAbyss && !hasMonster;
                        board[line, column] = new cCell(line, column, hasAbyss, hasMonster);
                        AdaptToNeighboursProperties(line, column);
                        AdaptNeighboursProperties(line, column, hasAbyss, hasMonster);
                    }
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

        public bool CheckNeighboursMonsters(int line, int column)
        {
            bool hasMonsterUp = (
                (line - 1) >= 0
                    ? board[line - 1, column].HasMonster
                    : false
            );
            bool hasMonsterLeft = (
                (column - 1) >= 0
                    ? board[line, column - 1].HasMonster
                    : false
            );
            Console.WriteLine(line + " " + column + " " + hasMonsterUp + " " + hasMonsterLeft);
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

        public void GenerateNextBoard()
        {
            AdaptSize(boardSize + 1);
        }

        private bool ThrowStone(int line, int column)
        {
            bool hadMonster = board[line, column].HasMonster;
            board[line, column].HasMonster = false;
            return hadMonster;
        }

        public bool IsDeadlyCell(int line, int column)
        {
            return board[line, column].HasMonster || board[line, column].HasAbyss;
        }

        public bool IsPortal(int line, int column)
        {
            return board[line, column].HasPortal;
        }
        // Smell and wind
        private bool IsCellSmelly(int line, int column)
        {
            return board[line, column].HasOdour;
        }
        private bool IsCellWindy(int line, int column)
        {
            return board[line, column].HasWind;
        }


        // Effector actions
        public int Throw(int line, int column)
        {
            ThrowStone(line, column);
            return _rockCost;
        }

        public int UsePortal()
        {
            int returnTotal = _portalCostPerCell * boardSize * boardSize;
            GenerateNextBoard();
            return returnTotal;
        }

        public int Move(int lineMovement, int columnMovement)
        {
            if(lineMovement < 0 
                || lineMovement >= boardSize
                || columnMovement < 0
                || columnMovement >= boardSize)
            {
                return int.MinValue;
            }

            agentPosX = columnMovement;
            agentPosY = lineMovement;

            if (IsDeadlyCell(agentPosY, agentPosX))
            {
                agent.Die(board[agentPosY, agentPosX].HasMonster, board[agentPosY, agentPosX].HasAbyss);
                agentPosX = 0;
                agentPosY = 0;
                return _movementCost + _deathCostPerCell * (boardSize * boardSize);
            }
            return _movementCost;
        }

        // Sensor tests
        public bool IsAgentOnPortal()
        {
            return IsPortal(agentPosY, agentPosX);
        }

        public bool IsAgentCellSmelly()
        {
            return IsCellSmelly(agentPosY, agentPosX);
        }

        public bool IsAgentCellWindy()
        {
            return IsCellWindy(agentPosY, agentPosX);
        }
    }
}
