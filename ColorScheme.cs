using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design;

namespace MaterialSkin
{
    [DesignTimeVisible(false)]
    
    public class ColorScheme : Component
    {
        public Color PrimaryColor_LightTheme, DarkPrimaryColor_LightTheme, LightPrimaryColor_LightTheme, AccentColor_LightTheme, LightAccentColor_LightTheme, DarkAccentColor_LightTheme, TextColor,Utility1Color_DarkTheme, Utility2Color_DarkTheme;
        public Color PrimaryColor_DarkTheme, DarkPrimaryColor_DarkTheme, LightPrimaryColor_DarkTheme, AccentColor_DarkTheme, LightAccentColor_DarkTheme, DarkAccentColor_DarkTheme, Utility1Color_LightTheme, Utility2Color_LightTheme;

        public Pen PrimaryPen_LightTheme, DarkPrimaryPen_LightTheme, LightPrimaryPen_LightTheme, AccentPen_LightTheme, LightAccentPen_LightTheme, DarkAccentPen_LightTheme, Utility1Pen_DarkTheme, Utility2Pen_DarkTheme, TextPen;
        public Pen PrimaryPen_DarkTheme, DarkPrimaryPen_DarkTheme, LightPrimaryPen_DarkTheme, AccentPen_DarkTheme, LightAccentPen_DarkTheme, DarkAccentPen_DarkTheme, Utility1Pen_LightTheme, Utility2Pen_LightTheme;

