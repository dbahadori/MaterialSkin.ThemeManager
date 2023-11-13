using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MaterialSkin.Controls
{
    public class MaterialVisualStyleElement:UserControl
    {
        private VisualStyleRenderer renderer;
        private Dictionary<string, VisualStyleElement> elementDictionary =
            new Dictionary<string, VisualStyleElement>();
        private string elementName = "ExplorerBar.SpecialGroupCollapse.Normal";
        public string ElementName { get { return elementName; } set { elementName = value; Invalidate(); } }

       
        
        public MaterialVisualStyleElement()
            {
            this.Size = new Size(20, 20); 

            SetStyle(ControlStyles.Selectable, false);
            SetupElementCollection();
        }
        private void SetupElementCollection()
        {
            StringBuilder elementName = new StringBuilder();
            VisualStyleElement currentElement;
            int plusSignIndex = 0;

            // Get array of first-level nested types within 
            // VisualStyleElement; these are the element classes.
            Type[] elementClasses =
                typeof(VisualStyleElement).GetNestedTypes();

            foreach (Type elementClass in elementClasses)
            {
                // Get an array of second-level nested types within
                // VisualStyleElement; these are the element parts.
                Type[] elementParts = elementClass.GetNestedTypes();

                // Get the index of the first '+' character in 
                // the full element class name.
                plusSignIndex = elementClass.FullName.IndexOf('+');

                foreach (Type elementPart in elementParts)
                {
                    // Get an array of static property details 
                    // for  the current type. Each of these types have 
                    // properties that return VisualStyleElement objects.
                    PropertyInfo[] elementProperties =
                        elementPart.GetProperties(BindingFlags.Static |
                        BindingFlags.Public);

                    // For each property, insert the unique full element   
                    // name and the element into the collection.
                    foreach (PropertyInfo elementProperty in
                        elementProperties)
                    {
                        // Get the element.
                        currentElement =
                            (VisualStyleElement)elementProperty.
                            GetValue(null, BindingFlags.Static, null,
                            null, null);

                        // Append the full element name.
                        elementName.Append(elementClass.FullName,
                            plusSignIndex + 1,
                            elementClass.FullName.Length -
                            plusSignIndex - 1);
                        elementName.Append("." +
                            elementPart.Name.ToString() + "." +
                            elementProperty.Name);

                        // Add the element and element name to 
                        // the Dictionary.
                        elementDictionary.Add(elementName.ToString(),
                            currentElement);

                        // Clear the element name for the 
                        // next iteration.
                        elementName.Remove(0, elementName.Length);
                    }
                }
            }
        }
        public void CustomControl()
            {

           
            VisualStyleElement element = elementDictionary.FirstOrDefault(e => e.Key == ElementName).Value;

            // this.Location = new Point(50, 50);
            if (AutoSize) this.Size = new Size(20, 20);
            else
                Size = this.Size;

                //this.BackColor = SystemColors.ActiveBorder;

                if (Application.RenderWithVisualStyles &&
                    VisualStyleRenderer.IsElementDefined(element))
                {
                    renderer = new VisualStyleRenderer(element);
                }
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                CustomControl();
                // Draw the element if the renderer has been set.
                if (renderer != null)
                {
                    renderer.DrawBackground(e.Graphics, this.ClientRectangle);
                }

                // Visual styles are disabled or the element is undefined, 
                // so just draw a message.
                else
                {
                    this.Text = "Visual styles are disabled.";
                    TextRenderer.DrawText(e.Graphics, this.Text, this.Font,
                        new Point(0, 0), this.ForeColor);
                }
            }
        }
    
}
