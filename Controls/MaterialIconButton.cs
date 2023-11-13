using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin.Animations;
using System.Windows.Forms.VisualStyles;

namespace MaterialSkin.Controls
{
    public class MaterialIconButton : Button, IMaterialControl
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

        [Browsable(true)]
        public Image MouseOverIcon { get; set; }

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

        public MaterialIconButton()
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

        public Rectangle ClickRectangle { get; private set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var state =(int)this.MouseState ;
            // Draw the smaller pressed button image
            if (state == (int)PushButtonState.Pressed)
            {
                // Set the background color to the parent if visual styles  
                // are disabled, because DrawParentBackground will only paint  
                // over the control background if visual styles are enabled.
                this.BackColor = Application.RenderWithVisualStyles ?
                    Color.Transparent : Color.Transparent;

                // If you comment out the call to DrawParentBackground, 
                // the background of the control will still be visible 
                // outside the pressed button, if visual styles are enabled.
                ButtonRenderer.DrawParentBackground(e.Graphics,
                    ClientRectangle, this);
                ButtonRenderer.DrawButton(e.Graphics, ClickRectangle,
                    this.Text, this.Font, true,(PushButtonState) state);
            }

            // Draw the bigger unpressed button image.
            else
            {
                ButtonRenderer.DrawButton(e.Graphics, ClientRectangle,
                    this.Text, this.Font, false,(PushButtonState) state);
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
                _hoverAnimationManager.StartNewAnimation(AnimationDirection.In);
                Icon = MouseOverIcon;
                Invalidate();

            };
            MouseLeave += (sender, args) =>
            {

                MouseState = MouseState.OUT;
                _hoverAnimationManager.StartNewAnimation(AnimationDirection.Out);
                Icon = NormalIcon;

                Invalidate();

            };
            MouseDown += (sender, args) =>
            {

                if (args.Button == MouseButtons.Left)
                {


                    MouseState = MouseState.DOWN;

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
    }
}
