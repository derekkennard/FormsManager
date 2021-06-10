using System;
using System.Drawing;
using System.Windows.Forms;

namespace FormsManager
{
    /// <summary>
    ///     A customizable Dialog box with 3 buttons, custom icon, and checkbox.
    /// </summary>
    partial class MsgBox : Form
    {
        /// <summary>
        ///     Create a new instance of the dialog box with a message and title and a standard windows messagebox icon.
        /// </summary>
        /// <param name="message">Message text.</param>
        /// <param name="title">Dialog Box title.</param>
        /// <param name="icon">Standard system messagebox icon.</param>
        public MsgBox(string message, string title, MessageBoxIcon icon = MessageBoxIcon.None) : this(message, title, GetMessageBoxIcon(icon))
        {
        }

        /// <summary>
        ///     Create a new instance of the dialog box with a message and title and a custom windows icon.
        /// </summary>
        /// <param name="message">Message text.</param>
        /// <param name="title">Dialog Box title.</param>
        /// <param name="icon">Custom icon.</param>
        public MsgBox(string message, string title, Icon icon)
        {
            InitializeComponent();

            messageLbl.Text = message;
            Text = title;

            _mSysIcon = icon;

            if (_mSysIcon == null)
                messageLbl.Location = new Point(FormXMargin, FormYMargin);
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        ///     Get system icon for MessageBoxIcon.
        /// </summary>
        /// <param name="icon">The MessageBoxIcon value.</param>
        /// <returns>SystemIcon type Icon.</returns>
        private static Icon GetMessageBoxIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Asterisk:
                    return SystemIcons.Asterisk;
                case MessageBoxIcon.Error:
                    return SystemIcons.Error;
                case MessageBoxIcon.Exclamation:
                    return SystemIcons.Exclamation;
                case MessageBoxIcon.Question:
                    return SystemIcons.Question;
                default:
                    return null;
            }
        }

        #region Setup API

        /// <summary>
        ///     The min required width of the button and checkbox row. Sum of button widths + checkbox width + margins.
        /// </summary>
        private int _mMinButtonRowWidth;

        /// <summary>
        ///     Min set height.
        /// </summary>
        private int _mMinHeight;

        /// <summary>
        ///     Min set width.
        /// </summary>
        private int _mMinWidth;

        /// <summary>
        ///     Sets the min size of the dialog box. If the text or button row needs more size then the dialog box will size to fit
        ///     the text.
        /// </summary>
        /// <param name="width">Min width value.</param>
        /// <param name="height">Min height value.</param>
        public void SetMinSize(int width, int height)
        {
            _mMinWidth = width;
            _mMinHeight = height;
        }

        /// <summary>
        ///     Create up to 3 buttons with no DialogResult values.
        /// </summary>
        /// <param name="names">Array of button names. Must of length 1-3.</param>
        public void SetButtons(params string[] names)
        {
            var drs = new DialogResult[names.Length];
            for (var i = 0; i < names.Length; i++)
                drs[i] = DialogResult.None;
            SetButtons(names, drs);
        }

        /// <summary>
        ///     Create up to 3 buttons with given DialogResult values.
        /// </summary>
        /// <param name="names">Array of button names. Must of length 1-3.</param>
        /// <param name="results">Array of DialogResult values. Must be same length as names.</param>
        public void SetButtons(string[] names, DialogResult[] results)
        {
            SetButtons(names, results, 1);
        }

        /// <summary>
        ///     Create up to 3 buttons with given DialogResult values.
        /// </summary>
        /// <param name="names">Array of button names. Must of length 1-3.</param>
        /// <param name="results">Array of DialogResult values. Must be same length as names.</param>
        /// <param name="def">Default Button number. Must be 1-3.</param>
        public void SetButtons(string[] names, DialogResult[] results, int def)
        {
            if (names == null)
                throw new ArgumentNullException("names", @"Button Text is null");

            var count = names.Length;

            if (count < 1 || count > 3)
                throw new ArgumentException("Invalid number of buttons. Must be between 1 and 3.");

            //---- Set Button 1
            _mMinButtonRowWidth += SetButtonParams(btn1, names[0], def == 1 ? 1 : 2, results[0]);

            //---- Set Button 2
            if (count > 1)
                _mMinButtonRowWidth += SetButtonParams(btn2, names[1], def == 2 ? 1 : 3, results[1]) + ButtonSpace;

            //---- Set Button 3
            if (count > 2)
                _mMinButtonRowWidth += SetButtonParams(btn3, names[2], def == 3 ? 1 : 4, results[2]) + ButtonSpace;
        }

