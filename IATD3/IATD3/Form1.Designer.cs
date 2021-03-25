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
            this.SuspendLayout();
            // 
            // Board
            // 
            this.Board.Location = new System.Drawing.Point(12, 41);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(200, 200);
            this.Board.TabIndex = 0;
            // 
            // boardNumber
            // 
            this.boardNumber.AutoSize = true;
            this.boardNumber.Location = new System.Drawing.Point(93, 17);
            this.boardNumber.Name = "boardNumber";
            this.boardNumber.Size = new System.Drawing.Size(35, 13);
            this.boardNumber.TabIndex = 1;
            this.boardNumber.Text = "label1";
            // 
            // bMove
            // 
            this.bMove.Location = new System.Drawing.Point(12, 12);
            this.bMove.Name = "bMove";
            this.bMove.Size = new System.Drawing.Size(75, 23);
            this.bMove.TabIndex = 2;
            this.bMove.Text = "Move";
            this.bMove.UseVisualStyleBackColor = true;
            this.bMove.Click += new System.EventHandler(this.bMove_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}

