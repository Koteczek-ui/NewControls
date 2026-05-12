using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NewControls.Dialogs
{
    public class Dialog
    {
        protected Form _form;
        protected string _title;
        protected Size _size;
        protected Size _minSize;
        protected Size _maxSize;
        protected Color _bgColor;
        protected bool _canClose;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint SC_CLOSE = 0xF060;
        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint MF_ENABLED = 0x00000000;

        protected Dialog(string title, bool canClose) {
            _form = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterParent
            };
            Title = title;
            _form.Load += (s, e) =>
            {
                if (!_canClose)
                    EnableCloseButton(false);
            };
        }

        [Category("Apperance")]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                _form.Text = _title;
            }
        }

        [Category("Layout")]
        public Size Size
        {
            get => _size;
            set
            {
                _size = value;
                _form.Size = _size;
            }
        }

        [Category("Layout")]
        public Size MinSize
        {
            get => _minSize;
            set
            {
                _minSize = value;
                _form.MinimumSize = _minSize;
            }
        }

        [Category("Layout")]
        public Size MaxSize
        {
            get => _maxSize;
            set
            {
                _maxSize = value;
                _form.MaximumSize = _maxSize;
            }
        }

        [Category("Apperance")]
        public Color BackColor
        {
            get => _bgColor;
            set
            {
                _bgColor = value;
                _form.BackColor = _bgColor;
            }
        }

        public DialogResult ShowDialog() => _form.ShowDialog();
        public DialogResult ShowDialog(IWin32Window owner) => _form.ShowDialog(owner);
        public void Close() => _form.Close();
        public void Show() => _form.Show();
        public void Show(IWin32Window owner) => _form.Show(owner);
        public void Hide() => _form.Hide();

        private void EnableCloseButton(bool enable)
        {
            IntPtr hMenu = GetSystemMenu(_form.Handle, false);
            if (hMenu != IntPtr.Zero)
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | (enable ? MF_ENABLED : MF_GRAYED));
        }
    }
}
