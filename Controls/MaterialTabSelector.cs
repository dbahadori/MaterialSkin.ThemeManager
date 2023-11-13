using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin.Animations;
using System.IO;
using System.Reflection;
using MaterialSkin.Properties;

namespace MaterialSkin.Controls
{
    public class MaterialTabSelector : Control, IMaterialControl
    {
        #region Join To Theme


        //selected tab Background
        private bool selectedTabBackGroundJoinToTheme;

        public bool SelectedTabBackGroundJoinToTheme { get { return selectedTabBackGroundJoinToTheme; } set { selectedTabBackGroundJoinToTheme = value;} }
        public ColorCategory SelectedTabBackGroundColor { get; set; }


        //selected tab Forecolor
        private bool selectedTabForeColorJoinToTheme;

        public bool SelectedTabForeColorJoinToTheme { get { return selectedTabForeColorJoinToTheme; } set { selectedTabForeColorJoinToTheme = value;  } }
        public ColorCategory SelectedTabForeColor_Color { get; set; }

        //selected tab Background
        private bool tabBackGroundJoinToTheme;

        public bool TabBackGroundJoinToTheme { get { return tabBackGroundJoinToTheme; } set { tabBackGroundJoinToTheme = value; } }
        public ColorCategory TabBackGroundColor { get; set; }


        //Forecolor
        private bool tabForeColorJoinToTheme;

        public bool TabForeColorJoinToTheme { get { return tabForeColorJoinToTheme; } set { tabForeColorJoinToTheme = value;  } }
        public ColorCategory TabForeColor_Color { get; set; }

        // Background
        private bool GroundJoinToTheme;

        public bool BackGroundJoinToTheme { get { return GroundJoinToTheme; } set { GroundJoinToTheme = value;  } }
        public ColorCategory BackGroundColor { get; set; }


       
        #endregion


        private Image image;
        private MaterialVisualStyleElement closeBttn;
        [Browsable(false)]
        public int Depth { get; set; }
        private MaterialSkinManager skinmanager = null;
        public event PaintEventHandler UserPaint;

        protected void OnUserPaint(PaintEventArgs e)
        {
            PaintEventHandler handler = UserPaint;
            if (handler != null)
                handler(this, e);
        }
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
        //public bool enabledTabIcon { get; set; }
        private bool newTabVisible;
        public bool NewTabVisible { get { return newTabVisible; } set { newTabVisible = value; Invalidate(); } }



        public event EventHandler TabCloseClick;
        public event EventHandler AddNewTab;
        public event EventHandler TabClick;
        private ColorScheme selectedColorScheme;
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }

        [Browsable(false)]
        public MouseState MouseState { get; set; }

        private MaterialTabControl _baseTabControl;
        private Image addIcon ;
        public MaterialTabControl BaseTabControl
        {
           
            get { return _baseTabControl; }
            set
            {
                _baseTabControl = value;
                if (_baseTabControl == null) return;
                _previousSelectedTabIndex = _baseTabControl.SelectedIndex;
                _baseTabControl.Deselected += (sender, args) =>
                {
                    _previousSelectedTabIndex = _baseTabControl.SelectedIndex;
                };
                _baseTabControl.SelectedIndexChanged += (sender, args) =>
                {
                    _animationManager.SetProgress(0);
                    _animationManager.StartNewAnimation(AnimationDirection.In);


                };
                _baseTabControl.ControlAdded += delegate
                {
                    Invalidate();
                };
                _baseTabControl.ControlRemoved += delegate
                {
                    Invalidate();
                };

                _baseTabControl.MaterialTabPages.changeTabPagesEvent += (sender, args) =>
                    {
                        UpdateTabRects();
                        _baseTabControl.SelectedIndex = 0;
                        Invalidate();
                    };
            }
        }

        private int _previousSelectedTabIndex;
        private Point _animationSource;
        private readonly AnimationManager _animationManager;

