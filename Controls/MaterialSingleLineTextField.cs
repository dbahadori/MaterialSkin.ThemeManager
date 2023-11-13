using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MaterialSkin.Animations;


namespace MaterialSkin.Controls
{
    public class MaterialSingleLineTextField : Control, IMaterialControl
    {


        //test change


        //Properties for managing the material design properties
        [Browsable(false)]
        public int Depth { get; set; }
        
        private Size oldSize = new Size();
        private MaterialSkinManager skinmanager = null;
        [Browsable(true)]
        public MaterialSkinManager SkinManager
        {
            get
            {
                
                return (skinmanager!=null)?skinmanager:MaterialSkinManager.Instance;
            }
            set
            {
                skinmanager = value;
            }
        }
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        [Browsable(true)]
        public bool EnabledClearIcon
        {
            get { return enabledClearIcon; }
            set { enabledClearIcon = value; Invalidate(); }
        }

        private ColorScheme selectedColorScheme;
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }

        [Browsable(false)]
        public ObjectSide IconPosition;
        [Browsable(false)]
        public ObjectSide ClearIconPosition;


        public bool EnabledForeColor { get; set; }
        public bool EnabledBackColor { get; set; }
        public bool EnabledUnderLineColor { get; set; }
        public Color UnderLineColor;
        public bool EnabledFont { get; set; }
        private readonly BaseTextBox _baseTextBox;
        private MaterialVisualStyleElement clearbttn;
        private bool enabledClearIcon = true;
        private readonly AnimationManager _animationManager;
        public override bool AutoSize
        {
            get
            {
                return _baseTextBox.AutoSize;
            }

            set
            {
                _baseTextBox.AutoSize = value;
            }
        }
        public override string Text { get { return _baseTextBox.Text; } set { _baseTextBox.Text = value; Invalidate(); } }
        public new object Tag { get { return _baseTextBox.Tag; } set { _baseTextBox.Tag = value; } }
        //public new int MaxLength { get { return _baseTextBox.MaxLength; } set { _baseTextBox.MaxLength = value; } }
        public int MaxLength { get { return _baseTextBox.MaxLength; } set { _baseTextBox.MaxLength = value; } }

        public string SelectedText { get { return _baseTextBox.SelectedText; } set { _baseTextBox.SelectedText = value; } }
        public string Hint { get { return _baseTextBox.WaterMarkText; } set { _baseTextBox.WaterMarkText = value; Invalidate(); } }
        public Color HintColor
        {
            get { return _baseTextBox.WaterMarkColor; }
            set { _baseTextBox.WaterMarkColor = value; Invalidate(); }
        }

        public Image Icon { get { return _baseTextBox.Icon; } set { _baseTextBox.Icon = value; Invalidate(); } }

        public int SelectionStart { get { return _baseTextBox.SelectionStart; } set { _baseTextBox.SelectionStart = value; } }
        public int SelectionLength { get { return _baseTextBox.SelectionLength; } set { _baseTextBox.SelectionLength = value; } }
        public int TextLength
        {
            get
            {
                return _baseTextBox.TextLength;
            }
        }

        public bool UseSystemPasswordChar { get { return _baseTextBox.UseSystemPasswordChar; } set { _baseTextBox.UseSystemPasswordChar = value; Invalidate(); } }
        public char PasswordChar { get { return _baseTextBox.PasswordChar; } set { _baseTextBox.PasswordChar = value; Invalidate(); } }

        public void SelectAll() { _baseTextBox.SelectAll(); }
        public void Clear() { _baseTextBox.Clear(); }
        public void Focus() { _baseTextBox.Focus(); }



        #region Forwarding events to baseTextBox
        public event EventHandler AcceptsTabChanged
        {
            add
            {
                _baseTextBox.AcceptsTabChanged += value;
            }
            remove
            {
                _baseTextBox.AcceptsTabChanged -= value;
            }
        }

        public new event EventHandler AutoSizeChanged
        {
            add
            {
                _baseTextBox.AutoSizeChanged += value;
            }
            remove
            {
                _baseTextBox.AutoSizeChanged -= value;
            }
        }

        public new event EventHandler BackgroundImageChanged
        {
            add
            {
                _baseTextBox.BackgroundImageChanged += value;
            }
            remove
            {
                _baseTextBox.BackgroundImageChanged -= value;
            }
        }

