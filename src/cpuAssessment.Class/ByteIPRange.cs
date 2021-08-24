using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace cpuAssessment.Class
{
    public class ByteIPRange
    {
        public List<ByteIP> ipList = new List<ByteIP>();
        public int q1Start { get; set; }
        public int q1End { get; set; }
        public int q2Start { get; set; }
        public int q2End { get; set; }
        public int q3Start { get; set; }
        public int q3End { get; set; }
        public int q4Start { get; set; }
        public int q4End { get; set; }

        public ByteIP[] GenerateList()
        {
            for (int q1Temp = q1Start; q1Temp <= q1End; q1Temp++)
            {
                for (int q2Temp = q2Start; q2Temp <= q2End; q2Temp++)
                {
                    for (int q3Temp = q3Start; q3Temp <= q3End; q3Temp++)
                    {
                        for (int q4Temp = q4Start; q4Temp <= q4End; q4Temp++)
                        {
                            ipList.Add(new ByteIP(q1Temp, q2Temp, q3Temp, q4Temp));
                        }
                    }
                }
            }

            return ipList.ToArray();
        }

        public IEnumerable<ByteIP> Coroutine()
        {
            for (int q1Temp = q1Start; q1Temp <= q1End; q1Temp++)
            {
                for (int q2Temp = q2Start; q2Temp <= q2End; q2Temp++)
                {
                    for (int q3Temp = q3Start; q3Temp <= q3End; q3Temp++)
                    {
                        for (int q4Temp = q4Start; q4Temp <= q4End; q4Temp++)
                        {
                            yield return new ByteIP(q1Temp, q2Temp, q3Temp, q4Temp);
                        }
                    }
                }
            }
        }
    }
}