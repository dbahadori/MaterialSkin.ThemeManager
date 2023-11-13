using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialSkin;
using System.ComponentModel;

namespace MaterialSkin
{
    public static class SkinManagerContainer
    {
       

        private static List<MaterialSkinManager> skinManagerCollection;
        public static void Load()
        {
            skinManagerCollection = new List<MaterialSkinManager>();
        }
        public static MaterialSkinManager getSkinManager(string skinManagerName)
        {
            return skinManagerCollection.FirstOrDefault(skm => skm.Name == skinManagerName);  
        }
        internal static void AddSkinManager(MaterialSkinManager skinManager)
        {
            if(skinManagerCollection==null)
                skinManagerCollection = new List<MaterialSkinManager>();
            skinManagerCollection.Add(skinManager);
        }
        public static List<MaterialSkinManager> getAllSkinManager()
        {
            return skinManagerCollection;
        }
    }
}