        /// <summary>
        ///     Sets button text and returns the width.
        /// </summary>
        /// <param name="btn">Button object.</param>
        /// <param name="text">Text of the button.</param>
        /// <param name="tab">TabIndex of the button.</param>
        /// <param name="dr">DialogResult of the button.</param>
        /// <returns>Width of the button.</returns>
        private static int SetButtonParams(Button btn, string text, int tab, DialogResult dr)
        {
            btn.Text = text;
            btn.Visible = true;
            btn.DialogResult = dr;
            btn.TabIndex = tab;
            return btn.Size.Width;
        }

        /// <summary>
        ///     Enables the checkbox. By default the checkbox is unchecked.
        /// </summary>
        /// <param name="text">Text of the checkbox.</param>
        public void SetCheckbox(string text)
        {
            SetCheckbox(text, false);
        }

        /// <summary>
        ///     Enables the checkbox and the default checked state.
        /// </summary>
        /// <param name="text">Text of the checkbox.</param>
        /// <param name="chcked">Default checked state of the box.</param>
        public void SetCheckbox(string text, bool chcked)
        {
            chkBx.Visible = true;
            chkBx.Text = text;
            chkBx.Checked = chcked;
            _mMinButtonRowWidth += chkBx.Size.Width + CheckboxSpace;
        }

        #endregion

        #region Sizes and Locations

        private const int FormYMargin = 10;
        private const int FormXMargin = 16;
        private const int ButtonSpace = 5;
        private const int CheckboxSpace = 15;
        private const int TextYMargin = 30;

        private void DialogBox_Load(object sender, EventArgs e)
        {
            if (!btn1.Visible)
                SetButtons(new[] {"OK"}, new[] {DialogResult.OK});

            _mMinButtonRowWidth += 2*FormXMargin; //add margin to the ends

            SetDialogSize();

            SetButtonRowLocations();
        }

        /// <summary>
        ///     Auto fits the dialog box to fit the text and the buttons.
        /// </summary>
        private void SetDialogSize()
        {
            var requiredWidth = messageLbl.Location.X + messageLbl.Size.Width + FormXMargin;
            requiredWidth = requiredWidth > _mMinButtonRowWidth ? requiredWidth : _mMinButtonRowWidth;

            var requiredHeight = messageLbl.Location.Y + messageLbl.Size.Height - btn2.Location.Y + ClientSize.Height + TextYMargin;

            var minSetWidth = ClientSize.Width > _mMinWidth ? ClientSize.Width : _mMinWidth;
            var minSetHeight = ClientSize.Height > _mMinHeight ? ClientSize.Height : _mMinHeight;

            var s = new Size {Width = requiredWidth > minSetWidth ? requiredWidth : minSetWidth, Height = requiredHeight > minSetHeight ? requiredHeight : minSetHeight};
            ClientSize = s;
        }

        /// <summary>
        ///     Sets the buttons and check boxes location.
        /// </summary>
        private void SetButtonRowLocations()
        {
            var formWidth = ClientRectangle.Width;

            var x = formWidth - FormXMargin;
            var y = btn1.Location.Y;

            if (btn3.Visible)
            {
                x -= btn3.Size.Width;
                btn3.Location = new Point(x, y);
                x -= ButtonSpace;
            }

            if (btn2.Visible)
            {
                x -= btn2.Size.Width;
                btn2.Location = new Point(x, y);
                x -= ButtonSpace;
            }

            x -= btn1.Size.Width;
            btn1.Location = new Point(x, y);

            if (chkBx.Visible)
                chkBx.Location = new Point(FormXMargin, chkBx.Location.Y);
        }

        #endregion

        #region Icon Pain

        /// <summary>
        ///     The icon to paint.
        /// </summary>
        private readonly Icon _mSysIcon;

        /// <summary>
        ///     Paint the System Icon in the top left corner.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");
            if (_mSysIcon != null)
            {
                var g = e.Graphics;
                g.DrawIconUnstretched(_mSysIcon, new Rectangle(FormXMargin, FormYMargin, _mSysIcon.Width, _mSysIcon.Height));
            }

            base.OnPaint(e);
        }

        #endregion

        #region Result API

        /// <summary>
        ///     If visible checkbox was checked.
        /// </summary>
        public bool CheckboxChecked
        {
            get { return chkBx.Checked; }
        }

        /// <summary>
        ///     Gets the button that was pressed.
        /// </summary>
        public DialogBoxResult DialogBoxResult { get; private set; }

        private void btn_Click(object sender, EventArgs e)
        {
            if (sender == btn1)
                DialogBoxResult = DialogBoxResult.Button1;
            else if (sender == btn2)
                DialogBoxResult = DialogBoxResult.Button2;
            else if (sender == btn3)
                DialogBoxResult = DialogBoxResult.Button3;

            if (((Button) sender).DialogResult == DialogResult.None)
                Close();
        }

        #endregion
    }

    internal enum DialogBoxResult
    {
        Button1,
        Button2,
        Button3
    }
}