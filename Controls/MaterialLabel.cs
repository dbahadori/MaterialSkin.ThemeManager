using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialLabel:Label,IMaterialControl
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
        [Browsable(false)]
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
        public MaterialLabel()
        {
            BackGroundJoinToTheme = true;
            ForeColorJoinToTheme = true;

            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;

        }

        private ColorScheme selectedColorScheme;
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }

       
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            base.OnPaint(e);
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;
            ForeColor = (!ForeColorJoinToTheme) ? getColorDisjoinFromTheme(ForeColor_Color) : SkinManager.GetPrimaryTextColor();

            if (!backGroundJoinToTheme)
                BackColor = getColorDisjoinFromTheme(BackGroundColor);

        }

        [Browsable(false)]
        public MouseState MouseState { get; set; }
        protected override void OnCreateControl()
        {
            base.OnCreateControl(); 

            ForeColor = (!ForeColorJoinToTheme)?this.ForeColor: SkinManager.GetPrimaryTextColor();
            //Font = SkinManager.ROBOTO_REGULAR_11;**

            BackColorChanged += (sender, args) => ForeColor = (!ForeColorJoinToTheme) ? getColorDisjoinFromTheme(ForeColor_Color) : SkinManager.GetPrimaryTextColor();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
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
