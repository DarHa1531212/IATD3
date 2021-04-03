using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IATD3
{
    public class cEnvironment
    {
        #region Constants

        // Generation
        private const int _boardSizeBeginning = 3;
        private const float _percentageRateMonster = 10.0f;
        private const float _percentageRateAbyss = 10.0f;

        // Costs
        private const int _deathCostPerCell = -10;
        private const int _portalCostPerCell = 10;
        private const int _movementCost = -1;
        private const int _rockCost = -10;

        #endregion

        #region Attributes
        
        // Board
        private int boardSize;
        private cCell[,] board;
        private bool sizeToBeAdapted;

        // Agent
        private cAgent agent;
        private int agentPosX;
        private int agentPosY;
        private int currentAgentScore;

        // Graphic display
        readonly Label actionsLog;
        readonly Button bMove;
        readonly Label gameOverMsg;

        #endregion

        #region Getters / Setters

        public cAgent Agent { get => agent; set => agent = value; }

        internal cCell[,] Board { get => board; set => board = value; }
        public int BoardSize { get => boardSize; set => boardSize = value; }
        public bool SizeToBeAdapted { get => sizeToBeAdapted; set => sizeToBeAdapted = value; }

        public int AgentPosX => agentPosX;
        public int AgentPosY => agentPosY;

        #endregion

        #region Constructor

        public cEnvironment(Label log = null, Button move = null, Label gameOver = null)
        {
            agentPosX = 0;
            agentPosY = 0;
            currentAgentScore = 0;
            AdaptSize(_boardSizeBeginning);

            actionsLog = log;
            bMove = move;
            gameOverMsg = gameOver;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Removes the odour around the old monster position (now dead).
        /// </summary>
        /// <param name="deadMonsterPositionX">The dead monster position x.</param>
        /// <param name="deadMonsterPositionY">The dead monster position y.</param>
        public void RemoveDeadMonsterOdour(int deadMonsterPositionX, int deadMonsterPositionY)
        {
            foreach (Tuple<int, int> neighbour in GetNeighbouringPositions(deadMonsterPositionX, deadMonsterPositionY))
            {
                board[neighbour.Item1, neighbour.Item2].HasOdour =
                    GetNeighbouringPositions(neighbour.Item1, neighbour.Item2).Any(
                        neighbours => board[neighbours.Item1, neighbours.Item2].HasMonster
                    );
            }
        }

        /// <summary>
        /// Generates the next board.
        /// </summary>
        public void GenerateNextBoard()
        {
            AdaptSize(boardSize + 1);
        }

        /// <summary>
        /// Throws the stone to the specified position.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>The action cost.</returns>
        public int Throw(int posX, int posY)
        {
            ThrowStone(posX, posY);
            currentAgentScore += _rockCost;
            return _rockCost;
        }

        /// <summary>
        /// Uses the portal.
        /// </summary>
        /// <returns>The action cost.</returns>
        public int UsePortal()
        {
            LogAction("UsePortal", new Tuple<int, int>(agentPosX, agentPosY));

            int returnTotal = _portalCostPerCell * boardSize * boardSize;
            currentAgentScore += returnTotal;
            GenerateNextBoard();
            agentPosX = 0;
            agentPosY = 0;
            currentAgentScore = 0;

            return returnTotal;
        }

        /// <summary>
        /// Checks the agent death.
        /// </summary>
        /// <returns>
        ///     - If the agent is dead
        ///     - The code of the source of death if previous is true, otherwise 0.
        /// </returns>
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

        /// <summary>
        /// Moves to the specified coordinates.
        /// </summary>
        /// <param name="xMovement">The x movement.</param>
        /// <param name="yMovement">The y movement.</param>
        /// <returns></returns>
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

            agent.UpdatePosition(agentPosX, agentPosY);
            Tuple<bool, int> agentDeath = CheckAgentDeath();
            if (agentDeath.Item1)
            {
                return agentDeath.Item2;
            }
            return _movementCost;
        }

        /// <summary>
        /// Determines whether agent is on portal.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the agent is on portal; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAgentOnPortal()
        {
            return IsPortal(agentPosX, agentPosY);
        }

        /// <summary>
        /// Determines whether the agent is at a smelly position.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the agent is at a smelly position; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAgentCellSmelly()
        {
            return IsCellSmelly(agentPosX, agentPosY);
        }

        /// <summary>
        /// Determines whether the agent is at a windy position.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the agent is at a windy position; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAgentCellWindy()
        {
            return IsCellWindy(agentPosX, agentPosY);
        }

        /// <summary>
        /// Determines whether the agent is at a position with an abyss.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the agent is at a position with an abyss; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAgentOnAbyss()
        {
            return board[agentPosX, agentPosY].HasAbyss;
        }

        /// <summary>
        /// Determines whether the agent is at a position with a monster.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the agent is at a position with a monster; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAgentOnMonster()
        {
            return board[agentPosX, agentPosY].HasMonster;
        }

        /// <summary>
        /// Gets the neighbouring positions.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>The list of neighbours positions.</returns>
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

        #endregion

        #region Private methods

        /// <summary>
        /// Adapts the size.
        /// </summary>
        /// <param name="size">The size.</param>
        private void AdaptSize(int size)
        {
            boardSize = size;
            board = new cCell[boardSize, boardSize];
            InitializeEnvironment();
            sizeToBeAdapted = true;
        }

        /// <summary>
        /// Initializes the environment.
        /// </summary>
        private void InitializeEnvironment()
        {
            Random rng = new Random();
            int portalPosX = rng.Next(0, boardSize);
            int portalPosY = rng.Next(0, boardSize);

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
                        board[posX, posY] = new cCell(posX, posY, hasAbyss, hasMonster);
                        AdaptToNeighboursProperties(posX, posY);
                        AdaptNeighboursProperties(posX, posY, hasAbyss, hasMonster);
                    }
                }
            }
        }

        /// <summary>
        /// Adapts to the neighbours properties.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
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

        /// <summary>
        /// Checks if the neighbours have monsters.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>If a monster is right up or left.</returns>
        private bool CheckNeighboursMonsters(int posX, int posY)
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

        /// <summary>
        /// Checks if the neighbours have abysses.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>If an abyss is right up or left.</returns>
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

        /// <summary>
        /// Adapts the neighbours properties.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <param name="hasAbyss">If there is an abyss on the position.</param>
        /// <param name="hasMonster">If there is a monster on the position.</param>
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

        /// <summary>
        /// Adapts the neighbours wind.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
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

        /// <summary>
        /// Adapts the neighbours monster.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
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

        /// <summary>
        /// Logs the action in the graphic display.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="pos">The position.</param>
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

        /// <summary>
        /// Throws the stone.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>Whether the cell contained a monster.</returns>
        private bool ThrowStone(int posX, int posY)
        {
            LogAction("ThrowStone", new Tuple<int, int>(posX, posY));

            bool hadMonster = board[posX, posY].HasMonster;
            board[posX, posY].HasMonster = false;
            RemoveOdoursOfDeadMonster(posX, posY);
            return hadMonster;
        }

        /// <summary>
        /// Determines whether the specified position is deadly to the agent.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>If the cell contains a monster or an abyss.</returns>
        private String IsDeadlyCell(int posX, int posY)
        {
            if (board[posX, posY].HasMonster)
            {
                return "hasMonster";
            }
            else if (board[posX, posY].HasAbyss)
            {
                return "hasAbyss";
            }
            return String.Empty;
        }

        /// <summary>
        /// Determines whether the specified position contains a portal.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>
        ///   <c>true</c> if the specified position contains a portal; otherwise, <c>false</c>.
        /// </returns>
        private bool IsPortal(int posX, int posY)
        {
            return board[posX, posY].HasPortal;
        }

        /// <summary>
        /// Determines whether the specified position contains an odour.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>
        ///   <c>true</c> if the specified position contains an odour; otherwise, <c>false</c>.
        /// </returns>
        private bool IsCellSmelly(int posX, int posY)
        {
            return board[posX, posY].HasOdour;
        }

        /// <summary>
        /// Determines whether the specified position contains some wind.
        /// </summary>
        /// <param name="posX">The position x.</param>
        /// <param name="posY">The position y.</param>
        /// <returns>
        ///   <c>true</c> if the specified position contains some wind; otherwise, <c>false</c>.
        /// </returns>
        private bool IsCellWindy(int posX, int posY)
        {
            return board[posX, posY].HasWind;
        }

        /// <summary>
        /// Kills the agent.
        /// </summary>
        /// <returns>The penalty to issue to the score.</returns>
        private int killAgent()
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

        /// <summary>
        /// Removes the odours of dead monster.
        /// </summary>
        /// <param name="monsterPosX">The monster position x.</param>
        /// <param name="monsterPosY">The monster position y.</param>
        private void RemoveOdoursOfDeadMonster(int monsterPosX, int monsterPosY)
        {
            foreach (Tuple<int, int> neighbour in GetNeighbouringPositions(monsterPosX, monsterPosY))
            {
                bool odor = false;
                foreach (Tuple<int, int> neighbourExtended in GetNeighbouringPositions(neighbour.Item1, neighbour.Item2))
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

        #endregion
    }
}
