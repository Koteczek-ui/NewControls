using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NewControls.Dialog
{
    public class ProgressDialog : Dialog
    {
        private string _txt;
        private PBar _pBar;
        private Label _txtLbl;
        private Color _pBarFgColor;
        private Btn _cancelBtn;

        [Category("Apperance")]
        public Color PBarForeColor
        {
            get => _pBarFgColor;
            set
            {
                _pBarFgColor = value;
                _pBar.ForeColor = _pBarFgColor;
            }
        }

        [Category("Apperance")]
        public string Text
        {
            get => _txt;
            set
            {
                _txt = value;
                _txtLbl.Text = _txt;
            }
        }

        public Btn CancelBtn
        {
            get => _cancelBtn;
        }

        public ProgressDialog(string title, string text, PBar pBar, bool cancelBtn = true, string cancelBtnText = "Cancel", bool cancelBtnVisible = true, Action cancelAction = null) : base(title, cancelBtn)
        {
            if (cancelAction == null) cancelAction = () => Close();

            _pBar = pBar;
            _canClose = cancelBtn;

            _txtLbl = new Label
            {
                Text = _txt,
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 10, 0)
            };

            Text = text;

            _pBar.Dock = DockStyle.Top;

            _cancelBtn = new Btn
            {
                Text = cancelBtnText,
                Dock = DockStyle.Bottom,
                Height = 25,
                Visible = cancelBtnVisible,
                Enabled = cancelBtn
            };
            _cancelBtn.Click += (s, e) => cancelAction();

            _form.Controls.Add(_pBar);
            _form.Controls.Add(_txtLbl);
            _form.Controls.Add(_cancelBtn);

            _form.Height = _txtLbl.Height + _pBar.Height + (_form.Height - _form.ClientRectangle.Height);

            if (cancelBtnVisible)
                _form.Height += _cancelBtn.Height;
        }
    }
}
