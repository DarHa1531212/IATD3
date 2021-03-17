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
            po.Invoke("AdaptToNeighboursProperties", 1, 1);

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
            po.Invoke("AdaptToNeighboursProperties", 1, 1);

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
            po.Invoke("AdaptToNeighboursProperties", 1, 1);

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
            po.Invoke("AdaptToNeighboursProperties", 0, 1);

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
            po.Invoke("AdaptToNeighboursProperties", 1, 0);

            // Assert
            Assert.IsFalse(board[1, 0].HasOdour, "There shouldn't be odour.");
            Assert.IsFalse(board[1, 0].HasWind, "There shouldn't be wind.");
        }

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
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", 1, 1);

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
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", 1, 1);

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
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", 1, 1);

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
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", 1, 0);

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
            bool hasMonster = (bool)po.Invoke("CheckNeighboursMonsters", 0, 1);

            // Assert
            Assert.IsFalse(hasMonster);
        }

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
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", 1, 1);

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
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", 1, 1);

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
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", 1, 1);

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
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", 1, 0);

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
            bool hasAbyss = (bool)po.Invoke("CheckNeighboursAbysses", 0, 1);

            // Assert
            Assert.IsFalse(hasAbyss);
        }

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
            po.Invoke("AdaptNeighboursWind", 1, 1);

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
            po.Invoke("AdaptNeighboursWind", 1, 0);

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
            po.Invoke("AdaptNeighboursWind", 0, 1);

            // Assert
            Assert.IsTrue(board[0, 0].HasWind);
        }

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
            po.Invoke("AdaptNeighboursMonster", 1, 1);

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
            po.Invoke("AdaptNeighboursMonster", 1, 0);

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
            po.Invoke("AdaptNeighboursMonster", 0, 1);

            // Assert
            Assert.IsTrue(board[0, 0].HasOdour);
        }

        [TestMethod]
        public void T_ThrowStone_OnMonster()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[2, 2].HasMonster = true;

            // Act
            bool monsterKilled = environment.ThrowStone(2, 2);

            // Assert
            Assert.IsFalse(board[2, 2].HasMonster);
            Assert.IsTrue(monsterKilled);
        }

        [TestMethod]
        public void T_ThrowStone_OnNothing()
        {
            // Arrange
            cEnvironment environment = new cEnvironment();
            PrivateObject po = new PrivateObject(environment);
            cCell[,] board = (cCell[,])po.GetProperty("Board");
            board[2, 2].HasMonster = false;

            // Act
            bool monsterKilled = environment.ThrowStone(2, 2);

            // Assert
            Assert.IsFalse(board[2, 2].HasMonster);
            Assert.IsFalse(monsterKilled);
        }

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
    }
}
