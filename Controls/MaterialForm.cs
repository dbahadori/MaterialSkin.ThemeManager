using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialForm : Form, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        private MaterialSkinManager skinmanager = null;
        [Browsable(true)]
        public MaterialSkinManager SkinManager
        {
            get
            {

                return (skinmanager != null) ? skinmanager : MaterialSkinManager.Instance;
            }
            set
            {
                skinmanager = value;
            }
        }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        public new FormBorderStyle FormBorderStyle { get { return base.FormBorderStyle; } set { base.FormBorderStyle = value; } }
        public bool Sizable { get; set; }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX info);
        
        #region Join To Theme
        // Actionbar 


        private bool actionBarJoinToTheme;

        public bool ActionBarJoinToTheme { get { return actionBarJoinToTheme; } set { actionBarJoinToTheme = value; Invalidate(); } }
        public ColorCategory ActionBarColor { get; set; }

        private bool statusBarJoinToTheme;
        public bool StatusBarJoinToTheme { get { return statusBarJoinToTheme; } set { statusBarJoinToTheme = value; Invalidate(); } }
        public ColorCategory StatusBarColor { get; set; }



        private bool bodyJoinToTheme;
        public bool BodyJoinToTheme { get { return bodyJoinToTheme; } set { bodyJoinToTheme = value; Invalidate(); } }
        public ColorCategory BodyColor { get; set; }

        private bool actionButtonHoverJoinToTheme;
        public bool ActionButtonHoverJoinToTheme { get { return actionButtonHoverJoinToTheme; } set { actionButtonHoverJoinToTheme = value; Invalidate(); } }
        public ColorCategory ActionButtonHoverColor { get; set; }

        private bool actionButtonDownJoinToTheme;
        public bool ActionButtonDownJoinToTheme { get { return actionButtonDownJoinToTheme; } set { actionButtonDownJoinToTheme = value; Invalidate(); } }
        public ColorCategory ActionButtonDownColor { get; set; }

        #endregion

        private ColorScheme selectedColorScheme;
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }


        private const int EM_SETCUEBANNER = 0x1501;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTBOTTOM = 15;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int BORDER_WIDTH = 7;
        private ResizeDirection _resizeDir;
        private ButtonState _buttonState = ButtonState.None;

        private const int WMSZ_TOP = 3;
        private const int WMSZ_TOPLEFT = 4;
        private const int WMSZ_TOPRIGHT = 5;
        private const int WMSZ_LEFT = 1;
        private const int WMSZ_RIGHT = 2;
        private const int WMSZ_BOTTOM = 6;
        private const int WMSZ_BOTTOMLEFT = 7;
        private const int WMSZ_BOTTOMRIGHT = 8;

        private readonly Dictionary<int, int> _resizingLocationsToCmd = new Dictionary<int, int>
        {
            {HTTOP,         WMSZ_TOP},
            {HTTOPLEFT,     WMSZ_TOPLEFT},
            {HTTOPRIGHT,    WMSZ_TOPRIGHT},
            {HTLEFT,        WMSZ_LEFT},
            {HTRIGHT,       WMSZ_RIGHT},
            {HTBOTTOM,      WMSZ_BOTTOM},
            {HTBOTTOMLEFT,  WMSZ_BOTTOMLEFT},
            {HTBOTTOMRIGHT, WMSZ_BOTTOMRIGHT}
        };

        private  int STATUS_BAR_BUTTON_WIDTH =35;
        private  int STATUS_BAR_HEIGHT = 35;
        private  int ACTION_BAR_HEIGHT = 38;
        public int Status_Bar_Height{ get { return STATUS_BAR_HEIGHT; } set { STATUS_BAR_HEIGHT = value; } }
        public int Action_Bar_Height { get { return ACTION_BAR_HEIGHT; } set { ACTION_BAR_HEIGHT = value; } }


        private const uint TPM_LEFTALIGN = 0x0000;
        private const uint TPM_RETURNCMD = 0x0100;

        private const int WM_SYSCOMMAND = 0x0112;
        private const int WS_MINIMIZEBOX = 0x20000;
        private const int WS_SYSMENU = 0x00080000;

        private const int MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class MONITORINFOEX
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szDevice = new char[32];
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public int Width()
            {
                return right - left;
            }

            public int Height()
            {
                return bottom - top;
            }
        }

        private bool showTitle = true;
        public bool ShowTitle
        {
            get
            {
                return showTitle;
            }
            set
            {
                showTitle = value;
            }
        }

        private enum ResizeDirection
        {
            BottomLeft,
            Left,
            Right,
            BottomRight,
            Bottom,
            None
        }

        private enum ButtonState
        {
            XOver,
            MaxOver,
            MinOver,
            XDown,
            MaxDown,
            MinDown,
            None
        }

        private readonly Cursor[] _resizeCursors = { Cursors.SizeNESW, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeWE, Cursors.SizeNS };

        private Rectangle _minButtonBounds;
        private Rectangle _maxButtonBounds;
        private Rectangle _xButtonBounds;
        private Rectangle _actionBarBounds;
        private Rectangle _statusBarBounds;

        private bool _maximized;
        private Size _previousSize;
        private Point _previousLocation;
        private bool _headerMouseDown;

        public MaterialForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            Sizable = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            // This enables the form to trigger the MouseMove event even when mouse is over another control
            Application.AddMessageFilter(new MouseMessageFilter());
            MouseMessageFilter.MouseMove += OnGlobalMouseMove;
            //ActionButtonHoverJoinToTheme = true;
            //ActionButtonDownJoinToTheme = true;
            //ActionBarJoinToTheme = true;
            //StatusBarJoinToTheme = true;
            //BodyJoinToTheme = true;
        }

        protected override void WndProc(ref Message m)
        {
            int a;
            if (m.Msg == EM_SETCUEBANNER)
                a = 2;

            base.WndProc(ref m);
            if (DesignMode || IsDisposed) return;

            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                MaximizeWindow(!_maximized);
            }
            else if (m.Msg == WM_MOUSEMOVE && _maximized &&
                (_statusBarBounds.Contains(PointToClient(Cursor.Position)) || _actionBarBounds.Contains(PointToClient(Cursor.Position))) &&
                !(_minButtonBounds.Contains(PointToClient(Cursor.Position)) || _maxButtonBounds.Contains(PointToClient(Cursor.Position)) || _xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (_headerMouseDown)
                {
                    _maximized = false;
                    _headerMouseDown = false;

                    var mousePoint = PointToClient(Cursor.Position);
                    if (mousePoint.X < Width / 2)
                        Location = mousePoint.X < _previousSize.Width / 2 ?
                            new Point(Cursor.Position.X - mousePoint.X, Cursor.Position.Y - mousePoint.Y) :
                            new Point(Cursor.Position.X - _previousSize.Width / 2, Cursor.Position.Y - mousePoint.Y);
                    else
                        Location = Width - mousePoint.X < _previousSize.Width / 2 ?
                            new Point(Cursor.Position.X - _previousSize.Width + Width - mousePoint.X, Cursor.Position.Y - mousePoint.Y) :
                            new Point(Cursor.Position.X - _previousSize.Width / 2, Cursor.Position.Y - mousePoint.Y);

                    Size = _previousSize;
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            }
            else if (m.Msg == WM_LBUTTONDOWN &&
                (_statusBarBounds.Contains(PointToClient(Cursor.Position)) || _actionBarBounds.Contains(PointToClient(Cursor.Position))) &&
                !(_minButtonBounds.Contains(PointToClient(Cursor.Position)) || _maxButtonBounds.Contains(PointToClient(Cursor.Position)) || _xButtonBounds.Contains(PointToClient(Cursor.Position))))
            {
                if (!_maximized)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
                else
                {
                    _headerMouseDown = true;
                }
            }
            else if (m.Msg == WM_RBUTTONDOWN)
            {
                Point cursorPos = PointToClient(Cursor.Position);

                if (_statusBarBounds.Contains(cursorPos) && !_minButtonBounds.Contains(cursorPos) &&
                    !_maxButtonBounds.Contains(cursorPos) && !_xButtonBounds.Contains(cursorPos))
                {
                    // Show default system menu when right clicking titlebar
                    var id = TrackPopupMenuEx(GetSystemMenu(Handle, false), TPM_LEFTALIGN | TPM_RETURNCMD, Cursor.Position.X, Cursor.Position.Y, Handle, IntPtr.Zero);

                    // Pass the command as a WM_SYSCOMMAND message
                    SendMessage(Handle, WM_SYSCOMMAND, id, 0);
                }
            }
            else if (m.Msg == WM_NCLBUTTONDOWN)
            {
                // This re-enables resizing by letting the application know when the
                // user is trying to resize a side. This is disabled by default when using WS_SYSMENU.
                if (!Sizable) return;

                byte bFlag = 0;

                // Get which side to resize from
                if (_resizingLocationsToCmd.ContainsKey((int)m.WParam))
                    bFlag = (byte)_resizingLocationsToCmd[(int)m.WParam];

                if (bFlag != 0)
                    SendMessage(Handle, WM_SYSCOMMAND, 0xF000 | bFlag, (int)m.LParam);
            }
            else if (m.Msg == WM_LBUTTONUP)
            {
                _headerMouseDown = false;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var par = base.CreateParams;
                // WS_SYSMENU: Trigger the creation of the system menu
                // WS_MINIMIZEBOX: Allow minimizing from taskbar
                par.Style = par.Style | WS_MINIMIZEBOX | WS_SYSMENU; // Turn on the WS_MINIMIZEBOX style flag
                return par;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons(e);

            if (e.Button == MouseButtons.Left && !_maximized)
                ResizeForm(_resizeDir);
            base.OnMouseDown(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (DesignMode) return;
            _buttonState = ButtonState.None;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (DesignMode) return;

            if (Sizable)
            {
                //True if the mouse is hovering over a child control
                var isChildUnderMouse = GetChildAtPoint(e.Location) != null;

                if (e.Location.X < BORDER_WIDTH && e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !_maximized)
                {
                    //_resizeDir = ResizeDirection.BottomLeft;
                    _resizeDir = (this.RightToLeft != RightToLeft.Yes && this.RightToLeftLayout != false) ? ResizeDirection.BottomLeft : ResizeDirection.BottomRight;
                    Cursor = Cursors.SizeNESW;
                }
                else if (e.Location.X < BORDER_WIDTH && !isChildUnderMouse && !_maximized)
                {
                    // _resizeDir = ResizeDirection.Left;
                    _resizeDir = (this.RightToLeft != RightToLeft.Yes && this.RightToLeftLayout != false) ? ResizeDirection.Left : ResizeDirection.Right;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.X > Width - BORDER_WIDTH && e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !_maximized)
                {
                    //_resizeDir = ResizeDirection.BottomRight;
                    _resizeDir = (this.RightToLeft != RightToLeft.Yes && this.RightToLeftLayout != false) ? ResizeDirection.BottomRight : ResizeDirection.BottomLeft;
                    Cursor = Cursors.SizeNWSE;
                }
                else if (e.Location.X > Width - BORDER_WIDTH && !isChildUnderMouse && !_maximized)
                {
                    //_resizeDir = ResizeDirection.Right;
                    _resizeDir = (this.RightToLeft != RightToLeft.Yes && this.RightToLeftLayout != false) ? ResizeDirection.Right : ResizeDirection.Left;
                    Cursor = Cursors.SizeWE;
                }
                else if (e.Location.Y > Height - BORDER_WIDTH && !isChildUnderMouse && !_maximized)
                {
                    _resizeDir = ResizeDirection.Bottom;
                    Cursor = Cursors.SizeNS;
                }
                else
                {
                    _resizeDir = ResizeDirection.None;

                    //Only reset the cursor when needed, this prevents it from flickering when a child control changes the cursor to its own needs
                    if (_resizeCursors.Contains(Cursor))
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

            UpdateButtons(e);
        }

        protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDisposed) return;
            // Convert to client position and pass to Form.MouseMove
            var clientCursorPos = PointToClient(e.Location);
            var newE = new MouseEventArgs(MouseButtons.None, 0, clientCursorPos.X, clientCursorPos.Y, 0);
            OnMouseMove(newE);
        }

        private void UpdateButtons(MouseEventArgs e, bool up = false)
        {
            if (DesignMode) return;
            var oldState = _buttonState;
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;

            if (e.Button == MouseButtons.Left && !up)
            {
                if (showMin && !showMax && _maxButtonBounds.Contains(e.Location))
                    _buttonState = ButtonState.MinDown;
                else if (showMin && showMax && _minButtonBounds.Contains(e.Location))
                    _buttonState = ButtonState.MinDown;
                else if (showMax && _maxButtonBounds.Contains(e.Location))
                    _buttonState = ButtonState.MaxDown;
                else if (ControlBox && _xButtonBounds.Contains(e.Location))
                    _buttonState = ButtonState.XDown;
                else
                    _buttonState = ButtonState.None;
            }
            else
            {
                if (showMin && !showMax && _maxButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown && up)
                        WindowState = FormWindowState.Minimized;
                }
                else if (showMin && showMax && _minButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MinOver;

                    if (oldState == ButtonState.MinDown && up)
                        WindowState = FormWindowState.Minimized;
                }
                else if (MaximizeBox && ControlBox && _maxButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.MaxOver;

                    if (oldState == ButtonState.MaxDown && up)
                        MaximizeWindow(!_maximized);

                }
                else if (ControlBox && _xButtonBounds.Contains(e.Location))
                {
                    _buttonState = ButtonState.XOver;

                    if (oldState == ButtonState.XDown && up)
                        Close();
                }
                else _buttonState = ButtonState.None;
            }

            if (oldState != _buttonState) Invalidate();
        }

        private void MaximizeWindow(bool maximize)
        {
            if (!MaximizeBox || !ControlBox) return;

            _maximized = maximize;

            if (maximize)
            {
                var monitorHandle = MonitorFromWindow(Handle, MONITOR_DEFAULTTONEAREST);
                var monitorInfo = new MONITORINFOEX();
                GetMonitorInfo(new HandleRef(null, monitorHandle), monitorInfo);
                _previousSize = Size;
                _previousLocation = Location;
                Size = new Size(monitorInfo.rcWork.Width(), monitorInfo.rcWork.Height());
                Location = new Point(monitorInfo.rcWork.left, monitorInfo.rcWork.top);
            }
            else
            {
                Size = _previousSize;
                Location = _previousLocation;
            }

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode) return;
            UpdateButtons(e, true);

            base.OnMouseUp(e);
            ReleaseCapture();
        }

        private void ResizeForm(ResizeDirection direction)
        {
            if (DesignMode) return;
            var dir = -1;
            switch (direction)
            {
                case ResizeDirection.BottomLeft:
                    dir = HTBOTTOMLEFT;
                    break;
                case ResizeDirection.Left:
                    dir = HTLEFT;
                    break;
                case ResizeDirection.Right:
                    dir = HTRIGHT;
                    break;
                case ResizeDirection.BottomRight:
                    dir = HTBOTTOMRIGHT;
                    break;
                case ResizeDirection.Bottom:
                    dir = HTBOTTOM;
                    break;
            }

            ReleaseCapture();
            if (dir != -1)
            {
                SendMessage(Handle, WM_NCLBUTTONDOWN, dir, 0);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            _minButtonBounds = new Rectangle((Width) - 3 * STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
            _maxButtonBounds = new Rectangle((Width) - 2 * STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
            _xButtonBounds = new Rectangle((Width) - STATUS_BAR_BUTTON_WIDTH, 0, STATUS_BAR_BUTTON_WIDTH, STATUS_BAR_HEIGHT);
            _statusBarBounds = new Rectangle(0, 0, Width, STATUS_BAR_HEIGHT);
            _actionBarBounds = new Rectangle(0, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            //g.Clear(SkinManager.GetApplicationBackgroundColor());
            //BackColor = SkinManager.GetApplicationBackgroundColor();


            if (!BodyJoinToTheme)


            {
                g.Clear(getColorDisjoinFromTheme(BodyColor));
                BackColor = getColorDisjoinFromTheme(BodyColor);
            }
            else
            {
                g.Clear(SkinManager.GetApplicationBackgroundColor());
                BackColor = SkinManager.GetApplicationBackgroundColor();
            }

            if (!ControlBox)
            {
                g.FillRectangle(SkinManager.GetPrimaryBrush(selectedColorScheme), new Rectangle(0, 0, 0, 0));
                g.FillRectangle(SkinManager.GetDarkPrimaryBrush(selectedColorScheme), new Rectangle(0, _actionBarBounds.Y - _statusBarBounds.Height, _actionBarBounds.Width, _actionBarBounds.Height));
            }
            else
            {


                if (!StatusBarJoinToTheme)
                {
                    g.FillRectangle(getBrushDisjoinFromTheme(StatusBarColor), _statusBarBounds);
                }
                else
                {
                    StatusBarColor = ColorCategory.DarkPrimary;
                    g.FillRectangle(SkinManager.GetDarkPrimaryBrush(selectedColorScheme), _statusBarBounds);
                }

                if (!ActionBarJoinToTheme)
                {
                    g.FillRectangle(getBrushDisjoinFromTheme(ActionBarColor), _actionBarBounds);

                }
                else
                {
                    ActionBarColor = ColorCategory.Primary;
                    g.FillRectangle(SkinManager.GetPrimaryBrush(selectedColorScheme), _actionBarBounds);
                }
            }

            //Draw border
            using (var borderPen = new Pen(SkinManager.GetDividersColor(), 1))
            {
                g.DrawLine(borderPen, new Point(0, _actionBarBounds.Bottom), new Point(0, Height - 2));
                g.DrawLine(borderPen, new Point(Width - 1, _actionBarBounds.Bottom), new Point(Width - 1, Height - 2));
                g.DrawLine(borderPen, new Point(0, Height - 1), new Point(Width - 1, Height - 1));
            }

            // Determine whether or not we even should be drawing the buttons.
            bool showMin = MinimizeBox && ControlBox;
            bool showMax = MaximizeBox && ControlBox;
            Brush hoverBrush;
            Brush downBrush;
            if (!ActionButtonHoverJoinToTheme)
            {
                hoverBrush = getBrushDisjoinFromTheme(ActionButtonHoverColor);
            }
            else { hoverBrush = SkinManager.GetFlatButtonHoverBackgroundBrush(); }

            if (!ActionButtonHoverJoinToTheme)
            {
                downBrush = getBrushDisjoinFromTheme(ActionButtonDownColor);
            }
            else { downBrush = SkinManager.GetFlatButtonPressedBackgroundBrush(); }



            // When MaximizeButton == false, the minimize button will be painted in its place
            if (_buttonState == ButtonState.MinOver && showMin)
                g.FillRectangle(hoverBrush, showMax ? _minButtonBounds : _maxButtonBounds);

            if (_buttonState == ButtonState.MinDown && showMin)
                g.FillRectangle(downBrush, showMax ? _minButtonBounds : _maxButtonBounds);

            if (_buttonState == ButtonState.MaxOver && showMax)
                g.FillRectangle(hoverBrush, _maxButtonBounds);

            if (_buttonState == ButtonState.MaxDown && showMax)
                g.FillRectangle(downBrush, _maxButtonBounds);

            if (_buttonState == ButtonState.XOver && ControlBox)
                g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#cb1a1a")), _xButtonBounds);

            if (_buttonState == ButtonState.XDown && ControlBox)
                g.FillRectangle(new SolidBrush(ColorTranslator.FromHtml("#cb1a1a")), _xButtonBounds);

            using (var formButtonsPen = new Pen(SkinManager.ACTION_BAR_TEXT_SECONDARY, 2))
            {
                Rectangle imageRect;

                // Minimize button.
                if (showMin)
                {
                    imageRect = new Rectangle(_minButtonBounds.X + (_minButtonBounds.Width / 2) - (MaterialSkin.Properties.Resources.min.Width / 2),
                        _minButtonBounds.Y + (_minButtonBounds.Height / 2) - (MaterialSkin.Properties.Resources.min.Height / 2),
                        MaterialSkin.Properties.Resources.min.Width,
                        MaterialSkin.Properties.Resources.min.Height);


                    g.DrawImage(MaterialSkin.Properties.Resources.min, imageRect);

                    // int x = showMax ? _minButtonBounds.X : _maxButtonBounds.X;
                    // int y = showMax ? _minButtonBounds.Y : _maxButtonBounds.Y;

                    // g.DrawLine(
                    //     formButtonsPen,
                    //     x + (int)(_minButtonBounds.Width * 0.33),
                    //     y + (int)(_minButtonBounds.Height * 0.66),
                    //     x + (int)(_minButtonBounds.Width * 0.66),
                    //     y + (int)(_minButtonBounds.Height * 0.66)
                    //);
                }

                // Maximize button
                if (showMax)
                {
                    Image maxImage;

                    if (_maximized)
                        maxImage = MaterialSkin.Properties.Resources.max2;
                    else
                        maxImage = MaterialSkin.Properties.Resources.max1;

                    imageRect = new Rectangle(_maxButtonBounds.X + (_maxButtonBounds.Width / 2) - (maxImage.Width / 2),
                        _maxButtonBounds.Y + (_maxButtonBounds.Height / 2) - (maxImage.Height / 2),
                        maxImage.Width,
                        maxImage.Height);


                    g.DrawImage(maxImage, imageRect);

                    // g.DrawRectangle(
                    //     formButtonsPen,
                    //     _maxButtonBounds.X + (int)(_maxButtonBounds.Width * 0.33),
                    //     _maxButtonBounds.Y + (int)(_maxButtonBounds.Height * 0.36),
                    //     (int)(_maxButtonBounds.Width * 0.39),
                    //     (int)(_maxButtonBounds.Height * 0.31)
                    //);
                }

                // Close button
                if (ControlBox)
                {

                    imageRect = new Rectangle(_xButtonBounds.X + (_xButtonBounds.Width / 2) - (MaterialSkin.Properties.Resources.close.Width / 2),
                        _xButtonBounds.Y + (_xButtonBounds.Height / 2) - (MaterialSkin.Properties.Resources.close.Height / 2),
                        MaterialSkin.Properties.Resources.close.Width,
                        MaterialSkin.Properties.Resources.close.Height);

                    g.DrawImage(MaterialSkin.Properties.Resources.close, imageRect);

                    // g.DrawLine(
                    //     formButtonsPen,
                    //     _xButtonBounds.X + (int)(_xButtonBounds.Width * 0.33),
                    //     _xButtonBounds.Y + (int)(_xButtonBounds.Height * 0.33),
                    //     _xButtonBounds.X + (int)(_xButtonBounds.Width * 0.66),
                    //     _xButtonBounds.Y + (int)(_xButtonBounds.Height * 0.66)
                    //);

                    // g.DrawLine(
                    //     formButtonsPen,
                    //     _xButtonBounds.X + (int)(_xButtonBounds.Width * 0.66),
                    //     _xButtonBounds.Y + (int)(_xButtonBounds.Height * 0.33),
                    //     _xButtonBounds.X + (int)(_xButtonBounds.Width * 0.33),
                    //     _xButtonBounds.Y + (int)(_xButtonBounds.Height * 0.66));
                }
            }

            //Form title


            // g.DrawString(Text, SkinManager.ROBOTO_MEDIUM_12, SkinManager.ColorScheme.TextBrush, new Rectangle(SkinManager.FORM_PADDING, STATUS_BAR_HEIGHT, Width, ACTION_BAR_HEIGHT), new StringFormat { LineAlignment = StringAlignment.Center });****

            if (showTitle)
            {
                if (!this.RightToLeftLayout && this.RightToLeft != RightToLeft.Yes)
                {
                    g.DrawString(Text, this.Font, SkinManager.ColorScheme.TextBrush, new Rectangle(SkinManager.FORM_PADDING, (ControlBox) ? STATUS_BAR_HEIGHT : 0, Width, ACTION_BAR_HEIGHT), new StringFormat { LineAlignment = StringAlignment.Center });
                }
                else
                {
                    DrawFlippedText(
                    g,
                    this.Text,
                    this.Font,
                    SkinManager.ColorScheme.TextBrush,
                    new Rectangle(
                    SkinManager.FORM_PADDING,
                    (ControlBox) ? STATUS_BAR_HEIGHT : 0,
                    Width,
                    ACTION_BAR_HEIGHT
                                        ),
                    true,
                    false,
                    new StringFormat
                    {
                        LineAlignment = StringAlignment.Center
                    });
                }
            }
        }

        public void DrawFlippedText(Graphics gr, string text, Font the_font, Brush the_brush, Rectangle rect, bool flip_x, bool flip_y, StringFormat stringFormat)
       {
            // Save the current graphics state.
            GraphicsState state = gr.Save();

            float rx = rect.X;
            float ry = rect.Y;
            float r_width = rect.Width;
            float r_height = rect.Height;

            // Set up the transformation.
            int scale_x = flip_x ? -1 : 1;
            int scale_y = flip_y ? -1 : 1;

            gr.ResetTransform();
            gr.ScaleTransform(scale_x, scale_y);

            // Figure out where to draw.
            SizeF txt_size = gr.MeasureString(text, the_font);

            rect.X = Convert.ToInt32(flip_x? (-rx - txt_size.Width) : rx);
            rect.Y = Convert.ToInt32(flip_y? (-ry - txt_size.Height) : ry);

            // Draw.
            gr.DrawString(text, the_font, the_brush, rect, stringFormat);

            // Restore the original graphics state.
            gr.Restore(state);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MaterialForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "MaterialForm";
            this.ResumeLayout(false);

        }
        private Brush getBrushDisjoinFromTheme(ColorCategory _color)
        {
            Brush brush = new SolidBrush(Color.White); ;

            switch (_color)
            {
                case ColorCategory.DarkPrimary:
                    brush = SkinManager.GetDarkPrimaryBrush(SelectedColorScheme);
                    break;
                case ColorCategory.LightPrimary:
                    brush = SkinManager.GetLightPrimaryBrush(SelectedColorScheme);
                    break;
                case ColorCategory.Utility1:
                    brush = SkinManager.GetUtility1Brush(SelectedColorScheme);
                    break;
                case ColorCategory.Utility2:
                    brush = SkinManager.GetUtility2Brush(SelectedColorScheme);
                    break;
                case ColorCategory.TextColor:
                    brush = new SolidBrush(SkinManager.GetForeColor());
                    break;
                case ColorCategory.DarkThemeTextColor:
                    brush = new SolidBrush(SkinManager.GetForeColor(MaterialSkinManager.Themes.DARK));
                    break;
                case ColorCategory.LightThemeTextColor:
                    brush = new SolidBrush(SkinManager.GetForeColor(MaterialSkinManager.Themes.LIGHT));
                    break;
                case ColorCategory.Secondary:
                    brush = SkinManager.GetAccentBrush(SelectedColorScheme);
                    break;
                case ColorCategory.DarkSecondary:
                    brush = SkinManager.GetDarkAccentBrush(SelectedColorScheme);
                    break;
                case ColorCategory.LightSecondary:
                    brush = SkinManager.GetLightAccentBrush(SelectedColorScheme);
                    break;
                default:
                    brush = SkinManager.GetPrimaryBrush(SelectedColorScheme);
                    break;


            }
            return brush;

        }
        private Color getColorDisjoinFromTheme(ColorCategory _color)
        {
            Color color = new Color();

            switch (_color)
            {
                case ColorCategory.DarkPrimary:
                    color = SkinManager.GetDarkPrimaryColor(SelectedColorScheme);
                    break;
                case ColorCategory.LightPrimary:
                    color = SkinManager.GetLightPrimaryColor(SelectedColorScheme);
                    break;
                case ColorCategory.Utility1:
                    color = SkinManager.GetUtility1Color(SelectedColorScheme);
                    break;
                case ColorCategory.Utility2:
                    color = SkinManager.GetUtility2Color(SelectedColorScheme);
                    break;
                case ColorCategory.TextColor:
                    color = SkinManager.GetForeColor();
                    break;
                case ColorCategory.DarkThemeTextColor:
                    color = SkinManager.GetForeColor(MaterialSkinManager.Themes.DARK);
                    break;
                case ColorCategory.LightThemeTextColor:
                    color = SkinManager.GetForeColor(MaterialSkinManager.Themes.LIGHT);
                    break;
                case ColorCategory.Secondary:
                    color = SkinManager.GetAccentColor(SelectedColorScheme);
                    break;
                case ColorCategory.DarkSecondary:
                    color = SkinManager.GetDarkAccentColor(SelectedColorScheme);
                    break;
                case ColorCategory.LightSecondary:
                    color = SkinManager.GetLightAccentColor(SelectedColorScheme);
                    break;
                default:
                    color = SkinManager.GetPrimaryColor(SelectedColorScheme);
                    break;


            }
            return color;

        }
    }

    public class MouseMessageFilter : IMessageFilter
    {
        private const int WM_MOUSEMOVE = 0x0200;
        private const int EM_SETCUEBANNER = 0x1501;

        public static event MouseEventHandler MouseMove;

        public bool PreFilterMessage(ref Message m)
        {
            int a;
            if(m.Msg== EM_SETCUEBANNER)
                 a=2;
           
            if (m.Msg == WM_MOUSEMOVE)
            {
                
                if (MouseMove != null)
                {
                    int x = Control.MousePosition.X, y = Control.MousePosition.Y;

                    MouseMove(null, new MouseEventArgs(MouseButtons.None, 0, x, y, 0));
                }
            }
            return false;
        }
      
    }
}
