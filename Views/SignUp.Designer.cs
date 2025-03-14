namespace DUH_Trends_Palas_POS.Views
{
    partial class SignUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignUp));
            label1 = new Label();
            txtSUUsername = new TextBox();
            txtSUPassword = new TextBox();
            cmbSUUserLevel = new ComboBox();
            btnCreateAccount = new Button();
            btnBackToHome = new Button();
            txtSUFirstname = new TextBox();
            txtSULastname = new TextBox();
            txtSUEmail = new TextBox();
            txtSUConfirmPassword = new TextBox();
            btnSUShowPassword = new Button();
            txtContactNumber = new TextBox();
            txtSUAddress = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Century Gothic", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(226, 61);
            label1.Name = "label1";
            label1.Size = new Size(368, 51);
            label1.TabIndex = 2;
            label1.Text = "Create new user";
            // 
            // txtSUUsername
            // 
            txtSUUsername.BackColor = Color.FromArgb(224, 224, 224);
            txtSUUsername.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUUsername.Location = new Point(93, 486);
            txtSUUsername.Name = "txtSUUsername";
            txtSUUsername.PlaceholderText = "Username";
            txtSUUsername.Size = new Size(628, 34);
            txtSUUsername.TabIndex = 6;
            // 
            // txtSUPassword
            // 
            txtSUPassword.BackColor = Color.FromArgb(224, 224, 224);
            txtSUPassword.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUPassword.Location = new Point(93, 549);
            txtSUPassword.Name = "txtSUPassword";
            txtSUPassword.PlaceholderText = "Password";
            txtSUPassword.Size = new Size(550, 34);
            txtSUPassword.TabIndex = 7;
            // 
            // cmbSUUserLevel
            // 
            cmbSUUserLevel.BackColor = Color.FromArgb(224, 224, 224);
            cmbSUUserLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSUUserLevel.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbSUUserLevel.FormattingEnabled = true;
            cmbSUUserLevel.Items.AddRange(new object[] { "Owner", "Admin", "Cashier" });
            cmbSUUserLevel.Location = new Point(93, 298);
            cmbSUUserLevel.Name = "cmbSUUserLevel";
            cmbSUUserLevel.Size = new Size(628, 33);
            cmbSUUserLevel.TabIndex = 8;
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.BackColor = SystemColors.GradientActiveCaption;
            btnCreateAccount.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCreateAccount.Location = new Point(318, 677);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(233, 39);
            btnCreateAccount.TabIndex = 12;
            btnCreateAccount.Text = "Create Account";
            btnCreateAccount.UseVisualStyleBackColor = false;
            btnCreateAccount.Click += btnCreateAccount_Click;
            // 
            // btnBackToHome
            // 
            btnBackToHome.BackColor = Color.OrangeRed;
            btnBackToHome.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBackToHome.Location = new Point(380, 727);
            btnBackToHome.Name = "btnBackToHome";
            btnBackToHome.Size = new Size(108, 39);
            btnBackToHome.TabIndex = 13;
            btnBackToHome.Text = "Back";
            btnBackToHome.UseVisualStyleBackColor = false;
            btnBackToHome.Click += btnBackToHome_Click;
            // 
            // txtSUFirstname
            // 
            txtSUFirstname.BackColor = Color.FromArgb(224, 224, 224);
            txtSUFirstname.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUFirstname.Location = new Point(93, 168);
            txtSUFirstname.Name = "txtSUFirstname";
            txtSUFirstname.PlaceholderText = "Firstname";
            txtSUFirstname.Size = new Size(311, 34);
            txtSUFirstname.TabIndex = 14;
            // 
            // txtSULastname
            // 
            txtSULastname.BackColor = Color.FromArgb(224, 224, 224);
            txtSULastname.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSULastname.Location = new Point(410, 168);
            txtSULastname.Name = "txtSULastname";
            txtSULastname.PlaceholderText = "Lastname";
            txtSULastname.Size = new Size(311, 34);
            txtSULastname.TabIndex = 15;
            // 
            // txtSUEmail
            // 
            txtSUEmail.BackColor = Color.FromArgb(224, 224, 224);
            txtSUEmail.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUEmail.Location = new Point(93, 233);
            txtSUEmail.Name = "txtSUEmail";
            txtSUEmail.PlaceholderText = "Email";
            txtSUEmail.Size = new Size(628, 34);
            txtSUEmail.TabIndex = 16;
            // 
            // txtSUConfirmPassword
            // 
            txtSUConfirmPassword.BackColor = Color.FromArgb(224, 224, 224);
            txtSUConfirmPassword.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUConfirmPassword.Location = new Point(93, 612);
            txtSUConfirmPassword.Name = "txtSUConfirmPassword";
            txtSUConfirmPassword.PlaceholderText = "Confirm Password";
            txtSUConfirmPassword.Size = new Size(550, 34);
            txtSUConfirmPassword.TabIndex = 17;
            // 
            // btnSUShowPassword
            // 
            btnSUShowPassword.BackColor = Color.LightSteelBlue;
            btnSUShowPassword.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSUShowPassword.Location = new Point(655, 579);
            btnSUShowPassword.Name = "btnSUShowPassword";
            btnSUShowPassword.Size = new Size(64, 31);
            btnSUShowPassword.TabIndex = 18;
            btnSUShowPassword.Text = "Show";
            btnSUShowPassword.UseVisualStyleBackColor = false;
            btnSUShowPassword.Click += btnSUShowPassword_Click;
            // 
            // txtContactNumber
            // 
            txtContactNumber.BackColor = Color.FromArgb(224, 224, 224);
            txtContactNumber.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtContactNumber.Location = new Point(93, 423);
            txtContactNumber.Name = "txtContactNumber";
            txtContactNumber.PlaceholderText = "Contact number";
            txtContactNumber.Size = new Size(628, 34);
            txtContactNumber.TabIndex = 19;
            // 
            // txtSUAddress
            // 
            txtSUAddress.BackColor = Color.FromArgb(224, 224, 224);
            txtSUAddress.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUAddress.Location = new Point(93, 360);
            txtSUAddress.Name = "txtSUAddress";
            txtSUAddress.PlaceholderText = "Address";
            txtSUAddress.Size = new Size(628, 34);
            txtSUAddress.TabIndex = 20;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(93, 142);
            label2.Name = "label2";
            label2.Size = new Size(109, 23);
            label2.TabIndex = 21;
            label2.Text = "Firstname:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(410, 142);
            label3.Name = "label3";
            label3.Size = new Size(110, 23);
            label3.TabIndex = 22;
            label3.Text = "Lastname:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(93, 207);
            label4.Name = "label4";
            label4.Size = new Size(68, 23);
            label4.TabIndex = 23;
            label4.Text = "Email:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.White;
            label5.Location = new Point(93, 272);
            label5.Name = "label5";
            label5.Size = new Size(59, 23);
            label5.TabIndex = 24;
            label5.Text = "Role:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.White;
            label6.Location = new Point(93, 334);
            label6.Name = "label6";
            label6.Size = new Size(94, 23);
            label6.TabIndex = 25;
            label6.Text = "Address:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.White;
            label7.Location = new Point(93, 397);
            label7.Name = "label7";
            label7.Size = new Size(176, 23);
            label7.TabIndex = 26;
            label7.Text = "Contact number:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.White;
            label8.Location = new Point(93, 460);
            label8.Name = "label8";
            label8.Size = new Size(114, 23);
            label8.TabIndex = 27;
            label8.Text = "Username:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.White;
            label9.Location = new Point(93, 523);
            label9.Name = "label9";
            label9.Size = new Size(106, 23);
            label9.TabIndex = 28;
            label9.Text = "Password:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Century Gothic", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.White;
            label10.Location = new Point(93, 586);
            label10.Name = "label10";
            label10.Size = new Size(191, 23);
            label10.TabIndex = 29;
            label10.Text = "Confirm password:";
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImage = Properties.Resources.bg1;
            ClientSize = new Size(810, 797);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtSUAddress);
            Controls.Add(txtContactNumber);
            Controls.Add(btnSUShowPassword);
            Controls.Add(txtSUConfirmPassword);
            Controls.Add(txtSUEmail);
            Controls.Add(txtSULastname);
            Controls.Add(txtSUFirstname);
            Controls.Add(btnBackToHome);
            Controls.Add(btnCreateAccount);
            Controls.Add(cmbSUUserLevel);
            Controls.Add(txtSUPassword);
            Controls.Add(txtSUUsername);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SignUp";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SignUp";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtSUUsername;
        private TextBox txtSUPassword;
        private ComboBox cmbSUUserLevel;
        private Button btnCreateAccount;
        private Button btnBackToHome;
        private TextBox txtSUFirstname;
        private TextBox txtSULastname;
        private TextBox txtSUEmail;
        private TextBox txtSUConfirmPassword;
        private Button btnSUShowPassword;
        private TextBox txtContactNumber;
        private TextBox txtSUAddress;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
    }
}