namespace DUH_Trends_Palas_POS.Views
{
    partial class LogIn
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogIn));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            cmbUserLevel = new ComboBox();
            btnLogin = new Button();
            btnExit = new Button();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnShowPassword = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImage = Properties.Resources.logo;
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(419, 445);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(518, 45);
            label1.Name = "label1";
            label1.Size = new Size(160, 51);
            label1.TabIndex = 1;
            label1.Text = "LOGIN";
            // 
            // cmbUserLevel
            // 
            cmbUserLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUserLevel.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbUserLevel.FormattingEnabled = true;
            cmbUserLevel.Items.AddRange(new object[] { "Owner", "Admin", "Cashier" });
            cmbUserLevel.Location = new Point(425, 131);
            cmbUserLevel.Name = "cmbUserLevel";
            cmbUserLevel.Size = new Size(344, 33);
            cmbUserLevel.TabIndex = 2;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.LightSteelBlue;
            btnLogin.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogin.Location = new Point(452, 324);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(125, 42);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.OrangeRed;
            btnExit.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnExit.Location = new Point(619, 324);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(125, 42);
            btnExit.TabIndex = 4;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(425, 194);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new Size(344, 34);
            txtUsername.TabIndex = 5;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(425, 253);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(344, 34);
            txtPassword.TabIndex = 6;
            // 
            // btnShowPassword
            // 
            btnShowPassword.BackColor = Color.LightSteelBlue;
            btnShowPassword.BackgroundImageLayout = ImageLayout.Zoom;
            btnShowPassword.Font = new Font("Century Gothic", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnShowPassword.Location = new Point(775, 253);
            btnShowPassword.Name = "btnShowPassword";
            btnShowPassword.Size = new Size(61, 36);
            btnShowPassword.TabIndex = 7;
            btnShowPassword.Text = "Show";
            btnShowPassword.UseVisualStyleBackColor = false;
            btnShowPassword.Click += btnShowPassword_Click;
            // 
            // LogIn
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(883, 445);
            Controls.Add(btnShowPassword);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(btnExit);
            Controls.Add(btnLogin);
            Controls.Add(cmbUserLevel);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LogIn";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Duh Trends Palas";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private ComboBox cmbUserLevel;
        private Button btnLogin;
        private Button btnExit;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnShowPassword;
    }
}