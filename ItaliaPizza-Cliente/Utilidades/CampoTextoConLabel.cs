using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ItaliaPizza_Cliente.Utilidades
{
    public class CampoTextoConLabel
    {
        public TextBox textBox { get; set; }
        public Label labelError { get; set; }

        public CampoTextoConLabel(TextBox textBox, Label labelError)
        {
            this.textBox = textBox;
            this.labelError = labelError;
        }
    }
}
