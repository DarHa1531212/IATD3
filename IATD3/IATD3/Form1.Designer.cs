namespace IATD3
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.Board = new System.Windows.Forms.Panel();
            this.boardNumber = new System.Windows.Forms.Label();
            this.bMove = new System.Windows.Forms.Button();
            this.actionsLog = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bReset = new System.Windows.Forms.Button();
            this.gameOverMsg = new System.Windows.Forms.Label();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.currentScore = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.scoreHistory = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Board
            // 
            this.Board.Location = new System.Drawing.Point(149, 53);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(200, 200);
            this.Board.TabIndex = 0;
            // 
            // boardNumber
            // 
            this.boardNumber.AutoSize = true;
            this.boardNumber.Location = new System.Drawing.Point(230, 17);
            this.boardNumber.Name = "boardNumber";
            this.boardNumber.Size = new System.Drawing.Size(35, 13);
            this.boardNumber.TabIndex = 1;
            this.boardNumber.Text = "label1";
            // 
            // bMove
            // 
            this.bMove.Location = new System.Drawing.Point(149, 12);
            this.bMove.Name = "bMove";
            this.bMove.Size = new System.Drawing.Size(75, 23);
            this.bMove.TabIndex = 2;
            this.bMove.Text = "Move";
            this.bMove.UseVisualStyleBackColor = true;
            this.bMove.Click += new System.EventHandler(this.bMove_Click);
            // 
            // actionsLog
            // 
            this.actionsLog.AutoSize = true;
            this.actionsLog.Location = new System.Drawing.Point(12, 58);
            this.actionsLog.Name = "actionsLog";
            this.actionsLog.Size = new System.Drawing.Size(0, 13);
            this.actionsLog.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Journal des actions";
            // 
            // bReset
            // 
            this.bReset.Location = new System.Drawing.Point(15, 12);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(94, 23);
            this.bReset.TabIndex = 5;
            this.bReset.Text = "Reset the game";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
            // 
            // gameOverMsg
            // 
            this.gameOverMsg.AutoSize = true;
            this.gameOverMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameOverMsg.Location = new System.Drawing.Point(153, 17);
            this.gameOverMsg.Name = "gameOverMsg";
            this.gameOverMsg.Size = new System.Drawing.Size(71, 13);
            this.gameOverMsg.TabIndex = 6;
            this.gameOverMsg.Text = "YOU DIED.";
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.Location = new System.Drawing.Point(317, 9);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(108, 13);
            this.scoreLabel.TabIndex = 7;
            this.scoreLabel.Text = "Score du niveau :";
            this.scoreLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // currentScore
            // 
            this.currentScore.AutoSize = true;
            this.currentScore.Location = new System.Drawing.Point(425, 9);
            this.currentScore.Name = "currentScore";
            this.currentScore.Size = new System.Drawing.Size(13, 13);
            this.currentScore.TabIndex = 9;
            this.currentScore.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(317, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Récapitulatif des scores :";
            // 
            // scoreHistory
            // 
            this.scoreHistory.AutoSize = true;
            this.scoreHistory.Location = new System.Drawing.Point(461, 28);
            this.scoreHistory.Name = "scoreHistory";
            this.scoreHistory.Size = new System.Drawing.Size(0, 13);
            this.scoreHistory.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 483);
            this.Controls.Add(this.scoreHistory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.currentScore);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.gameOverMsg);
            this.Controls.Add(this.bReset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.actionsLog);
            this.Controls.Add(this.bMove);
            this.Controls.Add(this.boardNumber);
            this.Controls.Add(this.Board);
            this.Name = "Form1";
            this.Text = "Welcome to the forest !";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Board;
        private System.Windows.Forms.Label boardNumber;
        private System.Windows.Forms.Button bMove;
        private System.Windows.Forms.Label actionsLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.Label gameOverMsg;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Label currentScore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label scoreHistory;
    }
}

