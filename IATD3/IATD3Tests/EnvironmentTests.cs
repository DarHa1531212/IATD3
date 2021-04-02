using IATD3;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace IATD3Tests
{
    /// <summary>
    /// Description résumée pour EnvironmentTests
    /// </summary>
    [TestClass]
    public class EnvironmentTests
    {
        #region AdaptToNeighboursProperties
        [TestMethod]
        public void T_AdaptToNeighboursProperties_MonsterUp()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasMonster = true;
            board[1, 0].HasMonster = false;
            board[1, 1].HasOdour = false;

            // Act
            po.Invoke("AdaptToNeighboursProperties", new object[] { 1, 1 });

            // Assert
            Assert.IsTrue(board[0, 1].HasMonster);
            Assert.IsFalse(board[1, 0].HasMonster);
            Assert.IsTrue(board[1, 1].HasOdour);
        }

        [TestMethod]
        public void T_AdaptToNeighboursProperties_MonsterLeft()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasMonster = false;
            board[1, 0].HasMonster = true;
            board[1, 1].HasOdour = false;

            // Act
            po.Invoke("AdaptToNeighboursProperties", new object[] { 1, 1 });

            // Assert
            Assert.IsFalse(board[0, 1].HasMonster);
            Assert.IsTrue(board[1, 0].HasMonster);
            Assert.IsTrue(board[1, 1].HasOdour);
        }

        [TestMethod]
        public void T_AdaptToNeighboursProperties_NoMonster()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasMonster = false;
            board[1, 0].HasMonster = false;
            board[1, 1].HasOdour = false;

            // Act
            po.Invoke("AdaptToNeighboursProperties", new object[] { 1, 1 });

            // Assert
            Assert.IsFalse(board[0, 1].HasMonster);
            Assert.IsFalse(board[1, 0].HasMonster);
            Assert.IsFalse(board[1, 1].HasOdour);
        }

        [TestMethod]
        public void T_AdaptToNeighboursProperties_BorderUp()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasMonster = false;
            board[0, 1].HasOdour = false;
            board[0, 0].HasAbyss = false;
            board[0, 1].HasWind = false;

            // Act
            po.Invoke("AdaptToNeighboursProperties", new object[] { 0, 1 });

            // Assert
            Assert.IsFalse(board[0, 1].HasOdour, "There shouldn't be odour.");
            Assert.IsFalse(board[0, 1].HasWind, "There shouldn't be wind.");
        }

        [TestMethod]
        public void T_AdaptToNeighboursProperties_BorderLeft()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasMonster = false;
            board[1, 0].HasOdour = false;
            board[0, 0].HasAbyss = false;
            board[1, 0].HasWind = false;

            // Act
            po.Invoke("AdaptToNeighboursProperties", new object[] { 1, 1 });

            // Assert
            Assert.IsFalse(board[1, 0].HasOdour, "There shouldn't be odour.");
            Assert.IsFalse(board[1, 0].HasWind, "There shouldn't be wind.");
        }
        #endregion

        #region CheckNeighboursMonsters
        [TestMethod]
        public void T_CheckNeighboursMonsters_MonsterUp()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasMonster = true;
            board[1, 0].HasMonster = false;
            board[1, 1].HasOdour = false;

            // Act
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", new object[] { 1, 1 });

            // Assert
            Assert.IsTrue(hasMonster);
        }

        [TestMethod]
        public void T_CheckNeighboursMonsters_MonsterLeft()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasMonster = false;
            board[1, 0].HasMonster = true;
            board[1, 1].HasOdour = false;

            // Act
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", new object[] { 1, 1 });

            // Assert
            Assert.IsTrue(hasMonster);
        }

        [TestMethod]
        public void T_CheckNeighboursMonsters_NoMonster()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasMonster = false;
            board[1, 0].HasMonster = false;
            board[1, 1].HasOdour = false;

            // Act
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", new object[] { 1, 1 });

            // Assert
            Assert.IsFalse(hasMonster);
        }

        [TestMethod]
        public void T_CheckNeighboursMonsters_BorderUp()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasMonster = false;
            board[1, 0].HasOdour = false;

            // Act
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", new object[] { 1, 0 });

            // Assert
            Assert.IsFalse(hasMonster);
        }
        
        [TestMethod]
        public void T_CheckNeighboursMonsters_BorderLeft()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasMonster = false;
            board[0, 1].HasOdour = false;

            // Act
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", new object[] { 0, 1 });

            // Assert
            Assert.IsFalse(hasMonster);
        }
        #endregion

        #region CheckNeighboursAbysses
        [TestMethod]
        public void T_CheckNeighboursAbysses_AbyssUp()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasAbyss = true;
            board[1, 0].HasAbyss = false;
            board[1, 1].HasWind = false;

            // Act
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", new object[] { 1, 1 });

            // Assert
            Assert.IsTrue(hasAbyss);
        }

        [TestMethod]
        public void T_CheckNeighboursAbysses_MonsterLeft()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasAbyss = false;
            board[1, 0].HasAbyss = true;
            board[1, 1].HasWind = false;

            // Act
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", new object[] { 1, 1 });

            // Assert
            Assert.IsTrue(hasAbyss);
        }

        [TestMethod]
        public void T_CheckNeighboursAbysses_NoMonster()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasAbyss = false;
            board[1, 0].HasAbyss = false;
            board[1, 1].HasWind = false;

            // Act
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", new object[] { 1, 1 });

            // Assert
            Assert.IsFalse(hasAbyss);
        }

        [TestMethod]
        public void T_CheckNeighboursAbysses_BorderUp()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasAbyss = false;
            board[1, 0].HasWind = false;

            // Act
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", new object[] { 1, 0 });

            // Assert
            Assert.IsFalse(hasAbyss);
        }

        [TestMethod]
        public void T_CheckNeighboursAbysses_BorderLeft()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasAbyss = false;
            board[0, 1].HasWind = false;

            // Act
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", new object[] { 0, 1 });

            // Assert
            Assert.IsFalse(hasAbyss);
        }
        #endregion

        #region AdaptNeighboursWind
        [TestMethod]
        public void T_AdaptNeighboursWind()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasWind = false;
            board[1, 0].HasWind = false;
            board[1, 1].HasAbyss = true;

            // Act
            po.Invoke("AdaptNeighboursWind", new object[] { 1, 1 });

            // Assert
            Assert.IsTrue(board[0, 1].HasWind, "Up propagation failed.");
            Assert.IsTrue(board[1, 0].HasWind, "Left propagation failed.");
        }

        [TestMethod]
        public void T_AdaptNeighboursWind_BorderUp()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasWind = false;
            board[1, 0].HasAbyss = true;

            // Act
            po.Invoke("AdaptNeighboursWind", new object[] { 1, 0 });

            // Assert
            Assert.IsTrue(board[0, 0].HasWind);
        }

        [TestMethod]
        public void T_AdaptNeighboursWind_BorderLeft()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasWind = false;
            board[0, 1].HasAbyss = true;

            // Act
            po.Invoke("AdaptNeighboursWind", new object[] { 0, 1 });

            // Assert
            Assert.IsTrue(board[0, 0].HasWind);
        }
        #endregion

        #region AdaptNeighboursMonster
        [TestMethod]
        public void T_AdaptNeighboursMonster()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasOdour = false;
            board[1, 0].HasOdour = false;
            board[1, 1].HasMonster = true;

            // Act
            po.Invoke("AdaptNeighboursMonster", new object[] { 1, 1 });

            // Assert
            Assert.IsTrue(board[0, 1].HasOdour, "Up propagation failed.");
            Assert.IsTrue(board[1, 0].HasOdour, "Left propagation failed.");
        }

        [TestMethod]
        public void T_AdaptNeighboursMonster_BorderUp()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasOdour = false;
            board[1, 0].HasMonster = true;

            // Act
            po.Invoke("AdaptNeighboursMonster", new object[] { 1, 0 });

            // Assert
            Assert.IsTrue(board[0, 0].HasOdour);
        }

        [TestMethod]
        public void T_AdaptNeighboursMonster_BorderLeft()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 0].HasOdour = false;
            board[0, 1].HasMonster = true;

            // Act
            po.Invoke("AdaptNeighboursMonster", new object[] { 0, 1 });

            // Assert
            Assert.IsTrue(board[0, 0].HasOdour);
        }
        #endregion

        #region IsDeadlyCell
        [TestMethod]
        public void T_IsDeadlyCell_Monster()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[2, 2].HasMonster = true;
            board[2, 2].HasAbyss = false;

            // Act
            bool isDeadlyCell = environment.IsDeadlyCell(2, 2);

            // Assert
            Assert.IsTrue(isDeadlyCell);
        }

        [TestMethod]
        public void T_IsDeadlyCell_Abyss()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[2, 2].HasMonster = false;
            board[2, 2].HasAbyss = true;

            // Act
            bool isDeadlyCell = environment.IsDeadlyCell(2, 2);

            // Assert
            Assert.IsTrue(isDeadlyCell);
        }

        [TestMethod]
        public void T_IsDeadlyCell_False()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[2, 2].HasMonster = false;
            board[2, 2].HasAbyss = false;

            // Act
            bool isDeadlyCell = environment.IsDeadlyCell(2, 2);

            // Assert
            Assert.IsFalse(isDeadlyCell);
        }
        #endregion

        #region Move

        #region OutOfBounds
        [TestMethod]
        public void T_Move_OutOfBounds_LineColumnInferior()
        {
            cEnvironment environment = new cEnvironment();
            int result = environment.Move(-1, -1);

            Assert.AreEqual(result, int.MinValue);
        }

        [TestMethod]
        public void T_Move_OutOfBounds_LineInferiorColumnIn()
        {
            cEnvironment environment = new cEnvironment();
            int result = environment.Move(-4, 1);

            Assert.AreEqual(result, int.MinValue);
        }

        [TestMethod]
        public void T_Move_OutOfBounds_LineInferiorColumnSuperior()
        {
            cEnvironment environment = new cEnvironment();
            int result = environment.Move(-4, 6);

            Assert.AreEqual(result, int.MinValue);
        }

        [TestMethod]
        public void T_Move_OutOfBounds_LineInColumnInferior()
        {
            cEnvironment environment = new cEnvironment();
            int result = environment.Move(1, -2);

            Assert.AreEqual(result, int.MinValue);
        }

        [TestMethod]
        public void T_Move_OutOfBounds_LineInColumnSuperior()
        {
            cEnvironment environment = new cEnvironment();
            int result = environment.Move(1, 9);

            Assert.AreEqual(result, int.MinValue);
        }

        [TestMethod]
        public void T_Move_OutOfBounds_LineSuperiorColumnInferior()
        {
            cEnvironment environment = new cEnvironment();
            int result = environment.Move(3, -3);

            Assert.AreEqual(result, int.MinValue);
        }

        [TestMethod]
        public void T_Move_OutOfBounds_LineSuperiorColumnIn()
        {
            cEnvironment environment = new cEnvironment();
            int result = environment.Move(5, 0);

            Assert.AreEqual(result, int.MinValue);
        }

        [TestMethod]
        public void T_Move_OutOfBounds_LineSuperiorColumnSuperior()
        {
            cEnvironment environment = new cEnvironment();
            int result = environment.Move(5, 7);

            Assert.AreEqual(result, int.MinValue);
        }
        #endregion

        #region InBounds

        #region Safe
        [TestMethod]
        public void T_Move_InBoundsSafe()
        {
            cEnvironment environment = new cEnvironment();
            cAgent agent = new cAgent(environment);
            environment.Agent = agent;
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 1].HasMonster = false;

            // Act
            object utility = po.Invoke("Move", new object[] { 0, 1 });

            // Assert
            Assert.AreEqual((int)utility, -1);
        }
        #endregion

        #region Deadly
        [TestMethod]
        public void T_Move_InBoundsMonsterSize3()
        {
            cEnvironment environment = new cEnvironment();
            cAgent agent = new cAgent(environment);
            environment.Agent = agent;
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[2, 1].HasMonster = true;

            // Act
            object utility = po.Invoke("Move", new object[] { 2, 1 });

            // Assert
            Assert.AreEqual((int)utility, -91);
        }

        [TestMethod]
        public void T_Move_InBoundsMonsterSize5()
        {
            cEnvironment environment = new cEnvironment();
            cAgent agent = new cAgent(environment);
            environment.Agent = agent;
            PrivateObject po = new PrivateObject(environment);
            po.Invoke("AdaptSize", new object[] { 5 });
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[3, 4].HasMonster = true;

            // Act
            object utility = po.Invoke("Move", new object[] { 3, 4 });

            // Assert
            Assert.AreEqual((int)utility, -251);
        }
        [TestMethod]
        public void T_Move_InBoundsAbyssSize4()
        {
            cEnvironment environment = new cEnvironment();
            cAgent agent = new cAgent(environment);
            environment.Agent = agent;
            PrivateObject po = new PrivateObject(environment);
            po.Invoke("AdaptSize", new object[] { 4 });
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[0, 3].HasAbyss = true;

            // Act
            object utility = po.Invoke("Move", new object[] { 0, 3 });

            // Assert
            Assert.AreEqual((int)utility, -161);
        }

        [TestMethod]
        public void T_Move_InBoundsAbyssSize7()
        {
            cEnvironment environment = new cEnvironment();
            cAgent agent = new cAgent(environment);
            environment.Agent = agent;
            PrivateObject po = new PrivateObject(environment);
            po.Invoke("AdaptSize", new object[] { 7 });
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[6, 6].HasAbyss = true;

            // Act
            object utility = po.Invoke("Move", new object[] { 6, 6 });

            // Assert
            Assert.AreEqual((int)utility, -491);
        }

        [TestMethod]
        public void T_Move_InBoundsMonsterAndAbyssSize8()
        {
            cEnvironment environment = new cEnvironment();
            cAgent agent = new cAgent(environment);
            environment.Agent = agent;
            PrivateObject po = new PrivateObject(environment);
            po.Invoke("AdaptSize", new object[] { 8 });
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[5, 3].HasAbyss = true;

            // Act
            object utility = po.Invoke("Move", new object[] { 5, 3 });

            // Assert
            Assert.AreEqual((int)utility, -641);
        }
        #endregion

        #endregion

        #endregion
    }
}
