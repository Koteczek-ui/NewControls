using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace NewControls.Controls
{
    public class Spinner : Control
    {
        private System.Windows.Forms.Timer timer;
        private float angle = 90;
        private float sweep = 30;
        private float currentSpeed = 5;
        private bool expanding = true;

        [Category("Behavior")]
        public bool Expandable { get; set; } = true;

        [Category("Behavior")]
        public float Speed { get; set; } = 1.0f;

        private float DefaultSpeed => 5.0f;

        public Spinner()
        {
            DoubleBuffered = true;
            Size = new Size(50, 50);
            timer = new System.Windows.Forms.Timer
            {
                Interval = 20
            };
            timer.Tick += UpdateAnimation;
            timer.Start();
        }

        private void UpdateAnimation(object sender, EventArgs e)
        {
            float baseSpeed = DefaultSpeed * Speed;

            if (Expandable)
            {
                if (expanding)
                {
                    sweep += baseSpeed;
                    currentSpeed = Math.Max(1, baseSpeed * (1 - (sweep / 270)));
                    if (sweep >= 270) expanding = false;
                }
                else
                {
                    sweep -= (baseSpeed * 0.4f);
                    currentSpeed = baseSpeed;
                    if (sweep <= 30) expanding = true;
                }
                angle += currentSpeed;
            }
            else
            {
                sweep = 120;
                angle += baseSpeed;
            }
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(5, 5, this.Width - 10, this.Height - 10);
            using (Pen pen = new Pen(Color.DodgerBlue, 4))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                e.Graphics.DrawArc(pen, rect, angle, sweep);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer.Stop();
                timer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
