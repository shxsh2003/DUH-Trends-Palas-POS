﻿namespace DUH_Trends_Palas_POS.Views
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
            btnBackToLogIn = new Button();
            txtSUFirstname = new TextBox();
            txtSULastname = new TextBox();
            txtSUEmail = new TextBox();
            txtSUConfirmPassword = new TextBox();
            btnSUShowPassword = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Century Gothic", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(252, 62);
            label1.Name = "label1";
            label1.Size = new Size(180, 51);
            label1.TabIndex = 2;
            label1.Text = "Sign Up";
            // 
            // txtSUUsername
            // 
            txtSUUsername.BackColor = Color.FromArgb(224, 224, 224);
            txtSUUsername.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUUsername.Location = new Point(28, 310);
            txtSUUsername.Name = "txtSUUsername";
            txtSUUsername.PlaceholderText = "Username";
            txtSUUsername.Size = new Size(628, 34);
            txtSUUsername.TabIndex = 6;
            // 
            // txtSUPassword
            // 
            txtSUPassword.BackColor = Color.FromArgb(224, 224, 224);
            txtSUPassword.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUPassword.Location = new Point(28, 366);
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
            cmbSUUserLevel.Location = new Point(28, 257);
            cmbSUUserLevel.Name = "cmbSUUserLevel";
            cmbSUUserLevel.Size = new Size(628, 33);
            cmbSUUserLevel.TabIndex = 8;
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.BackColor = SystemColors.GradientActiveCaption;
            btnCreateAccount.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCreateAccount.Location = new Point(235, 487);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(233, 39);
            btnCreateAccount.TabIndex = 12;
            btnCreateAccount.Text = "Create Account";
            btnCreateAccount.UseVisualStyleBackColor = false;
            // 
            // btnBackToLogIn
            // 
            btnBackToLogIn.BackColor = Color.OrangeRed;
            btnBackToLogIn.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBackToLogIn.Location = new Point(297, 537);
            btnBackToLogIn.Name = "btnBackToLogIn";
            btnBackToLogIn.Size = new Size(108, 39);
            btnBackToLogIn.TabIndex = 13;
            btnBackToLogIn.Text = "Back";
            btnBackToLogIn.UseVisualStyleBackColor = false;
            btnBackToLogIn.Click += btnBackToLogIn_Click;
            // 
            // txtSUFirstname
            // 
            txtSUFirstname.BackColor = Color.FromArgb(224, 224, 224);
            txtSUFirstname.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUFirstname.Location = new Point(28, 151);
            txtSUFirstname.Name = "txtSUFirstname";
            txtSUFirstname.PlaceholderText = "Firstname";
            txtSUFirstname.Size = new Size(311, 34);
            txtSUFirstname.TabIndex = 14;
            // 
            // txtSULastname
            // 
            txtSULastname.BackColor = Color.FromArgb(224, 224, 224);
            txtSULastname.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSULastname.Location = new Point(345, 151);
            txtSULastname.Name = "txtSULastname";
            txtSULastname.PlaceholderText = "Lastname";
            txtSULastname.Size = new Size(311, 34);
            txtSULastname.TabIndex = 15;
            // 
            // txtSUEmail
            // 
            txtSUEmail.BackColor = Color.FromArgb(224, 224, 224);
            txtSUEmail.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUEmail.Location = new Point(28, 205);
            txtSUEmail.Name = "txtSUEmail";
            txtSUEmail.PlaceholderText = "Email";
            txtSUEmail.Size = new Size(628, 34);
            txtSUEmail.TabIndex = 16;
            // 
            // txtSUConfirmPassword
            // 
            txtSUConfirmPassword.BackColor = Color.FromArgb(224, 224, 224);
            txtSUConfirmPassword.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSUConfirmPassword.Location = new Point(28, 419);
            txtSUConfirmPassword.Name = "txtSUConfirmPassword";
            txtSUConfirmPassword.PlaceholderText = "Confirm Password";
            txtSUConfirmPassword.Size = new Size(550, 34);
            txtSUConfirmPassword.TabIndex = 17;
            // 
            // btnSUShowPassword
            // 
            btnSUShowPassword.BackColor = Color.LightSteelBlue;
            btnSUShowPassword.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSUShowPassword.Location = new Point(592, 391);
            btnSUShowPassword.Name = "btnSUShowPassword";
            btnSUShowPassword.Size = new Size(64, 31);
            btnSUShowPassword.TabIndex = 18;
            btnSUShowPassword.Text = "Show";
            btnSUShowPassword.UseVisualStyleBackColor = false;
            btnSUShowPassword.Click += btnSUShowPassword_Click;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImage = Properties.Resources.bg1;
            ClientSize = new Size(685, 622);
            Controls.Add(btnSUShowPassword);
            Controls.Add(txtSUConfirmPassword);
            Controls.Add(txtSUEmail);
            Controls.Add(txtSULastname);
            Controls.Add(txtSUFirstname);
            Controls.Add(btnBackToLogIn);
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
        private Button btnBackToLogIn;
        private TextBox txtSUFirstname;
        private TextBox txtSULastname;
        private TextBox txtSUEmail;
        private TextBox txtSUConfirmPassword;
        private Button btnSUShowPassword;
    }
}