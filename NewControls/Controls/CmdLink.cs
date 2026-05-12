using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace NewControls.Controls
{
    [ToolboxItem(true)]
    public partial class CmdLink : Btn
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, uint wParam, string lParam);

        private const uint BS_COMMANDLINK = 0x0000000E;
        private const uint BCM_SETNOTE = 0x1609;

        private string _note = GetDefaultNote();

        public static string GetDefaultNote() => "This is a descriptive explanation of the action.";

        public CmdLink()
        {
            InitializeComponent();
            FlatStyle = FlatStyle.System;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= (int)BS_COMMANDLINK;
                return cp;
            }
        }

        [Category("Appearance")]
        [Description("The note text that appears below the main button text.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                SetNote(_note);
            }
        }

        private void SetNote(string note)
        {
            if (IsHandleCreated)
            {
                SendMessage(new HandleRef(this, Handle), BCM_SETNOTE, 0, note);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetNote(_note);
            Cursor = Cursors.Hand;
        }
    }
}
