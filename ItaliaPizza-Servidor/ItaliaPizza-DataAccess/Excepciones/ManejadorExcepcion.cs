﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess.Excepciones
{
    public class ManejadorExcepcion
    {
        public static void ManejarExcepcionError(Exception ex)
        {
            Console.WriteLine(ex.Source + " - " + ex.Message + "\n" + ex.StackTrace + "\n");
        }

        public static void ManejarExcepcionFatal(Exception ex)
        {
            Console.WriteLine(ex.Source + " - " + ex.Message + "\n" + ex.StackTrace + "\n");
        }
    }
}
