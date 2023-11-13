using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin.Animations;

namespace MaterialSkin.Controls
{
    public class MaterialRadioButton : RadioButton, IMaterialControl
    {
        #region Join To Theme


        //Background
        private bool backGroundJoinToTheme;

        public bool BackGroundJoinToTheme { get { return backGroundJoinToTheme; } set { backGroundJoinToTheme = value; Invalidate(); } }
        public ColorCategory BackGroundColor { get; set; }

        //Forecolor
        private bool foreColorJoinToTheme;

        public bool ForeColorJoinToTheme { get { return foreColorJoinToTheme; } set { foreColorJoinToTheme = value; Invalidate(); } }
        public ColorCategory ForeColor_Color { get; set; }
        

        #endregion

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
        private ColorScheme selectedColorScheme;
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        [Browsable(false)]
        public Point MouseLocation { get; set; }
       // public bool EnabledForeColor { get; set; }
       // public bool EnabledBackColor { get; set; }

        private bool ripple;
        [Category("Behavior")]
        public bool Ripple
        {
            get { return ripple; }
            set
            {
                ripple = value;
                AutoSize = AutoSize; //Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

        // animation managers
        private readonly AnimationManager _animationManager;
        private readonly AnimationManager _rippleAnimationManager;

        // size related variables which should be recalculated onsizechanged
        private Rectangle _radioButtonBounds;
        private int _boxOffset;

        // size constants
        private const int RADIOBUTTON_SIZE = 19;
        private const int RADIOBUTTON_SIZE_HALF = RADIOBUTTON_SIZE / 2;
        private const int RADIOBUTTON_OUTER_CIRCLE_WIDTH = 2;
        private const int RADIOBUTTON_INNER_CIRCLE_SIZE = RADIOBUTTON_SIZE - (2 * RADIOBUTTON_OUTER_CIRCLE_WIDTH);

        public MaterialRadioButton()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

            _animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.06
            };
            _rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();
            _rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged += (sender, args) => _animationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);

            SizeChanged += OnSizeChanged;

            Ripple = true;
            MouseLocation = new Point(-1, -1);

            backGroundJoinToTheme = true;

            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;
        }
        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            int _boxOffset_rtl = 0;
            _boxOffset = Height / 2 - (int)Math.Ceiling(RADIOBUTTON_SIZE / 2d);
            _boxOffset_rtl = (RightToLeft != RightToLeft.Yes) ? _boxOffset : GetPreferredSize(new Size()).Width - (RADIOBUTTON_SIZE);
            //_radioButtonBounds = new Rectangle(_boxOffset, _boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE);
            _radioButtonBounds = new Rectangle(_boxOffset_rtl, _boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            //var width = _boxOffset + 20 + (int)CreateGraphics().MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10).Width;
            int width = _boxOffset + 27 + (int)CreateGraphics().MeasureString(Text, this.Font).Width;
            return Ripple ? new Size(width, 30) : new Size(width, 20);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;

            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;


            int _boxOffset_rtl = 0;
            _boxOffset_rtl = (RightToLeft != RightToLeft.Yes) ? _boxOffset : ((int)GetPreferredSize(new Size()).Width - RADIOBUTTON_OUTER_CIRCLE_WIDTH - 25);

            //// clear the control
            //if (EnabledBackColor)
            //    g.Clear(BackColor);
            //else g.Clear(Parent.BackColor);
            Color bcolor;

            if (!BackGroundJoinToTheme)
            {
                g.Clear(getColorDisjoinFromTheme(BackGroundColor));
                bcolor = getColorDisjoinFromTheme(BackGroundColor);
            }
            else
            { g.Clear(Parent.BackColor); bcolor = Parent.BackColor; }


            //var RADIOBUTTON_CENTER = _boxOffset + RADIOBUTTON_SIZE_HALF; 

            var RADIOBUTTON_CENTER_X = _boxOffset_rtl + RADIOBUTTON_SIZE_HALF;
            var RADIOBUTTON_CENTER_Y = _boxOffset + RADIOBUTTON_SIZE_HALF;

            var animationProgress = _animationManager.GetProgress();

            int colorAlpha = Enabled ? (int)(animationProgress * 255.0) : SkinManager.GetCheckBoxOffDisabledColor().A;
            int backgroundAlpha = Enabled ? (int)(SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress)) : SkinManager.GetCheckBoxOffDisabledColor().A;
            float animationSize = (float)(animationProgress * 8f);
            float animationSizeHalf = animationSize / 2;
            animationSize = (float)(animationProgress * 9f);

