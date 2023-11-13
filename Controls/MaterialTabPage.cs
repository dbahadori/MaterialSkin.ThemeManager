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
    public class MaterialTabPage:TabPage
    {
        private Image tabIcon;
        [Browsable(false)]
        public Image TabIcon
        {
            get { return tabIcon; }
            set { tabIcon = value; /*Invalidate();*/ }
        }
         
        private ObjectSide iconPosition;
        [Browsable(false)]
        public ObjectSide IconPosition
        {
            get { return iconPosition; }
            set { iconPosition = value; /*Invalidate();*/ }
        }
        private bool enabledCloseButton;
        public bool EnabledCloseButton { get { return enabledCloseButton; } set { enabledCloseButton = value;Invalidate(); } }
        private string itemCount;
        public string ItemCount { get { return itemCount; } set { itemCount = value; Invalidate(); } }
        private bool enabledItemCount;
        public bool EnabledItemCount { get { return enabledItemCount; }set { enabledItemCount = value; Invalidate(); } }
        public MaterialTabPage()
        {
            IconPosition = ObjectSide.Left;
        }
    }
}
