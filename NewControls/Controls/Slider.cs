using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NewControls.Controls
{
    [ToolboxItem(true)]
    public class Slider : UserControl
    {
        private bool _checked = false;
        private float _animPos = 0f;
        private System.Windows.Forms.Timer _timer;
        private const float AnimStep = 0.12f;

        private string _labelText = "";
        public string LabelText
        {
            get => _labelText;
            set { _labelText = value; Invalidate(); }
        }

        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return;
                _checked = value;
                _timer.Start();
                CheckedChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetValueImmediate(bool val)
        {
            _checked = val;
            _animPos = val ? 1f : 0f;
            _timer.Stop();
            Invalidate();
        }

        public event EventHandler CheckedChanged;

        private const int TrackW = 56;
        private const int TrackH = 26;
        private const int KnobPad = 3;
        private const int KnobD = TrackH - KnobPad * 2;

        public Slider()
        {
            DoubleBuffered = true;
            Cursor = Cursors.Hand;
            Size = new Size(TrackW + 200, TrackH + 4);

            _timer = new System.Windows.Forms.Timer { Interval = 16 };
            _timer.Tick += (s, e) =>
            {
                float target = _checked ? 1f : 0f;
                float diff = target - _animPos;
                if (Math.Abs(diff) < AnimStep) { _animPos = target; _timer.Stop(); }
                else _animPos += diff > 0 ? AnimStep : -AnimStep;
                Invalidate();
            };

            Click += (s, e) => Checked = !Checked;

            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int trackTop = (Height - TrackH) / 2;
            var trackRect = new Rectangle(0, trackTop, TrackW, TrackH);

            Color offC = Color.FromArgb(220, 60, 60);
            Color onC = Color.FromArgb(60, 180, 75);
            Color trackColor = InterpolateColor(offC, onC, _animPos);

            using (var path = RoundedRect(trackRect, TrackH / 2))
            {
                using (var brush = new SolidBrush(trackColor))
                {
                    g.FillPath(brush, path);
                }

                using (var pen = new Pen(Color.FromArgb(80, 0, 0, 0), 1.2f))
                {
                    g.DrawPath(pen, path);
                }
            }

            float knobLeft = KnobPad;
            float knobRight = TrackW - KnobPad - KnobD;
            float knobX = knobLeft + (knobRight - knobLeft) * _animPos;
            float knobY = trackTop + KnobPad;
            var knobRect = new RectangleF(knobX, knobY, KnobD, KnobD);

            using (var shadow = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
            {
                g.FillEllipse(shadow, knobRect.X + 1, knobRect.Y + 2, knobRect.Width, knobRect.Height);
            }
            using (var brush = new SolidBrush(Color.White))
            {
                g.FillEllipse(brush, knobRect);
            }
            using (var pen = new Pen(Color.FromArgb(160, 160, 160), 1f))
            {
                g.DrawEllipse(pen, knobRect);
            }

            if (!string.IsNullOrEmpty(_labelText))
            {
                int textX = TrackW + 10;
                var textRect = new Rectangle(textX, 0, Width - textX, Height);
                using (var sf = new StringFormat { LineAlignment = StringAlignment.Center })
                {
                    using (var br = new SolidBrush(ForeColor == Color.Empty ? Color.Black : ForeColor))
                    {
                        g.DrawString(_labelText, Font, br, textRect, sf);
                    }
                }
            }
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(r.Right - radius * 2, r.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(r.Right - radius * 2, r.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(r.X, r.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Color InterpolateColor(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _timer != null) _timer.Dispose();
            base.Dispose(disposing);
        }
    }
}
