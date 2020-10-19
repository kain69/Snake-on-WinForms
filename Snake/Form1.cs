using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        // Snake step
        private const int snakeStep = 30;

        // Snake list of (x, y) positions
        private List<Point> snake = new List<Point>()
        {
            new Point(snakeStep, snakeStep)
        };

        // Size 
        private int fieldWidth;
        private int fieldHeight;

        // Snake movement direction
        private Point snakeDir = new Point(snakeStep, 0);
        private Point newSnakeDir = new Point(snakeStep, 0);

        // Food
        private Point food = new Point();

        // Random generator
        private Random rnd = new Random();        

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Centers the form on the current screen
            CenterToScreen();

            // Size 
            fieldWidth = this.Width;
            fieldHeight = this.Height;

            // Generate an initial random position for the food
            GenerateFood();         

            // Создание таймера для метода GameLoop
            var timer = new Timer();
            timer.Tick += GameLoop;
            timer.Interval = 200;
            timer.Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Draw(e);
        }

        private void GameLoop(object sender, System.EventArgs e)
        {
            Update();
            Invalidate();
        }

        private new void Update()
        {
            snakeDir = newSnakeDir;
            // Calc a new position of the head
            Point newHeadPosition = new Point(
                snake[0].X + snakeDir.X,
                snake[0].Y + snakeDir.Y
            );

            // Insert new position in the beginning of the snake list
            snake.Insert(0, newHeadPosition);

            if (!checkCollision(newHeadPosition))
            {
                Application.Exit();
                System.Diagnostics.Process.Start(Application.ExecutablePath);
            }

            // Remove the last element
            snake.RemoveAt(snake.Count - 1);

            // Check collision with the food
            if (snake[0].X == food.X &&
                snake[0].Y == food.Y)
            {
                // Add new element in the snake
                snake.Add(new Point(food.X, food.Y));

                // Generate a new food position
                GenerateFood();
            }
        }

        private void Draw(PaintEventArgs e)
        {
            DrawFood(e);
            DrawSnake(e);
        }

        private void DrawFood(PaintEventArgs e)
        {
            DrawRect(e,food.X, food.Y, Color.OrangeRed);
        }

        private void DrawSnake(PaintEventArgs e)
        {
            foreach (var cell in snake)
            {
                DrawRect(e,cell.X, cell.Y, Color.Green);
            }
        }

        private void DrawRect(PaintEventArgs e, int x, int y, Color color)
        {
            SolidBrush brush = new SolidBrush(color);
            Pen pen = new Pen(Color.Black);
            pen.Width = 1;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawRectangle(pen, new Rectangle(x, y, snakeStep, snakeStep));
            e.Graphics.FillRectangle(brush, new Rectangle(x, y, snakeStep, snakeStep));
            brush.Dispose();
            pen.Dispose();


        }

        private void GenerateFood()
        {
            bool ok = false;
            int checks = 0;
            while (!ok)
            {
                ok = true;
                bool inSneak = false;
                checks++;
                food.X = snakeStep * rnd.Next(0, fieldHeight / snakeStep - 1);
                food.Y = snakeStep * rnd.Next(0, fieldHeight / snakeStep - 1);
                foreach (Point cell in snake)
                {
                    if (food.X == cell.X && food.Y == cell.Y)
                    {
                        inSneak = true;
                        break;
                    }
                }
                if (inSneak)
                    ok = false;
            }
        }

        private bool checkCollision(Point head)
        {
            if (head.X < 0 || head.X >= this.Width || head.Y < 0 || head.Y >= this.Height)
                return false;
            for (int i = 1; i < snake.Count-1; i++)
            {
                if (head.X == snake[i].X && head.Y == snake[i].Y)
                    return false;
            }
            return true;

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    if(snakeDir.X != 0 && snakeDir.Y != snakeStep)
                    {
                        newSnakeDir.X = 0;
                        newSnakeDir.Y = -snakeStep;
                    }
                    break;
                case 'a':
                    if (snakeDir.X != snakeStep && snakeDir.Y != 0)
                    {
                        newSnakeDir.X = -snakeStep;
                        newSnakeDir.Y = 0;
                    }
                    
                    break;
                case 's':
                    if (snakeDir.X != 0 && snakeDir.Y != -snakeStep)
                    {
                        newSnakeDir.X = 0;
                        newSnakeDir.Y = snakeStep;
                    }
                    
                    break;
                case 'd':
                    if (snakeDir.X != -snakeStep && snakeDir.Y != 0)
                    {
                        newSnakeDir.X = snakeStep;
                        newSnakeDir.Y = 0;
                    }
                    break;
                case 'ц':
                    if (snakeDir.X != 0 && snakeDir.Y != snakeStep)
                    {
                        newSnakeDir.X = 0;
                        newSnakeDir.Y = -snakeStep;
                    }
                    break;
                case 'ф':
                    if (snakeDir.X != snakeStep && snakeDir.Y != 0)
                    {
                        newSnakeDir.X = -snakeStep;
                        newSnakeDir.Y = 0;
                    }

                    break;
                case 'ы':
                    if (snakeDir.X != 0 && snakeDir.Y != -snakeStep)
                    {
                        newSnakeDir.X = 0;
                        newSnakeDir.Y = snakeStep;
                    }

                    break;
                case 'в':
                    if (snakeDir.X != -snakeStep && snakeDir.Y != 0)
                    {
                        newSnakeDir.X = snakeStep;
                        newSnakeDir.Y = 0;
                    }
                    break;
                case (char)Keys.Escape:
                    Application.Exit();
                    break;
            }
        }
    }
}
