using FarsiLibrary.Win.Controls;
using FarsiLibrary.Win.Drawing;
using FarsiLibrary.Win.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialFADatePicker: FADatePicker, IMaterialControl
    {
        #region Join To Theme


        //Background
        private bool backGroundJoinToTheme;
        private Color bcolor;
        public bool BackGroundJoinToTheme { get { return backGroundJoinToTheme; } set { backGroundJoinToTheme = value; Invalidate(); } }

        ColorCategory backGroundColor;
        public ColorCategory BackGroundColor { get { return backGroundColor; } set { backGroundColor = value; bcolor = (SelectedColorScheme != null) ? getColorDisjoinFromTheme(backGroundColor) : this.BackColor; }
        }


        //Forecolor
        private bool foreColorJoinToTheme;

        public bool ForeColorJoinToTheme { get { return foreColorJoinToTheme; } set { foreColorJoinToTheme = value; Invalidate(); } }
        public ColorCategory ForeColor_Color { get; set; }

        //Arrowcolor
        private bool arrowColorJoinToTheme;

        public bool ArrowColorJoinToTheme { get { return arrowColorJoinToTheme; } set { arrowColorJoinToTheme = value; Invalidate(); } }
        public ColorCategory ArrowColor_Color { get; set; }

        //DisabledTextcolor
        private bool disabledTextColorJoinToTheme;

        public bool DisabledTextColorJoinToTheme { get { return disabledTextColorJoinToTheme; } set { disabledTextColorJoinToTheme = value; Invalidate(); } }
        public ColorCategory DisabledTextColor { get; set; }

        //DisabledTextcolor
        private bool disabledButtonColorJoinToTheme;

        public bool DisabledButtonColorJoinToTheme { get { return disabledButtonColorJoinToTheme; } set { disabledButtonColorJoinToTheme = value; Invalidate(); } }
        public ColorCategory DisabledButtonColor { get; set; }

        private XPThemeType datePickerThemeName;
        public XPThemeType DatePickerThemeName { get { return datePickerThemeName; }set{ datePickerThemeName = value; } }
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

        private ColorScheme selectedColorScheme;
        public ColorScheme SelectedColorScheme { get { return selectedColorScheme; } set { selectedColorScheme = value; } }

        public int Depth { get; set; }

        public MouseState MouseState { get; set; }
        ColorScheme a;
        public MaterialFADatePicker()
        {
            a =  new ColorScheme(Primary.Gray900, Primary.DarkGray900, Primary.LightGray900,
                Primary.DarkGray900, Primary.DarkGray900, Primary.LightGray300,Primary.Gray400,Primary.Gray400,Primary.Gray600, Primary.Gray600,
                Accent.Bule400, Accent.LightBlue400, Accent.LightBlue400,
                Accent.Bule400, Accent.DarkBlue400, Accent.DarkBlue400,
                TextShade.WHITE, "baseColorScheme");

            if (SelectedColorScheme == null)
                SelectedColorScheme = a;

           SkinManager.PropertyChanged += skinmanager_PropertyChanged;
            ArrowColorJoinToTheme = true;
            BackGroundJoinToTheme = true;
            DisabledButtonColorJoinToTheme = true;
            DisabledTextColorJoinToTheme = true;
            ForeColorJoinToTheme = true;
            this.DatePickerThemeName = XPThemeType.RAYANCUSTOM1;


            SetColors();

            base.Update();
            base.Invalidate();
            this.Update();
            this.Invalidate();

        }
        protected override void OnCreateControl()
        {
         

            SetColors();
        }
        private void skinmanager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            SetColors();
        }
    
        private int ColorToUInt(Color color)
        {
            // return Convert.ToInt32( string.Format("0x{0:X8}", Color.FromArgb(color.R, color.G, color.B).ToArgb()));
            return 0xffffff;
        }
        private void SetColors()
        {
            var id = 3;
            if (DatePickerThemeName == XPThemeType.RAYANCUSTOM1)
                id = 3;
            else if (DatePickerThemeName == XPThemeType.RAYANCUSTOM2)
                id = 4;

            Office2003Colors office2003colors = new Office2003Colors();
           Office2003Colors.Office11Colors[id][4] =( getColorDisjoinFromTheme(ColorCategory.Secondary)).ToArgb();
          Office2003Colors.Office11Colors[id][5] = (SkinManager.GetDividersColor()).ToArgb();
            
            if (!BackGroundJoinToTheme)
            {

                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][0] = ColorToUInt(getColorDisjoinFromTheme(BackGroundColor)));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][1] = ColorToUInt(getColorDisjoinFromTheme(BackGroundColor)));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][9] = ColorToUInt(getColorDisjoinFromTheme(BackGroundColor)));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][10] = ColorToUInt(getColorDisjoinFromTheme(BackGroundColor)));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][11] = ColorToUInt(getColorDisjoinFromTheme(BackGroundColor)));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][12] = ColorToUInt(getColorDisjoinFromTheme(BackGroundColor)));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][13] = ColorToUInt(getColorDisjoinFromTheme(BackGroundColor)));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][14] = ColorToUInt(SkinManager.GetPrimaryTextColor()));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][15] = ColorToUInt(SkinManager.Disabled_Or_Hint_Text));
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][22] = ColorToUInt(getColorDisjoinFromTheme(BackGroundColor)));
                ControlPaint.Dark(office2003colors[Office2003Color.Button2], 0.05f);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][31]);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][23]);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][24] = (getColorDisjoinFromTheme(BackGroundColor)).ToArgb());
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][2] = (getColorDisjoinFromTheme(ArrowColor_Color)).ToArgb());
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][3] = (getColorDisjoinFromTheme(DisabledButtonColor)).ToArgb());

            }
            else
            {
                var color = (SkinManager.ConvertPrimaryToColor(ColorCategory.LightPrimary, SkinManager.ColorScheme)).ToArgb();
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][0] = color);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][1] = color);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][9] = color);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][10] = color);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][11] = color);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][12] = color);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][13] = color);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][22] = color);
                ControlPaint.Dark(office2003colors[Office2003Color.Button2], 0.05f);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][31]);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][23]);
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][24] = color);

            }
            if (!ForeColorJoinToTheme)
            Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][14] = (getColorDisjoinFromTheme(ForeColor_Color)).ToArgb());
            else
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][14] = (SkinManager.GetPrimaryTextColor()).ToArgb());
            if (!ArrowColorJoinToTheme)
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][2] = (getColorDisjoinFromTheme(ArrowColor_Color)).ToArgb());
            else
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][2] = (SkinManager.GetPrimaryTextColor()).ToArgb());
            if (!DisabledTextColorJoinToTheme)
            Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][15] = (getColorDisjoinFromTheme(DisabledTextColor)).ToArgb());
            else
            Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][15] = (SkinManager.Disabled_Or_Hint_Text).ToArgb());
            if (!DisabledButtonColorJoinToTheme)
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][3] = (getColorDisjoinFromTheme(DisabledButtonColor)).ToArgb());
            else
                Office2003Colors.FromRgb(Office2003Colors.Office11Colors[id][3] = (SkinManager.GetDisabledOrHintColor()).ToArgb());

            base.Update();
            base.Invalidate();
            this.Update();
            this.Invalidate();
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
