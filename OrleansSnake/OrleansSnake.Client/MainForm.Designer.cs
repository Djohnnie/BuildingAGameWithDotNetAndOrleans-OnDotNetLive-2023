namespace OrleansSnake.Client
{
    partial class MainForm
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
            mainMenuPanel = new Panel();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label1 = new Label();
            joinGameButton = new Button();
            newGameButton = new Button();
            statusLabel = new Label();
            lobbyPanel = new Panel();
            currentGameCodeLabel = new Label();
            readyButton = new Button();
            playersListBox = new ListBox();
            joinPanel = new Panel();
            gameCodeLabel = new Label();
            playerNameLabel = new Label();
            gameCodeTextBox = new TextBox();
            playerNameTextBox = new TextBox();
            lobbyButton = new Button();
            mainMenuPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            lobbyPanel.SuspendLayout();
            joinPanel.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenuPanel
            // 
            mainMenuPanel.Anchor = AnchorStyles.None;
            mainMenuPanel.BackColor = Color.White;
            mainMenuPanel.Controls.Add(pictureBox1);
            mainMenuPanel.Controls.Add(label2);
            mainMenuPanel.Controls.Add(label1);
            mainMenuPanel.Controls.Add(joinGameButton);
            mainMenuPanel.Controls.Add(newGameButton);
            mainMenuPanel.Location = new Point(249, 305);
            mainMenuPanel.Margin = new Padding(4, 2, 4, 2);
            mainMenuPanel.Name = "mainMenuPanel";
            mainMenuPanel.Size = new Size(1151, 435);
            mainMenuPanel.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.snake;
            pictureBox1.Location = new Point(383, 15);
            pictureBox1.Margin = new Padding(4, 2, 4, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(370, 305);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe Print", 28.125F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(805, 81);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(280, 132);
            label2.TabIndex = 4;
            label2.Text = "Snake";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe Print", 28.125F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(58, 81);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(339, 132);
            label1.TabIndex = 3;
            label1.Text = "Orleans";
            // 
            // joinGameButton
            // 
            joinGameButton.BackColor = Color.FromArgb(63, 50, 102);
            joinGameButton.ForeColor = Color.White;
            joinGameButton.Location = new Point(605, 337);
            joinGameButton.Margin = new Padding(4, 2, 4, 2);
            joinGameButton.Name = "joinGameButton";
            joinGameButton.Size = new Size(409, 79);
            joinGameButton.TabIndex = 1;
            joinGameButton.Text = "JOIN EXISTING GAME";
            joinGameButton.UseVisualStyleBackColor = false;
            joinGameButton.Click += joinGameButton_Click;
            // 
            // newGameButton
            // 
            newGameButton.BackColor = Color.FromArgb(63, 50, 102);
            newGameButton.ForeColor = Color.White;
            newGameButton.Location = new Point(124, 337);
            newGameButton.Margin = new Padding(4, 2, 4, 2);
            newGameButton.Name = "newGameButton";
            newGameButton.Size = new Size(409, 79);
            newGameButton.TabIndex = 0;
            newGameButton.Text = "START NEW GAME";
            newGameButton.UseVisualStyleBackColor = false;
            newGameButton.Click += newGameButton_Click;
            // 
            // statusLabel
            // 
            statusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            statusLabel.ForeColor = Color.White;
            statusLabel.Location = new Point(11, 1090);
            statusLabel.Margin = new Padding(4, 0, 4, 0);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(1623, 41);
            statusLabel.TabIndex = 1;
            statusLabel.Text = "Connecting to the game server...";
            statusLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lobbyPanel
            // 
            lobbyPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lobbyPanel.BackColor = Color.White;
            lobbyPanel.Controls.Add(currentGameCodeLabel);
            lobbyPanel.Controls.Add(readyButton);
            lobbyPanel.Controls.Add(playersListBox);
            lobbyPanel.Location = new Point(50, 53);
            lobbyPanel.Margin = new Padding(6);
            lobbyPanel.Name = "lobbyPanel";
            lobbyPanel.Size = new Size(1543, 954);
            lobbyPanel.TabIndex = 2;
            // 
            // currentGameCodeLabel
            // 
            currentGameCodeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            currentGameCodeLabel.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            currentGameCodeLabel.Location = new Point(6, 0);
            currentGameCodeLabel.Margin = new Padding(6, 0, 6, 0);
            currentGameCodeLabel.Name = "currentGameCodeLabel";
            currentGameCodeLabel.Size = new Size(1532, 194);
            currentGameCodeLabel.TabIndex = 3;
            currentGameCodeLabel.Text = "#";
            currentGameCodeLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // readyButton
            // 
            readyButton.Anchor = AnchorStyles.Bottom;
            readyButton.BackColor = Color.FromArgb(63, 50, 102);
            readyButton.ForeColor = Color.White;
            readyButton.Location = new Point(613, 858);
            readyButton.Margin = new Padding(4, 2, 4, 2);
            readyButton.Name = "readyButton";
            readyButton.Size = new Size(409, 79);
            readyButton.TabIndex = 2;
            readyButton.Text = "I'M READY";
            readyButton.UseVisualStyleBackColor = false;
            readyButton.Click += readyButton_Click;
            // 
            // playersListBox
            // 
            playersListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            playersListBox.BorderStyle = BorderStyle.None;
            playersListBox.Font = new Font("Segoe Print", 36F, FontStyle.Bold, GraphicsUnit.Point);
            playersListBox.FormattingEnabled = true;
            playersListBox.IntegralHeight = false;
            playersListBox.ItemHeight = 170;
            playersListBox.Location = new Point(78, 201);
            playersListBox.Margin = new Padding(6);
            playersListBox.Name = "playersListBox";
            playersListBox.SelectionMode = SelectionMode.None;
            playersListBox.Size = new Size(1382, 640);
            playersListBox.TabIndex = 1;
            // 
            // joinPanel
            // 
            joinPanel.Anchor = AnchorStyles.None;
            joinPanel.BackColor = Color.Gainsboro;
            joinPanel.Controls.Add(gameCodeLabel);
            joinPanel.Controls.Add(playerNameLabel);
            joinPanel.Controls.Add(gameCodeTextBox);
            joinPanel.Controls.Add(playerNameTextBox);
            joinPanel.Controls.Add(lobbyButton);
            joinPanel.Location = new Point(422, 284);
            joinPanel.Margin = new Padding(4, 2, 4, 2);
            joinPanel.Name = "joinPanel";
            joinPanel.Size = new Size(787, 435);
            joinPanel.TabIndex = 3;
            // 
            // gameCodeLabel
            // 
            gameCodeLabel.AutoSize = true;
            gameCodeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            gameCodeLabel.Location = new Point(308, 13);
            gameCodeLabel.Margin = new Padding(6, 0, 6, 0);
            gameCodeLabel.Name = "gameCodeLabel";
            gameCodeLabel.Size = new Size(187, 45);
            gameCodeLabel.TabIndex = 4;
            gameCodeLabel.Text = "Game Code";
            // 
            // playerNameLabel
            // 
            playerNameLabel.AutoSize = true;
            playerNameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            playerNameLabel.Location = new Point(303, 171);
            playerNameLabel.Margin = new Padding(6, 0, 6, 0);
            playerNameLabel.Name = "playerNameLabel";
            playerNameLabel.Size = new Size(199, 45);
            playerNameLabel.TabIndex = 3;
            playerNameLabel.Text = "Player Name";
            // 
            // gameCodeTextBox
            // 
            gameCodeTextBox.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            gameCodeTextBox.Location = new Point(6, 64);
            gameCodeTextBox.Margin = new Padding(6);
            gameCodeTextBox.Name = "gameCodeTextBox";
            gameCodeTextBox.Size = new Size(773, 86);
            gameCodeTextBox.TabIndex = 2;
            gameCodeTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // playerNameTextBox
            // 
            playerNameTextBox.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            playerNameTextBox.Location = new Point(6, 222);
            playerNameTextBox.Margin = new Padding(6);
            playerNameTextBox.Name = "playerNameTextBox";
            playerNameTextBox.Size = new Size(773, 86);
            playerNameTextBox.TabIndex = 1;
            playerNameTextBox.TextAlign = HorizontalAlignment.Center;
            // 
            // lobbyButton
            // 
            lobbyButton.BackColor = Color.FromArgb(63, 50, 102);
            lobbyButton.ForeColor = Color.White;
            lobbyButton.Location = new Point(191, 331);
            lobbyButton.Margin = new Padding(4, 2, 4, 2);
            lobbyButton.Name = "lobbyButton";
            lobbyButton.Size = new Size(409, 79);
            lobbyButton.TabIndex = 0;
            lobbyButton.Text = "OPEN GAME LOBBY";
            lobbyButton.UseVisualStyleBackColor = false;
            lobbyButton.Click += lobbyButton_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(63, 50, 102);
            ClientSize = new Size(1647, 1060);
            Controls.Add(joinPanel);
            Controls.Add(statusLabel);
            Controls.Add(mainMenuPanel);
            Controls.Add(lobbyPanel);
            DoubleBuffered = true;
            KeyPreview = true;
            Margin = new Padding(4, 2, 4, 2);
            Name = "MainForm";
            Text = "OrleansSnake";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            Paint += MainForm_Paint;
            KeyDown += MainForm_KeyDown;
            mainMenuPanel.ResumeLayout(false);
            mainMenuPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            lobbyPanel.ResumeLayout(false);
            joinPanel.ResumeLayout(false);
            joinPanel.PerformLayout();
            ResumeLayout(false);
        }

        private Panel mainMenuPanel;
        private Button joinGameButton;
        private Button newGameButton;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label statusLabel;
        private Panel lobbyPanel;
        private ListBox playersListBox;
        private Button readyButton;
        private Panel joinPanel;
        private Button lobbyButton;
        private TextBox playerNameTextBox;
        private TextBox gameCodeTextBox;
        private Label gameCodeLabel;
        private Label playerNameLabel;
        private Label currentGameCodeLabel;

        #endregion
    }
}