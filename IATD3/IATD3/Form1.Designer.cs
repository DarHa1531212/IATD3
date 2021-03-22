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
            this.SuspendLayout();
            // 
            // Board
            // 
            this.Board.Location = new System.Drawing.Point(16, 36);
            this.Board.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(267, 246);
            this.Board.TabIndex = 0;
            // 
            // boardNumber
            // 
            this.boardNumber.AutoSize = true;
            this.boardNumber.Location = new System.Drawing.Point(18, 16);
            this.boardNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.boardNumber.Name = "boardNumber";
            this.boardNumber.Size = new System.Drawing.Size(46, 17);
            this.boardNumber.TabIndex = 1;
            this.boardNumber.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.boardNumber);
            this.Controls.Add(this.Board);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Welcome to the forest !";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Board;
        private System.Windows.Forms.Label boardNumber;
    }
}

