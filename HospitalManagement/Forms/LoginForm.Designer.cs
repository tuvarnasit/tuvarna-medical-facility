namespace HospitalManagement.Forms
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            emailTextBox = new TextBox();
            errorLabel = new Label();
            label2 = new Label();
            loginButton = new Button();
            passwordTextBox = new TextBox();
            label3 = new Label();
            label1 = new Label();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(156, 136, 255);
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1.39417F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 98.60583F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.Controls.Add(panel1, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 85.15625F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 14.84375F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 173F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.Controls.Add(emailTextBox);
            panel1.Controls.Add(errorLabel);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(loginButton);
            panel1.Controls.Add(passwordTextBox);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(14, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(772, 444);
            panel1.TabIndex = 0;
            // 
            // emailTextBox
            // 
            emailTextBox.Anchor = AnchorStyles.None;
            emailTextBox.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            emailTextBox.Location = new Point(264, 202);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new Size(234, 27);
            emailTextBox.TabIndex = 6;
            emailTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // errorLabel
            // 
            errorLabel.Anchor = AnchorStyles.None;
            errorLabel.AutoSize = true;
            errorLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            errorLabel.ForeColor = Color.DarkRed;
            errorLabel.Location = new Point(279, 144);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(199, 21);
            errorLabel.TabIndex = 12;
            errorLabel.Text = "Грешен имейл или парола";
            errorLabel.Visible = false;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(245, 246, 250);
            label2.Location = new Point(264, 230);
            label2.Name = "label2";
            label2.Size = new Size(78, 25);
            label2.TabIndex = 10;
            label2.Text = "Парола";
            // 
            // loginButton
            // 
            loginButton.Anchor = AnchorStyles.None;
            loginButton.BackColor = Color.FromArgb(25, 42, 86);
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.ForeColor = Color.FromArgb(245, 246, 250);
            loginButton.Location = new Point(326, 309);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(103, 30);
            loginButton.TabIndex = 11;
            loginButton.Text = "Вход";
            loginButton.UseVisualStyleBackColor = false;
            loginButton.Click += loginButton_Click;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Anchor = AnchorStyles.None;
            passwordTextBox.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            passwordTextBox.Location = new Point(264, 258);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PasswordChar = '*';
            passwordTextBox.Size = new Size(234, 27);
            passwordTextBox.TabIndex = 8;
            passwordTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 26.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(245, 246, 250);
            label3.Location = new Point(333, 74);
            label3.Name = "label3";
            label3.Size = new Size(96, 47);
            label3.TabIndex = 9;
            label3.Text = "Вход";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(245, 246, 250);
            label1.Location = new Point(264, 174);
            label1.Name = "label1";
            label1.Size = new Size(70, 25);
            label1.TabIndex = 7;
            label1.Text = "Имейл";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Вход";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel1;
        private TextBox emailTextBox;
        private Label errorLabel;
        private Label label2;
        private Button loginButton;
        private TextBox passwordTextBox;
        private Label label3;
        private Label label1;
    }
}