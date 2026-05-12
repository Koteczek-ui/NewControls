using NewControls.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace NewControls.Dialogs
{
    public class ActionDialog : Dialog
    {
        private string _txt;
        private Dictionary<CmdLink, Action<ActionDialog>> _actions;
        private Font _txtFont;
        private Color _txtFgColor;
        private Color _txtBgColor;

        private Label _txtLbl;
        private Panel _actionsPanel;

        [Category("Appearance")]
        public string Text
        {
            get => _txt;
            set
            {
                _txt = value;
                _txtLbl.Text = _txt;
            }
        }

        [Category("Behavior")]
        public Dictionary<CmdLink, Action<ActionDialog>> Actions
        {
            get => _actions;
            set
            {
                _actions = value;
                UpdateCmdLinks();
            }
        }

        [Category("Appearance")]
        public Font TextFont
        {
            get => _txtFont;
            set
            {
                _txtFont = value;
                _txtLbl.Font = _txtFont;
            }
        }

        [Category("Appearance")]
        public Color TextForeColor
        {
            get => _txtFgColor;
            set
            {
                _txtFgColor = value;
                _txtLbl.ForeColor = _txtFgColor;
            }
        }

        [Category("Appearance")]
        public Color TextBackColor
        {
            get => _txtBgColor;
            set
            {
                _txtBgColor = value;
                _txtLbl.BackColor = _txtBgColor;
            }
        }

        public ActionDialog(string title, string text, Dictionary<CmdLink, Action<ActionDialog>> actions, bool canClose = true) : base(title, canClose)
        {
            _form.AutoSize = true;
            _form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            
            MinSize = new Size(350, 0);
            _form.Padding = new Padding(20);

            _txtLbl = new Label()
            {
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 15)
            };

            _actionsPanel = new Panel()
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Width = 310
            };

            _form.Controls.Add(_actionsPanel);
            _form.Controls.Add(_txtLbl);

            Text = text;
            Actions = actions;
        }

        private void UpdateCmdLinks()
        {
            _actionsPanel.Controls.Clear();
            if (_actions == null) return;

            List<CmdLink> links = new List<CmdLink>(_actions.Keys);
            links.Reverse();

            foreach (var lnk in links)
            {
                Action<ActionDialog> action = _actions[lnk];

                lnk.Dock = DockStyle.Top;
                lnk.Height = string.IsNullOrEmpty(lnk.Note) ? 42 : 56;
                lnk.Margin = new Padding(0, 0, 0, 10);

                lnk.Click += (sender, e) => action(this);

                _actionsPanel.Controls.Add(lnk);
            }

            _form.PerformLayout();
        }
    }
}
