using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3
{
    public class Data
    {
        public List<WzorzecNagrania> patterns { get; set; } = new List<WzorzecNagrania>();
    }

    public class WzorzecNagrania
    {
        public string Number { get; set; } 
        public string FilePath { get; set; }

        public List<double[]> FFT { get; set; }
    }


}