        public new event EventHandler BackgroundImageLayoutChanged
        {
            add
            {
                _baseTextBox.BackgroundImageLayoutChanged += value;
            }
            remove
            {
                _baseTextBox.BackgroundImageLayoutChanged -= value;
            }
        }

        public new event EventHandler BindingContextChanged
        {
            add
            {
                _baseTextBox.BindingContextChanged += value;
            }
            remove
            {
                _baseTextBox.BindingContextChanged -= value;
            }
        }

        public event EventHandler BorderStyleChanged
        {
            add
            {
                _baseTextBox.BorderStyleChanged += value;
            }
            remove
            {
                _baseTextBox.BorderStyleChanged -= value;
            }
        }

        public new event EventHandler CausesValidationChanged
        {
            add
            {
                _baseTextBox.CausesValidationChanged += value;
            }
            remove
            {
                _baseTextBox.CausesValidationChanged -= value;
            }
        }

        public new event UICuesEventHandler ChangeUICues
        {
            add
            {
                _baseTextBox.ChangeUICues += value;
            }
            remove
            {
                _baseTextBox.ChangeUICues -= value;
            }
        }

        public new event EventHandler Click
        {
            add
            {
                _baseTextBox.Click += value;
            }
            remove
            {
                _baseTextBox.Click -= value;
            }
        }

        public new event EventHandler ClientSizeChanged
        {
            add
            {
                _baseTextBox.ClientSizeChanged += value;
            }
            remove
            {
                _baseTextBox.ClientSizeChanged -= value;
            }
        }

        public new event EventHandler ContextMenuChanged
        {
            add
            {
                _baseTextBox.ContextMenuChanged += value;
            }
            remove
            {
                _baseTextBox.ContextMenuChanged -= value;
            }
        }

        public new event EventHandler ContextMenuStripChanged
        {
            add
            {
                _baseTextBox.ContextMenuStripChanged += value;
            }
            remove
            {
                _baseTextBox.ContextMenuStripChanged -= value;
            }
        }

        public new event ControlEventHandler ControlAdded
        {
            add
            {
                _baseTextBox.ControlAdded += value;
            }
            remove
            {
                _baseTextBox.ControlAdded -= value;
            }
        }

        public new event ControlEventHandler ControlRemoved
        {
            add
            {
                _baseTextBox.ControlRemoved += value;
            }
            remove
            {
                _baseTextBox.ControlRemoved -= value;
            }
        }

        public new event EventHandler CursorChanged
        {
            add
            {
                _baseTextBox.CursorChanged += value;
            }
            remove
            {
                _baseTextBox.CursorChanged -= value;
            }
        }

        public new event EventHandler Disposed
        {
            add
            {
                _baseTextBox.Disposed += value;
            }
            remove
            {
                _baseTextBox.Disposed -= value;
            }
        }

        public new event EventHandler DockChanged
        {
            add
            {
                _baseTextBox.DockChanged += value;
            }
            remove
            {
                _baseTextBox.DockChanged -= value;
            }
        }

        public new event EventHandler DoubleClick
        {
            add
            {
                _baseTextBox.DoubleClick += value;
            }
            remove
            {
                _baseTextBox.DoubleClick -= value;
            }
        }

        public new event DragEventHandler DragDrop
        {
            add
            {
                _baseTextBox.DragDrop += value;
            }
            remove
            {
                _baseTextBox.DragDrop -= value;
            }
        }

        public new event DragEventHandler DragEnter
        {
            add
            {
                _baseTextBox.DragEnter += value;
            }
            remove
            {
                _baseTextBox.DragEnter -= value;
            }
        }

        public new event EventHandler DragLeave
        {
            add
            {
                _baseTextBox.DragLeave += value;
            }
            remove
            {
                _baseTextBox.DragLeave -= value;
            }
        }

        public new event DragEventHandler DragOver
        {
            add
            {
                _baseTextBox.DragOver += value;
            }
            remove
            {
                _baseTextBox.DragOver -= value;
            }
        }

        public new event EventHandler EnabledChanged
        {
            add
            {
                _baseTextBox.EnabledChanged += value;
            }
            remove
            {
                _baseTextBox.EnabledChanged -= value;
            }
        }

        public new event EventHandler Enter
        {
            add
            {
                _baseTextBox.Enter += value;
            }
            remove
            {
                _baseTextBox.Enter -= value;
            }
        }

        public new event EventHandler FontChanged
        {
            add
            {
                _baseTextBox.FontChanged += value;
            }
            remove
            {
                _baseTextBox.FontChanged -= value;
            }
        }

