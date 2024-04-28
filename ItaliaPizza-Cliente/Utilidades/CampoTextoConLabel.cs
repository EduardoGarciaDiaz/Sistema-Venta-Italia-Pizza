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
        public TextBox TextBox { get; set; }
        public Label LabelError { get; set; }

        public CampoTextoConLabel(TextBox textBox, Label labelError)
        {
            this.TextBox = textBox;
            this.LabelError = labelError;
        }
    }
}
