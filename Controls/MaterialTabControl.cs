using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialTabControl : TabControl, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        private MaterialSkinManager skinmanager = null;
       
        private MaterialTabPageCollection materialTabPages = new MaterialTabPageCollection();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public MaterialTabPageCollection MaterialTabPages
        {
            get { return materialTabPages; }
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
        private MaterialTabPage clickedTab;
        public MaterialTabPage ClickedTab { get { return clickedTab; } set { clickedTab = value; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        public MaterialTabControl()
        {

            materialTabPages.insertEvent += new EventHandler(UpdateTabPages);
            materialTabPages.removeEvent += new EventHandler(UpdateTabPages);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x1328 && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }
        protected virtual void UpdateTabPages(object sender, EventArgs e)
        {

            var selectedTabIndex = this.SelectedIndex;
            TabPage selectedtab = new TabPage();

            if (this.materialTabPages.Count == 1)
                this.SelectedTab = this.MaterialTabPages[0];

            if (this.materialTabPages.Count > 0)
                selectedtab = this.SelectedTab;

            this.TabPages.Clear();

            if (MaterialTabPages != null && MaterialTabPages.Count > 0)
                foreach (MaterialTabPage mtp in materialTabPages)
                    this.TabPages.Add(mtp);
            if (MaterialTabPages == null || MaterialTabPages.Count <= 1)
                selectedTabIndex = 0;


            //if (this.materialTabPages.Count > 0 && this.SelectedTab != null && selectedTabIndex != 0)
            if (this.Contains(selectedtab)) this.SelectedTab = selectedtab;

            else this.SelectedIndex = selectedTabIndex;
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
    public class MaterialTabPageCollection : Collection<MaterialTabPage>
    {
        public event EventHandler insertEvent;
        public event EventHandler removeEvent;
        public event EventHandler changeTabPagesEvent;
        protected override void InsertItem(int index, MaterialTabPage item)
        {

            if (insertEvent != null)
            {
                base.InsertItem(index, item);
                insertEvent(this, new EventArgs());

            }
            if (changeTabPagesEvent != null)
            {
              
                changeTabPagesEvent(this, new EventArgs());

            }

        }
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            removeEvent(this, new EventArgs());
        }
     
    }

}
