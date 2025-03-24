using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace QualityTime
{
    public partial class Form1 : Form
    {
        private bool _isMoving = false;
        private Thread _movementThread;
        private Random _random = new Random();

        public Form1()
        {
            InitializeComponent();
            this.Text = "QualityTime"; // Set the title bar text
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (_isMoving) return;

            _isMoving = true;
            StartButton.Enabled = false;
            StopButton.Enabled = true;

            Console.WriteLine("QualityTime: Mouse movement started."); // Debug output

            _movementThread = new Thread(MoveMouse);
            _movementThread.IsBackground = true;
            _movementThread.Start();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _isMoving = false;
            StartButton.Enabled = true;
            StopButton.Enabled = false;

            Console.WriteLine("QualityTime: Mouse movement stopped."); // Debug output

            if (_movementThread != null && _movementThread.IsAlive)
            {
                _movementThread.Join(2000);
                if (_movementThread.IsAlive)
                {
                    Console.WriteLine("QualityTime: Movement thread did not stop gracefully.");
                }
                _movementThread = null;
            }
        }

        private void MoveMouse()
        {
            while (_isMoving)
            {
                Point currentPosition = Cursor.Position;
                int xOffset = _random.Next(-10, 11); // Increased range
                int yOffset = _random.Next(-10, 11); // Increased range
                Point newPosition = new Point(currentPosition.X + xOffset, currentPosition.Y + yOffset);

                try
                {
                    this.Invoke((MethodInvoker)delegate { Cursor.Position = newPosition; });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error setting Cursor.Position: {ex.Message}");
                }

                Console.WriteLine($"QualityTime: Mouse moved to X:{newPosition.X}, Y:{newPosition.Y}"); // Debug output
                Thread.Sleep(60 * 1000);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopButton_Click(sender, e);
        }
    }
}