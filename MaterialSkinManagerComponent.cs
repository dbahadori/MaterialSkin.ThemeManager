using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialSkin
{
    public class MaterialSkinManagerComponent:MaterialSkinManager
    {
        
       
        public override Themes GlobalTheme { get { return MaterialSkinManager.Instance.GlobalTheme; }  }
        private Themes theme;
        public override Themes Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                UpdateBackgrounds();
                OnPropertyChanged("ApplicationTheme");
            }
        }
        public MaterialSkinManagerComponent():base() {
            //base.PropertyChanged+= skinmanagerBase_PropertyChanged;
            //globalTheme = base.GlobalTheme;
        }
        //private void skinmanagerBase_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    globalTheme = MaterialSkinManager.Instance.GlobalTheme;
        //}

    }
}
