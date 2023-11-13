using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public sealed class MaterialDivider : Control, IMaterialControl
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

        public MaterialDivider()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Height = 1;
            BackColor = SkinManager.GetDividersColor();
        }
        //private Brush getBrushDisjoinFromTheme(ColorCategory _color)
        //{
        //    Brush brush = new SolidBrush(Color.White); ;

        //    switch (_color)
        //    {
        //        case ColorCategory.DarkPrimary:
        //            brush = SkinManager.GetDarkPrimaryBrush(SelectedColorScheme);
        //            break;
        //        case ColorCategory.LightPrimary:
        //            brush = SkinManager.GetLightPrimaryBrush(SelectedColorScheme);
        //            break;
        //        case ColorCategory.Utility1:
        //            brush = SkinManager.GetUtility1Brush(SelectedColorScheme);
        //            break;
        //        case ColorCategory.Utility2:
        //            brush = SkinManager.GetUtility2Brush(SelectedColorScheme);
        //            break;
        //        case ColorCategory.TextColor:
        //            brush = new SolidBrush(SkinManager.GetForeColor());
        //            break;
        //        case ColorCategory.DarkThemeTextColor:
        //            brush = new SolidBrush(SkinManager.GetForeColor(MaterialSkinManager.Themes.DARK));
        //            break;
        //        case ColorCategory.LightThemeTextColor:
        //            brush = new SolidBrush(SkinManager.GetForeColor(MaterialSkinManager.Themes.LIGHT));
        //            break;
        //        case ColorCategory.Secondary:
        //            brush = SkinManager.GetAccentBrush(SelectedColorScheme);
        //            break;
        //        case ColorCategory.DarkSecondary:
        //            brush = SkinManager.GetDarkAccentBrush(SelectedColorScheme);
        //            break;
        //        case ColorCategory.LightSecondary:
        //            brush = SkinManager.GetLightAccentBrush(SelectedColorScheme);
        //            break;
        //        default:
        //            brush = SkinManager.GetPrimaryBrush(SelectedColorScheme);
        //            break;


        //    }
        //    return brush;

        //}
        //private Color getColorDisjoinFromTheme(ColorCategory _color)
        //{
        //    Color color = new Color();

        //    switch (_color)
        //    {
        //        case ColorCategory.DarkPrimary:
        //            color = SkinManager.GetDarkPrimaryColor(SelectedColorScheme);
        //            break;
        //        case ColorCategory.LightPrimary:
        //            color = SkinManager.GetLightPrimaryColor(SelectedColorScheme);
        //            break;
        //        case ColorCategory.Utility1:
        //            color = SkinManager.GetUtility1Color(SelectedColorScheme);
        //            break;
        //        case ColorCategory.Utility2:
        //            color = SkinManager.GetUtility2Color(SelectedColorScheme);
        //            break;
        //        case ColorCategory.TextColor:
        //            color = SkinManager.GetForeColor();
        //            break;
        //        case ColorCategory.DarkThemeTextColor:
        //            color = SkinManager.GetForeColor(MaterialSkinManager.Themes.DARK);
        //            break;
        //        case ColorCategory.LightThemeTextColor:
        //            color = SkinManager.GetForeColor(MaterialSkinManager.Themes.LIGHT);
        //            break;
        //        case ColorCategory.Secondary:
        //            color = SkinManager.GetAccentColor(SelectedColorScheme);
        //            break;
        //        case ColorCategory.DarkSecondary:
        //            color = SkinManager.GetDarkAccentColor(SelectedColorScheme);
        //            break;
        //        case ColorCategory.LightSecondary:
        //            color = SkinManager.GetLightAccentColor(SelectedColorScheme);
        //            break;
        //        default:
        //            color = SkinManager.GetPrimaryColor(SelectedColorScheme);
        //            break;


        //    }
        //    return color;

        //}
    }
}
