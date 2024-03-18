using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza_Cliente.Utilidades
{
    public static class Utilidad
    {
        public static float ConvertirStringAFloat(string numeroEnString, Label lbError)
        {
            float numeroConvertido = -1;

            try
            {
                numeroConvertido = Convert.ToSingle(numeroEnString);
            }
            catch (FormatException)
            {
                string mensajeErrorFormato = "Solo puede incluir números. Ej. 2, .5";
                MostrarTextoError(lbError, mensajeErrorFormato);
            }
            catch (OverflowException)
            {
                string mensajeErrorOverFlow = "Cantidad no permitida, por favor corrigelo.";
                MostrarTextoError(lbError, mensajeErrorOverFlow);
            }

            return numeroConvertido;
        }

        public static void MostrarTextoError(FrameworkElement elemento, string mensajeError)
        {
            if (elemento != null)
            {
                if (elemento is Label label)
                {
                    label.Content = mensajeError;
                }
                else if (elemento is TextBlock textBlock)
                {
                    textBlock.Text = mensajeError;
                }

                elemento.Visibility = Visibility.Visible;
            }
        }
    }
}
