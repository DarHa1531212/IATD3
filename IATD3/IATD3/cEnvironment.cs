using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        Label actionsLog;
        Button bMove;
        Label gameOverMsg;
        private cAgent agent;

        public cAgent Agent { get => agent; set => agent = value; }

        internal cCell[,] Board { get => board; set => board = value; }
        public int BoardSize { get => boardSize; set => boardSize = value; }
        public bool SizeToBeAdapted { get => sizeToBeAdapted; set => sizeToBeAdapted = value; }

        public int AgentPosX { get => agentPosX; }
        public int AgentPosY { get => agentPosY; }

        private int currentAgentScore;

        public cEnvironment(Label log = null, Button move = null, Label gameOver = null)
        {
            agentPosX = 0;
            agentPosY = 0;
            currentAgentScore = 0;
            AdaptSize(_boardSizeBeginning);
            //agent = new cAgent(this);
            actionsLog = log;
            bMove = move;
            gameOverMsg = gameOver;
        }

        public void RemoveDeadMonsterOdour(int deadMonsterlocationX, int deadMonsterlocationY)
        {
            foreach (var neighbour in GetNeighbouringPositions(deadMonsterlocationX, deadMonsterlocationY))
            {
                board[neighbour.Item1, neighbour.Item2].HasOdour = 
                    GetNeighbouringPositions(neighbour.Item1, neighbour.Item2).Any(
                        neighbours => board[neighbours.Item1, neighbours.Item2].HasMonster
                    );
            }
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
            // delete l.82
            // delete l.83
            portalPosX = 2;
            portalPosY = 2;

            for (int posX = 0; posX < boardSize; posX++)
            {
                for (int posY = 0; posY < boardSize; posY++)
                {
                    if (posX == portalPosY && posY == portalPosX)
                    {
                        board[posX, posY] = new cCell(posX, posY, true);
                        AdaptToNeighboursProperties(posX, posY);
                    }
                    else
                    {
                        bool hasMonster = rng.Next(0, 100) <= _percentageRateMonster;
                        bool hasAbyss = rng.Next(0, 100) <= _percentageRateAbyss && !hasMonster;

                        // TODO : delete le if
                        if((posX == 0 && posY == 1) || (posX == 1 && posY == 2))
                        {
                            hasMonster = true;
                            hasAbyss = false;
                        }

                        board[posX, posY] = new cCell(posX, posY, hasAbyss, hasMonster);
                        AdaptToNeighboursProperties(posX, posY);
                        AdaptNeighboursProperties(posX, posY, hasAbyss, hasMonster);
                    }
                }
            }
        }

        private void AdaptToNeighboursProperties(int posX, int posY)
        {
            if (CheckNeighboursMonsters(posX, posY))
            {
                board[posX, posY].HasOdour = true;
            }
            if (CheckNeighboursAbysses(posX, posY))
            {
                board[posX, posY].HasWind = true;
            }
        }

        public bool CheckNeighboursMonsters(int posX, int posY)
        {
            bool hasMonsterUp = (
                (posX - 1) >= 0
                    ? board[posX - 1, posY].HasMonster
                    : false
            );
            bool hasMonsterLeft = (
                (posY - 1) >= 0
                    ? board[posX, posY - 1].HasMonster
                    : false
            );
            return hasMonsterUp || hasMonsterLeft;
        }

        private bool CheckNeighboursAbysses(int posX, int posY)
        {
            bool hasAbyssUp = (
                (posX - 1) >= 0 &&
                board[posX - 1, posY].HasAbyss
            );
            bool hasAbyssLeft = (
                (posY - 1) >= 0 &&
                board[posX, posY - 1].HasAbyss
            );
            return hasAbyssUp || hasAbyssLeft;
        }

        private void AdaptNeighboursProperties(int posX, int posY, bool hasAbyss, bool hasMonster)
        {
            if (hasAbyss)
            {
                AdaptNeighboursWind(posX, posY);
            }
            if (hasMonster)
            {
                AdaptNeighboursMonster(posX, posY);
            }
        }

        private void AdaptNeighboursWind(int posX, int posY)
        {
            if ((posX - 1) >= 0)
            {
                board[posX - 1, posY].HasWind = true;
            }
            if ((posY - 1) >= 0)
            {
                board[posX, posY - 1].HasWind = true;
            }
        }

        private void AdaptNeighboursMonster(int posX, int posY)
        {
            if ((posX - 1) >= 0)
            {
                board[posX - 1, posY].HasOdour = true;
            }
            if ((posY - 1) >= 0)
            {
                board[posX, posY - 1].HasOdour = true;
            }
        }

        public void GenerateNextBoard()
        {
            AdaptSize(boardSize + 1);
        }

        private void LogAction(String action, Tuple<int, int> pos)
        {
            if (actionsLog != null)
            {
                String text =
                    action +
                    (pos != null ? " at (" + pos.Item1 + ", " + pos.Item2 + ")" : "") +
                    "\n";
                actionsLog.Text += text;
            }
        }

        private bool ThrowStone(int posX, int posY)
        {
            LogAction("ThrowStone", new Tuple<int, int>(posX, posY));

            bool hadMonster = board[posX, posY].HasMonster;
            board[posX, posY].HasMonster = false;
            RemoveOdoursOfDeadMonster(posX, posY);
            return hadMonster;
        }

        public String IsDeadlyCell(int posX, int posY)
        {
            if (board[posX, posY].HasMonster) {
                return "hasMonster";
            }
            else if (board[posX, posY].HasAbyss)
            {
                return "hasAbyss";
            }
            return String.Empty;
        }

        public bool IsPortal(int posX, int posY)
        {
            return board[posX, posY].HasPortal;
        }
        // Smell and wind
        private bool IsCellSmelly(int posX, int posY)
        {
            return board[posX, posY].HasOdour;
        }
        private bool IsCellWindy(int posX, int posY)
        {
            return board[posX, posY].HasWind;
        }


        // Effector actions
        public int Throw(int posX, int posY)
        {
            ThrowStone(posX, posY);
            currentAgentScore += _rockCost;
            return _rockCost;
        }

        public int UsePortal()
        {
            LogAction("UsePortal", new Tuple<int, int>(agentPosX, agentPosY));

            int returnTotal = _portalCostPerCell * boardSize * boardSize;
            currentAgentScore += returnTotal;
            //agent.SetUtility(currentAgentScore);
            GenerateNextBoard();
            agentPosX = 0;
            agentPosY = 0;
            currentAgentScore = 0;

            return returnTotal;
        }

        public Tuple<bool, int> CheckAgentDeath()
        {
            String isDeadlyCell = IsDeadlyCell(agentPosX, agentPosY);
            if (isDeadlyCell != String.Empty)
            {
                agent.UpdateCellDeath(agentPosX, agentPosY, isDeadlyCell);
                return new Tuple<bool, int>(true, killAgent());
            }
            return new Tuple<bool, int>(false, 0);
        }

        public int Move(int xMovement, int yMovement)
        {
            LogAction("Move", new Tuple<int, int>(xMovement, yMovement));

            if (xMovement < 0
                || xMovement >= boardSize
                || yMovement < 0
                || yMovement >= boardSize)
            {
                return int.MinValue;
            }

            agentPosX = xMovement;
            agentPosY = yMovement;

            /*if (IsDeadlyCell(agentPosY, agentPosX))
            {
                return killAgent();
            }*/
            Tuple<bool, int> agentDeath = CheckAgentDeath();
            if (agentDeath.Item1)
            {
                return agentDeath.Item2;
            }

            agent.UpdatePosition(agentPosX, agentPosY);
            return _movementCost;
        }

        public int killAgent()
        {
            LogAction("killAgent", new Tuple<int, int>(agentPosX, agentPosY));
            if (agentPosX == 0 && agentPosY == 0)
            {
                gameOverMsg.Visible = true;
                bMove.Visible = false;
            }

            agent.Die(board[agentPosX, agentPosY].HasMonster, board[agentPosX, agentPosY].HasAbyss);
            agentPosX = 0;
            agentPosY = 0;
            return _movementCost + _deathCostPerCell * (boardSize * boardSize);
        }

        // Sensor tests
        public bool IsAgentOnPortal()
        {
            return IsPortal(agentPosX, agentPosY);
        }

        public bool IsAgentCellSmelly()
        {
            return IsCellSmelly(agentPosX, agentPosY);
        }

        public bool IsAgentCellWindy()
        {
            return IsCellWindy(agentPosX, agentPosY);
        }

        public List<Tuple<int, int>> GetNeighbouringPositions(int posX, int posY)
        {
            List<Tuple<int, int>> neighbours = new List<Tuple<int, int>>();
            if (posX - 1 >= 0)
            {
                neighbours.Add(new Tuple<int, int>(posX - 1, posY));
            }
            if (posY - 1 >= 0)
            {
                neighbours.Add(new Tuple<int, int>(posX, posY - 1));
            }
            if (posX + 1 < boardSize)
            {
                neighbours.Add(new Tuple<int, int>(posX + 1, posY));
            }
            if (posY + 1 < boardSize)
            {
                neighbours.Add(new Tuple<int, int>(posX, posY + 1));
            }
            return neighbours;
        }

        public void RemoveOdoursOfDeadMonster(int monsterPosX, int monsterPosY)
        {
            foreach (var neighbour in GetNeighbouringPositions(monsterPosX, monsterPosY))
            {
                bool odor = false;
                foreach (var neighbourExtended in GetNeighbouringPositions(neighbour.Item1, neighbour.Item2))
                {
                    if (board[neighbourExtended.Item1, neighbourExtended.Item2].HasMonster)
                    {
                        odor = true;
                        break;
                    }
                }
                board[neighbour.Item1, neighbour.Item2].HasOdour = odor;
            }
        }
    }
}
