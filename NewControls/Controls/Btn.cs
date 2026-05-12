using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace NewControls.Controls
{
    [ToolboxItem(true)]
    public partial class Btn : Button
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, uint wParam, uint lParam);

        private const uint BCM_SETSHIELD = 0x160C;

        private bool _hasUacShield = false;

        public Btn()
        {
            InitializeComponent();
            FlatStyle = FlatStyle.System;
        }

        [Category("Appearance")]
        [Description("Shows or hides the UAC shield icon on the button.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool HasUACShield
        {
            get => _hasUacShield;
            set
            {
                _hasUacShield = value;
                UpdateShield();
            }
        }

        private void UpdateShield()
        {
            if (IsHandleCreated)
            {
                HandleRef h = new HandleRef(this, Handle);
                var v = _hasUacShield ? 1u : 0u;
                SendMessage(h, BCM_SETSHIELD, 0, v);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateShield();
            Cursor = Cursors.Hand;
        }
    }
}
