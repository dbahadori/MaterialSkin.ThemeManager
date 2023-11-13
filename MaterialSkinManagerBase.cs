using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialSkin
{
    public abstract class MaterialSkinManagerBase : ISkinManager
    {
        protected abstract Color GetPrimaryTextBrushtest();

        Color ISkinManager.GetPrimaryTextBrushtest()
        {
            throw new NotImplementedException();
        }
    }
}
