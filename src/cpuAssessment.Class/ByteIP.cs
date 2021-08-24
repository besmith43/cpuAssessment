using System;

namespace cpuAssessment.Class
{
    public class ByteIP
    {
        public byte q1 { get; set; }
        public byte q2 { get; set; }
        public byte q3 { get; set; }
        public byte q4 { get; set; }

        public ByteIP(int _q1, int _q2, int _q3, int _q4)
        {
            q1 = (byte) _q1;
            q2 = (byte) _q2;
            q3 = (byte) _q3;
            q4 = (byte) _q4;
        }

        public Int64 ToInt64()
        {
            return ((int) q1 * 1000000000) + ((int) q2 * 1000000) + ((int) q3 * 1000) + (int) q4;
        }
    }
}