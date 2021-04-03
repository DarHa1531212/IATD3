using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace IATD3
{
    public partial class Form1 : Form
    {
        private const bool debugMode = true;
        private static readonly Dictionary<String, String> filenames = new Dictionary<String, String>(){
            { "Portal", "Portal.png" },
            { "Abyss", "Abyss.png" },
            { "Monster", "Monster.png" },
            { "Wind", "Wind.png" },
            { "Odour", "Odour.png" },
            { "Agent", "Agent.png" }
        };
        private static readonly Dictionary<String, Color> colors = new Dictionary<String, Color>(){
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
            AutoSize = true;
            environment = new cEnvironment(actionsLog, bMove, gameOverMsg);
            Board.AutoSize = true;
            boardCpt = 0;
            boardNumber.Text = String.Empty;
            actionsLog.Text = String.Empty;
            formAgent = new cAgent(environment);
            environment.Agent = formAgent;
            gameOverMsg.Visible = false;
            bMove.Visible = true;
            resetScoreLabel();
            resetHistory();
            if (environment.SizeToBeAdapted)
            {
                AdaptSize();
            }
            formAgent.UseSensors();    //mettre à jour les faits
        }

        private void createLabels()
        {
            // design inspired by code found at https://playwithcsharpdotnet.blogspot.com/
            int boardSize = environment.BoardSize;
            Board.Controls.Clear();
            for (int posX = 0; posX < boardSize; posX++)
            {
                for (int posY = 0; posY < boardSize; posY++)
                {
                    Label label = new Label
                    {
                        //cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 180 / size);
                        //cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 180 / size);
                        //label.TextAlign = ContentAlignment.MiddleCenter;
                        Size = new Size(360 / boardSize, 360 / boardSize),
                        BorderStyle = BorderStyle.Fixed3D,
                        ForeColor = SystemColors.ControlDarkDark,
                        Location = new Point(posX * 360 / boardSize, posY * 360 / boardSize),
                        BackColor = Color.White
                    };
                    /*label.Text = "(" + posX + ", " + posY + ")\n" +
                        (environment.Board[posX, posY].HasAbyss ? "Abyss\n" : "") +
                        (environment.Board[posX, posY].HasMonster ? "Monster\n" : "") +
                        (environment.Board[posX, posY].HasOdour ? "Odour\n" : "") +
                        (environment.Board[posX, posY].HasWind ? "Wind\n" : "");*/

                    if (debugMode)
                    {
                        if (environment.AgentPosX == posX && environment.AgentPosY == posY)
                        {
                            createPictureBox("Agent", label, new Point(label.Size.Width / 4, label.Size.Width / 4));
                            label.BringToFront();
                        }
                        if (environment.Board[posX, posY].HasPortal)
                        {
                            createPictureBox("Portal", label, new Point(0, 0), true);
                        }
                        if (environment.Board[posX, posY].HasAbyss)
                        {
                            createPictureBox("Abyss", label, new Point(0, 0));
                        }
                        if (environment.Board[posX, posY].HasMonster)
                        {
                            createPictureBox("Monster", label, new Point(0, label.Size.Width / 2));
                        }
                        if (environment.Board[posX, posY].HasWind)
                        {
                            createPictureBox("Wind", label, new Point(label.Size.Width / 2, 0));
                        }
                        if (environment.Board[posX, posY].HasOdour)
                        {
                            createPictureBox("Odour", label, new Point(label.Size.Width / 2, label.Size.Width / 2));
                        }
                    }

                    environment.Board[posX, posY].Label = label;

                    Board.Controls.Add(label);
                }
            }
        }

        private void AdaptSize()
        {
            if (environment.SizeToBeAdapted)
            {
                resetScoreLabel();
            }
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
            PictureBox img = new PictureBox
            {
                ImageLocation = @"../../images/" + filenames[pictureKey],
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = location,
                BackColor = colors[pictureKey],
                Size = fullSize
                ? new Size(parent.Size.Width, parent.Size.Height)
                : new Size(parent.Size.Width / 2, parent.Size.Height / 2)
            };
            new ToolTip().SetToolTip(img, pictureKey);
            parent.Controls.Add(img);
        }


        private void bMove_Click(object sender, EventArgs e)
        {
            int actionCost = formAgent.Act(); //l'agent fait une action
            updateScoreLabel(actionCost);
            if (actionCost > 0)
            {
                addScoreToHistory();
            }
            //test end condition
            //if end reached, generate new board

            formAgent.UseSensors();    //mettre à jour les faits
            AdaptSize(); // maj de l'affichage
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            InitObject();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void currentScore_Click(object sender, EventArgs e)
        {

        }

        private void updateScoreLabel(int addedScore)
        {
            int currentScore = int.Parse(this.currentScore.Text);
            this.currentScore.Text = (currentScore + addedScore).ToString();
        }

        private void resetScoreLabel()
        {
            currentScore.Text = "0";
        }

        private void addScoreToHistory()
        {
            if (scoreHistory.Text == String.Empty)
            {
                scoreHistory.Text += currentScore.Text;
            }
            else
            {
                scoreHistory.Text += ", " + currentScore.Text;
            }
        }

        private void resetHistory()
        {
            scoreHistory.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
