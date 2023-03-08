using OrleansSnake.Client.Enums;
using OrleansSnake.Client.Renderers;
using OrleansSnake.Contracts;
using Microsoft.AspNetCore.SignalR.Client;
using Orientation = OrleansSnake.Contracts.Orientation;

namespace OrleansSnake.Client
{
    internal partial class MainForm : Form
    {
        private static string SignalRHost = "https://localhost:7243/ticker";
        private static Color ClearColor = Color.FromArgb(63, 50, 102);

        private readonly Random _random = new();
        private readonly GameClient _gameClient;
        private bool _closing;

        private SnakeGameState _snakeGameState = SnakeGameState.MainMenu;
        public SnakeGameState SnakeGameState
        {
            get => _snakeGameState;
            set
            {
                _snakeGameState = value;
                RefreshGameState();
            }
        }

        private GameState _gameState;
        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
                Invalidate();
            }
        }

        public MainForm(GameClient gameClient)
        {
            _gameClient = gameClient;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            InitializeComponent();
        }

        private HubConnection _connection;

        private async void MainForm_Load(object sender, EventArgs e)
        {
            SnakeGameState = SnakeGameState.MainMenu;

            _connection = new HubConnectionBuilder()
                .WithUrl(SignalRHost)
                .Build();

            _connection.Closed += async (error) =>
            {
                if (!_closing)
                {
                    await Task.Delay(_random.Next(0, 5) * 1000);
                    await _connection.StartAsync();
                }
            };

            _connection.On<GameState>("ReceiveGameState", gameState =>
            {
                Invoke(() =>
                {
                    GameState = gameState;

                    playersListBox.Items.Clear();
                    currentGameCodeLabel.Text = gameState.GameCode;

                    foreach (var player in gameState.Players)
                    {
                        playersListBox.Items.Add($"{player.PlayerName} ({(player.IsReady ? "Ready" : "Not Ready")})");
                    }

                    SnakeGameState = gameState.IsReady ? SnakeGameState.Game : SnakeGameState.GameLobby;
                });
            });

            try
            {
                while (_connection.State != HubConnectionState.Connected)
                {
                    await Task.Delay(1000);

                    try
                    {
                        await _connection.StartAsync();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Text = ex.Message;
            }
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_closing)
            {
                _closing = true;

                e.Cancel = true;

                await _connection.StopAsync();

                if (!string.IsNullOrWhiteSpace(currentGameCodeLabel.Text) &&
                    !string.IsNullOrWhiteSpace(playerNameTextBox.Text))
                {
                    await _gameClient.Abandon(currentGameCodeLabel.Text, playerNameTextBox.Text);
                }

                Close();
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            if (SnakeGameState == SnakeGameState.Game)
            {
                e.Graphics.RenderWorld(ClientRectangle, GameState);
                e.Graphics.RenderSnakes(ClientRectangle, GameState);
                e.Graphics.RenderFood(ClientRectangle, GameState);
            }
            else
            {
                e.Graphics.Clear(ClearColor);
            }
        }

        private async void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (_connection.State == HubConnectionState.Connected && SnakeGameState == SnakeGameState.Game)
            {
                var currentPlayerState = _gameState.Players.SingleOrDefault(x => x.PlayerName == playerNameTextBox.Text);
                var currentOrientation = currentPlayerState != null ? currentPlayerState.Snake.Orientation : Orientation.North;
                var newOrientation = currentOrientation;

                switch (e.KeyCode)
                {
                    case Keys.Z:
                    case Keys.Up:
                        if (currentOrientation != Orientation.South)
                        {
                            newOrientation = Orientation.North;
                        }
                        break;
                    case Keys.D:
                    case Keys.Right:
                        if (currentOrientation != Orientation.West)
                        {
                            newOrientation = Orientation.East;
                        }
                        break;
                    case Keys.S:
                    case Keys.Down:
                        if (currentOrientation != Orientation.North)
                        {
                            newOrientation = Orientation.South;
                        }
                        break;
                    case Keys.Q:
                    case Keys.Left:
                        if (currentOrientation != Orientation.East)
                        {
                            newOrientation = Orientation.West;
                        }
                        break;
                }

                if (newOrientation != currentOrientation)
                {
                    await _connection.SendAsync("Turn", currentGameCodeLabel.Text, playerNameTextBox.Text, newOrientation);
                }
            }
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            SnakeGameState = SnakeGameState.NewGame;
        }

        private void joinGameButton_Click(object sender, EventArgs e)
        {
            SnakeGameState = SnakeGameState.JoinGame;
        }

        private async void lobbyButton_Click(object sender, EventArgs e)
        {
            var gameCode = string.Empty;

            if (SnakeGameState == SnakeGameState.NewGame)
            {
                gameCode = await _gameClient.CreateGame(playerNameTextBox.Text);
            }

            if (SnakeGameState == SnakeGameState.JoinGame)
            {
                gameCode = await _gameClient.JoinGame(gameCodeTextBox.Text, playerNameTextBox.Text);
            }

            await _connection.SendAsync("JoinGame", gameCode);

            SnakeGameState = SnakeGameState.GameLobby;
        }

        private async void readyButton_Click(object sender, EventArgs e)
        {
            await _gameClient.PlayerReady(currentGameCodeLabel.Text, playerNameTextBox.Text);
            readyButton.Visible = false;
        }

        private void RefreshGameState()
        {
            mainMenuPanel.Visible = SnakeGameState == SnakeGameState.MainMenu;
            joinPanel.Visible = SnakeGameState == SnakeGameState.NewGame || SnakeGameState == SnakeGameState.JoinGame;
            gameCodeTextBox.Visible = gameCodeLabel.Visible = SnakeGameState == SnakeGameState.JoinGame;
            lobbyButton.Text = SnakeGameState == SnakeGameState.NewGame ? "CREATE GAME" : "JOIN GAME";
            lobbyPanel.Visible = SnakeGameState == SnakeGameState.GameLobby;
        }
    }
}