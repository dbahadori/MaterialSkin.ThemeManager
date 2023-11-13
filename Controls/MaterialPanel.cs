using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using MaterialSkin;

namespace MaterialSkin.Controls
{
    public class MaterialPanel : Panel, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(true)]
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
        [Browsable(true)]
        public int StatusBarHeight { get; set; }

        [Browsable(true)]
        public int ActionBarHeight { get; set; }
        private PanelParts themedareas;
        public PanelParts ThemedAreas { get { return themedareas; } set { themedareas = value; } }
        #region Join To Theme
        // Actionbar 


        private bool actionBarJoinToTheme;

        public bool ActionBarJoinToTheme { get { return actionBarJoinToTheme; } set { actionBarJoinToTheme = value; Invalidate(); } }
        public ColorCategory ActionBarColor { get; set; }

        private bool statusBarJoinToTheme;
        public bool StatusBarJoinToTheme { get { return statusBarJoinToTheme; } set { statusBarJoinToTheme = value; Invalidate(); } }
        public ColorCategory StatusBarColor { get; set; }

        private bool bodyJoinToTheme;
        public bool BodyJoinToTheme { get { return bodyJoinToTheme; } set { bodyJoinToTheme = value; Invalidate(); } }
        public ColorCategory BodyColor { get; set; }



        //private bool bodyJoinToTheme;
        //public bool BodyJoinToTheme { get { return bodyJoinToTheme; } set { bodyJoinToTheme = value; Invalidate(); } }
        //public BrushCategory BodyBrush { get; set; }

        #endregion
        //private const int STATUS_BAR_BUTTON_WIDTH = STATUS_BAR_HEIGHT;

        public MaterialPanel()
        {
            this.ResizeRedraw = true;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);
            ActionBarColor = ColorCategory.DarkPrimary;
            ActionBarJoinToTheme = true;
            StatusBarJoinToTheme = true;
            BodyJoinToTheme = true;
            ThemedAreas = PanelParts.All;

            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;
        }

        [Browsable(false)]
        public MouseState MouseState { get; set; }


        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (DesignMode) return;


            base.OnMouseUp(e);
            //  ReleaseCapture();
        }
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);

        }
        private void SetPartsJoinToTheme(PanelParts themeareas)
        {
           
            BodyJoinToTheme = !((themedareas & PanelParts.Body).Equals(PanelParts.None));
            ActionBarJoinToTheme=!((themedareas & PanelParts.Actionbar).Equals(PanelParts.None));
            StatusBarJoinToTheme = !((themedareas & PanelParts.Statusbar).Equals(PanelParts.None));

        }
       
        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            
            //if the local color scheme does not inherit from base color scheme
            if (selectedColorScheme == null)
                SelectedColorScheme = SkinManager.ColorScheme;


            var g = e.Graphics;

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            var x = 0;
            var y = 0;
            var w = this.Size.Width;
           

            if (!BodyJoinToTheme)
                {
                g.Clear(getColorDisjoinFromTheme(BodyColor));
                BackColor = getColorDisjoinFromTheme(BodyColor);
            }
            else { g.Clear(SkinManager.GetLightPrimaryColor(selectedColorScheme)); BackColor = SkinManager.GetLightPrimaryColor(selectedColorScheme); }

            // BackColor = Parent.BackColor;
            g.FillRectangle(SkinManager.GetDarkPrimaryBrush(selectedColorScheme), x, y, w, StatusBarHeight);

            if (!StatusBarJoinToTheme)
                g.FillRectangle(getBrushDisjoinFromTheme(StatusBarColor), x, y, w, StatusBarHeight);
            else {
                g.FillRectangle(SkinManager.GetPrimaryBrush(selectedColorScheme), x, y, w, StatusBarHeight);
                StatusBarColor = ColorCategory.DarkPrimary;
            }

            if (!ActionBarJoinToTheme)
                g.FillRectangle(getBrushDisjoinFromTheme(ActionBarColor), x, y + StatusBarHeight, w, ActionBarHeight);
            else { ActionBarColor = ColorCategory.DarkPrimary; g.FillRectangle(SkinManager.GetDarkPrimaryBrush(selectedColorScheme), x, y + StatusBarHeight, w, ActionBarHeight); }

            

        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // BackColor = SkinManager.GetApplicationBackgroundColor();
            // BackColor = Parent.BackColor;
            ForeColor = SkinManager.GetPrimaryTextColor();
            //Font = SkinManager.ROBOTO_REGULAR_11;**

            BackColorChanged += (sender, args) => ForeColor = SkinManager.GetPrimaryTextColor();
           // SetPartsJoinToTheme(ThemedAreas);
        }

        private void SetColorScheme(ColorScheme selectedColorScheme)
        {
            //SkinManager.ColorScheme = selectedColorScheme;
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
//public enum PanelParts 
//    {
//    All=0x7,
//    Statusbar = 0x1,
//    Actionbar=0x2,
//    Body=0x4,
//    None=0x0

//}
public struct PanelParts
{
    private int InternalValue { get; set; }
    private static readonly int all=0x7;
    private static readonly int statusbar = 0x1;
    private static readonly int actionbar = 0x2;
    private static readonly int body = 0x4;
    private static readonly int none = 0x0;

    public static  PanelParts All { get { PanelParts panelpart = PanelParts.all; return panelpart; } }
    public static PanelParts Statusbar { get { PanelParts panelpart = PanelParts.statusbar; return panelpart; } }
    public static PanelParts Actionbar { get { PanelParts panelpart = PanelParts.actionbar; return panelpart; } }
    public static PanelParts Body { get { PanelParts panelpart = PanelParts.body; return panelpart; } }
    public static PanelParts None { get { PanelParts panelpart = PanelParts.none; return panelpart; } }

    public override bool Equals(object obj)
    {
        PanelParts otherObj = (PanelParts)obj;
        return otherObj.InternalValue.Equals(this.InternalValue);
    }
    public static PanelParts operator +(PanelParts left, PanelParts right)
    {
        if (left.InternalValue == 7 || right.InternalValue == 7)
            return 0x7;
        return (left.InternalValue | right.InternalValue);
    }
    public static PanelParts operator &(PanelParts left, PanelParts right)
    {
       
        return (left.InternalValue & right.InternalValue);
    }
    public static implicit operator PanelParts(int otherType)
    {
        return new PanelParts
        {
            InternalValue = otherType
        };
    }
}
