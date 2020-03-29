using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Economic_v2
{
    class TempData
    {
        private static Window window;

        private TempData() { }

        public TempData(Window w)
        {
            if(window == null)
            {
                window = w;
            }
        }

        public static Window GetWindow()
        {
            return window;
        }
    }
}
