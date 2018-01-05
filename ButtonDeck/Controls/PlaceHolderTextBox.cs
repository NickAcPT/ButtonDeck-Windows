using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ButtonDeck.Controls
{
    class PlaceHolderTextBox : System.Windows.Forms.TextBox
    {
        Color DefaultColor { get; set; }
        public string PlaceHolderText { get; set; }
        public PlaceHolderTextBox(string placeholdertext)
        {
            // get default color of text
            DefaultColor = this.ForeColor;
            // Add event handler for when the control gets focus
            this.GotFocus += (object sender, EventArgs e) => {
                this.Text = String.Empty;
                this.ForeColor = DefaultColor;
            };

            // add event handling when focus is lost
            this.LostFocus += (Object sender, EventArgs e) => {
                if (String.IsNullOrEmpty(this.Text) || Text == PlaceHolderText) {
                    this.ForeColor = System.Drawing.Color.Gray;
                    this.Text = PlaceHolderText;
                } else {
                    this.ForeColor = DefaultColor;
                }
            };



            if (!string.IsNullOrEmpty(placeholdertext)) {
                // change style   
                this.ForeColor = System.Drawing.Color.Gray;
                // Add text
                PlaceHolderText = placeholdertext;
                Text = placeholdertext;
            }



        }

    }

}
