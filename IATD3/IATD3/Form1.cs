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
        private const bool debugMode = true;
        private static Dictionary<String, String> filenames = new Dictionary<String, String>(){
            { "Portal", "Portal.png" },
            { "Abyss", "Abyss.png" },
            { "Monster", "Monster.png" },
            { "Wind", "Wind.png" },
            { "Odour", "Odour.png" },
            { "Agent", "Agent.png" }
        };
        private static Dictionary<String, Color> colors = new Dictionary<String, Color>(){
            { "Portal", Color.Beige },
            { "Abyss", Color.LightSkyBlue },
            { "Monster", Color.PaleVioletRed },
            { "Wind", Color.RoyalBlue },
            { "Odour", Color.Violet },
            { "Agent", Color.LimeGreen }
        };

        static cEnvironment environment;
        private cAgent formAgent;
        private int boardCpt;

        public Form1()
        {
            InitializeComponent();
            InitObject();
        }

        private void InitObject()
        {
            this.AutoSize = true;
            environment = new cEnvironment(this.actionsLog, this.bMove, this.gameOverMsg);
            Board.AutoSize = true;
            boardCpt = 0;
            boardNumber.Text = String.Empty;
            actionsLog.Text = String.Empty;
            formAgent = new cAgent(environment);
            environment.Agent = formAgent;
            gameOverMsg.Visible = false;
            bMove.Visible = true;

            if (environment.SizeToBeAdapted)
            {
                AdaptSize();
            }
        }

        private void createLabels()
        {
            // design inspired by code found at https://playwithcsharpdotnet.blogspot.com/
            int boardSize = environment.BoardSize;
            Board.Controls.Clear();
            for (int line = 0; line < boardSize; line++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    Label label = new Label();
                    //cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 180 / size);
                    //cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 180 / size);
                    //label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Size = new Size(360 / boardSize, 360 / boardSize);
                    label.BorderStyle = BorderStyle.Fixed3D;
                    label.ForeColor = SystemColors.ControlDarkDark;
                    label.Location = new Point(line * 360 / boardSize, column * 360 / boardSize);
                    label.BackColor = Color.White;
                    /*label.Text = "(" + line + ", " + column + ")\n" +
                        (environment.Board[line, column].HasAbyss ? "Abyss\n" : "") +
                        (environment.Board[line, column].HasMonster ? "Monster\n" : "") +
                        (environment.Board[line, column].HasOdour ? "Odour\n" : "") +
                        (environment.Board[line, column].HasWind ? "Wind\n" : "");*/

                    if (debugMode)
                    {
                        if (environment.AgentPosX == column && environment.AgentPosY == line)
                        {
                            createPictureBox("Agent", label, new Point(label.Size.Width / 4, label.Size.Width / 4));
                            label.BringToFront();
                        }
                        if (environment.Board[line, column].HasPortal)
                        {
                            createPictureBox("Portal", label, new Point(0, 0), true);
                        }
                        if (environment.Board[line, column].HasAbyss)
                        {
                            createPictureBox("Abyss", label, new Point(0, 0));
                        }
                        if (environment.Board[line, column].HasMonster)
                        {
                            createPictureBox("Monster", label, new Point(0, label.Size.Width / 2));
                        }
                        if (environment.Board[line, column].HasWind)
                        {
                            createPictureBox("Wind", label, new Point(label.Size.Width / 2, 0));
                        }
                        if (environment.Board[line, column].HasOdour)
                        {
                            createPictureBox("Odour", label, new Point(label.Size.Width / 2, label.Size.Width / 2));
                        }
                    }

                    environment.Board[line, column].Label = label;

                    Board.Controls.Add(label);
                }
            }
        }

        private void AdaptSize()
        {
            createLabels();
            environment.SizeToBeAdapted = false;
            boardNumber.Text = "Board number " + boardCpt;

            Tuple<bool, int> agentDeath = environment.CheckAgentDeath();
            if (agentDeath.Item1)
            {
                // score += agentDeath.Item2;
            }
        }

        private void createPictureBox(string pictureKey, Label parent, Point location, bool fullSize = false)
        {
            PictureBox img = new PictureBox();
            img.ImageLocation = @"../../images/" + filenames[pictureKey];
            img.SizeMode = PictureBoxSizeMode.Zoom;
            img.Location = location;
            img.BackColor = colors[pictureKey];
            img.Size = fullSize
                ? new Size(parent.Size.Width, parent.Size.Height)
                : new Size(parent.Size.Width / 2, parent.Size.Height / 2);
            new ToolTip().SetToolTip(img, pictureKey);
            parent.Controls.Add(img);
        }


        private void bMove_Click(object sender, EventArgs e)
        {
            Console.WriteLine("NEW MOVE ACTION *****************");
            formAgent.UseSensors();    //mettre à jour les faits

            int actionCost = formAgent.Act(); //l'agent fait une action
            //test end condition
            //if end reached, generate new board

            AdaptSize(); // maj de l'affichage
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            InitObject();
        }
    }
}
