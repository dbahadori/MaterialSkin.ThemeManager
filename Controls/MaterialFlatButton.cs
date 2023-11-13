using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin.Animations;

namespace MaterialSkin.Controls
{
    public class MaterialFlatButton : Button, IMaterialControl 
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
        //changed
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
        private bool enabledAnimation;
        public bool EnabledAnimation { get { return enabledAnimation; } set { enabledAnimation = value; } }
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        [Browsable(true)]
        public Image MouseOverIcon { get; set; }

        private ContentAlignment _iconposition=ContentAlignment.MiddleCenter;
        public ContentAlignment IconPosition {
            get {
                return _iconposition;
            }
            set {
               
                 _iconposition = value;
            }
        }

        [Browsable(true)]
        public Image NormalIcon { get; set; }

       

        [Browsable(true)]
        public Image DisableIcon { get; set; }

        public bool Primary { get; set; }

        private readonly AnimationManager _animationManager;
        private readonly AnimationManager _hoverAnimationManager;

        private SizeF _textSize;

        private Image _icon;
       
        [Browsable(false)]
        public Image Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                if (AutoSize)
                    Size = GetPreferredSize();
                Invalidate();
            }
        }

        public MaterialFlatButton()
        {
            Primary = false;

            _animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            _hoverAnimationManager = new AnimationManager
            {
                Increment = 0.07,
                AnimationType = AnimationType.Linear
            };

            _hoverAnimationManager.OnAnimationProgress += sender => Invalidate();
            _animationManager.OnAnimationProgress += sender => Invalidate();

            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //AutoSize = true;**
            Margin = new Padding(4, 6, 4, 6);
            Padding = new Padding(0);
            if (Enabled)
                Icon = NormalIcon;
            else Icon = DisableIcon;

            enabledAnimation = true;
            ForeColorJoinToTheme = true;
            BackGroundJoinToTheme = true;
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                _textSize = CreateGraphics().MeasureString(value.ToUpper(), this.Font);
                if (AutoSize)
                    Size = GetPreferredSize();
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {

            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;
            var g = pevent.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;


            if (!BackGroundJoinToTheme)
                g.Clear(getColorDisjoinFromTheme(BackGroundColor));
            else
                g.Clear(Parent.BackColor); 

            //Hover
            Color c = SkinManager.GetFlatButtonHoverBackgroundColor();
            using (Brush b = new SolidBrush(Color.FromArgb((int)(_hoverAnimationManager.GetProgress() * c.A), c.RemoveAlpha())))
                g.FillRectangle(b, ClientRectangle);

            //Ripple

            if (_animationManager.IsAnimating())
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                for (var i = 0; i < _animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = _animationManager.GetProgress(i);
                    var animationSource = _animationManager.GetSource(i);

                    using (Brush rippleBrush = new SolidBrush(Color.FromArgb((int)(101 - (animationValue * 100)), Color.Black)))
                    {
                        var rippleSize = (int)(animationValue * Width * 2);
                        g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                    }
                }
                g.SmoothingMode = SmoothingMode.None;
            }

            //Icon
            Rectangle iconRect;
            if (Icon != null)
                iconRect = SetIconPosition(this, Icon, IconPosition);
            else iconRect = new Rectangle((this.Width - 24) - 8, 6, 24, 24);

            if (string.IsNullOrEmpty(Text))
                // Center Icon
                iconRect.X += 2;

            if (Icon != null)
                g.DrawImage(Icon, iconRect);

            //Text
            var textRect = ClientRectangle;

            if (Icon != null)
            {
                //
                // Resize and move Text container
                //

                // First 8: left padding
                // 24: icon width
                // Second 4: space between Icon and Text
                // Third 8: right padding
                //  textRect.Width -= 8 + 24 + 4 + 8;
                if (IconPosition.Equals(ContentAlignment.MiddleLeft)|| IconPosition.Equals(ContentAlignment.TopLeft)|| IconPosition.Equals(ContentAlignment.BottomLeft))
                {
                    textRect.Width -= Icon.Size.Width;
                    textRect.X += Icon.Size.Width;
                }
                if (IconPosition.Equals(ContentAlignment.MiddleRight)|| IconPosition.Equals(ContentAlignment.BottomRight)|| IconPosition.Equals(ContentAlignment.TopRight))
                {
                    textRect.Width -= Icon.Size.Width;
                   
                }


                // First 8: left padding
                // 24: icon width
                // Second 4: space between Icon and Text
                //textRect.X += 8 + 24 + 4;
                //textRect.X += (this.Width - Icon.Size.Width) + Icon.Size.Width + 4;
            }

            /*g.DrawString(
                Text.ToUpper(),
                SkinManager.ROBOTO_MEDIUM_10,
                Enabled ? (Primary ? SkinManager.ColorScheme.PrimaryBrush : SkinManager.GetPrimaryTextBrush()) : SkinManager.GetFlatButtonDisabledTextBrush(),
                textRect,
                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center }
                );*/
            //   if(this.BackgroundImage!=null)
            //  g.DrawImage(this.BackgroundImage, new Point() { X = 0, Y = 0 });

            

            if (!ForeColorJoinToTheme)
            { Brush brush = getBrushDisjoinFromTheme(ForeColor_Color);
                
                g.DrawString(Text.ToUpper(), this.Font, Enabled ? brush : SkinManager.GetFlatButtonDisabledTextBrush(), textRect, SetAlignment());
                    }
            else

                g.DrawString(Text.ToUpper(), this.Font, Enabled ? (Primary ? SkinManager.GetPrimaryBrush(selectedColorScheme) : SkinManager.GetPrimaryTextBrush()) : SkinManager.GetFlatButtonDisabledTextBrush(), ClientRectangle, SetAlignment());
        }
        private StringFormat SetAlignment()
        {
            if (TextAlign.Equals(ContentAlignment.TopRight))
            {
                return new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near };
                
            }
            else if (TextAlign.Equals(ContentAlignment.TopLeft))
            {
                return new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            }

            else if (TextAlign.Equals(ContentAlignment.TopCenter))
            {
                return new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
            }

            else if (TextAlign.Equals(ContentAlignment.BottomLeft))
            {
                return new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Far };
            }
            else if (TextAlign.Equals(ContentAlignment.BottomCenter))
            {
                return new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
            }

            else if (TextAlign.Equals(ContentAlignment.BottomRight))
            {
                return new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };
            }

            else if (TextAlign.Equals(ContentAlignment.MiddleCenter))
            {
                return new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            }
            else if (TextAlign.Equals(ContentAlignment.MiddleLeft))
            {
                return new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            }
            else if (TextAlign.Equals(ContentAlignment.MiddleRight))
            {
                return new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

            }
            return null;
                
        }
        private Rectangle SetIconPosition(Control ctr, Image icon, ContentAlignment iconposition)
        {
            Rectangle position=new Rectangle((this.Width - 24) - 8, 6, 24, 24);

            int x = 0;
            int y = 0;

            //set x
            if (iconposition.Equals(ContentAlignment.TopRight))
            {
                x = (this.Width - Icon.Size.Width) - 3;
                y = 3;
                
                
            }
            else if (iconposition.Equals(ContentAlignment.TopLeft))
            {
                x = 3;
                y = 3;
                
            }

            else if (iconposition.Equals(ContentAlignment.TopCenter))
            {
                x = ((this.Width - Icon.Size.Width) / 2);
                y = 3;
            }

            else if (iconposition.Equals(ContentAlignment.BottomLeft))
            {
                x = 3;
                y = (this.Height - Icon.Size.Height) - 3;
              
            }
            else if (iconposition.Equals(ContentAlignment.BottomCenter))
            {
                x = ((this.Width - Icon.Size.Width) / 2);
                y = (this.Height - Icon.Size.Height) - 3;
            }

            else if (iconposition.Equals(ContentAlignment.BottomRight))
            {
                x = (this.Width - Icon.Size.Width) - 3;
                y = (this.Height - Icon.Size.Height) - 3;
                
            }

            else if (iconposition.Equals(ContentAlignment.MiddleCenter))
            {
                x = ((this.Width - Icon.Size.Width) / 2);
                y = ((this.Height - Icon.Size.Height) / 2);
            }
            else if (iconposition.Equals(ContentAlignment.MiddleLeft))
            {
                x = 3;
                y = ((this.Height - Icon.Size.Height) / 2);
             
            }
            else if (iconposition.Equals(ContentAlignment.MiddleRight))
            {
                x = (this.Width - Icon.Size.Width) - 3;
                y = ((this.Height - Icon.Size.Height) / 2);
                
            }
            //set y



            
                


            position = new Rectangle(x, y, icon.Size.Width, icon.Size.Height);

            return position;
        }

        private Size GetPreferredSize()
        {
            return GetPreferredSize(new Size(0, 0));
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            // Provides extra space for proper padding for content
            var extra = 16;

            if (Icon != null)
                // 24 is for icon size
                // 4 is for the space between icon & text
                extra += 24 + 4;

            return new Size((int)Math.Ceiling(_textSize.Width) + extra, 36);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            if (this.Enabled)
                Icon = NormalIcon;
            else Icon = DisableIcon;

        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
           
            if (Enabled)
                Icon = NormalIcon;
            else Icon = DisableIcon;

            if (DesignMode) return;

            MouseState = MouseState.OUT;
            MouseEnter += (sender, args) =>
            {


                MouseState = MouseState.HOVER;
                if (EnabledAnimation)
                    _hoverAnimationManager.StartNewAnimation(AnimationDirection.In);
                Icon = MouseOverIcon;
                Invalidate();

            };
            MouseLeave += (sender, args) =>
            {

                MouseState = MouseState.OUT;
                if (EnabledAnimation)
                    _hoverAnimationManager.StartNewAnimation(AnimationDirection.Out);
                Icon = NormalIcon;

                Invalidate();

            };
            MouseDown += (sender, args) =>
            {

                if (args.Button == MouseButtons.Left)
                {


                    MouseState = MouseState.DOWN;
                    if (EnabledAnimation)
                        _animationManager.StartNewAnimation(AnimationDirection.In, args.Location);
                    Invalidate();

                }
            };
            MouseUp += (sender, args) =>
            {


                MouseState = MouseState.HOVER;

                Invalidate();

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
