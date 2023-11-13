using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin.Animations;
using System.Runtime.InteropServices;

namespace MaterialSkin.Controls
{
    public class MaterialCheckBox : CheckBox, IMaterialControl
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
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        [Browsable(false)]
        public Point MouseLocation { get; set; }

      

        private ColorScheme selectedColorScheme;
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }

        private bool _ripple;
        [Category("Behavior")]
        public bool Ripple
        {
            get { return _ripple; }
            set
            {
                _ripple = value;
                AutoSize = AutoSize; //Make AutoSize directly set the bounds.

                if (value)
                {
                    Margin = new Padding(0);
                }

                Invalidate();
            }
        }

        private readonly AnimationManager _animationManager;
        private readonly AnimationManager _rippleAnimationManager;

        private const int CHECKBOX_SIZE = 18;
        private const int CHECKBOX_SIZE_HALF = CHECKBOX_SIZE / 2;
        private const int CHECKBOX_INNER_BOX_SIZE = CHECKBOX_SIZE - 4;

        private int _boxOffset;
        private Rectangle _boxRectangle;

        public MaterialCheckBox()
        {
            _animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseInOut,
                Increment = 0.05
            };
            _rippleAnimationManager = new AnimationManager(false)
            {
                AnimationType = AnimationType.Linear,
                Increment = 0.10,
                SecondaryIncrement = 0.08
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();
            _rippleAnimationManager.OnAnimationProgress += sender => Invalidate();

            CheckedChanged += (sender, args) =>
            {
                _animationManager.StartNewAnimation(Checked ? AnimationDirection.In : AnimationDirection.Out);
            };
            
            Ripple = true;
            MouseLocation = new Point(-1, -1);
            backGroundJoinToTheme = true;
            ForeColorJoinToTheme = true;

            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;

        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            _boxOffset = Height / 2 - 9;
            // _boxRectangle = new Rectangle(_boxOffset, _boxOffset, CHECKBOX_SIZE - 1, CHECKBOX_SIZE - 1);*****
            // Finding text width
            Graphics g = this.CreateGraphics();
            SizeF stringSize = g.MeasureString(Text, this.Font);
            
                        // Fixing Offset in RTL
            int boxOffset_rtl = 0;
                        //boxOffset_rtl = (RightToLeft != RightToLeft.Yes) ? boxOffset : (boxOffset + Convert.ToInt32(stringSize.Width)) - (CHECKBOX_SIZE_HALF*2) + TEXT_OFFSET;
            boxOffset_rtl = (RightToLeft != RightToLeft.Yes) ? _boxOffset : ((int)GetPreferredSize(new Size()).Width + TEXT_OFFSET + 22);
            
            _boxRectangle = new Rectangle(this.Width - CHECKBOX_SIZE - 1, _boxOffset, CHECKBOX_SIZE - 1, CHECKBOX_SIZE - 1);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            //var w = _boxOffset + CHECKBOX_SIZE + 2 + (int)CreateGraphics().MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10).Width;*****
            int w = 0;
            w = _boxOffset + CHECKBOX_SIZE + 2 + (int)CreateGraphics().MeasureString(Text, this.Font).Width;
            return Ripple ? new Size(w, 30) : new Size(w, 20);
        }

        private static readonly Point[] CheckmarkLine = { new Point(3, 8), new Point(7, 12), new Point(14, 5) };
        private const int TEXT_OFFSET = 22;
       
       protected override void OnPaint(PaintEventArgs pevent)
        {
         

              //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;

            var  g = pevent.Graphics;
          g.SmoothingMode = SmoothingMode.AntiAlias;
           g.TextRenderingHint = TextRenderingHint.AntiAlias;
            
            SizeF stringSize = g.MeasureString(Text, this.Font);
            int boxOffset_rtl = 0;

            // clear the control


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



            //boxOffset_rtl = (RightToLeft != RightToLeft.Yes) ? boxOffset : (boxOffset + Convert.ToInt32(stringSize.Width)) - (CHECKBOX_SIZE_HALF*2) + TEXT_OFFSET;
            boxOffset_rtl = (RightToLeft != RightToLeft.Yes) ? _boxOffset : (_boxOffset + Convert.ToInt32(stringSize.Width));
            var CHECKBOX_CENTER_X = boxOffset_rtl + CHECKBOX_SIZE_HALF - 1;
            var CHECKBOX_CENTER_Y = _boxOffset + CHECKBOX_SIZE_HALF - 1;

            //var CHECKBOX_CENTER = _boxOffset + CHECKBOX_SIZE_HALF - 1;*****

            var animationProgress = _animationManager.GetProgress();

            var colorAlpha = Enabled ? (int)(animationProgress * 255.0) : SkinManager.GetCheckBoxOffDisabledColor().A;
            var backgroundAlpha = Enabled ? (int)(SkinManager.GetCheckboxOffColor().A * (1.0 - animationProgress)) : SkinManager.GetCheckBoxOffDisabledColor().A;

            var brush = new SolidBrush(Color.FromArgb(colorAlpha, Enabled ? SkinManager.GetAccentColor(selectedColorScheme) : SkinManager.GetCheckBoxOffDisabledColor()));
            var brush3 = new SolidBrush(Enabled ? SkinManager.GetAccentColor(selectedColorScheme) : SkinManager.GetCheckBoxOffDisabledColor());
            var pen = new Pen(brush.Color);

            // draw ripple animation
            if (Ripple && _rippleAnimationManager.IsAnimating())
            {
                for (var i = 0; i < _rippleAnimationManager.GetAnimationCount(); i++)
                {
                    var animationValue = _rippleAnimationManager.GetProgress(i);
                    // var animationSource = new Point(CHECKBOX_CENTER, CHECKBOX_CENTER);*****
                    var animationSource = new Point(CHECKBOX_CENTER_X, CHECKBOX_CENTER_Y);
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

            brush3.Dispose();

            // var checkMarkLineFill = new Rectangle(_boxOffset, _boxOffset, (int)(17.0 * animationProgress), 17);**
            // using (var checkmarkPath = DrawHelper.CreateRoundRect(_boxOffset, _boxOffset, 17, 17, 1f))****
            var checkMarkLineFill = new Rectangle(boxOffset_rtl, _boxOffset, (int)(17.0 * animationProgress), 17);
            using (var checkmarkPath = DrawHelper.CreateRoundRect(boxOffset_rtl, _boxOffset, 17, 17, 1f))
            {
               
              //Color bcolor = Parent.BackColor;
                var brush2 = new SolidBrush(DrawHelper.BlendColor(bcolor, Enabled ? SkinManager.GetCheckboxOffColor() : SkinManager.GetCheckBoxOffDisabledColor(), backgroundAlpha));
                var pen2 = new Pen(brush2.Color);
                //g.FillPath(brush2, checkmarkPath);
                g.DrawPath(pen2, checkmarkPath);

                //g.FillRectangle(new SolidBrush(Parent.BackColor), _boxOffset + 2, _boxOffset + 2, CHECKBOX_INNER_BOX_SIZE - 1, CHECKBOX_INNER_BOX_SIZE - 1);**
                //g.DrawRectangle(new Pen(Parent.BackColor), _boxOffset + 2, _boxOffset + 2, CHECKBOX_INNER_BOX_SIZE - 1, CHECKBOX_INNER_BOX_SIZE - 1);**
                g.FillRectangle(new SolidBrush(bcolor), boxOffset_rtl + 2, boxOffset_rtl + 2, CHECKBOX_INNER_BOX_SIZE - 1, CHECKBOX_INNER_BOX_SIZE - 1);
                g.DrawRectangle(new Pen(bcolor), boxOffset_rtl + 2, boxOffset_rtl + 2, CHECKBOX_INNER_BOX_SIZE - 1, CHECKBOX_INNER_BOX_SIZE - 1);

                brush2.Dispose();
                pen2.Dispose();
               
                if (Enabled)
                { // fill checkbox
                    g.FillPath(brush, checkmarkPath);
                    // border checkbox
                    if (!ForeColorJoinToTheme)
                        pen = new Pen(getColorDisjoinFromTheme(ForeColor_Color));

                        g.DrawPath(pen, checkmarkPath);
                }
                else if (Checked)
                {
                    g.SmoothingMode = SmoothingMode.None;
                    //g.FillRectangle(brush, _boxOffset + 2, _boxOffset + 2, CHECKBOX_INNER_BOX_SIZE, CHECKBOX_INNER_BOX_SIZE);***
                    g.FillRectangle(brush, boxOffset_rtl + 2, _boxOffset + 2, CHECKBOX_INNER_BOX_SIZE, CHECKBOX_INNER_BOX_SIZE);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                }

                g.DrawImageUnscaledAndClipped(DrawCheckMarkBitmap(), checkMarkLineFill);
            }

            // draw checkbox text
            /*SizeF stringSize = g.MeasureString(Text, SkinManager.ROBOTO_MEDIUM_10);
            g.DrawString(
                Text,
                SkinManager.ROBOTO_MEDIUM_10,
                Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush(),
                _boxOffset + TEXT_OFFSET, Height / 2 - stringSize.Height / 2);
                */
            //*******
            Brush textbrush=new SolidBrush(Color.White);
            if (!foreColorJoinToTheme)
            {
                textbrush = getBrushDisjoinFromTheme(ForeColor_Color);
            }
            else
            {
                textbrush = Enabled ? SkinManager.GetPrimaryTextBrush() : SkinManager.GetDisabledOrHintBrush();
            }
                if (this.RightToLeft == RightToLeft.Yes)
            {
                g.DrawString(
                Text,
                this.Font, textbrush
               ,
                0,
                Height / 2 - stringSize.Height / 2
                                 );
                            }
                        else
            {
                g.DrawString(
                Text,
                this.Font,
                textbrush,
                boxOffset_rtl + TEXT_OFFSET, Height / 2 - stringSize.Height / 2);
                            }

            // dispose used paint objects
            
            pen.Dispose();
            brush.Dispose();

            
         
        


    }
       

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            
        }
        private Bitmap DrawCheckMarkBitmap()
        {
            Color bcolor = (!BackGroundJoinToTheme) ? getColorDisjoinFromTheme(BackGroundColor) : Parent.BackColor;
            var checkMark = new Bitmap(CHECKBOX_SIZE, CHECKBOX_SIZE);
            var g = Graphics.FromImage(checkMark);

            // clear everything, transparent
            g.Clear(Color.Transparent);

            // draw the checkmark lines
            using (var pen = new Pen(bcolor, 2))
            {
                g.DrawLines(pen, CheckmarkLine);
            }

            return checkMark;
        }
       


        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;
                if (value)
                {
                    Size = new Size(10, 10);
                }
            }
        }

        private bool IsMouseInCheckArea()
        {
            return _boxRectangle.Contains(MouseLocation);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            //Font = SkinManager.ROBOTO_MEDIUM_10;****

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
