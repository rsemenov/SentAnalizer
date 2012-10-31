using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace TestMe
{
    public class Test
    {
        [DllExport("Add", CallingConvention = CallingConvention.StdCall)]
        public static int Add(int left, int right)
        {
            return left + right;
        }
    }
}
