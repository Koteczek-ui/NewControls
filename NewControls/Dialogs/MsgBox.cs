using System;
using System.Windows.Forms;

namespace NewControls.Dialogs
{
    public class MsgBox
    {
        public static DialogResult Info(string msg, string title, MessageBoxButtons btns = MessageBoxButtons.OK) => MessageBox.Show(msg, title, btns, MessageBoxIcon.Information);
        public static DialogResult Ask(string msg, string title, MessageBoxButtons btns = MessageBoxButtons.YesNo) => MessageBox.Show(msg, title, btns, MessageBoxIcon.Question);
        public static DialogResult Warn(string msg, string title, MessageBoxButtons btns = MessageBoxButtons.OKCancel) => MessageBox.Show(msg, title, btns, MessageBoxIcon.Warning);
        public static DialogResult Err(string msg, string title, MessageBoxButtons btns = MessageBoxButtons.RetryCancel) => MessageBox.Show(msg, title, btns, MessageBoxIcon.Error);
        public static DialogResult Msg(string msg, string title, MessageBoxButtons btns) => MessageBox.Show(msg, title, btns, MessageBoxIcon.None);
        public static DialogResult RetryErr(string msg, Action retryAction, string title = "Error")
        {
            DialogResult result = Err(msg, title);
            if (result == DialogResult.Retry) retryAction();
            return result;
        }
        public static DialogResult RetryErrFromEx(Exception ex, Action retryAction, string title = "Error") => RetryErr(ex.Message, retryAction, title);
    }
}
