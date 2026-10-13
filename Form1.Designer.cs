namespace Imperialism2SaveEditor
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LoadSaveGame = new Button();
            Resources = new DataGridView();
            lblPlayerNation = new Label();
            lblTreasury = new Label();
            txtTreasury = new TextBox();
            btnApplyTreasury = new Button();
            btnApplyResources = new Button();
            ((System.ComponentModel.ISupportInitialize)Resources).BeginInit();
            SuspendLayout();
            // 
            // LoadSaveGame
            // 
            LoadSaveGame.Location = new Point(20, 12);
            LoadSaveGame.Name = "LoadSaveGame";
            LoadSaveGame.Size = new Size(150, 35);
            LoadSaveGame.TabIndex = 0;
            LoadSaveGame.Text = "Load Game";
            LoadSaveGame.UseVisualStyleBackColor = true;
            LoadSaveGame.Click += btnLoadSave_Click;
            // 
            // Resources
            // 
            Resources.AllowUserToAddRows = false;
            Resources.AllowUserToDeleteRows = false;
            Resources.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Resources.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Resources.Location = new Point(20, 149);
            Resources.Name = "Resources";
            Resources.ReadOnly = true;
            Resources.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Resources.Size = new Size(450, 600);
            Resources.TabIndex = 1;
            Resources.CellValidating += Resources_CellValidating;
            // 
            // lblPlayerNation
            // 
            lblPlayerNation.AutoSize = true;
            lblPlayerNation.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlayerNation.Location = new Point(220, 9);
            lblPlayerNation.Name = "lblPlayerNation";
            lblPlayerNation.Size = new Size(180, 17);
            lblPlayerNation.TabIndex = 2;
            lblPlayerNation.Text = "Player Nation: Not detected";
            // 
            // lblTreasury
            // 
            lblTreasury.AutoSize = true;
            lblTreasury.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTreasury.Location = new Point(231, 39);
            lblTreasury.Name = "lblTreasury";
            lblTreasury.Size = new Size(149, 17);
            lblTreasury.TabIndex = 3;
            lblTreasury.Text = "Treasury: Not detected";
            // 
            // txtTreasury
            // 
            txtTreasury.Location = new Point(220, 69);
            txtTreasury.Name = "txtTreasury";
            txtTreasury.Size = new Size(100, 23);
            txtTreasury.TabIndex = 5;
            // 
            // btnApplyTreasury
            // 
            btnApplyTreasury.Location = new Point(347, 69);
            btnApplyTreasury.Name = "btnApplyTreasury";
            btnApplyTreasury.Size = new Size(101, 23);
            btnApplyTreasury.TabIndex = 6;
            btnApplyTreasury.Text = "Apply Treasury";
            btnApplyTreasury.UseVisualStyleBackColor = true;
            btnApplyTreasury.Click += btnApplyTreasury_Click;
            // 
            // btnApplyResources
            // 
            btnApplyResources.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnApplyResources.Location = new Point(335, 755);
            btnApplyResources.Name = "btnApplyResources";
            btnApplyResources.Size = new Size(135, 53);
            btnApplyResources.TabIndex = 7;
            btnApplyResources.Text = "Apply Resources";
            btnApplyResources.UseVisualStyleBackColor = true;
            btnApplyResources.Click += btnApplyResources_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(494, 820);
            Controls.Add(btnApplyResources);
            Controls.Add(btnApplyTreasury);
            Controls.Add(txtTreasury);
            Controls.Add(lblTreasury);
            Controls.Add(lblPlayerNation);
            Controls.Add(Resources);
            Controls.Add(LoadSaveGame);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Imperialism II Savegame Editor";
            ((System.ComponentModel.ISupportInitialize)Resources).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LoadSaveGame;
        private DataGridView Resources;
        private Label lblPlayerNation;
        private Label lblTreasury;
        private TextBox txtTreasury;
        private Button btnApplyTreasury;
        private Button btnApplyResources;
    }
}
