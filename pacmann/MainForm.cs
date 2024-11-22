using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace PacManGame
{
    public partial class MainForm : Form
    {
        private Timer gameTimer;
        private PictureBox pacman;
        private PictureBox[] walls;
        private PictureBox[] coins;
        private PictureBox[] ghosts;
        private SoundPlayer soundPlayer;

        private int pacmanSpeed = 8;
        private int ghostSpeed = 4;
        private int score = 0;
        private int lives = 3;
        private int level = 1;
        private bool goUp, goDown, goLeft, goRight;

        private string resourcePath;
        private Label scoreLabel;
        private Label levelLabel;

        public MainForm()
        {
            InitializeComponent();
            resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            InitializeGame();
            PlayMusic();
        }

        private void InitializeGame()
        {
            this.Text = "Pac-Man";
            this.ClientSize = new Size(800, 600);
            this.BackColor = Color.Black;

            scoreLabel = new Label
            {
                Text = $"Рахунок: {score}",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(scoreLabel);

            levelLabel = new Label
            {
                Text = $"Рівень: {level}",
                ForeColor = Color.White,
                BackColor = Color.Black,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(700, 10),
                AutoSize = true
            };
            this.Controls.Add(levelLabel);

            pacman = new PictureBox
            {
                Size = new Size(30, 30),
                Location = new Point(50, 50),
                BackColor = Color.Transparent,
                Image = LoadImage("pacman.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
            this.Controls.Add(pacman);

            walls = CreateClassicMazeWalls();
            foreach (var wall in walls)
                this.Controls.Add(wall);

            coins = CreateCoins();
            foreach (var coin in coins)
                this.Controls.Add(coin);

            ghosts = CreateGhosts();
            foreach (var ghost in ghosts)
                this.Controls.Add(ghost);

            gameTimer = new Timer { Interval = 20 };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            this.KeyDown += MainForm_KeyDown;
            this.KeyUp += MainForm_KeyUp;
        }

        private void ResetGame()
        {
            score = 0;
            lives = 3;
            level = 1;
            scoreLabel.Text = $"Рахунок: {score}";
            levelLabel.Text = $"Рівень: {level}";


            this.Controls.Clear();
            InitializeGame();
        }

        private void NextLevel()
        {
            level++;
            levelLabel.Text = $"Рівень: {level}";
            ResetPositions();
        }

        private void ResetPositions()
        {
            pacman.Location = new Point(50, 50);
            foreach (var ghost in ghosts)
            {
                ghost.Location = new Point(300, 150);
            }
        }

        private PictureBox[] CreateClassicMazeWalls()
        {
            return new PictureBox[]
            {

        CreateWall(0, 0, 800, 20),
                CreateWall(0, 0, 20, 600),
                CreateWall(780, 0, 20, 600),
                CreateWall(0, 580, 800, 20),

                CreateWall(560, 40, 160, 20),
                CreateWall(320, 40, 160, 20),
                CreateWall(80, 80, 20, 100),
                CreateWall(700, 80, 20, 100),
                CreateWall(200, 80, 160, 20),
                CreateWall(440, 80, 160, 20),
                CreateWall(320, 100, 160, 20),

                CreateWall(120, 200, 80, 20),
                CreateWall(600, 200, 80, 20),
                CreateWall(320, 200, 160, 20),
                CreateWall(400, 240, 20, 60),

                CreateWall(200, 260, 160, 20),
                CreateWall(440, 260, 160, 20),
                CreateWall(320, 300, 160, 20),
                CreateWall(80, 300, 20, 100),
                CreateWall(700, 300, 20, 100),
                CreateWall(400, 320, 20, 80),

                CreateWall(320, 400, 160, 20),
                CreateWall(80, 480, 160, 20),
                CreateWall(560, 480, 160, 20),
                CreateWall(200, 400, 20, 80),
                CreateWall(580, 400, 20, 80),

                CreateWall(120, 520, 80, 20),
                CreateWall(600, 520, 80, 20),
                CreateWall(320, 480, 160, 20),
                CreateWall(400, 520, 20, 40),

                CreateWall(120, 360, 40, 20),
                CreateWall(640, 360, 40, 20),
                CreateWall(200, 360, 20, 80),
                CreateWall(580, 360, 20, 80),


                CreateWall(80, 160, 20, 60),
                CreateWall(700, 160, 20, 60),
                CreateWall(320, 360, 160, 20),


                CreateWall(320, 260, 20, 60),
                CreateWall(460, 260, 20, 60),
                CreateWall(340, 300, 120, 20),
            };
        }




        private PictureBox[] CreateCoins()
        {

            var coinList = new PictureBox[50];
            Random rnd = new Random();
            for (int i = 0; i < coinList.Length; i++)
            {
                int x = rnd.Next(30, 770);
                int y = rnd.Next(30, 570);
                coinList[i] = CreateCoin(x, y);
            }
            return coinList;
        }

        private PictureBox[] CreateGhosts()
        {
            return new PictureBox[]
            {
                CreateGhost(300, 150, "blinky.png"),
                CreateGhost(500, 400, "pinky.png"),
                CreateGhost(200, 300, "inky.png"),
                CreateGhost(600, 300, "clyde.png")
            };
        }

        private PictureBox CreateWall(int x, int y, int width, int height)
        {
            return new PictureBox
            {
                BackColor = Color.Blue,
                Location = new Point(x, y),
                Width = width,
                Height = height
            };
        }

        private PictureBox CreateCoin(int x, int y)
        {
            return new PictureBox
            {
                BackColor = Color.Transparent,
                Location = new Point(x, y),
                Size = new Size(15, 15),
                Image = LoadImage("coin.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
        }

        private PictureBox CreateGhost(int x, int y, string imageFile)
        {
            return new PictureBox
            {
                BackColor = Color.Transparent,
                Location = new Point(x, y),
                Width = 30,
                Height = 30,
                Image = LoadImage(imageFile),
                SizeMode = PictureBoxSizeMode.StretchImage
            };
        }

        private void PlayMusic()
        {
            try
            {
                soundPlayer = new SoundPlayer(Path.Combine(resourcePath, "pacman_theme.wav"));
                soundPlayer.PlayLooping();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка загрузки музыки: {ex.Message}", "Помилка");
            }
        }

        private Image LoadImage(string fileName)
        {
            try
            {
                string filePath = Path.Combine(resourcePath, fileName);
                if (File.Exists(filePath))
                {
                    return Image.FromFile(filePath);
                }
                else
                {
                    MessageBox.Show($"Зображення '{fileName}' не знайдено: {filePath}", "Помилка");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка загрузки зображення: {ex.Message}", "Помилка");
                return null;
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            MovePacman();
            MoveGhosts();
            CheckCollisions();
        }

        private void MovePacman()
        {
            var newLocation = pacman.Location;

            if (goUp) newLocation.Y -= pacmanSpeed;
            if (goDown) newLocation.Y += pacmanSpeed;
            if (goLeft) newLocation.X -= pacmanSpeed;
            if (goRight) newLocation.X += pacmanSpeed;

            if (!IsCollision(newLocation, pacman.Size))
            {
                pacman.Location = newLocation;
            }
        }

        private void MoveGhosts()
        {
            foreach (var ghost in ghosts)
            {
                var direction = pacman.Location - new Size(ghost.Location);
                var normalized = new Point(
                    direction.X > 0 ? 1 : -1,
                    direction.Y > 0 ? 1 : -1
                );

                var newLocation = ghost.Location;
                newLocation.X += normalized.X * ghostSpeed;
                newLocation.Y += normalized.Y * ghostSpeed;

                if (!IsCollision(newLocation, ghost.Size))
                {
                    ghost.Location = newLocation;
                }
            }
        }

        private bool IsCollision(Point location, Size size)
        {
            var rect = new Rectangle(location, size);
            foreach (var wall in walls)
            {
                if (rect.IntersectsWith(wall.Bounds))
                {
                    return true;
                }
            }
            return false;
        }

        private void CheckCollisions()
        {
            foreach (var coin in coins)
            {
                if (pacman.Bounds.IntersectsWith(coin.Bounds) && coin.Visible)
                {
                    coin.Visible = false;
                    score++;
                    scoreLabel.Text = $"Рахунок: {score}";
                }
            }

            foreach (var ghost in ghosts)
            {
                if (pacman.Bounds.IntersectsWith(ghost.Bounds))
                {
                    lives--;
                    if (lives <= 0)
                    {
                        GameOver("Ви програли!");
                    }
                    else
                    {
                        MessageBox.Show("Ви втратили життя!", "Увага!");
                        ResetPositions();
                    }
                }
            }

            if (AllCoinsCollected())
            {
                NextLevel();
            }
        }

        private bool AllCoinsCollected()
        {
            foreach (var coin in coins)
            {
                if (coin.Visible)
                {
                    return false;
                }
            }
            return true;
        }

        private void GameOver(string message)
        {
            gameTimer.Stop();
            soundPlayer?.Stop();
            MessageBox.Show(message + $"\nВаш рахунок: {score}", "Гра закінчена");
            Application.Exit();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) goUp = true;
            if (e.KeyCode == Keys.Down) goDown = true;
            if (e.KeyCode == Keys.Left) goLeft = true;
            if (e.KeyCode == Keys.Right) goRight = true;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) goUp = false;
            if (e.KeyCode == Keys.Down) goDown = false;
            if (e.KeyCode == Keys.Left) goLeft = false;
            if (e.KeyCode == Keys.Right) goRight = false;
        }
    }
}