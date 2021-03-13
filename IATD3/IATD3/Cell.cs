using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IATD3
{
    class Cell
    {
        private Label label;
        private int posX;
        private int posY;
        private bool hasAbyss;
        private bool hasMonster;
        private bool hasOdour;
        private bool hasWind;
        private bool hasPortal;

        public bool HasWind { get => hasWind; set => hasWind = value; }
        public bool HasOdour { get => hasOdour; set => hasOdour = value; }
        public bool HasAbyss { get => hasAbyss; set => hasAbyss = value; }
        public bool HasMonster { get => hasMonster; set => hasMonster = value; }
        public bool HasPortal { get => hasPortal; set => hasPortal = value; }
        public Label Label { get => label; set => label = value; }

        public Cell(int posX, int posY, bool hasAbyss, bool hasMonster) {
            this.posX = posX;
            this.posY = posY;
            this.hasAbyss = hasAbyss;
            this.hasMonster = hasMonster;
            this.hasOdour = false;
            this.hasWind = false;
        }
        public Cell(int posX, int posY, bool hasPortal)
        {
            this.posX = posX;
            this.posY = posY;
            this.hasPortal = hasPortal;
            this.hasAbyss = false;
            this.hasMonster = false;
            this.hasOdour = false;
            this.hasWind = false;
        }
    }
}