        public Brush PrimaryBrush_LightTheme, DarkPrimaryBrush_LightTheme, LightPrimaryBrush_LightTheme, AccentBrush_LightTheme, LightAccentBrush_LightTheme, DarkAccentBrush_LightTheme, Utility1Brush_DarkTheme, Utility2Brush_DarkTheme, TextBrush;
        public Brush PrimaryBrush_DarkTheme, DarkPrimaryBrush_DarkTheme, LightPrimaryBrush_DarkTheme, AccentBrush_DarkTheme, LightAccentBrush_DarkTheme, DarkAccentBrush_DarkTheme, Utility1Brush_LightTheme, Utility2Brush_LightTheme;

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
                name = value;
            }
        }


        private Primary utility1_LightTheme;
        public Primary Utility1_LightTheme { get { return utility1_LightTheme; } set { utility1_LightTheme = value; Utility1Color_LightTheme = ((int)utility1_LightTheme).ToColor(); Utility1Pen_LightTheme = new Pen(Utility1Color_LightTheme); Utility1Brush_LightTheme = new SolidBrush(Utility1Color_LightTheme); } }

        private Primary utility2_LightTheme;
        public Primary Utility2_LightTheme { get { return utility2_LightTheme; } set { utility2_LightTheme = value; Utility2Color_LightTheme = ((int)utility2_LightTheme).ToColor(); Utility2Pen_LightTheme = new Pen(Utility2Color_LightTheme); Utility2Brush_LightTheme = new SolidBrush(Utility2Color_LightTheme); } }

        private Primary primary_LightTheme;
        public Primary Primary_LightTheme { get { return primary_LightTheme; } set { primary_LightTheme = value; PrimaryColor_LightTheme = ((int)primary_LightTheme).ToColor(); PrimaryPen_LightTheme = new Pen(PrimaryColor_LightTheme); PrimaryBrush_LightTheme = new SolidBrush(PrimaryColor_LightTheme); } }

        private Primary primary_DarkTheme;
        public Primary Primary_DarkTheme { get { return primary_DarkTheme; } set { primary_DarkTheme = value; PrimaryColor_DarkTheme = ((int)primary_DarkTheme).ToColor(); PrimaryPen_DarkTheme = new Pen(PrimaryColor_DarkTheme); PrimaryBrush_DarkTheme = new SolidBrush(PrimaryColor_DarkTheme); } }


        private Primary darkPrimary_LightTheme;
        public Primary DarkPrimary_LightTheme { get { return darkPrimary_LightTheme; } set { darkPrimary_LightTheme = value; DarkPrimaryColor_LightTheme = ((int)darkPrimary_LightTheme).ToColor(); DarkPrimaryPen_LightTheme = new Pen(DarkPrimaryColor_LightTheme); DarkPrimaryBrush_LightTheme = new SolidBrush(DarkPrimaryColor_LightTheme); } }

        private Primary utility1_DarkTheme;
        public Primary Utility1_DarkTheme { get { return utility1_DarkTheme; } set { utility1_DarkTheme = value; Utility1Color_DarkTheme = ((int)utility1_DarkTheme).ToColor(); Utility1Pen_DarkTheme = new Pen(Utility1Color_DarkTheme); Utility1Brush_DarkTheme = new SolidBrush(Utility1Color_DarkTheme); } }


        private Primary darkPrimary_DarkTheme;
        public Primary DarkPrimary_DarkTheme { get { return darkPrimary_DarkTheme; } set { darkPrimary_DarkTheme = value; DarkPrimaryColor_DarkTheme = ((int)darkPrimary_DarkTheme).ToColor(); DarkPrimaryPen_DarkTheme = new Pen(DarkPrimaryColor_DarkTheme); DarkPrimaryBrush_DarkTheme = new SolidBrush(DarkPrimaryColor_DarkTheme); } }


        private Primary lightPrimary_LightTheme;
        public Primary LightPrimary_LightTheme { get { return lightPrimary_LightTheme; } set { lightPrimary_LightTheme = value; LightPrimaryColor_LightTheme = ((int)lightPrimary_LightTheme).ToColor(); LightPrimaryPen_LightTheme = new Pen(LightPrimaryColor_LightTheme); LightPrimaryBrush_LightTheme = new SolidBrush(LightPrimaryColor_LightTheme); } }

        private Primary lightPrimary_DarkTheme;
        public Primary LightPrimary_DarkTheme { get { return lightPrimary_DarkTheme; } set { lightPrimary_DarkTheme = value; LightPrimaryColor_DarkTheme = ((int)lightPrimary_DarkTheme).ToColor(); LightPrimaryPen_DarkTheme = new Pen(LightPrimaryColor_DarkTheme); LightPrimaryBrush_DarkTheme = new SolidBrush(LightPrimaryColor_DarkTheme); } }


        private Accent accent_LightTheme;
        public Accent Accent_LightTheme { get { return accent_LightTheme; } set { accent_LightTheme = value; AccentColor_LightTheme = ((int)accent_LightTheme).ToColor(); AccentPen_LightTheme = new Pen(AccentColor_LightTheme); AccentBrush_LightTheme = new SolidBrush(AccentColor_LightTheme); } }

        private Accent accent_DarkTheme;
        public Accent Accent_DarkTheme { get { return accent_DarkTheme; } set { accent_DarkTheme = value; AccentColor_DarkTheme = ((int)accent_DarkTheme).ToColor(); AccentPen_DarkTheme = new Pen(AccentColor_DarkTheme); AccentBrush_DarkTheme = new SolidBrush(AccentColor_DarkTheme); } }


        private Accent darkAccent_LightTheme;
        public Accent DarkAccent_LightTheme { get { return darkAccent_LightTheme; } set { darkAccent_LightTheme = value; DarkAccentColor_LightTheme = ((int)accent_LightTheme).ToColor(); DarkAccentPen_LightTheme = new Pen(DarkAccentColor_LightTheme); DarkAccentBrush_LightTheme = new SolidBrush(DarkAccentColor_LightTheme); } }

        private Accent darkAccent_DarkTheme;
        public Accent DarkAccent_DarkTheme { get { return darkAccent_DarkTheme; } set { darkAccent_DarkTheme = value; DarkAccentColor_DarkTheme = ((int)darkAccent_DarkTheme).ToColor(); DarkAccentPen_DarkTheme = new Pen(DarkAccentColor_DarkTheme); DarkAccentBrush_DarkTheme = new SolidBrush(DarkAccentColor_DarkTheme); } }

        private Accent lightAccent_LightTheme;
        public Accent LightAccent_LightTheme { get { return lightAccent_LightTheme; } set { lightAccent_LightTheme = value; LightAccentColor_LightTheme = ((int)lightAccent_LightTheme).ToColor(); LightAccentPen_LightTheme = new Pen(LightAccentColor_LightTheme); LightAccentBrush_LightTheme = new SolidBrush(LightAccentColor_LightTheme); } }

        private Accent lightAccent_DarkTheme;
        public Accent LightAccent_DarkTheme { get { return lightAccent_DarkTheme; } set { lightAccent_DarkTheme = value; LightAccentColor_DarkTheme = ((int)lightAccent_DarkTheme).ToColor(); LightAccentPen_DarkTheme = new Pen(LightAccentColor_DarkTheme); LightAccentBrush_DarkTheme = new SolidBrush(LightAccentColor_DarkTheme); } }


        private TextShade textShade;
        public TextShade TextShade { get { return textShade; } set { textShade = value; TextPen = new Pen(TextColor); TextColor= ((int)textShade).ToColor(); TextBrush= new SolidBrush(TextColor); } }

        public object Component { get; private set; }






        /// <summary>
        /// Defines the Color Scheme to be used for all forms.
        /// </summary>
        /// <param name="primary">The primary color, a -500 color is suggested here.</param>
        /// <param name="darkPrimary">A darker version of the primary color, a -700 color is suggested here.</param>
        /// <param name="lightPrimary">A lighter version of the primary color, a -100 color is suggested here.</param>
        /// <param name="accent">The accent color, a -200 color is suggested here.</param>
        /// <param name="lightaccent">The lighter version of the accent color, a ... color is suggested here.</param>
        /// <param name="darkaccent">The darker version of the accent color, a ... color is suggested here.</param>
        /// <param name="textShade">The text color, the one with the highest contrast is suggested.</param>
        public ColorScheme(Primary primary_DarkTheme, Primary darkPrimary_DarkTheme, Primary lightPrimary_DarkTheme, Primary primary_LightTheme, Primary darkPrimary_LightTheme, Primary lightPrimary_LightTheme,
            Accent accent_DarkTheme, Accent darkAccent_DarkTheme, Accent lightAccent_DarkTheme, Accent accent_LightTheme, Accent darkAccent_LightTheme, Accent lightAccent_LightTheme,
            TextShade textShade, string _name)
        {
            // Dark Theme
            //Color
            PrimaryColor_DarkTheme = ((int)primary_DarkTheme).ToColor();
            DarkPrimaryColor_DarkTheme = ((int)darkPrimary_DarkTheme).ToColor();
            LightPrimaryColor_DarkTheme = ((int)lightPrimary_DarkTheme).ToColor();
            AccentColor_DarkTheme = ((int)accent_DarkTheme).ToColor();
            LightAccentColor_DarkTheme = ((int)lightAccent_DarkTheme).ToColor();
            DarkAccentColor_DarkTheme = ((int)darkAccent_DarkTheme).ToColor();
            TextColor = ((int)textShade).ToColor();

            //Pen
            PrimaryPen_DarkTheme = new Pen(PrimaryColor_DarkTheme);
            DarkPrimaryPen_DarkTheme = new Pen(DarkPrimaryColor_DarkTheme);
            LightPrimaryPen_DarkTheme = new Pen(LightPrimaryColor_DarkTheme);
            AccentPen_DarkTheme = new Pen(AccentColor_DarkTheme);
            LightAccentPen_DarkTheme = new Pen(LightAccentColor_DarkTheme);
            DarkAccentPen_DarkTheme = new Pen(DarkAccentColor_DarkTheme);
            TextPen = new Pen(TextColor);

            //Brush
            PrimaryBrush_DarkTheme = new SolidBrush(PrimaryColor_DarkTheme);
            DarkPrimaryBrush_DarkTheme = new SolidBrush(DarkPrimaryColor_DarkTheme);
            LightPrimaryBrush_DarkTheme = new SolidBrush(LightPrimaryColor_DarkTheme);
            AccentBrush_DarkTheme = new SolidBrush(AccentColor_DarkTheme);
            LightAccentBrush_DarkTheme = new SolidBrush(LightAccentColor_DarkTheme);
            DarkAccentBrush_DarkTheme = new SolidBrush(DarkAccentColor_DarkTheme);
            TextBrush = new SolidBrush(TextColor);

            //Light Theme

            //Color
            PrimaryColor_LightTheme = ((int)primary_LightTheme).ToColor();
            DarkPrimaryColor_LightTheme = ((int)darkPrimary_LightTheme).ToColor();
            LightPrimaryColor_LightTheme = ((int)lightPrimary_LightTheme).ToColor();
            AccentColor_LightTheme = ((int)accent_LightTheme).ToColor();
            LightAccentColor_LightTheme = ((int)lightAccent_LightTheme).ToColor();
            DarkAccentColor_LightTheme = ((int)darkAccent_LightTheme).ToColor();
            TextColor = ((int)textShade).ToColor();

            //Pen
            PrimaryPen_LightTheme = new Pen(PrimaryColor_LightTheme);
            DarkPrimaryPen_LightTheme = new Pen(DarkPrimaryColor_LightTheme);
            LightPrimaryPen_LightTheme = new Pen(LightPrimaryColor_LightTheme);
            AccentPen_LightTheme = new Pen(AccentColor_LightTheme);
            LightAccentPen_LightTheme = new Pen(LightAccentColor_LightTheme);
            DarkAccentPen_LightTheme = new Pen(DarkAccentColor_LightTheme);
            TextPen = new Pen(TextColor);

            //Brush
            PrimaryBrush_LightTheme = new SolidBrush(PrimaryColor_LightTheme);
            DarkPrimaryBrush_LightTheme = new SolidBrush(DarkPrimaryColor_LightTheme);
            LightPrimaryBrush_LightTheme = new SolidBrush(LightPrimaryColor_LightTheme);
            AccentBrush_LightTheme = new SolidBrush(AccentColor_LightTheme);
            LightAccentBrush_LightTheme = new SolidBrush(LightAccentColor_LightTheme);
            DarkAccentBrush_LightTheme = new SolidBrush(DarkAccentColor_LightTheme);
            TextBrush = new SolidBrush(TextColor);

            //Name
            this.Name = _name;
        }
        public ColorScheme(Primary primary_DarkTheme, Primary darkPrimary_DarkTheme, Primary lightPrimary_DarkTheme, Primary primary_LightTheme, Primary darkPrimary_LightTheme, Primary lightPrimary_LightTheme, Primary utility1_DarkTheme, Primary utility1_LightTheme ,
          Accent accent_DarkTheme, Accent darkAccent_DarkTheme, Accent lightAccent_DarkTheme, Accent accent_LightTheme, Accent darkAccent_LightTheme, Accent lightAccent_LightTheme,
          TextShade textShade, string _name)
        {
            // Dark Theme
            //Color
            Utility1Color_DarkTheme = ((int)utility1_DarkTheme).ToColor();
            PrimaryColor_DarkTheme = ((int)primary_DarkTheme).ToColor();
            DarkPrimaryColor_DarkTheme = ((int)darkPrimary_DarkTheme).ToColor();
            LightPrimaryColor_DarkTheme = ((int)lightPrimary_DarkTheme).ToColor();
            AccentColor_DarkTheme = ((int)accent_DarkTheme).ToColor();
            LightAccentColor_DarkTheme = ((int)lightAccent_DarkTheme).ToColor();
            DarkAccentColor_DarkTheme = ((int)darkAccent_DarkTheme).ToColor();
            TextColor = ((int)textShade).ToColor();

            //Pen
            Utility1Pen_DarkTheme = new Pen(Utility1Color_DarkTheme);
            PrimaryPen_DarkTheme = new Pen(PrimaryColor_DarkTheme);
            DarkPrimaryPen_DarkTheme = new Pen(DarkPrimaryColor_DarkTheme);
            LightPrimaryPen_DarkTheme = new Pen(LightPrimaryColor_DarkTheme);
            AccentPen_DarkTheme = new Pen(AccentColor_DarkTheme);
            LightAccentPen_DarkTheme = new Pen(LightAccentColor_DarkTheme);
            DarkAccentPen_DarkTheme = new Pen(DarkAccentColor_DarkTheme);
            TextPen = new Pen(TextColor);

            //Brush
            Utility1Brush_DarkTheme = new SolidBrush(Utility1Color_DarkTheme);
            PrimaryBrush_DarkTheme = new SolidBrush(PrimaryColor_DarkTheme);
            DarkPrimaryBrush_DarkTheme = new SolidBrush(DarkPrimaryColor_DarkTheme);
            LightPrimaryBrush_DarkTheme = new SolidBrush(LightPrimaryColor_DarkTheme);
            AccentBrush_DarkTheme = new SolidBrush(AccentColor_DarkTheme);
            LightAccentBrush_DarkTheme = new SolidBrush(LightAccentColor_DarkTheme);
            DarkAccentBrush_DarkTheme = new SolidBrush(DarkAccentColor_DarkTheme);
            TextBrush = new SolidBrush(TextColor);

            //Light Theme

            //Color
            Utility1Color_LightTheme = ((int)utility1_LightTheme).ToColor();
            PrimaryColor_LightTheme = ((int)primary_LightTheme).ToColor();
            DarkPrimaryColor_LightTheme = ((int)darkPrimary_LightTheme).ToColor();
            LightPrimaryColor_LightTheme = ((int)lightPrimary_LightTheme).ToColor();
            AccentColor_LightTheme = ((int)accent_LightTheme).ToColor();
            LightAccentColor_LightTheme = ((int)lightAccent_LightTheme).ToColor();
            DarkAccentColor_LightTheme = ((int)darkAccent_LightTheme).ToColor();
            TextColor = ((int)textShade).ToColor();

            //Pen
            Utility1Pen_LightTheme = new Pen(Utility1Color_LightTheme);
            PrimaryPen_LightTheme = new Pen(PrimaryColor_LightTheme);
            DarkPrimaryPen_LightTheme = new Pen(DarkPrimaryColor_LightTheme);
            LightPrimaryPen_LightTheme = new Pen(LightPrimaryColor_LightTheme);
            AccentPen_LightTheme = new Pen(AccentColor_LightTheme);
            LightAccentPen_LightTheme = new Pen(LightAccentColor_LightTheme);
            DarkAccentPen_LightTheme = new Pen(DarkAccentColor_LightTheme);
            TextPen = new Pen(TextColor);

            //Brush
            Utility1Brush_LightTheme = new SolidBrush(Utility1Color_LightTheme);
            PrimaryBrush_LightTheme = new SolidBrush(PrimaryColor_LightTheme);
            DarkPrimaryBrush_LightTheme = new SolidBrush(DarkPrimaryColor_LightTheme);
            LightPrimaryBrush_LightTheme = new SolidBrush(LightPrimaryColor_LightTheme);
            AccentBrush_LightTheme = new SolidBrush(AccentColor_LightTheme);
            LightAccentBrush_LightTheme = new SolidBrush(LightAccentColor_LightTheme);
            DarkAccentBrush_LightTheme = new SolidBrush(DarkAccentColor_LightTheme);
            TextBrush = new SolidBrush(TextColor);

            //Name
            this.Name = _name;
        }
        public ColorScheme(Primary primary_DarkTheme, Primary darkPrimary_DarkTheme, Primary lightPrimary_DarkTheme, Primary primary_LightTheme, Primary darkPrimary_LightTheme, Primary lightPrimary_LightTheme,Primary utility1_DarkTheme,Primary utility1_LightTheme, Primary utility2_DarkTheme, Primary utility2_LightTheme,
           Accent accent_DarkTheme, Accent darkAccent_DarkTheme, Accent lightAccent_DarkTheme, Accent accent_LightTheme, Accent darkAccent_LightTheme, Accent lightAccent_LightTheme,
           TextShade textShade, string _name)
        {
            // Dark Theme
            //Color
            Utility1Color_DarkTheme = ((int)utility1_DarkTheme).ToColor();
            Utility2Color_DarkTheme = ((int)utility2_DarkTheme).ToColor();
            PrimaryColor_DarkTheme = ((int)primary_DarkTheme).ToColor();
            DarkPrimaryColor_DarkTheme = ((int)darkPrimary_DarkTheme).ToColor();
            LightPrimaryColor_DarkTheme = ((int)lightPrimary_DarkTheme).ToColor();
            AccentColor_DarkTheme = ((int)accent_DarkTheme).ToColor();
            LightAccentColor_DarkTheme = ((int)lightAccent_DarkTheme).ToColor();
            DarkAccentColor_DarkTheme = ((int)darkAccent_DarkTheme).ToColor();
            TextColor = ((int)textShade).ToColor();

            //Pen
            Utility1Pen_DarkTheme = new Pen(Utility1Color_DarkTheme);
            Utility2Pen_DarkTheme = new Pen(Utility2Color_DarkTheme);
            PrimaryPen_DarkTheme = new Pen(PrimaryColor_DarkTheme);
            DarkPrimaryPen_DarkTheme = new Pen(DarkPrimaryColor_DarkTheme);
            LightPrimaryPen_DarkTheme = new Pen(LightPrimaryColor_DarkTheme);
            AccentPen_DarkTheme = new Pen(AccentColor_DarkTheme);
            LightAccentPen_DarkTheme = new Pen(LightAccentColor_DarkTheme);
            DarkAccentPen_DarkTheme = new Pen(DarkAccentColor_DarkTheme);
            TextPen = new Pen(TextColor);

            //Brush
            Utility1Brush_DarkTheme = new SolidBrush(Utility1Color_DarkTheme);
            Utility2Brush_DarkTheme = new SolidBrush(Utility2Color_DarkTheme);
            PrimaryBrush_DarkTheme = new SolidBrush(PrimaryColor_DarkTheme);
            DarkPrimaryBrush_DarkTheme = new SolidBrush(DarkPrimaryColor_DarkTheme);
            LightPrimaryBrush_DarkTheme = new SolidBrush(LightPrimaryColor_DarkTheme);
            AccentBrush_DarkTheme = new SolidBrush(AccentColor_DarkTheme);
            LightAccentBrush_DarkTheme = new SolidBrush(LightAccentColor_DarkTheme);
            DarkAccentBrush_DarkTheme = new SolidBrush(DarkAccentColor_DarkTheme);
            TextBrush = new SolidBrush(TextColor);

            //Light Theme

            //Color
            Utility1Color_LightTheme = ((int)utility1_LightTheme).ToColor();
            Utility2Color_LightTheme = ((int)utility2_LightTheme).ToColor();
            PrimaryColor_LightTheme = ((int)primary_LightTheme).ToColor();
            DarkPrimaryColor_LightTheme = ((int)darkPrimary_LightTheme).ToColor();
            LightPrimaryColor_LightTheme = ((int)lightPrimary_LightTheme).ToColor();
            AccentColor_LightTheme = ((int)accent_LightTheme).ToColor();
            LightAccentColor_LightTheme = ((int)lightAccent_LightTheme).ToColor();
            DarkAccentColor_LightTheme = ((int)darkAccent_LightTheme).ToColor();
            TextColor = ((int)textShade).ToColor();

            //Pen
            Utility1Pen_LightTheme = new Pen(Utility1Color_LightTheme);
            Utility2Pen_LightTheme = new Pen(Utility2Color_LightTheme);
            PrimaryPen_LightTheme = new Pen(PrimaryColor_LightTheme);
            DarkPrimaryPen_LightTheme = new Pen(DarkPrimaryColor_LightTheme);
            LightPrimaryPen_LightTheme = new Pen(LightPrimaryColor_LightTheme);
            AccentPen_LightTheme = new Pen(AccentColor_LightTheme);
            LightAccentPen_LightTheme = new Pen(LightAccentColor_LightTheme);
            DarkAccentPen_LightTheme = new Pen(DarkAccentColor_LightTheme);
            TextPen = new Pen(TextColor);

            //Brush
            Utility1Brush_LightTheme = new SolidBrush(Utility1Color_LightTheme);
            Utility2Brush_LightTheme = new SolidBrush(Utility2Color_LightTheme);
            PrimaryBrush_LightTheme = new SolidBrush(PrimaryColor_LightTheme);
            DarkPrimaryBrush_LightTheme = new SolidBrush(DarkPrimaryColor_LightTheme);
            LightPrimaryBrush_LightTheme = new SolidBrush(LightPrimaryColor_LightTheme);
            AccentBrush_LightTheme = new SolidBrush(AccentColor_LightTheme);
            LightAccentBrush_LightTheme = new SolidBrush(LightAccentColor_LightTheme);
            DarkAccentBrush_LightTheme = new SolidBrush(DarkAccentColor_LightTheme);
            TextBrush = new SolidBrush(TextColor);

            //Name
            this.Name = _name;
        }

        public ColorScheme()
        {

        }
    }

    public static class ColorExtension
    {
        /// <summary>
        /// Convert an integer number to a Color.
        /// </summary>
        /// <returns></returns>
        public static Color ToColor(this int argb)
        {
            return Color.FromArgb(
                (argb & 0xff0000) >> 16,
                (argb & 0xff00) >> 8,
                 argb & 0xff);
        }

        /// <summary>
        /// Removes the alpha component of a color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color RemoveAlpha(this Color color)
        {
            return Color.FromArgb(color.R, color.G, color.B);
        }

        /// <summary>
        /// Converts a 0-100 integer to a 0-255 color component.
        /// </summary>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static int PercentageToColorComponent(this int percentage)
        {
            return (int)((percentage / 100d) * 255d);
        }
    }

    //Color constantes
    public enum TextShade
    {
        WHITE = 0xFFFFFF,
        BLACK = 0x212121
    }

    public enum Primary
    {
      
        Gray900 = 0x212121,
        DarkGray900 = 0x000000,
        LightGray900 = 0x484848,
        RayanRed = 0xf7725a,
        ErrorRed = 0xcb1a1a,
        RayanGreen = 0x9acd32,
        RayanGreen1=0x729925,
        RayanOrange = 0xff8c00,
        Gray300=0xe0e0e0,
        LightGray300=0xffffff,
        DarkGray300=0xaeaeae,
        //
        RayanGray1=0xe2e2e2,
        RayanGray2=0xc5c5c5,
        Gray400=0xbdbdbd,
        DarkGray700=0x373737,
        Gray600=0x757575,
        // MessageBox Popup[deactive tab=RayanGray4, back=RayanGray3] 
        RayanGray3 = 0x8a8a8a,
        RayanGray4 = 0xa3a3a3,

        //TradeOrder Tabpages[back(dark)=RaaynGray5, back(light)=RayanGray6]
        RayanGray5 = 0x303030,
        RayanGray6=0xbfbfbf,
        RayanGray7=0xcfcfcf,
        RayanGray8=0x404040,
        RayanGray9=0xe3e3e3,
        RayanGray10=0xf2f2f2,

        RayanLineDark = 0x505050,
        RayanLineLight = 0xdcdcdc



    }

    public enum Accent
    {
        
        Bule400 = 0x42a5f5,
        LightBlue400 = 0x80d6ff,
        DarkBlue400 = 0x0077c2,
        Yellow500 = 0xffeb3b,
        LightYellow500 = 0xffff72,
        DarkYellow500 = 0xc8b900,
        Cyan500 = 0x00bcd4,
        LightCyan500 = 0x62efff,
        DarkCyan500 = 0x008ba3,

    }

    public enum ColorCategory
    {
        Primary,
        LightPrimary,
        DarkPrimary,
        Utility1,
        Utility2,
        ApplicationBackGround,
        TextColor,
        DarkThemeTextColor,
        LightThemeTextColor,
        Secondary,
        LightSecondary,
        DarkSecondary

    }
    public enum BrushCategory
    {
        PrimaryBrush,
        LightPrimaryBrush,
        DarkPrimaryBrush,
        Utility1Brush,
        Utility2Brush,
        ApplicationBackGround,
        DarkThemeTextColor,
        TextColor,
        LightThemeTextColor,
        SecondaryBrush,
        LightSecondaryBrush,
        DarkSecondaryBrush
    }
   
   

}