        public new event EventHandler ForeColorChanged
        {
            add
            {
                _baseTextBox.ForeColorChanged += value;
            }
            remove
            {
                _baseTextBox.ForeColorChanged -= value;
            }
        }

        public new event GiveFeedbackEventHandler GiveFeedback
        {
            add
            {
                _baseTextBox.GiveFeedback += value;
            }
            remove
            {
                _baseTextBox.GiveFeedback -= value;
            }
        }

        public new event EventHandler GotFocus
        {
            add
            {
                _baseTextBox.GotFocus += value;
            }
            remove
            {
                _baseTextBox.GotFocus -= value;
            }
        }

        public new event EventHandler HandleCreated
        {
            add
            {
                _baseTextBox.HandleCreated += value;
            }
            remove
            {
                _baseTextBox.HandleCreated -= value;
            }
        }

        public new event EventHandler HandleDestroyed
        {
            add
            {
                _baseTextBox.HandleDestroyed += value;
            }
            remove
            {
                _baseTextBox.HandleDestroyed -= value;
            }
        }

        public new event HelpEventHandler HelpRequested
        {
            add
            {
                _baseTextBox.HelpRequested += value;
            }
            remove
            {
                _baseTextBox.HelpRequested -= value;
            }
        }

        public event EventHandler HideSelectionChanged
        {
            add
            {
                _baseTextBox.HideSelectionChanged += value;
            }
            remove
            {
                _baseTextBox.HideSelectionChanged -= value;
            }
        }

        public new event EventHandler ImeModeChanged
        {
            add
            {
                _baseTextBox.ImeModeChanged += value;
            }
            remove
            {
                _baseTextBox.ImeModeChanged -= value;
            }
        }

        public new event InvalidateEventHandler Invalidated
        {
            add
            {
                _baseTextBox.Invalidated += value;
            }
            remove
            {
                _baseTextBox.Invalidated -= value;
            }
        }

        public new event KeyEventHandler KeyDown
        {
            add
            {
                _baseTextBox.KeyDown += value;
            }
            remove
            {
                _baseTextBox.KeyDown -= value;
            }
        }

        public new event KeyPressEventHandler KeyPress
        {
            add
            {
                _baseTextBox.KeyPress += value;
            }
            remove
            {
                _baseTextBox.KeyPress -= value;
            }
        }

        public new event KeyEventHandler KeyUp
        {
            add
            {
                _baseTextBox.KeyUp += value;
            }
            remove
            {
                _baseTextBox.KeyUp -= value;
            }
        }

        public new event LayoutEventHandler Layout
        {
            add
            {
                _baseTextBox.Layout += value;
            }
            remove
            {
                _baseTextBox.Layout -= value;
            }
        }

        public new event EventHandler Leave
        {
            add
            {
                _baseTextBox.Leave += value;
            }
            remove
            {
                _baseTextBox.Leave -= value;
            }
        }

        public new event EventHandler LocationChanged
        {
            add
            {
                _baseTextBox.LocationChanged += value;
            }
            remove
            {
                _baseTextBox.LocationChanged -= value;
            }
        }

        public new event EventHandler LostFocus
        {
            add
            {
                _baseTextBox.LostFocus += value;
            }
            remove
            {
                _baseTextBox.LostFocus -= value;
            }
        }

        public new event EventHandler MarginChanged
        {
            add
            {
                _baseTextBox.MarginChanged += value;
            }
            remove
            {
                _baseTextBox.MarginChanged -= value;
            }
        }

        public event EventHandler ModifiedChanged
        {
            add
            {
                _baseTextBox.ModifiedChanged += value;
            }
            remove
            {
                _baseTextBox.ModifiedChanged -= value;
            }
        }

        public new event EventHandler MouseCaptureChanged
        {
            add
            {
                _baseTextBox.MouseCaptureChanged += value;
            }
            remove
            {
                _baseTextBox.MouseCaptureChanged -= value;
            }
        }

        public new event MouseEventHandler MouseClick
        {
            add
            {
                _baseTextBox.MouseClick += value;
            }
            remove
            {
                _baseTextBox.MouseClick -= value;
            }
        }

        public new event MouseEventHandler MouseDoubleClick
        {
            add
            {
                _baseTextBox.MouseDoubleClick += value;
            }
            remove
            {
                _baseTextBox.MouseDoubleClick -= value;
            }
        }

