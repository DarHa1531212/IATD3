using System.Windows.Forms;

namespace IATD3
{
    public class cCell
    {
        #region Attributes

        private Label label;
        private readonly int posX;
        private readonly int posY;
        private bool hasAbyss;
        private bool hasMonster;
        private bool hasOdour;
        private bool hasWind;
        private bool hasPortal;

        #endregion

        #region Getters / Setters

        public bool HasWind { get => hasWind; set => hasWind = value; }
        public bool HasOdour { get => hasOdour; set => hasOdour = value; }
        public bool HasAbyss { get => hasAbyss; set => hasAbyss = value; }
        public bool HasMonster { get => hasMonster; set => hasMonster = value; }
        public bool HasPortal { get => hasPortal; set => hasPortal = value; }
        public Label Label { get => label; set => label = value; }

        #endregion

        #region Constructors

        public cCell(int posX, int posY, bool hasAbyss, bool hasMonster)
        {
            this.posX = posX;
            this.posY = posY;
            this.hasAbyss = hasAbyss;
            this.hasMonster = hasMonster;
            hasOdour = false;
            hasWind = false;
        }
        public cCell(int posX, int posY, bool hasPortal)
        {
            this.posX = posX;
            this.posY = posY;
            this.hasPortal = hasPortal;
            hasAbyss = false;
            hasMonster = false;
            hasOdour = false;
            hasWind = false;
        }

        #endregion
    }
}
