using System;
using System.Collections.Generic;
using System.Text;
using RGiesecke.DllExport;

namespace Test
{
   internal static class UnmanagedExports
   {
      [DllExport("adddays", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
      static double AddDays(double dateValue, int days)
      {
         return DateTime.FromOADate(dateValue).AddDays(days).ToOADate();
      }

      [DllExport("add", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
      static int Add()
       {
           return 12;
       }
   }
}
