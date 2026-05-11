using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NewControls
{
    [ToolboxItem(true)]
    public partial class PBar : UserControl
    {
        private double _value = 50.0;
        private double _animValue = 0;
        private double _min = 0.0;
        private double _max = 100.0;
        private double _interval = 15.0;
        private bool _isMarquee = false;
        private bool _hasMovingAnimation = true;
        private int _marqueeOffset = -100;
        private System.Windows.Forms.Timer _animTimer;

        public PBar()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ForeColor = GetDefaultColor();

            _animTimer = new System.Windows.Forms.Timer();
            UpdateInterval();
            _animTimer.Tick += (s, e) =>
            {
                if (_isMarquee)
                {
                    _marqueeOffset += 5;
                    if (_marqueeOffset > this.Width) _marqueeOffset = -100;
                    this.Invalidate();
                }
                else if (_hasMovingAnimation)
                {
                    double diff = _value - _animValue;
                    if (Math.Abs(diff) < 0.1)
                    {
                        _animValue = _value;
                        if (!_isMarquee) _animTimer.Stop();
                    }
                    else _animValue += diff * 0.1;
                    this.Invalidate();
                }
            };

            UseWaitCursor = true;
            Cursor = Cursors.WaitCursor;

            Value = 50.0;
            Size = new Size(Size.Width, 15);
        }

        private void UpdateInterval() { _animTimer.Interval = (int)_interval; Invalidate(); }

        public static Color GetDefaultColor() { return GetWindowsAccentColor();  }

        private static Color GetWindowsAccentColor()
        {
            try
            {
                var dwmColor = Color.Empty;
                uint colorValue = 0;
                bool opaque = false;

                DwmGetColorizationColor(out colorValue, out opaque);

                return Color.FromArgb(
                    (int)((colorValue >> 16) & 0xFF),
                    (int)((colorValue >> 8) & 0xFF),
                    (int)(colorValue & 0xFF)
                );
            }
            catch
            {
                return SystemColors.Highlight;
            }
        }

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmGetColorizationColor(out uint ColorizationColor, [MarshalAs(UnmanagedType.Bool)] out bool ColorizationOpaqueBlend);

        [Category("Animation")]
        [DefaultValue(true)]
        public bool HasMovingAnimation
        {
            get => _hasMovingAnimation;
            set
            {
                _hasMovingAnimation = value;
                if (!_hasMovingAnimation) _animValue = _value;
                this.Invalidate();
            }
        }

        [DefaultValue(false)]
        [Category("Animation")]
        public bool IsMarquee
        {
            get => _isMarquee;
            set
            {
                _isMarquee = value;
                if (_isMarquee) _animTimer.Start();
                else if (Math.Abs(_animValue - _value) < 0.1) _animTimer.Stop();
                this.Invalidate();
            }
        }

        [Category("Bar")]
        [DefaultValue(0.0)]
        public double Min
        {
            get => _min;
            set
            {
                _min = value;
                if (_min > _max) _max = _min;
                if (_value < _min) Value = _min;
                _animValue = _value;
                this.Invalidate();
            }
        }

        [Category("Bar")]
        [DefaultValue(100.0)]
        public double Max
        {
            get => _max;
            set
            {
                _max = value;
                if (_max < _min) _min = _max;
                if (_value > _max) Value = _max;
                this.Invalidate();
            }
        }

        [Category("Bar")]
        [DefaultValue(50.0)]
        public double Value
        {
            get => _value;
            set
            {
                _value = Math.Max(_min, Math.Min(_max, value));
                if (_hasMovingAnimation) _animTimer.Start();
                else _animValue = _value;
                this.Invalidate();
            }
        }

        [Category("Animation")]
        [DefaultValue(15.0)]
        public double Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                UpdateInterval();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.LightGray);

            using (SolidBrush brush = new SolidBrush(this.ForeColor))
            {
                if (_isMarquee) g.FillRectangle(brush, _marqueeOffset, 0, 100, this.Height);
                else
                {
                    double range = _max - _min;
                    double percentage = range > 0 ? (_animValue - _min) / range : 0;
                    int sliderWidth = (int)(this.Width * percentage);

                    if (sliderWidth > 0) g.FillRectangle(brush, 0, 0, sliderWidth, this.Height);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _animTimer != null)
            {
                _animTimer.Stop();
                _animTimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