        public new event MouseEventHandler MouseDown
        {
            add
            {
                _baseTextBox.MouseDown += value;
            }
            remove
            {
                _baseTextBox.MouseDown -= value;
            }
        }

        public new event EventHandler MouseEnter
        {
            add
            {
                _baseTextBox.MouseEnter += value;
            }
            remove
            {
                _baseTextBox.MouseEnter -= value;
            }
        }

        public new event EventHandler MouseHover
        {
            add
            {
                _baseTextBox.MouseHover += value;
            }
            remove
            {
                _baseTextBox.MouseHover -= value;
            }
        }

        public new event EventHandler MouseLeave
        {
            add
            {
                _baseTextBox.MouseLeave += value;
            }
            remove
            {
                _baseTextBox.MouseLeave -= value;
            }
        }

        public new event MouseEventHandler MouseMove
        {
            add
            {
                _baseTextBox.MouseMove += value;
            }
            remove
            {
                _baseTextBox.MouseMove -= value;
            }
        }

        public new event MouseEventHandler MouseUp
        {
            add
            {
                _baseTextBox.MouseUp += value;
            }
            remove
            {
                _baseTextBox.MouseUp -= value;
            }
        }

        public new event MouseEventHandler MouseWheel
        {
            add
            {
                _baseTextBox.MouseWheel += value;
            }
            remove
            {
                _baseTextBox.MouseWheel -= value;
            }
        }

        public new event EventHandler Move
        {
            add
            {
                _baseTextBox.Move += value;
            }
            remove
            {
                _baseTextBox.Move -= value;
            }
        }

        public event EventHandler MultilineChanged
        {
            add
            {
                _baseTextBox.MultilineChanged += value;
            }
            remove
            {
                _baseTextBox.MultilineChanged -= value;
            }
        }

        public new event EventHandler PaddingChanged
        {
            add
            {
                _baseTextBox.PaddingChanged += value;
            }
            remove
            {
                _baseTextBox.PaddingChanged -= value;
            }
        }

        public new event PaintEventHandler Paint
        {
            add
            {
                _baseTextBox.Paint += value;
            }
            remove
            {
                _baseTextBox.Paint -= value;
            }
        }

        public new event EventHandler ParentChanged
        {
            add
            {
                _baseTextBox.ParentChanged += value;
            }
            remove
            {
                _baseTextBox.ParentChanged -= value;
            }
        }

        public new event PreviewKeyDownEventHandler PreviewKeyDown
        {
            add
            {
                _baseTextBox.PreviewKeyDown += value;
            }
            remove
            {
                _baseTextBox.PreviewKeyDown -= value;
            }
        }

        public new event QueryAccessibilityHelpEventHandler QueryAccessibilityHelp
        {
            add
            {
                _baseTextBox.QueryAccessibilityHelp += value;
            }
            remove
            {
                _baseTextBox.QueryAccessibilityHelp -= value;
            }
        }

        public new event QueryContinueDragEventHandler QueryContinueDrag
        {
            add
            {
                _baseTextBox.QueryContinueDrag += value;
            }
            remove
            {
                _baseTextBox.QueryContinueDrag -= value;
            }
        }

        public event EventHandler ReadOnlyChanged
        {
            add
            {
                _baseTextBox.ReadOnlyChanged += value;
            }
            remove
            {
                _baseTextBox.ReadOnlyChanged -= value;
            }
        }

        public new event EventHandler RegionChanged
        {
            add
            {
                _baseTextBox.RegionChanged += value;
            }
            remove
            {
                _baseTextBox.RegionChanged -= value;
            }
        }

        public new event EventHandler Resize
        {
            add
            {
                _baseTextBox.Resize += value;
            }
            remove
            {
                _baseTextBox.Resize -= value;
            }
        }

        public new event EventHandler RightToLeftChanged
        {
            add
            {
                _baseTextBox.RightToLeftChanged += value;
            }
            remove
            {
                _baseTextBox.RightToLeftChanged -= value;
            }
        }

        public new event EventHandler SizeChanged
        {
            add
            {
                _baseTextBox.SizeChanged += value;
            }
            remove
            {
                _baseTextBox.SizeChanged -= value;
            }
        }

        public new event EventHandler StyleChanged
        {
            add
            {
                _baseTextBox.StyleChanged += value;
            }
            remove
            {
                _baseTextBox.StyleChanged -= value;
            }
        }

        public new event EventHandler SystemColorsChanged
        {
            add
            {
                _baseTextBox.SystemColorsChanged += value;
            }
            remove
            {
                _baseTextBox.SystemColorsChanged -= value;
            }
        }

