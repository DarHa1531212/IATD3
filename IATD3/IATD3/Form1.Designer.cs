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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Board
            // 
            this.Board.Location = new System.Drawing.Point(16, 50);
            this.Board.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(267, 246);
            this.Board.TabIndex = 0;
            // 
            // boardNumber
            // 
            this.boardNumber.AutoSize = true;
            this.boardNumber.Location = new System.Drawing.Point(124, 21);
            this.boardNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.boardNumber.Name = "boardNumber";
            this.boardNumber.Size = new System.Drawing.Size(46, 17);
            this.boardNumber.TabIndex = 1;
            this.boardNumber.Text = "label1";
            // 
            // bMove
            // 
            this.bMove.Location = new System.Drawing.Point(16, 15);
            this.bMove.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bMove.Name = "bMove";
            this.bMove.Size = new System.Drawing.Size(100, 28);
            this.bMove.TabIndex = 2;
            this.bMove.Text = "Move";
            this.bMove.UseVisualStyleBackColor = true;
            this.bMove.Click += new System.EventHandler(this.bMove_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1005, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(320, 246);
            this.textBox1.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1337, 594);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.bMove);
            this.Controls.Add(this.boardNumber);
            this.Controls.Add(this.Board);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Welcome to the forest !";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Board;
        private System.Windows.Forms.Label boardNumber;
        private System.Windows.Forms.Button bMove;
        private System.Windows.Forms.TextBox textBox1;
    }
}

