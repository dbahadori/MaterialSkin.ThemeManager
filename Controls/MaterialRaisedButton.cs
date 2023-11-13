using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin.Animations;
using System;

namespace MaterialSkin.Controls
{
    public class MaterialRaisedButton : Button, IMaterialControl
    {
        //changed
        [Browsable(false)]
        public int Depth { get; set; }
        public bool EnabledBackColor { get; set; }
        public bool EnabledForeColor { get; set; }

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

        /*  private ContentAlignment _iconposition = ContentAlignment.MiddleCenter;
          public ContentAlignment IconPosition
          {
              get
              {
                  return _iconposition;
              }
              set
              {

                  _iconposition = value;
              }
          }
          public Image DisableIcon { get; set; }
          public Image NormalIcon { get; set; }*/
        public bool Primary { get; set; }

        private readonly AnimationManager _animationManager;

        private SizeF _textSize;

        private Image _icon;
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

        public MaterialRaisedButton()
        {
            Primary = true;

            _animationManager = new AnimationManager(false)
            {
                Increment = 0.03,
                AnimationType = AnimationType.EaseOut
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();

            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            // AutoSize = true;
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                _textSize = CreateGraphics().MeasureString(value.ToUpper(), SkinManager.ROBOTO_MEDIUM_10);
                if (AutoSize)
                    Size = GetPreferredSize();
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            _animationManager.StartNewAnimation(AnimationDirection.In, mevent.Location);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;
            var g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (EnabledBackColor)
                g.Clear(this.BackColor);
            else
                g.Clear(Parent.BackColor);



            using (var backgroundPath = DrawHelper.CreateRoundRect(ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width - 1,
                ClientRectangle.Height - 1,
                1f))
            {
                if (EnabledBackColor)
                    g.FillPath(new SolidBrush(this.BackColor), backgroundPath);
                else
                    g.FillPath(Primary ? SkinManager.GetPrimaryBrush(selectedColorScheme) : SkinManager.GetRaisedButtonBackgroundBrush(), backgroundPath);


            }

            if (_animationManager.IsAnimating())
            {
                for (int i = 0; i < _animationManager.GetAnimationCount(); i++)
                {
                    var animationValue = _animationManager.GetProgress(i);
                    var animationSource = _animationManager.GetSource(i);
                    var rippleBrush = new SolidBrush(Color.FromArgb((int)(51 - (animationValue * 50)), Color.White));
                    var rippleSize = (int)(animationValue * Width * 2);
                    g.FillEllipse(rippleBrush, new Rectangle(animationSource.X - rippleSize / 2, animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                }
            }

            //Icon
            var iconRect = new Rectangle(8, 6, 24, 24);

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
                textRect.Width -= 8 + 24 + 4 + 8;

                // First 8: left padding
                // 24: icon width
                // Second 4: space between Icon and Text
                textRect.X += 8 + 24 + 4;
            }
            if (EnabledForeColor)
            {
                Brush brush = new SolidBrush(this.ForeColor);
                g.DrawString(
                Text.ToUpper(),
                 //SkinManager.ROBOTO_MEDIUM_10,
                 this.Font,
                brush,
                textRect,
                SetAlignment());
            }
            else
            {
                MaterialSkinManager.Themes raistextbrushthem;
                if (SkinManager.Theme == MaterialSkinManager.Themes.CUSTOM)
                    raistextbrushthem = MaterialSkinManager.Themes.CUSTOM;
                else if (Primary) raistextbrushthem = MaterialSkinManager.Themes.DARK;
                else raistextbrushthem = MaterialSkinManager.Themes.LIGHT;

                g.DrawString(
                                Text.ToUpper(),
                                 //SkinManager.ROBOTO_MEDIUM_10,
                                 this.Font,
                                SkinManager.GetRaisedButtonTextBrush(raistextbrushthem),
                                textRect,
                                SetAlignment());
            }


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
    }
}
