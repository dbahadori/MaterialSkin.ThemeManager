using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin.Properties;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;

namespace MaterialSkin
{
    public class MaterialSkinManager : Component, INotifyPropertyChanged 
    {
        // Global instacne
        private static MaterialSkinManager _instance;

        //Forms to control
        private readonly List<MaterialForm> _formsToManage = new List<MaterialForm>();


        //Name

        private string name = string.Empty;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string Name
        {
            get
            {
                if (base.Site != null)
                {
                    name = base.Site.Name;
                }
                return name;
            }
            set
            {
                if (value != "" && value != null)
                    name = value;
                else name = base.Site.Name;

            }
        }

        //Theme
        private  Themes _theme;

        public virtual Themes Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                UpdateBackgrounds();
                OnPropertyChanged("ApplicationTheme");
                globalTheme = value;
            }
        }
        private Themes globalTheme;
        public virtual Themes GlobalTheme {
            get {
                return globalTheme;
                //} set {
                //    globalTheme = value;

            }
        }

        //public  Themes GlobalTheme { get { return globalThem; } }
        //private Themes globalThem;
        //public  Themes LocalTheme { get; set; }

        //private bool useBaseColorScheme;

        //[Browsable(true)]
        //public bool UseBaseColorScheme { get { return useBaseColorScheme; }
        //    set {
        //        useBaseColorScheme = value;
        //    } }

        private ColorScheme _colorScheme;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Disposed;


        [DisplayName("BaseColorScheme")]
        public ColorScheme ColorScheme
        {
            get { return _colorScheme; }
            set
            {
                _colorScheme = value;
                UpdateBackgrounds();
                OnPropertyChanged("ApplicationColorScheme");
            }
        }




        private ColorSchemeCollection colorSchemes = new ColorSchemeCollection();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColorSchemeCollection ColorSchemes
        {
            get { return colorSchemes; }
        }

        public enum Themes : byte
        {
            LIGHT,
            DARK,
            CUSTOM
        }

        #region Custom Theme
        private static Color primary_text;
        private static Color secondary_text;
        private static Color disabled_Or_Hint_Text;
        private static Color dividers;
        //Constant color values
        public Color Primary_Text { get { return primary_text; } set { primary_text = value; } }
        public Brush Primary_Text_Brush = new SolidBrush(primary_text);
        public Color Secondary_text { get { return secondary_text; } set { secondary_text = value; } }
        public Brush Secondary_text_Brush = new SolidBrush(secondary_text);
        public Color Disabled_Or_Hint_Text { get { return disabled_Or_Hint_Text; } set { disabled_Or_Hint_Text = value; } }
        public Brush Disabled_Or_Hint_Text_brush = new SolidBrush(disabled_Or_Hint_Text);
        public Color Dividers { get { return dividers; } set { dividers = value; } }
        public Brush Dividers_Brush = new SolidBrush(dividers);

        // Checkbox colors
        private static Color checkBox_Off;

        public Color CheckBox_Off
        {
            get { return checkBox_Off; }
            set { checkBox_Off = value; }
        }
        public Brush CheckBox_Off_Brush = new SolidBrush(checkBox_Off);


        private static Color checkBox_Off_Disabled;
        public Color CheckBox_Off_Disabled
        {
            get { return checkBox_Off_Disabled; }
            set { checkBox_Off_Disabled = value; }
        }
        public Brush CheckBox_Off_Disabled_Brush = new SolidBrush(checkBox_Off_Disabled);

        //Raised button
        private static Color raised_Button_Text;
        public Color Raised_Button_Text
        {
            get { return raised_Button_Text; }
            set { raised_Button_Text = value; }
        }
        public Brush Raised_Button_Text_Brush = new SolidBrush(raised_Button_Text);
        //  public static Color RAISED_BUTTON_BACKGROUND_CUSTOM { get; set; }
        //  public static Brush RAISED_BUTTON_BACKGROUND_BRUSHH_CUSTOM { get; set; }

        //Flat button
        private static Color flat_Button_Background_Hover;
        public Color Flat_Button_Background_Hover
        {
            get { return flat_Button_Background_Hover; }
            set { flat_Button_Background_Hover = value; }
        }
        public Brush Flat_Button_Background_Hover_Brush = new SolidBrush(flat_Button_Background_Hover);


        private static Color flat_Button_DisabledText;
        public Color Flat_Button_DisabledText
        {
            get { return flat_Button_DisabledText; }
            set { flat_Button_DisabledText = value; }
        }
        public Brush Flat_Button_DisabledText_Brush = new SolidBrush(flat_Button_DisabledText);

        private static Color flat_Button_Background_Pressed;
        public Color Flat_Button_Background_Pressed
        {
            get { return flat_Button_Background_Pressed; }
            set { flat_Button_Background_Pressed = value; }
        }
        public Brush Flat_Button_Background_Pressed_Brush = new SolidBrush(flat_Button_Background_Pressed);


        //ContextMenuStrip
        private static Color cMS_Background_Hover;
        public Color CMS_Background_Hover
        {
            get { return cMS_Background_Hover; }
            set { cMS_Background_Hover = value; }
        }
        public Brush CMS_Background_Hover_Brush = new SolidBrush(cMS_Background_Hover);


        //Application background


        private static Color background_Custom;
        public Color CustomBackground
        {
            get { return background_Custom; }
            set { background_Custom = value; }
        }
        public Brush Background_Custom_Brush = new SolidBrush(background_Custom);

        //Application action bar
        private static Color action_Bar_Text_Custom;
        public Color Action_Bar_Text_Custom
        {
            get { return action_Bar_Text_Custom; }
            set { action_Bar_Text_Custom = value; }
        }
        public Brush Action_Bar_Text_Custom_Brush = new SolidBrush(action_Bar_Text_Custom);

        private static Color action_Bar_Text_Secondary_Custom;
        public Color Action_Bar_Text_Secondary_Custom
        {
            get { return action_Bar_Text_Secondary_Custom; }
            set { action_Bar_Text_Secondary_Custom = value; }
        }
        public Brush Action_Bar_Text_Secondary_Custom_Brush = new SolidBrush(action_Bar_Text_Secondary_Custom);

        #endregion


        //Constant color values
        private static readonly Color PRIMARY_TEXT_BLACK = Color.FromArgb(222, 0, 0, 0);
        private static readonly Brush PRIMARY_TEXT_BLACK_BRUSH = new SolidBrush(PRIMARY_TEXT_BLACK);
        public static Color SECONDARY_TEXT_BLACK = Color.FromArgb(138, 0, 0, 0);
        public static Brush SECONDARY_TEXT_BLACK_BRUSH = new SolidBrush(SECONDARY_TEXT_BLACK);
        private static readonly Color DISABLED_OR_HINT_TEXT_BLACK = Color.FromArgb(66, 0, 0, 0);
        private static readonly Brush DISABLED_OR_HINT_TEXT_BLACK_BRUSH = new SolidBrush(DISABLED_OR_HINT_TEXT_BLACK);
        private static readonly Color DIVIDERS_BLACK = Color.FromArgb(31, 0, 0, 0);
        private static readonly Brush DIVIDERS_BLACK_BRUSH = new SolidBrush(DIVIDERS_BLACK);

        private static readonly Color PRIMARY_TEXT_WHITE = Color.FromArgb(255, 255, 255, 255);
        private static readonly Brush PRIMARY_TEXT_WHITE_BRUSH = new SolidBrush(PRIMARY_TEXT_WHITE);
        public static Color SECONDARY_TEXT_WHITE = Color.FromArgb(179, 255, 255, 255);
        public static Brush SECONDARY_TEXT_WHITE_BRUSH = new SolidBrush(SECONDARY_TEXT_WHITE);
        private static readonly Color DISABLED_OR_HINT_TEXT_WHITE = Color.FromArgb(77, 255, 255, 255);
        private static readonly Brush DISABLED_OR_HINT_TEXT_WHITE_BRUSH = new SolidBrush(DISABLED_OR_HINT_TEXT_WHITE);
        private static readonly Color DIVIDERS_WHITE = Color.FromArgb(31, 255, 255, 255);
        private static readonly Brush DIVIDERS_WHITE_BRUSH = new SolidBrush(DIVIDERS_WHITE);

        // Checkbox colors
        private static readonly Color CHECKBOX_OFF_LIGHT = Color.Black; //Color.FromArgb(0, 0, 0, 0);
        private static readonly Brush CHECKBOX_OFF_LIGHT_BRUSH = new SolidBrush(CHECKBOX_OFF_LIGHT);
        private static readonly Color CHECKBOX_OFF_DISABLED_LIGHT = Color.FromArgb(66, 0, 0, 0);
        private static readonly Brush CHECKBOX_OFF_DISABLED_LIGHT_BRUSH = new SolidBrush(CHECKBOX_OFF_DISABLED_LIGHT);

        private static readonly Color CHECKBOX_OFF_DARK = Color.FromArgb(179, 255, 255, 255);
        private static readonly Brush CHECKBOX_OFF_DARK_BRUSH = new SolidBrush(CHECKBOX_OFF_DARK);
        private static readonly Color CHECKBOX_OFF_DISABLED_DARK = Color.FromArgb(77, 255, 255, 255);
        private static readonly Brush CHECKBOX_OFF_DISABLED_DARK_BRUSH = new SolidBrush(CHECKBOX_OFF_DISABLED_DARK);

        //Raised button
        private static readonly Color RAISED_BUTTON_BACKGROUND = Color.FromArgb(255, 255, 255, 255);
        private static readonly Brush RAISED_BUTTON_BACKGROUND_BRUSH = new SolidBrush(RAISED_BUTTON_BACKGROUND);
        private static readonly Color RAISED_BUTTON_TEXT_LIGHT = PRIMARY_TEXT_WHITE;
        private static readonly Brush RAISED_BUTTON_TEXT_LIGHT_BRUSH = new SolidBrush(RAISED_BUTTON_TEXT_LIGHT);
        private static readonly Color RAISED_BUTTON_TEXT_DARK = PRIMARY_TEXT_BLACK;
        private static readonly Brush RAISED_BUTTON_TEXT_DARK_BRUSH = new SolidBrush(RAISED_BUTTON_TEXT_DARK);

        //Flat button
        private static readonly Color FLAT_BUTTON_BACKGROUND_HOVER_LIGHT = Color.FromArgb(20.PercentageToColorComponent(), 0x999999.ToColor());
        private static readonly Brush FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_HOVER_LIGHT);
        private static readonly Color FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT = Color.FromArgb(40.PercentageToColorComponent(), 0x999999.ToColor());
        private static readonly Brush FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT);
        private static readonly Color FLAT_BUTTON_DISABLEDTEXT_LIGHT = Color.FromArgb(26.PercentageToColorComponent(), 0x000000.ToColor());
        private static readonly Brush FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH = new SolidBrush(FLAT_BUTTON_DISABLEDTEXT_LIGHT);

        private static readonly Color FLAT_BUTTON_BACKGROUND_HOVER_DARK = Color.FromArgb(100.PercentageToColorComponent(), 0x0077c2.ToColor());
        private static readonly Brush FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_HOVER_DARK);
        private static readonly Color FLAT_BUTTON_BACKGROUND_PRESSED_DARK = Color.FromArgb(100.PercentageToColorComponent(), 0x0077c2.ToColor());
        private static readonly Brush FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_BACKGROUND_PRESSED_DARK);
        private static readonly Color FLAT_BUTTON_DISABLEDTEXT_DARK = Color.FromArgb(30.PercentageToColorComponent(), 0xFFFFFF.ToColor());
        private static readonly Brush FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH = new SolidBrush(FLAT_BUTTON_DISABLEDTEXT_DARK);

        //ContextMenuStrip
        private static readonly Color CMS_BACKGROUND_LIGHT_HOVER = Color.FromArgb(255, 238, 238, 238);
        private static readonly Brush CMS_BACKGROUND_HOVER_LIGHT_BRUSH = new SolidBrush(CMS_BACKGROUND_LIGHT_HOVER);

        private static readonly Color CMS_BACKGROUND_DARK_HOVER = Color.FromArgb(38, 204, 204, 204);
        private static readonly Brush CMS_BACKGROUND_HOVER_DARK_BRUSH = new SolidBrush(CMS_BACKGROUND_DARK_HOVER);

        //forecolor 
        private static Color FORECOLOR_LIGHT = ((int)0xffffff).ToColor();
        private static Color FORECOLOR_DARK = ((int)0x212121).ToColor();
        //Application background
        private static Color BACKGROUND_LIGHT = ((int)0xd1d1d1).ToColor();
        private static Brush BACKGROUND_LIGHT_BRUSH = new SolidBrush(BACKGROUND_LIGHT);
        public Color LightBackGround { get { return BACKGROUND_LIGHT; } set { BACKGROUND_LIGHT = value; } }

        private static Color BACKGROUND_DARK= ((int)0x757575).ToColor();
        private static Brush BACKGROUND_DARK_BRUSH = new SolidBrush(BACKGROUND_DARK);
        public Color DarkBackGround { get { return BACKGROUND_DARK; } set { BACKGROUND_DARK = value; } }
        //Application action bar
        public readonly Color ACTION_BAR_TEXT = Color.FromArgb(255, 255, 255, 255);
        public readonly Brush ACTION_BAR_TEXT_BRUSH = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
        public readonly Color ACTION_BAR_TEXT_SECONDARY = Color.FromArgb(153, 255, 255, 255);
        public readonly Brush ACTION_BAR_TEXT_SECONDARY_BRUSH = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

        public ColorScheme GetColorScheme(ColorScheme colorScheme)
        {
            //maybe colorscheme object be basecolorscheme, therefore we check to determine it is an item of 
            //colorscheme collection or not.

            if (colorScheme.Name == this.ColorScheme.Name)
                return this.ColorScheme;
            else {
                List<ColorScheme>.Enumerator e = (List<ColorScheme>.Enumerator)colorSchemes.GetEnumerator();

                while (e.MoveNext())
                {

                    if (e.Current.Name == colorScheme.Name)
                        return e.Current;
                }
            }
           
            return null;
        }
        
        public bool  ContainsColorScheme(ColorScheme colorScheme)
        {

            return (GetColorScheme(colorScheme) == null) ? false : true;
        }
       
        public ColorScheme FindColorScheme(string colorSchemeName)
        {
            if (colorSchemeName == this.ColorScheme.Name)
                return this.ColorScheme;
            else
            {
                List<ColorScheme>.Enumerator e = (List<ColorScheme>.Enumerator)colorSchemes.GetEnumerator();

                while (e.MoveNext())
                {

                    if (e.Current.Name == colorSchemeName)
                        return e.Current;
                }
            }
            return null;

            

        }
        public Color GetSecondaryColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.AccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.AccentColor_LightTheme;

                default: return new Color();

            }
        }
        public Color GetDarkSecondaryColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.DarkAccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.DarkAccentColor_LightTheme;

                default: return new Color();

            }
        }
        public Color GetLightSecondaryColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.LightAccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.LightAccentColor_LightTheme;

                default: return new Color();

            }
        }
        public Brush GetSecondaryColorBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.AccentBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.AccentBrush_LightTheme;

                default: return null;

            }
        }
        public Brush GetDarkSecondaryColorBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.DarkAccentBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.DarkAccentBrush_LightTheme;

                default: return null;

            }
        }
        public Brush GetLightSecondaryColorBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.LightAccentBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.LightAccentBrush_LightTheme;

                default: return null;

            }
        }
        public Brush GetUtility1Brush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.Utility1Brush_DarkTheme;
                   
                case Themes.LIGHT:
                    return selectedColorScheme.Utility1Brush_LightTheme;
                   
                default: return null;
                   
            }
        }
        public Brush GetUtility2Brush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.Utility2Brush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.Utility2Brush_LightTheme;

                default: return null;

            }
        }
        public Color GetUtility1Color(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.Utility1Color_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.Utility1Color_LightTheme;

                default: return new Color();

            }
        }
        public Color GetUtility1Color(ColorScheme scolorScheme, Themes theme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.Utility1Color_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.Utility1Color_LightTheme;

                default: return new Color();

            }
        }
        public Color GetUtility2Color(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.Utility2Color_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.Utility2Color_LightTheme;

                default: return new Color();

            }
        }
        public Color GetUtility2Color(ColorScheme scolorScheme, Themes theme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.Utility2Color_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.Utility2Color_LightTheme;

                default: return new Color();

            }
        }
        public Brush GetDarkPrimaryBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.DarkPrimaryBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.DarkPrimaryBrush_LightTheme;

                default: return null;

            }
        }
        public Brush GetLightPrimaryBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.LightPrimaryBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.LightPrimaryBrush_LightTheme;
                default: return null;

            }
        }
        public Brush GetPrimaryBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.PrimaryBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.PrimaryBrush_LightTheme;
                default: return null;

            }
        }

        public Color GetDarkPrimaryColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.DarkPrimaryColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.DarkPrimaryColor_LightTheme;

                default: return new Color ();

            }
        }
        public Color GetDarkPrimaryColor(ColorScheme scolorScheme, Themes theme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.DarkPrimaryColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.DarkPrimaryColor_LightTheme;

                default: return new Color();

            }
        }
        public Color GetLightPrimaryColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.LightPrimaryColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.LightPrimaryColor_LightTheme;
                default: return new Color ();

            }
        }
        public Color GetLightPrimaryColor(ColorScheme scolorScheme, Themes theme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.LightPrimaryColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.LightPrimaryColor_LightTheme;
                default: return new Color();

            }
        }
        public Color GetPrimaryColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.PrimaryColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.PrimaryColor_LightTheme;
                default: return new Color();

            }
        }

        public Color GetPrimaryColor(ColorScheme scolorScheme, Themes theme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.PrimaryColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.PrimaryColor_LightTheme;
                default: return new Color();

            }
        }
        public Brush GetAccentBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.AccentBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.AccentBrush_LightTheme;
                default: return null;

            }
        }
        public Brush GetDarkAccentBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.DarkAccentBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.DarkAccentBrush_LightTheme;
                default: return null;

            }
        }
        public Brush GetLightAccentBrush(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.LightAccentBrush_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.LightAccentBrush_LightTheme;
                default: return null;

            }
        }

        public Color GetAccentColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.AccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.AccentColor_LightTheme;
                default: return new Color();

            }
        }
        public Color GetAccentColor(ColorScheme scolorScheme, Themes theme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.AccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.AccentColor_LightTheme;
                default: return new Color();

            }
        }
        public Color GetDarkAccentColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.DarkAccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.DarkAccentColor_LightTheme;
                default: return new Color();

            }
        }
        public Color GetDarkAccentColor(ColorScheme scolorScheme, Themes theme)
        {
           ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.DarkAccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.DarkAccentColor_LightTheme;
                default: return new Color();

            }
        }
        public Color GetLightAccentColor(ColorScheme scolorScheme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (this.Theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.LightAccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.LightAccentColor_LightTheme;
                default: return new Color();

            }
        }
        public Color GetLightAccentColor(ColorScheme scolorScheme, Themes theme)
        {
            ColorScheme selectedColorScheme = GetColorScheme(scolorScheme);
            switch (theme)
            {
                case Themes.DARK:
                    return selectedColorScheme.LightAccentColor_DarkTheme;

                case Themes.LIGHT:
                    return selectedColorScheme.LightAccentColor_LightTheme;
                default: return new Color();

            }
        }

        public Color GetPrimaryTextColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = PRIMARY_TEXT_WHITE;
                    break;
                case Themes.LIGHT:
                    theme = PRIMARY_TEXT_BLACK;
                    break;
                default:
                    theme = Primary_Text;
                    break;
            }

            return theme;
        }
        public Color GetPrimaryTextColor(Themes _theme)
        {
            Color theme;
            switch (_theme)
            {
                case Themes.DARK:
                    theme = PRIMARY_TEXT_WHITE;
                    break;
                case Themes.LIGHT:
                    theme = PRIMARY_TEXT_BLACK;
                    break;
                default:
                    theme = Primary_Text;
                    break;
            }

            return theme;
        }

        public Brush GetPrimaryTextBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = PRIMARY_TEXT_WHITE_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = PRIMARY_TEXT_BLACK_BRUSH;
                    break;
                default:
                    theme = Primary_Text_Brush;
                    break;
            }

            return theme;

        }
        protected Color GetPrimaryTextBrushtest()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme =  Color.Red;
                    break;
                case Themes.LIGHT:
                    theme = Color.Yellow;
                    break;
                default:
                    theme = Color.Black;
                    break;
            }

            return theme;

        }
        public Color GetSecondaryTextColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = SECONDARY_TEXT_WHITE;
                    break;
                case Themes.LIGHT:
                    theme = SECONDARY_TEXT_BLACK;
                    break;
                default:
                    theme = Secondary_text;
                    break;
            }

            return theme;

        }
        public Color GetSecondaryTextColor(Themes _theme)
        {
            Color theme;
            switch (_theme)
            {
                case Themes.DARK:
                    theme = SECONDARY_TEXT_WHITE;
                    break;
                case Themes.LIGHT:
                    theme = SECONDARY_TEXT_BLACK;
                    break;
                default:
                    theme = Secondary_text;
                    break;
            }

            return theme;

        }

        public Brush GetSecondaryTextBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = SECONDARY_TEXT_WHITE_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = SECONDARY_TEXT_BLACK_BRUSH;
                    break;
                default:
                    theme = Secondary_text_Brush;
                    break;
            }

            return theme;
        }

        public Color GetDisabledOrHintColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = DISABLED_OR_HINT_TEXT_WHITE;
                    break;
                case Themes.LIGHT:
                    theme = DISABLED_OR_HINT_TEXT_BLACK;
                    break;
                default:
                    theme = Disabled_Or_Hint_Text;
                    break;
            }

            return theme;


        }

        public Brush GetDisabledOrHintBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = DISABLED_OR_HINT_TEXT_WHITE_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = DISABLED_OR_HINT_TEXT_BLACK_BRUSH;
                    break;
                default:
                    theme = Disabled_Or_Hint_Text_brush;
                    break;
            }

            return theme;
        }

        public Color GetDividersColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = DIVIDERS_WHITE;
                    break;
                case Themes.LIGHT:
                    theme = DIVIDERS_BLACK;
                    break;
                default:
                    theme = Dividers;
                    break;
            }

            return theme;

        }

        public Brush GetDividersBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = DIVIDERS_WHITE_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = DIVIDERS_BLACK_BRUSH;
                    break;
                default:
                    theme = Dividers_Brush;
                    break;
            }

            return theme;
        }


        public Color GetCheckboxOffColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = CHECKBOX_OFF_DARK;
                    break;
                case Themes.LIGHT:
                    theme = CHECKBOX_OFF_LIGHT;
                    break;
                default:
                    theme = CheckBox_Off;
                    break;
            }

            return theme;

        }

        public Brush GetCheckboxOffBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = CHECKBOX_OFF_DARK_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = CHECKBOX_OFF_LIGHT_BRUSH;
                    break;
                default:
                    theme = CheckBox_Off_Brush;
                    break;
            }

            return theme;
        }

        public Color GetCheckBoxOffDisabledColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = CHECKBOX_OFF_DISABLED_DARK;
                    break;
                case Themes.LIGHT:
                    theme = CHECKBOX_OFF_DISABLED_LIGHT;
                    break;
                default:
                    theme = CheckBox_Off_Disabled;
                    break;
            }

            return theme;
        }

        public Brush GetCheckBoxOffDisabledBrush()
        {

            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = CHECKBOX_OFF_DISABLED_DARK_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = CHECKBOX_OFF_DISABLED_LIGHT_BRUSH;
                    break;
                default:
                    theme = CheckBox_Off_Disabled_Brush;
                    break;
            }

            return theme;
        }

        public Brush GetRaisedButtonBackgroundBrush()
        {

            return RAISED_BUTTON_BACKGROUND_BRUSH;
        }

        public Brush GetRaisedButtonTextBrush(Themes primary)
        {
            Brush theme;
            switch (primary)
            {
                case Themes.DARK:
                    theme = RAISED_BUTTON_TEXT_LIGHT_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = RAISED_BUTTON_TEXT_DARK_BRUSH;
                    break;
                default:
                    theme = Raised_Button_Text_Brush;
                    break;
            }

            return theme;
        }
        public Color GetForeColor()
        {
            return (this.Theme == Themes.DARK) ? FORECOLOR_LIGHT : FORECOLOR_DARK;


        }
        public Color GetForeColor(Themes theme)
        {
           
            return (theme == Themes.LIGHT) ?FORECOLOR_DARK: FORECOLOR_LIGHT;


        }
        public Color GetFlatButtonHoverBackgroundColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = FLAT_BUTTON_BACKGROUND_HOVER_DARK;
                    break;
                case Themes.LIGHT:
                    theme = FLAT_BUTTON_BACKGROUND_HOVER_LIGHT;
                    break;
                default:
                    theme = Flat_Button_Background_Hover;
                    break;
            }

            return theme;
        }

        public Brush GetFlatButtonHoverBackgroundBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = FLAT_BUTTON_BACKGROUND_HOVER_DARK_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = FLAT_BUTTON_BACKGROUND_HOVER_LIGHT_BRUSH;
                    break;
                default:
                    theme = Flat_Button_Background_Hover_Brush;
                    break;
            }

            return theme;
        }

        public Color GetFlatButtonPressedBackgroundColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = FLAT_BUTTON_BACKGROUND_PRESSED_DARK;
                    break;
                case Themes.LIGHT:
                    theme = FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT;
                    break;
                default:
                    theme = Flat_Button_Background_Pressed;
                    break;
            }

            return theme;
        }

        public Brush GetFlatButtonPressedBackgroundBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = FLAT_BUTTON_BACKGROUND_PRESSED_LIGHT_BRUSH;
                    break;
                default:
                    theme = Flat_Button_Background_Pressed_Brush;
                    break;
            }
            return theme;
        }

        public Brush GetFlatButtonDisabledTextBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = FLAT_BUTTON_DISABLEDTEXT_DARK_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = FLAT_BUTTON_DISABLEDTEXT_LIGHT_BRUSH;
                    break;
                default:
                    theme = Flat_Button_DisabledText_Brush;
                    break;
            }
            return theme;
        }

        public Brush GetCmsSelectedItemBrush()
        {
            Brush theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = CMS_BACKGROUND_HOVER_DARK_BRUSH;
                    break;
                case Themes.LIGHT:
                    theme = CMS_BACKGROUND_HOVER_LIGHT_BRUSH;
                    break;
                default:
                    theme = CMS_Background_Hover_Brush;
                    break;
            }
            return theme;

        }

        public Color GetApplicationBackgroundColor()
        {
            Color theme;
            switch (Theme)
            {
                case Themes.DARK:
                    theme = BACKGROUND_DARK;
                    break;
                case Themes.LIGHT:
                    theme = BACKGROUND_LIGHT;
                    break;
                default:
                    theme = background_Custom;
                    break;
            }
            return theme;
        }

        //public ColorScheme GetColorSchemeInstance(string colorSchemeName)
        //{
        //    //return (from c in ColorSchemes where c.Name == colorSchemeName select c).First();
        //}
        //Naskh Fonts
        public FontFamily ff_NOTO_NASKH_REGULAR;
        public FontFamily ff_NOTO_NASKH_BOLD;
        public FontFamily ff_DROID_ARABIC_UI;

        //Roboto font
        public Font ROBOTO_MEDIUM_12;
        public Font ROBOTO_REGULAR_11;
        public Font ROBOTO_MEDIUM_11;
        public Font ROBOTO_MEDIUM_10;

        //Other constants
        public int FORM_PADDING = 14;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pvd, [In] ref uint pcFonts);

        public MaterialSkinManager()
        {
            ff_NOTO_NASKH_REGULAR = LoadFont(Resources.NotoNaskh_Regular);
            ff_NOTO_NASKH_BOLD = LoadFont(Resources.NotoNaskh_Bold);
            ff_DROID_ARABIC_UI = LoadFont(Resources.DroidNaskhUI_Regular);

            ROBOTO_MEDIUM_12 = new Font(LoadFont(Resources.Roboto_Medium), 12f);
            ROBOTO_MEDIUM_10 = new Font(LoadFont(Resources.Roboto_Medium), 10f);
            ROBOTO_REGULAR_11 = new Font(LoadFont(Resources.Roboto_Regular), 11f);
            ROBOTO_MEDIUM_11 = new Font(LoadFont(Resources.Roboto_Medium), 11f);

            Theme = Themes.DARK;
            ColorScheme = new ColorScheme(Primary.Gray900, Primary.DarkGray900, Primary.LightGray900,
                Primary.DarkGray900, Primary.DarkGray900, Primary.LightGray300,Primary.Gray400,Primary.Gray400,Primary.Gray600, Primary.Gray600,
                Accent.Bule400, Accent.LightBlue400, Accent.LightBlue400,
                Accent.Bule400, Accent.DarkBlue400, Accent.DarkBlue400,
                TextShade.WHITE, "baseColorScheme");
            //ColorScheme = new ColorScheme(); ColorScheme.Name = "baseColorScheme";

            ColorSchemes.insertEvent += new EventHandler(UpdateColorSchemes);
            ColorSchemes.updateEvent += new EventHandler(UpdateColorSchemes);
            this.ColorSchemes.Add(ColorScheme);
            
            //SkinManagerContainer.Load();
            //SkinManagerContainer.AddSkinManager(this);
             //foreach(MaterialForm mf in this._formsToManage)
             //   mf.

        }

        //return all component of form
        //private IEnumerable<Component> EnumerateComponents(MaterialForm mform)
        //{
        //    return from form in mform.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        //           where typeof(Component).IsAssignableFrom(form.FieldType)
        //           let component = (Component)form.GetValue(this)
        //           where component != null
        //           select component;
        //}

        private void UpdateColorSchemes(object sender, EventArgs e)
        {
            UpdateBackgrounds();
            OnPropertyChanged("ApplicationColorScheme");
        }
        public static MaterialSkinManager Instance
        {
            get
            {
                return _instance ?? (_instance = new MaterialSkinManager());

            }
            set
            {
                _instance = value;
            }

        }


        public void AddFormToManage(MaterialForm materialForm)
        {
            _formsToManage.Add(materialForm);
            UpdateBackgrounds();
        }

        public  void RemoveFormToManage(MaterialForm materialForm)
        {
            _formsToManage.Remove(materialForm);
        }

        private readonly PrivateFontCollection privateFontCollection = new PrivateFontCollection();

        private FontFamily LoadFont(byte[] fontResource)
        {
            int dataLength = fontResource.Length;
            IntPtr fontPtr = Marshal.AllocCoTaskMem(dataLength);
            Marshal.Copy(fontResource, 0, fontPtr, dataLength);

            uint cFonts = 0;
            AddFontMemResourceEx(fontPtr, (uint)fontResource.Length, IntPtr.Zero, ref cFonts);
            privateFontCollection.AddMemoryFont(fontPtr, dataLength);

            return privateFontCollection.Families.Last();
        }

        protected  void UpdateBackgrounds()
        {
            var newBackColor = GetApplicationBackgroundColor();
            foreach (var materialForm in _formsToManage)
            {
                materialForm.BackColor = newBackColor;
                UpdateControl(materialForm, newBackColor);
            }
        }

        private void UpdateToolStrip(ToolStrip toolStrip, Color newBackColor)
        {
            if (toolStrip == null) return;

            toolStrip.BackColor = newBackColor;
            foreach (ToolStripItem control in toolStrip.Items)
            {
                control.BackColor = newBackColor;
                if (control is MaterialToolStripMenuItem && (control as MaterialToolStripMenuItem).HasDropDown)
                {

                    //recursive call
                    UpdateToolStrip((control as MaterialToolStripMenuItem).DropDown, newBackColor);
                }
            }
        }

        private void UpdateControl(Control controlToUpdate, Color newBackColor)
        {
            if (controlToUpdate == null) return;

            if (controlToUpdate.ContextMenuStrip != null)
            {
                UpdateToolStrip(controlToUpdate.ContextMenuStrip, newBackColor);
            }
            var tabControl = controlToUpdate as MaterialTabControl;
            if (tabControl != null)
            {
                foreach (MaterialTabPage tabPage in tabControl.MaterialTabPages)
                {
                    tabPage.BackColor = newBackColor;
                }
            }

            if (controlToUpdate is MaterialDivider)
            {
                controlToUpdate.BackColor = GetDividersColor();
            }

            if (controlToUpdate is MaterialListView)
            {
                controlToUpdate.BackColor = newBackColor;

            }

            //recursive call
            foreach (Control control in controlToUpdate.Controls)
            {
                UpdateControl(control, newBackColor);
            }

            controlToUpdate.Invalidate();
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        public class ColorSchemeCollection : Collection<MaterialSkin.ColorScheme>
        {
            public event EventHandler insertEvent;
            public event EventHandler removeEvent;
            public event EventHandler updateEvent;
            
            protected override void InsertItem(int index, MaterialSkin.ColorScheme item)
            {
                bool isRepetitive = false;
                if (insertEvent != null)
                {
                    List<ColorScheme>.Enumerator e = (List<ColorScheme>.Enumerator)base.GetEnumerator();

                    while (e.MoveNext())
                    {
                        if (e.Current.Name != "")
                            if (e.Current.Name == item.Name)
                                // throw new Exception("The Added ColorScheme Exist in Collection Already!");
                                isRepetitive = true;
                    }


                    if (!isRepetitive)
                    {
                        base.InsertItem(index, item);
                        insertEvent(item, new EventArgs());
                    }
                   

                }

            }
            protected override void RemoveItem(int index)
            {
                base.RemoveItem(index);
                removeEvent(this, new EventArgs());
            }
            protected override void SetItem(int index, ColorScheme item)
            {
                if (updateEvent != null)
                {
                    base.SetItem(index, item);
                    updateEvent(this, new EventArgs());
                }
                
            }

        }
        public Color ConvertPrimaryToColor(ColorCategory primaryColor, ColorScheme colorscheme)
        {
            Color color=new Color ();
            switch (primaryColor)
            {
                case ColorCategory.ApplicationBackGround:
                    color = GetApplicationBackgroundColor();
                    break;
                case ColorCategory.DarkPrimary:
                    color = GetDarkPrimaryColor(colorscheme);
                    break;
                case ColorCategory.LightPrimary:
                    color = GetLightPrimaryColor(colorscheme);
                    break;
                case ColorCategory.Primary:
                    color = GetPrimaryColor(colorscheme);
                    break;
                case ColorCategory.Utility1:
                    color = GetUtility1Color(colorscheme);
                    break;
                case ColorCategory.Secondary:
                    color = GetAccentColor(colorscheme);
                    break;
                case ColorCategory.LightSecondary:
                    color = GetLightAccentColor(colorscheme);
                    break;
                case ColorCategory.DarkSecondary:
                    color = GetDarkAccentColor(colorscheme);
                    break;
                default:
                    color = GetUtility2Color(colorscheme);
                    break;

            }
            return color;
        }
    }
}
