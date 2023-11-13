using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin.Animations;
using System.Runtime.InteropServices;
using Janus.Windows.EditControls;
using System.Threading;
using FarsiLibrary.Win.Drawing;
using FarsiLibrary.Win.Enums;

namespace MaterialSkin.Controls
{
    public class MaterialComboBox : UIComboBox, IMaterialControl
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

        //UnderLineColor
        private bool underlineJoinToTheme;

        public bool UnderlineJoinToTheme { get { return underlineJoinToTheme; } set { underlineJoinToTheme = value; Invalidate(); } }
        public ColorCategory UnderlineColor { get; set; }

        #endregion
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
        private int animateCounter = -1;
        private ColorScheme selectedColorScheme;
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }
        public MaterialComboBox()
        {
         
            this.BorderStyle = Janus.Windows.UI.BorderStyle.None;
            this.RightToLeft = RightToLeft.Yes;
            ForeColorJoinToTheme = true;
            BackGroundJoinToTheme = true;
            SkinManager.PropertyChanged += SkinManager_PropertyChanged;

           

            Setup();
        }

        private void SkinManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Setup(); 
        }

        private void Setup()
        {
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;


            if (!ForeColorJoinToTheme)
                    this.ForeColor = getColorDisjoinFromTheme(ForeColor_Color);
                else
                    this.ForeColor = SkinManager.GetPrimaryTextColor();

                if (!BackGroundJoinToTheme)
                    this.BackColor = getColorDisjoinFromTheme(BackGroundColor);
                else
                    this.BackColor = SkinManager.GetApplicationBackgroundColor();

            this.ItemsFormatStyle.BackColor = SkinManager.GetApplicationBackgroundColor();
            this.ItemsFormatStyle.ForeColor = SkinManager.GetPrimaryTextColor();
            this.SelectedItemFormatStyle.BackColor = SkinManager.GetAccentColor(SkinManager.ColorScheme);
            this.SelectedItemFormatStyle.ForeColor = Color.Black;
        }

        public int Depth { get; set; }

        public MouseState MouseState { get; set; }
        
       

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            animateCounter = 0;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
           
            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;

            Setup();

            this.TextBox.Top = 1;


            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, 20, this.Height);

            var top = (this.Height / 2) - 2;

            DrawArrow(e.Graphics, new Rectangle(0, 0, 20, this.Height), true, false, 3);

            //for (int i = 0; i <= 3; i++)
            //    e.Graphics.DrawLine(new System.Drawing.Pen(SkinManager.GetPrimaryTextColor()), i + 4, top + i, 11 - i, top + i);

            if (animateCounter < 0 && !Focused)
                if(!UnderlineJoinToTheme)
                    e.Graphics.FillRectangle(getBrushDisjoinFromTheme(UnderlineColor), 0, this.Height - 3, this.Width, 2);
                else
                e.Graphics.FillRectangle(SkinManager.GetDividersBrush(), 0, this.Height - 3, this.Width, 2);
            else
            {
                if (animateCounter == -1 && Focused)
                {
                    e.Graphics.FillRectangle(SkinManager.GetSecondaryColorBrush(SelectedColorScheme), 0, this.Height - 3, this.Width, 2);
                }
                else
                {
                    Thread.Sleep(1);

                    var x = (this.Width / 2) - (animateCounter / 2);

                    e.Graphics.FillRectangle(SkinManager.GetSecondaryColorBrush(SelectedColorScheme), x, this.Height - 3, animateCounter, 2);

                    animateCounter += 8;

                    if (animateCounter >= this.Width)
                        animateCounter = -1;

                    Invalidate();
                }
            }
           // Setup();
        }

        public Rectangle DrawArrow(Graphics g, Rectangle rc, bool isLeft, bool isDisabled, int arrowSize)
        {
            int middle = rc.Height / 2;
            Point[] pntArrow = new Point[3];
            SolidBrush br;

            if (isLeft)
            {
                pntArrow[0] = new Point(rc.Width - 11, middle - 1);
                pntArrow[1] = new Point(rc.Width - 9, middle + 2);
                pntArrow[2] = new Point(rc.Width - 6, middle - 1);
            }
            else
            {
                pntArrow[0] = new Point(rc.Left + 6, middle - 1);
                pntArrow[1] = new Point(rc.Left + 8, middle + 2);
                pntArrow[2] = new Point(rc.Left + 11, middle - 1);
            }

            if (isDisabled)
            {
                br = new SolidBrush(Color.DarkGray);
            }
            else
            {


                br = new SolidBrush(this.SkinManager.GetForeColor()/*Office2003Colors.Default[Office2003Color.ArrowColor]*/);
            }

            g.FillPolygon(br, pntArrow);
            br.Dispose();

            return rc;
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