        private List<Rectangle> _tabRects;
        private List<Rectangle> _tabCloseRects;
        private const int TAB_HEADER_PADDING = 24;
        private const int TAB_INDICATOR_HEIGHT = 2;
        public int b = TAB_INDICATOR_HEIGHT;
        ColorScheme selectorControltabPageColorScheme;
        public MaterialTabSelector()
        {

            addIcon = (this.SkinManager.Theme==MaterialSkinManager.Themes.DARK)? Resources.plus_light: Resources.plus_dark;
            //_baseTabControl.MaterialTabPages.
            if (SelectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;

            closeBttn = new MaterialVisualStyleElement() { ElementName = "Window.MdiCloseButton.Normal" };
            image = Resources.clear;

            image = resizeImage(image, new Size(15, 15));
            //custom code
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
            //end of custom code
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            //Height = 48;
            _animationManager = new AnimationManager
            {
                AnimationType = AnimationType.EaseOut,
                //Increment = 0.04
                Increment = 5
            };
            _animationManager.OnAnimationProgress += sender => Invalidate();
            BackGroundJoinToTheme = true;
            SelectedTabBackGroundJoinToTheme = true;
            SelectedTabForeColorJoinToTheme = true;
            TabBackGroundJoinToTheme = true;
            TabForeColorJoinToTheme = true;

          


        }
        //protected override void OnCreateControl()
        //{
        //    base.OnCreateControl();
        //    closeBttn.Location = new Point(_tabRects[0].X, _tabRects[0].Y);
        //    this.Controls.Add(closeBttn);


        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            addIcon = (this.SkinManager.Theme == MaterialSkinManager.Themes.DARK) ? Resources.plus_light : Resources.plus_dark;
            //if the local color scheme does not inherit from base color scheme
            if (SelectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;
            _tabCloseRects = new List<Rectangle>();
            var g = e.Graphics;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (!BackGroundJoinToTheme)
                g.Clear(getColorDisjoinFromTheme(BackGroundColor));
            else
                g.Clear(SkinManager.GetDarkPrimaryColor(SelectedColorScheme));



            if (_baseTabControl == null) return;


            if (!_animationManager.IsAnimating() || _tabRects == null || _tabRects.Count != _baseTabControl.TabCount)
                UpdateTabRects();

            var animationProgress = _animationManager.GetProgress();




            //Draw new tab header when materialtabpages is empty 
            if (_baseTabControl.MaterialTabPages.Count <= 0)
            {
                if (NewTabVisible)
                {
                    Brush textBrush2 = new SolidBrush(Color.FromArgb(CalculateTextAlpha(), SkinManager.ColorScheme.TextColor));
                    DrawNewTabRect(g, textBrush2);
                }

                return;
            }
            else
            {

               //fill selected tab page
               if(!SelectedTabBackGroundJoinToTheme)
                    g.FillRectangle(getBrushDisjoinFromTheme(SelectedTabBackGroundColor), _tabRects[_baseTabControl.SelectedIndex]);
                else
                    g.FillRectangle(SkinManager.GetLightPrimaryBrush(SelectedColorScheme), _tabRects[_baseTabControl.SelectedIndex]);
            }

            //Click feedback
            if (_animationManager.IsAnimating())        
            {
                var rippleBrush =  new SolidBrush(Color.FromArgb((int)(51 - (animationProgress * 50)), Color.White));
                var rippleSize = (int)(animationProgress * _tabRects[_baseTabControl.SelectedIndex].Width * 1.75);

              

                g.SetClip(_tabRects[_baseTabControl.SelectedIndex]);
                g.FillEllipse(rippleBrush, new Rectangle(_animationSource.X - rippleSize / 2, _animationSource.Y - rippleSize / 2, rippleSize, rippleSize));
                g.ResetClip();
                rippleBrush.Dispose();
            }


            //Draw tab headers
            foreach (MaterialTabPage tabPage in _baseTabControl.MaterialTabPages)
            {

                var currentTabIndex = _baseTabControl.MaterialTabPages.IndexOf(tabPage);
                Brush textBrush;
                if (!TabForeColorJoinToTheme)
                    textBrush = getBrushDisjoinFromTheme(TabForeColor_Color);
                else
                    textBrush = new SolidBrush(Color.FromArgb(CalculateTextAlpha(currentTabIndex, animationProgress), SkinManager.GetPrimaryTextColor()));
                Brush selectedTabTextBrush;
                if (!SelectedTabForeColorJoinToTheme)
                    selectedTabTextBrush = new SolidBrush(Color.FromArgb(CalculateTextAlpha(currentTabIndex, animationProgress), getColorDisjoinFromTheme(SelectedTabForeColor_Color)));
                else
                    selectedTabTextBrush = new SolidBrush(Color.FromArgb(CalculateTextAlpha(currentTabIndex, animationProgress), SkinManager.GetForeColor()));

                //fill all tabs except selected tab
                if (currentTabIndex != _baseTabControl.SelectedIndex)
                {
                    if (!tabBackGroundJoinToTheme)
                        g.FillRectangle(getBrushDisjoinFromTheme(TabBackGroundColor), _tabRects[currentTabIndex]);
                    else
                        g.FillRectangle(SkinManager.GetPrimaryBrush(SelectedColorScheme), _tabRects[currentTabIndex]);

                    g.DrawString(tabPage.Text.ToUpper(), this.Font, textBrush, _tabRects[currentTabIndex], new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                }
                else
                    g.DrawString(tabPage.Text.ToUpper(), this.Font, selectedTabTextBrush, _tabRects[currentTabIndex], new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });



                DrawTabPageIcon(g, tabPage, _tabRects[currentTabIndex], (currentTabIndex == _baseTabControl.SelectedIndex));
                DrawTabPageCloseElement(g, tabPage, _tabRects[currentTabIndex], currentTabIndex);
                textBrush.Dispose();
            }

            //Draw add new tab
            if (NewTabVisible)
            {
                Brush textBrush1 = new SolidBrush(Color.FromArgb(CalculateTextAlpha(), SkinManager.GetForeColor()));
                DrawNewTabRect(g, textBrush1);
            }
           


            //Animate tab indicator
            var previousSelectedTabIndexIfHasOne = _previousSelectedTabIndex == -1 ? _baseTabControl.SelectedIndex : _previousSelectedTabIndex;
            var previousActiveTabRect = _tabRects[previousSelectedTabIndexIfHasOne];
            var activeTabPageRect = _tabRects[_baseTabControl.SelectedIndex];

            var y = activeTabPageRect.Bottom - 2;
            //var x = previousActiveTabRect.X + (int)((activeTabPageRect.X - previousActiveTabRect.X) * animationProgress);

            int x = 0;
            if (this.RightToLeft != RightToLeft.No)
            {
                x = previousActiveTabRect.X + (int)((activeTabPageRect.X - previousActiveTabRect.X) * animationProgress);
            }
            else
            {
                x = this.Width - (previousActiveTabRect.X + (int)((activeTabPageRect.X - previousActiveTabRect.X) * animationProgress));
            }

            var width = previousActiveTabRect.Width + (int)((activeTabPageRect.Width - previousActiveTabRect.Width) * animationProgress);

           g.FillRectangle(SkinManager.GetAccentBrush(SelectedColorScheme), x, y, width, TAB_INDICATOR_HEIGHT);


            
            OnUserPaint(e);
        }


        //draw new tab  rectangle
        private void DrawNewTabRect(Graphics g, Brush textBrush)
        {

            // g.DrawString("جدید", this.Font, new SolidBrush(SkinManager.GetForeColor()), _tabRects[_baseTabControl.MaterialTabPages.Count], new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            g.FillRectangle(SkinManager.GetAccentBrush(SelectedColorScheme), _tabRects[_baseTabControl.MaterialTabPages.Count]);
            DrawAddTabPageIcon(g, _tabRects[_baseTabControl.MaterialTabPages.Count]);
           
        }
        //draw tab page icon
        private void DrawTabPageIcon(Graphics g, MaterialTabPage tabPage, Rectangle tabRect, bool isSelectedTab)
        {
            if (tabPage == null) DrawAddTabPageIcon(g, tabRect);

            //Image icon = (tabPage != null) ? : null;
            // set by radius in circle2Rect method
            var iconHeight = 0;
            var iconWidth = 0;

           
                Rectangle iconRect;
                if (tabPage.EnabledItemCount)
                {
                    if (tabPage.IconPosition.Equals(ObjectSide.Left))
                        iconRect = new Rectangle(tabRect.X, (this.Height - iconHeight) / 2, iconWidth, iconHeight);
                    else
                        iconRect = new Rectangle((this.Width - iconWidth), (this.Height - iconHeight) / 2, iconWidth, iconHeight);
                    iconRect.X = tabRect.X;
                    iconRect.Y = tabRect.Y;
                    // e.Graphics.DrawImage(icon, iconRect);

                    DrawCircle(g, new Point(iconRect.X + 15, iconRect.Y + 15), tabPage.ItemCount, isSelectedTab);


                }
          
        }
        private void DrawAddTabPageIcon(Graphics g, Rectangle tabRect)
        {
             Rectangle addRect;


                // iconRect = new Rectangle(tabRect.X, (this.Height - addIcon.Height) / 2, addIcon.Size.Width, addIcon.Size.Height);
                addRect = new Rectangle(tabRect.X + (tabRect.Width - addIcon.Width)/2, (this.Height - addIcon.Height) / 2, addIcon.Width, addIcon.Height);
                //iconRect = new Rectangle((this.Width - addIcon.Size.Width), (this.Height - addIcon.Height) / 2, addIcon.Size.Width, addIcon.Size.Height);
               
                g.DrawImage(addIcon, addRect);


        }
        private void DrawCircle(Graphics graphics, Point center, string itemCount, bool isSelectedTab)
        {
            Brush numberBrush =new SolidBrush( Color.White);
            Brush brush;

            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            if (isSelectedTab)
                brush = SkinManager.GetAccentBrush(SelectedColorScheme);

            else
            {
                brush =new SolidBrush(((int)0xbdbdbd).ToColor());// SkinManager.GetUtility1Brush(SelectedColorScheme);
                numberBrush = new SolidBrush(((int)0x757575).ToColor()); //SkinManager.GetUtility2Brush(SelectedColorScheme);
            }
            if(brush==null)
                brush= SkinManager.GetAccentBrush(SelectedColorScheme);
            if(numberBrush==null)
                 numberBrush = new SolidBrush(Color.White);

            var circleFont = new Font("Shabnam", 11, FontStyle.Bold);

            graphics.FillEllipse(brush, circle2Rect(center, 13));
            center = new Point(center.X - (int)graphics.MeasureString(itemCount, circleFont).Width / 2, center.Y - (int)graphics.MeasureString(itemCount, circleFont).Height / 2);
            graphics.DrawString(itemCount.ToString(), circleFont, numberBrush, center);
        }
        RectangleF circle2Rect(Point midPoint, float radius)
        {
            return new RectangleF(midPoint.X - radius,
                                 midPoint.Y - radius,
                                 radius * 2,
                                 radius * 2);
        }
        private void DrawTabPageCloseElement(Graphics g, MaterialTabPage materialTabPage, Rectangle tabRect, int currentTabIndex)
        {
            Rectangle closeRect = new Rectangle();
            if (materialTabPage.EnabledCloseButton)
            {

                //if (materialTabPage.IconPosition.Equals(ObjectSide.Left))
                closeRect = new Rectangle(tabRect.X + tabRect.Width - image.Width - 5, (this.Height - image.Height) / 2, image.Width, image.Height);
                //else
                //    closeRect = new Rectangle((tabRect.X - image.Width), (this.Height - image.Height) / 2, image.Width, image.Height);
                g.DrawImage(image, closeRect);

            }
            _tabCloseRects.Add(closeRect);
        }
        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }
        private int CalculateTextAlpha(int tabIndex, double animationProgress)
        {
            
            int primaryA = SkinManager.ACTION_BAR_TEXT.A;
            int secondaryA = SkinManager.ACTION_BAR_TEXT_SECONDARY.A;

            if (tabIndex == _baseTabControl.SelectedIndex && !_animationManager.IsAnimating())
            {
                return primaryA;
            }
            if (tabIndex != _previousSelectedTabIndex && tabIndex != _baseTabControl.SelectedIndex)
            {
                return secondaryA;
            }
            if (tabIndex == _previousSelectedTabIndex)
            {
                return primaryA - (int)((primaryA - secondaryA) * animationProgress);
            }
            return secondaryA + (int)((primaryA - secondaryA) * animationProgress);
        }
        private int CalculateTextAlpha()
        {
            int primaryA = SkinManager.ACTION_BAR_TEXT.A;
            int secondaryA = SkinManager.ACTION_BAR_TEXT_SECONDARY.A;

            return primaryA;
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {

            base.OnMouseUp(e);

            if (_tabRects == null) UpdateTabRects();
            if (_tabRects.Count == 1)
            {
                if (_tabRects[0].Contains(e.Location))
                {
                    //rais AddTab event
                    if (AddNewTab != null)
                    {

                        AddNewTab(this, new EventArgs());
                        UpdateTabRects();
                        _baseTabControl.SelectedIndex = 0;
                        Invalidate();
                    }

                    return;

                }
            }
            for (var i = 0; i < _tabRects.Count - 1; i++)
            {

                if (_tabRects[_tabRects.Count - 1].Contains(e.Location))
                {
                    //rais AddTab event
                    if (AddNewTab != null)
                    {

                        AddNewTab(this, new EventArgs());
                        UpdateTabRects();
                        Invalidate();
                    }

                    return;

                }

                if (_baseTabControl.MaterialTabPages[i].EnabledCloseButton)
                {
                    if (_tabCloseRects[i].Contains(e.Location))
                    {

                        //rais close click event
                        if (TabCloseClick != null)
                        {
                            _baseTabControl.ClickedTab = _baseTabControl.MaterialTabPages[i];
                            TabCloseClick(this, new EventArgs());
                            UpdateTabRects();
                            Invalidate();
                            return;
                        }
                    }
                }


                if (_tabRects[i].Contains(e.Location))
                {
                    if (TabClick != null)
                    {
                        _baseTabControl.ClickedTab = _baseTabControl.MaterialTabPages[i];

                        TabClick(this, new EventArgs());

                    }
                    _baseTabControl.SelectedIndex = i;



                }


            }

            _animationSource = e.Location;
            Invalidate();
        }
      
        private void UpdateTabRects()
        {



            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
            int text_w = 0;
            _tabRects = new List<Rectangle>();

            //If there isn't a base tab control, the rects shouldn't be calculated
            //If there aren't tab pages in the base tab control, the list should just be empty which has been set already; exit the void


            //Calculate the bounds of each tab header specified in the base tab control
            using (var b = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(b))
                {
                    if (_baseTabControl == null || _baseTabControl.MaterialTabPages.Count == 0)
                    {
                        if (_tabRects.Count == 0)
                            CreateAddNewTabRec(g);

                        return;

                    }
                    else
                    {
                        var firsticonwidth = (((MaterialTabPage)_baseTabControl.MaterialTabPages[0]).EnabledItemCount) ? 20 : 0;
                        var closebttnwidth = ((_baseTabControl.MaterialTabPages[0].EnabledCloseButton) ? image.Width : 0);
                        //_tabRects.Add(new Rectangle(SkinManager.FORM_PADDING, 0, TAB_HEADER_PADDING * 2 + (int)g.MeasureString(_baseTabControl.TabPages[0].Text, SkinManager.ROBOTO_MEDIUM_10).Width, Height));
                        _tabRects.Add(new Rectangle((RightToLeft != RightToLeft.Yes) ? SkinManager.FORM_PADDING :
                            Width - ((int)g.MeasureString(_baseTabControl.MaterialTabPages[0].Text, this.Font).Width) - SkinManager.FORM_PADDING - TAB_HEADER_PADDING * 2 - firsticonwidth - closebttnwidth,
                            0, TAB_HEADER_PADDING * 2 + (int)g.MeasureString(_baseTabControl.MaterialTabPages[0].Text, this.Font).Width + firsticonwidth + closebttnwidth, Height));

                        for (int i = 1; i < _baseTabControl.MaterialTabPages.Count; i++)
                        {
                            //_tabRects.Add(new Rectangle(_tabRects[i - 1].Right, 0, TAB_HEADER_PADDING * 2 + (int)g.MeasureString(_baseTabControl.TabPages[i].Text, SkinManager.ROBOTO_MEDIUM_10).Width, Height));
                            text_w = (int)g.MeasureString(_baseTabControl.MaterialTabPages[i].Text, this.Font).Width;

                            //icon width to each tabpage 
                            var iconwidth = (((MaterialTabPage)_baseTabControl.MaterialTabPages[i]).EnabledItemCount) ? 20 : 0;
                            var closbttnwidth = (_baseTabControl.MaterialTabPages[i].EnabledCloseButton) ? image.Width : 0;

                            w = TAB_HEADER_PADDING * 2 + text_w + iconwidth + closebttnwidth;
                            h = Height;
                            x = (RightToLeft == RightToLeft.Yes) ? (_tabRects[i - 1].Left) - TAB_HEADER_PADDING * 2 - text_w - iconwidth - closebttnwidth : _tabRects[i - 1].Right;
                            y = 0;


                            _tabRects.Add(new Rectangle(x, y, w, h));
                        }

                        //Add new tab rect  
                        CreateAddNewTabRec(g);
                    }

                }
            }
        }
        private void CreateAddNewTabRec(Graphics g)
        {

            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;
       

           
            h = Height;
            if (_tabRects.Count == 0)
            {
                _tabRects.Add(new Rectangle(Width  - SkinManager.FORM_PADDING - TAB_HEADER_PADDING  - addIcon.Width,
                        0,Height+10, Height));
                //Console.WriteLine(new Rectangle(Width - SkinManager.FORM_PADDING - TAB_HEADER_PADDING * 2 - addIcon.Width,
                //        0, w, Height).ToString());

            }
            else
            {
                x = (RightToLeft == RightToLeft.Yes) ? (_tabRects[_tabRects.Count - 1].Left) - TAB_HEADER_PADDING   - addIcon.Width : _tabRects[_tabRects.Count - 1].Right;
                y = 0;

                _tabRects.Add(new Rectangle(x, y, Height+10, h));

            }

        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
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
