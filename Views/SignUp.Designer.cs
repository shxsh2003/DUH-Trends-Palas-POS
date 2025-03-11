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
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            cmbUserLevel = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            btnCreateAccount = new Button();
            btnBackToLogIn = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = SystemColors.ControlLightLight;
            label1.Location = new Point(265, 117);
            label1.Name = "label1";
            label1.Size = new Size(180, 51);
            label1.TabIndex = 2;
            label1.Text = "Sign Up";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(335, 291);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new Size(344, 34);
            txtUsername.TabIndex = 6;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(335, 352);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(344, 34);
            txtPassword.TabIndex = 7;
            // 
            // cmbUserLevel
            // 
            cmbUserLevel.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUserLevel.Font = new Font("Century Gothic", 13.2000008F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbUserLevel.FormattingEnabled = true;
            cmbUserLevel.Items.AddRange(new object[] { "Owner", "Admin", "Cashier" });
            cmbUserLevel.Location = new Point(335, 221);
            cmbUserLevel.Name = "cmbUserLevel";
            cmbUserLevel.Size = new Size(344, 33);
            cmbUserLevel.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Black;
            label2.Font = new Font("Century Gothic", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.White;
            label2.Location = new Point(86, 221);
            label2.Name = "label2";
            label2.Size = new Size(206, 27);
            label2.TabIndex = 9;
            label2.Text = "Employee Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Black;
            label3.Font = new Font("Century Gothic", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.White;
            label3.Location = new Point(86, 291);
            label3.Name = "label3";
            label3.Size = new Size(131, 27);
            label3.TabIndex = 10;
            label3.Text = "Username:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Black;
            label4.Font = new Font("Century Gothic", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.White;
            label4.Location = new Point(86, 350);
            label4.Name = "label4";
            label4.Size = new Size(121, 27);
            label4.TabIndex = 11;
            label4.Text = "Password:";
            // 
            // btnCreateAccount
            // 
            btnCreateAccount.BackColor = SystemColors.GradientActiveCaption;
            btnCreateAccount.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCreateAccount.Location = new Point(244, 427);
            btnCreateAccount.Name = "btnCreateAccount";
            btnCreateAccount.Size = new Size(233, 39);
            btnCreateAccount.TabIndex = 12;
            btnCreateAccount.Text = "Create Account";
            btnCreateAccount.UseVisualStyleBackColor = false;
            // 
            // btnBackToLogIn
            // 
            btnBackToLogIn.BackColor = Color.Crimson;
            btnBackToLogIn.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBackToLogIn.Location = new Point(306, 477);
            btnBackToLogIn.Name = "btnBackToLogIn";
            btnBackToLogIn.Size = new Size(108, 39);
            btnBackToLogIn.TabIndex = 13;
            btnBackToLogIn.Text = "Back";
            btnBackToLogIn.UseVisualStyleBackColor = false;
            btnBackToLogIn.Click += btnBackToLogIn_Click;
            // 
            // SignUp
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(745, 589);
            Controls.Add(btnBackToLogIn);
            Controls.Add(btnCreateAccount);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cmbUserLevel);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
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
        private TextBox txtUsername;
        private TextBox txtPassword;
        private ComboBox cmbUserLevel;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button btnCreateAccount;
        private Button btnBackToLogIn;
    }
}