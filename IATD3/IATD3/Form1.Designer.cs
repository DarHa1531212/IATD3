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
            this.Board.Location = new System.Drawing.Point(18, 45);
            this.Board.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(300, 308);
            this.Board.TabIndex = 0;
            // 
            // boardNumber
            // 
            this.boardNumber.AutoSize = true;
            this.boardNumber.Location = new System.Drawing.Point(20, 20);
            this.boardNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.boardNumber.Name = "boardNumber";
            this.boardNumber.Size = new System.Drawing.Size(51, 20);
            this.boardNumber.TabIndex = 1;
            this.boardNumber.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.boardNumber);
            this.Controls.Add(this.Board);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Welcome to the forest !";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Board;
        private System.Windows.Forms.Label boardNumber;
    }
}

