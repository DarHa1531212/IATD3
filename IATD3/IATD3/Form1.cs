using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IATD3
{
    public partial class Form1 : Form
    {
        Environment environment;

        public Form1()
        {
            InitializeComponent();
            this.AutoSize = true;
            environment = new Environment();
            createLabels();
            Board.AutoSize = true;
        }

        private void createLabels()
        {
            // design inspired by code found at https://playwithcsharpdotnet.blogspot.com/
            int boardSize = environment.BoardSize;
            for (int line = 0; line < boardSize; line++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    Label label = new Label();
                    //cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 180 / size);
                    //cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 180 / size);
                    label.Size = new Size(360 / boardSize, 360 / boardSize);
                    label.BorderStyle = BorderStyle.Fixed3D;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.ForeColor = SystemColors.ControlDarkDark;
                    label.Location = new Point(line * 360 / boardSize, column * 360 / boardSize);
                    label.BackColor = Color.White;
                    label.Text = "(" + line + ", " + column + ")\n" + 
                        (environment.Board[line, column].HasAbyss ? "Abyss\n" : "") +
                        (environment.Board[line, column].HasMonster ? "Monster\n" : "") +
                        (environment.Board[line, column].HasOdour ? "Odour\n" : "") +
                        (environment.Board[line, column].HasWind ? "Wind\n" : "");
                    environment.Board[line, column].Label = label;

                    Board.Controls.Add(label);
                }
            }
        }
    }
}