            var brush = new SolidBrush(Color.FromArgb(colorAlpha, Enabled ? SkinManager.GetAccentColor(selectedColorScheme) : SkinManager.GetCheckBoxOffDisabledColor()));
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (Ripple && _rippleAnimationManager.IsAnimating())
            {
                for (var i = 0; i < _rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = _rippleAnimationManager.GetProgress(i);
                    //var animationSource = new Point(RADIOBUTTON_CENTER, RADIOBUTTON_CENTER);
                    var animationSource = new Point(RADIOBUTTON_CENTER_X, RADIOBUTTON_CENTER_Y);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)((animationValue * 40)), ((bool)_rippleAnimationManager.GetData(i)[0]) ? Color.Black : brush.Color));
                    var rippleHeight = (Height % 2 == 0) ? Height - 3 : Height - 2;
                    var rippleSize = (_rippleAnimationManager.GetDirection(i) == AnimationDirection.InOutIn) ? (int)(rippleHeight * (0.8d + (0.2d * animationValue))) : rippleHeight;
                    using (var path = DrawHelper.CreateRoundRect(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize, rippleSize / 2))
                    {
                        g.FillPath(rippleBrush, path);
                    }

                    rippleBrush.Dispose();
                }
            }
            
            // draw radiobutton circle
            //if forecolor join to theme use the default related color otherwise use set forecolor
            //Color bcolor2= (EnabledBackColor)?BackColor:Parent.BackColor;

            Color uncheckedColor = DrawHelper.BlendColor(bcolor, Enabled ?(ForeColorJoinToTheme) ?SkinManager.GetCheckboxOffColor(): getColorDisjoinFromTheme(ForeColor_Color) : SkinManager.GetCheckBoxOffDisabledColor(), backgroundAlpha);

            //using (var path = DrawHelper.CreateRoundRect(_boxOffset, _boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE, 9f))
            using (var path = DrawHelper.CreateRoundRect(_boxOffset_rtl, _boxOffset, RADIOBUTTON_SIZE, RADIOBUTTON_SIZE, 9f))
            {
                //arround circle
                g.FillPath(new SolidBrush(uncheckedColor), path);

                if (Enabled)
                {
                    g.FillPath(brush, path);
                }
            }
            
            //inside circle
            g.FillEllipse(
                new SolidBrush(bcolor),
                //RADIOBUTTON_OUTER_CIRCLE_WIDTH + _boxOffset,
                RADIOBUTTON_OUTER_CIRCLE_WIDTH + _boxOffset_rtl,
                RADIOBUTTON_OUTER_CIRCLE_WIDTH + _boxOffset,
                RADIOBUTTON_INNER_CIRCLE_SIZE,
                RADIOBUTTON_INNER_CIRCLE_SIZE);

            if (Checked)
            {
                //using (var path = DrawHelper.CreateRoundRect(RADIOBUTTON_CENTER - animationSizeHalf, RADIOBUTTON_CENTER - animationSizeHalf, animationSize, animationSize, 4f))
                using (var path = DrawHelper.CreateRoundRect(RADIOBUTTON_CENTER_X - animationSizeHalf, RADIOBUTTON_CENTER_Y - animationSizeHalf, animationSize, animationSize, 4f))
                {
                    g.FillPath(brush, path);
                }
            }
            // SizeF stringSize = g.MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10);
            //g.DrawString(Text, SkinManager.ROBOTO_MEDIUM_10, Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(), _boxOffset + 22, Height / 2 - stringSize.Height / 2);
            Brush textbrush = new SolidBrush(Color.White);
            if (!ForeColorJoinToTheme)
            {
                textbrush= getBrushDisjoinFromTheme(ForeColor_Color);
            }
            else
            {
                textbrush = Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush();
            }

            SizeF stringSize = g.MeasureString(Text, this.Font);
            g.DrawString(
            Text,
            this.Font,
            textbrush,
            (this.RightToLeft != RightToLeft.Yes) ? _boxOffset_rtl + 20 : 2,
            Height / 2 - stringSize.Height / 2
                            );
            
            brush.Dispose();
            pen.Dispose();
        }

        private bool IsMouseInCheckArea()
        {
            return _radioButtonBounds.Contains(MouseLocation);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            //Font = SkinManager.ROBOTO_MEDIUM_10;

            if (DesignMode) return;

            MouseState = MouseState.OUT;
            MouseEnter += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
            };
            MouseLeave += (sender, args) =>
            {
                MouseLocation = new Point(-1, -1);
                MouseState = MouseState.OUT;
            };
            MouseDown += (sender, args) =>
            {
                MouseState = MouseState.DOWN;

                if (Ripple && args.Button == MouseButtons.Left && IsMouseInCheckArea())
                {
                    _rippleAnimationManager.SecondaryIncrement = 0;
                    _rippleAnimationManager.StartNewAnimation(AnimationDirection.InOutIn, new object[] { Checked });
                }
            };
            MouseUp += (sender, args) =>
            {
                MouseState = MouseState.HOVER;
                _rippleAnimationManager.SecondaryIncrement = 0.08;
            };
            MouseMove += (sender, args) =>
            {
                MouseLocation = args.Location;
                Cursor = IsMouseInCheckArea() ? Cursors.Hand : Cursors.Default;
            };
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