        public new event EventHandler TabIndexChanged
        {
            add
            {
                _baseTextBox.TabIndexChanged += value;
            }
            remove
            {
                _baseTextBox.TabIndexChanged -= value;
            }
        }

        public new event EventHandler TabStopChanged
        {
            add
            {
                _baseTextBox.TabStopChanged += value;
            }
            remove
            {
                _baseTextBox.TabStopChanged -= value;
            }
        }

        public event EventHandler TextAlignChanged
        {
            add
            {
                _baseTextBox.TextAlignChanged += value;
            }
            remove
            {
                _baseTextBox.TextAlignChanged -= value;
            }
        }

        public new event EventHandler TextChanged
        {
            add
            {
                _baseTextBox.TextChanged += value;
            }
            remove
            {
                _baseTextBox.TextChanged -= value;
            }
        }

        public new event EventHandler Validated
        {
            add
            {
                _baseTextBox.Validated += value;
            }
            remove
            {
                _baseTextBox.Validated -= value;
            }
        }

        public new event CancelEventHandler Validating
        {
            add
            {
                _baseTextBox.Validating += value;
            }
            remove
            {
                _baseTextBox.Validating -= value;
            }
        }

        public new event EventHandler VisibleChanged
        {
            add
            {
                _baseTextBox.VisibleChanged += value;
            }
            remove
            {
                _baseTextBox.VisibleChanged -= value;
            }
        }
        #endregion

        
        public MaterialSingleLineTextField()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);
            IconPosition = (this.RightToLeft == RightToLeft.Yes) ? ObjectSide.Right : ObjectSide.Left;
            ClearIconPosition = (this.RightToLeft == RightToLeft.No) ? ObjectSide.Right : ObjectSide.Left;
            _animationManager = new AnimationManager
            {
                Increment = 0.06,
                AnimationType = AnimationType.EaseInOut,
                InterruptAnimation = false
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();

            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;

            _baseTextBox = new BaseTextBox()
            {
                BorderStyle = BorderStyle.None,
                //Font = SkinManager.ROBOTO_REGULAR_11,
                Font = Font,

                ForeColor = SkinManager.GetPrimaryTextColor(),

                Location = new Point(0, 0),
                Width = Width,
                Height = Height - 5

            };
            oldSize = _baseTextBox.Size;
            if (!Controls.Contains(_baseTextBox) && !DesignMode)
            {
                Controls.Add(_baseTextBox);
            }

            //Fix for tabstop
            _baseTextBox.TabStop = true;
            this.TabStop = false;

            clearbttn = new MaterialSkin.Controls.MaterialVisualStyleElement();
            clearbttn.ElementName = "ToolTip.Close.Normal";
            clearbttn.Size=new Size(15, 15);
            JoinEvents(true);
            EnabledForeColor = false;
            EnabledBackColor = false;
            EnabledUnderLineColor = false;
        }
        private void JoinEvents(Boolean join)
        {

            if (join)
            {

                FontChanged += (sendr, args) =>
                {
                    if (!EnabledBackColor) { _baseTextBox.BackColor = BackColor; this.BackColor = BackColor; }
                    _baseTextBox.ForeColor = (EnabledForeColor) ? ForeColor : SkinManager.GetPrimaryTextColor();

                };
                BackColorChanged += (sender, args) =>
                {

                    if (!EnabledBackColor) { _baseTextBox.BackColor = BackColor; this.BackColor = BackColor; };

                    _baseTextBox.ForeColor = (EnabledForeColor) ? ForeColor : SkinManager.GetPrimaryTextColor();



                };
                RightToLeftChanged += (sender, args) =>
                {

                    if (this.RightToLeft.Equals(RightToLeft.No))
                    {
                        IconPosition = ObjectSide.Left;
                        ClearIconPosition = ObjectSide.Right;

                    }
                    else
                    {
                        IconPosition = ObjectSide.Right;
                        ClearIconPosition = ObjectSide.Left;

                    }
                    
                    Invalidate();
                };
                _baseTextBox.GotFocus += (sender, args) => _animationManager.StartNewAnimation(AnimationDirection.In);
                _baseTextBox.LostFocus += (sender, args) => _animationManager.StartNewAnimation(AnimationDirection.Out);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {


            base.OnPaint(pevent);
            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;

            var g = pevent.Graphics;
            if(EnabledBackColor)
                g.Clear(BackColor);
            else
            g.Clear(Parent.BackColor);

           
            _baseTextBox.BackColor = (EnabledBackColor) ? BackColor : Parent.BackColor;
            this.BackColor = (EnabledBackColor) ? BackColor : Parent.BackColor;
            _baseTextBox.ForeColor = (EnabledForeColor) ? ForeColor : SkinManager.GetPrimaryTextColor();
            _baseTextBox.Font = (EnabledFont) ? Font : Parent.Font;

            //draw animation
            DrawAnimation(g);

            //set icon 
            DrawIcon(g);

            //set clear button 
            DrawClearButton(clearbttn);

            //set text field height
            this.Size = new Size(this.Size.Width, _baseTextBox.Bottom + 5); 
        }
        private void DrawAnimation(Graphics g)
        {
            // position and width of animation
            var lineY = _baseTextBox.Bottom + 4;
            var lineX = 3;
            var width = (enabledClearIcon) ? (Icon != null) ? _baseTextBox.Width + clearbttn.Width + Icon.Width : _baseTextBox.Width + clearbttn.Width : _baseTextBox.Width;
            if (!_animationManager.IsAnimating())
            {

                //No animation
                if (EnabledUnderLineColor)
                    g.FillRectangle(_baseTextBox.Focused ? SkinManager.GetAccentBrush(selectedColorScheme) :new SolidBrush(UnderLineColor), lineX, lineY, width, _baseTextBox.Focused ? 2 : 1);
                else
                    g.FillRectangle(_baseTextBox.Focused ? SkinManager.GetAccentBrush(selectedColorScheme) : SkinManager.GetDividersBrush(), lineX, lineY, width, _baseTextBox.Focused ? 2 : 1);
            }
            else
            {
                //Animate
                int animationWidth = (int)(width * _animationManager.GetProgress());
                int halfAnimationWidth = animationWidth / 2;
                int animationStart = lineX + width / 2;

                //Unfocused background
                if (EnabledUnderLineColor)
                    g.FillRectangle(new SolidBrush(UnderLineColor), lineX, lineY, width, 1);
                else
                    g.FillRectangle(SkinManager.GetDividersBrush(), lineX, lineY, width, 1);

                //Animated focus transition
                g.FillRectangle(SkinManager.GetAccentBrush(selectedColorScheme), animationStart - halfAnimationWidth, lineY, animationWidth, 2);
            }
        }
        private void DrawIcon(Graphics graphic)
        {

            Rectangle iconRect;
            if (this.Icon != null)
            {
                if (IconPosition.Equals(ObjectSide.Left))
                    iconRect = new Rectangle(0, (this.Height-Icon.Height)/2, Icon.Size.Width, Icon.Size.Height);
                else
                    iconRect = new Rectangle((this.Width - Icon.Size.Width) , (this.Height - Icon.Height) / 2, Icon.Size.Width, Icon.Size.Height);

                graphic.DrawImage(Icon, iconRect);
            }

           

        }
        
        private void  DrawClearButton(MaterialVisualStyleElement cbttn)
        {

            if (enabledClearIcon) {
                cbttn.Click += (sender, e) =>
                {
                    this.Text = "";
                    this.Focus();
                };
                this.Controls.Add(cbttn);

                cbttn.Show();
                cbttn.Location = (ClearIconPosition.Equals(ObjectSide.Left)) ? new Point(0, (this.Height - clearbttn.Height) / 2) : new Point(this.Width - cbttn.Width, (this.Height - clearbttn.Height) / 2);


                cbttn.BringToFront();
            }

                



        }

        //change base text box location based on icon
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var txtWidth = Width + 5;
            var txtLocation=_baseTextBox.Location;
            var iconWidthUtility = 0;
            var clearIconWidthUtility = 0;

            if (Icon != null)
                iconWidthUtility = Icon.Width;
            if (enabledClearIcon)
                clearIconWidthUtility = clearbttn.Width;


                if (EnabledClearIcon)
                {
                    txtWidth = Width - (iconWidthUtility + clearIconWidthUtility + 5);
                }
                else
                {
                    txtWidth = Width - (iconWidthUtility + 5);
                }
                if (IconPosition.Equals(ObjectSide.Left))
                {
                    txtLocation = new Point(iconWidthUtility + 5, 0);
                }
                else
                {
                    txtLocation = new Point(clearIconWidthUtility + 5, 0);
                }

            _baseTextBox.Width = txtWidth-2;
            _baseTextBox.Location = txtLocation;

        }

      

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            //    _baseTextBox.Font = Font;


            _baseTextBox.Font = (EnabledFont) ? Font : Parent.Font;
            _baseTextBox.BackColor = (EnabledBackColor) ? BackColor : Parent.BackColor;
            this.BackColor = (EnabledBackColor) ? BackColor : Parent.BackColor;
            _baseTextBox.ForeColor = (EnabledForeColor) ? ForeColor : SkinManager.GetPrimaryTextColor();
            
        }

        private class BaseTextBox : TextBox
        {

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr PostMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);
            [DllImport("user32", CharSet = CharSet.Auto)]
            private extern static IntPtr FindWindowEx(
                IntPtr hwndParent,
                IntPtr hwndChildAfter,
                [MarshalAs(UnmanagedType.LPTStr)]
            string lpszClass,
                [MarshalAs(UnmanagedType.LPTStr)]
            string lpszWindow);
            [DllImport("user32", CharSet = CharSet.Auto)]
            private extern static int GetWindowLong(
                IntPtr hWnd,
                int dwStyle);
            [DllImport("user32")]
            private extern static IntPtr GetDC(
                IntPtr hwnd);
            private const int EM_SETCUEBANNER = 0x1501;
            private const char EmptyChar = (char)0;
            private const char VisualStylePasswordChar = '\u25CF';
            private const char NonVisualStylePasswordChar = '\u002A';
            private const int GWL_EXSTYLE = (-20);
            private const int WS_EX_RIGHT = 0x00001000;
            private const int WS_EX_LEFT = 0x00000000;
            private const int WS_EX_RTLREADING = 0x00002000;
            private const int WS_EX_LTRREADING = 0x00000000;
            private const int WS_EX_LEFTSCROLLBAR = 0x00004000;
            private const int WS_EX_RIGHTSCROLLBAR = 0x00000000;
            private const int EC_LEFTMARGIN = 0x1;
            private const int EC_RIGHTMARGIN = 0x2;
            private const int EC_USEFONTINFO = 0xFFFF;
            private const int EM_SETMARGINS = 0xD3;
            private const int EM_GETMARGINS = 0xD4;


            private string hint = string.Empty;
            private Image icon = null;
            private ObjectSide iconposition;
            

            public string Hint
            {
                get { return hint; }
                set
                {
                    hint = value;
                    //IntPtr _hintIntptr = Marshal.StringToHGlobalUni(Hint);
                    //SendMessage(Handle, EM_SETCUEBANNER, IntPtr.Zero, _hintIntptr);
                }
            }


            //watermarker
        

            #region Attributes
            private Color _waterMarkColor = Color.Gray;


            public Color WaterMarkColor
            {

                get { return _waterMarkColor; }
                set
                {
                    _waterMarkColor = value; Invalidate();/*thanks to Bernhard Elbl
for Invalidate()*/
                }
            }

            private string _waterMarkText = "";
            public string WaterMarkText
            {

                get { return _waterMarkText; }
                set { _waterMarkText = value; Invalidate(); }
            }
            #endregion


            public Size sizeDiff = new Size(0,0);
            private Size oldSize = new Size (0,0);
            public Image Icon
            {
                get { return icon; }
                set
                {
                    icon = value;

                }
            }
            public ObjectSide IconPosition
            {
                get
                {
                    return iconposition;
                }
                set
                {
                    iconposition = value;
                    // if(icon!=null) SetMargin(this, icon.Size.Width, IconPosition.Equals(ObjectSide.Left) ? MarginSide.Left : MarginSide.Right);

                }
            }
            private char _passwordChar = EmptyChar;
            public new char PasswordChar
            {
                get { return _passwordChar; }
                set
                {
                    _passwordChar = value;
                    SetBasePasswordChar();
                }
            }
            public new void SelectAll()
            {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    base.Focus();
                    base.SelectAll();
                });
            }

            public new void Focus()
            {
                BeginInvoke((MethodInvoker)delegate ()
                {
                    base.Focus();
                });
            }
            
            private char _useSystemPasswordChar = EmptyChar;
            public new bool UseSystemPasswordChar
            {
                get { return _useSystemPasswordChar != EmptyChar; }
                set
                {
                    if (value)
                    {
                        _useSystemPasswordChar = Application.RenderWithVisualStyles ? VisualStylePasswordChar : NonVisualStylePasswordChar;
                    }
                    else
                    {
                        _useSystemPasswordChar = EmptyChar;
                    }

                    SetBasePasswordChar();
                }
            }

            private void SetBasePasswordChar()
            {
                base.PasswordChar = UseSystemPasswordChar ? _useSystemPasswordChar : _passwordChar;
            }
            // private TextBoxMarginCustomise customPaint;
            public BaseTextBox()
            {
                oldSize = this.Size;
               
                MaterialContextMenuStrip cms = new TextBoxContextMenuStrip();
                cms.Opening += ContextMenuStripOnOpening;
                cms.OnItemClickStart += ContextMenuStripOnItemClickStart;
                ContextMenuStrip = cms;
              //  JoinEvents(true);


            }

            //private void JoinEvents(Boolean join)
            //{

            //    if (join)
            //    {

            //        this.FontChanged += (sender, e) => { MessageBox.Show("base textbox font changed : old size is: "+oldSize+ "  and new size is" + Size.ToString());
            //            sizeDiff= this.Size - oldSize;
            //            oldSize = this.Size;
            //            Invalidate();
                      
            //        };
            //    }
            //}

            private void ContextMenuStripOnItemClickStart(object sender, ToolStripItemClickedEventArgs toolStripItemClickedEventArgs)
            {
                switch (toolStripItemClickedEventArgs.ClickedItem.Text)
                {
                    case "Undo":
                        Undo();
                        break;
                    case "Cut":
                        Cut();
                        break;
                    case "Copy":
                        Copy();
                        break;
                    case "Paste":
                        Paste();
                        break;
                    case "Delete":
                        SelectedText = string.Empty;
                        break;
                    case "Select All":
                        SelectAll();
                        break;
                }
            }

            private void ContextMenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
            {
                var strip = sender as TextBoxContextMenuStrip;
                if (strip != null)
                {
                    strip.Undo.Enabled = CanUndo;
                    strip.Cut.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.Copy.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.Paste.Enabled = Clipboard.ContainsText();
                    strip.Delete.Enabled = !string.IsNullOrEmpty(SelectedText);
                    strip.SelectAll.Enabled = !string.IsNullOrEmpty(Text);
                }
            }

            //Override OnPaint
            protected override void OnPaint(PaintEventArgs args)
            {
                base.OnPaint(args);
                
                if (string.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.WaterMarkText))
                {
                    args.Graphics.Clear(BackColor);
                    // Use the same font that was defined in base class
                    System.Drawing.Font drawFont = new System.Drawing.Font(Parent.Font.FontFamily,
                    Parent.Font.Size, Parent.Font.Style, Parent.Font.Unit);

                    //Create new brush with gray color or
                    SolidBrush drawBrush = new SolidBrush(WaterMarkColor);//use Water mark color

                    var x = this.Width - 5;
                    var stringformat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };


                    // if textbox is left to right
                    if (this.RightToLeft == RightToLeft.No)
                    {
                        x = 5;
                        stringformat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
                    }

                    //Draw Text or WaterMark
                    args.Graphics.DrawString(WaterMarkText,
                    drawFont, drawBrush, x, 0, stringformat
                    );
                }
            }

            const int WM_PAINT = 0x000F;
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == WM_PAINT)
                {
                    this.OnPaint(new PaintEventArgs(Graphics.FromHwnd(m.HWnd), this.ClientRectangle));
                }
            }
        }

        private class TextBoxContextMenuStrip : MaterialContextMenuStrip
        {
            public readonly ToolStripItem Undo = new MaterialToolStripMenuItem { Text = "Undo" };
            public readonly ToolStripItem Seperator1 = new ToolStripSeparator();
            public readonly ToolStripItem Cut = new MaterialToolStripMenuItem { Text = "Cut" };
            public readonly ToolStripItem Copy = new MaterialToolStripMenuItem { Text = "Copy" };
            public readonly ToolStripItem Paste = new MaterialToolStripMenuItem { Text = "Paste" };
            public readonly ToolStripItem Delete = new MaterialToolStripMenuItem { Text = "Delete" };
            public readonly ToolStripItem Seperator2 = new ToolStripSeparator();
            public readonly ToolStripItem SelectAll = new MaterialToolStripMenuItem { Text = "Select All" };

            public TextBoxContextMenuStrip()
            {
                Items.AddRange(new[]
                {
                    Undo,
                    Seperator1,
                    Cut,
                    Copy,
                    Paste,
                    Delete,
                    Seperator2,
                    SelectAll
                });
            }
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
}
