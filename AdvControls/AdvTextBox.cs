using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AdvControls
{
    public class AdvTextBox : TextBox
    {
        private string _defaultText = "Enter value";

        public string DefaultText
        {
            get { return _defaultText; }
            set
            {
                _defaultText = value;
                SetDefaultText();
            }
        }

        public AdvTextBox()
        {
            this.GotFocus += (sender, e) =>
            { if (this.Text.Equals(DefaultText)) { this.Text = String.Empty; } };
            this.LostFocus += (sender, e) => { SetDefaultText(); };
        }

        private void SetDefaultText()
        {
            if (this.Text.Trim().Length == 0)
            {
                this.Text = DefaultText;
            }
        }
    }
}
