using System;
using System.Threading.Tasks;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics.Arm;

namespace cpuAssessment.Class
{
    public class Class1
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public unsafe void RunAVX2()
        {
            int item1 = 8192;
            int item2 = 4096;
            Vector256<int> a = Avx2.LoadVector256(&item1);
            Vector256<int> b = Avx2.LoadVector256(&item2);
            var result = Avx2.Add(a, b);
            //Console.WriteLine(result);
        }

        public bool FindIPSerial(ByteIP ipCanidate, ByteIP[] ipPool)
        {
            bool containsFlag = false;

            foreach(ByteIP tempIP in ipPool)
            {
                bool q1Equal = ipCanidate.q1 == tempIP.q1;
                bool q2Equal = ipCanidate.q2 == tempIP.q2;
                bool q3Equal = ipCanidate.q3 == tempIP.q3;
                bool q4Equal = ipCanidate.q4 == tempIP.q4;

                if (q1Equal && q2Equal && q3Equal && q4Equal)
                {
                    containsFlag = true;
                }
            }

            return containsFlag;
        }

        public bool FindIPParallel(ByteIP ipCanidate, ByteIP[] ipPool, ParallelOptions numThreads)
        {
            bool containsFlag = false;

            Parallel.ForEach(ipPool, numThreads, (tempIP) => {
                bool q1Equal = ipCanidate.q1 == tempIP.q1;
                bool q2Equal = ipCanidate.q2 == tempIP.q2;
                bool q3Equal = ipCanidate.q3 == tempIP.q3;
                bool q4Equal = ipCanidate.q4 == tempIP.q4;

                if (q1Equal && q2Equal && q3Equal && q4Equal)
                {
                    containsFlag = true;
                }
            });

            return containsFlag;
        }

        public unsafe bool FindIPAVX2Serial(ByteIP ipCandidate, ByteIP[] ipPool)
        {
            bool containsFlag = false;
            int i = 0;

            if (Avx2.IsSupported)
            {
                Vector256<byte> ipVector = Vector256.Create(
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4
                );

                for (i = 0; i + 7 < ipPool.Length; i = i + 8)
                {
                    Vector256<byte> comparables = Vector256.Create(
                        ipPool[i].q1, ipPool[i].q2, ipPool[i].q3, ipPool[i].q4,
                        ipPool[i+1].q1, ipPool[i+1].q2, ipPool[i+1].q3, ipPool[i+1].q4,
                        ipPool[i+2].q1, ipPool[i+2].q2, ipPool[i+2].q3, ipPool[i+2].q4,
                        ipPool[i+3].q1, ipPool[i+3].q2, ipPool[i+3].q3, ipPool[i+3].q4,
                        ipPool[i+4].q1, ipPool[i+4].q2, ipPool[i+4].q3, ipPool[i+4].q4,
                        ipPool[i+5].q1, ipPool[i+5].q2, ipPool[i+5].q3, ipPool[i+5].q4,
                        ipPool[i+6].q1, ipPool[i+6].q2, ipPool[i+6].q3, ipPool[i+6].q4,
                        ipPool[i+7].q1, ipPool[i+7].q2, ipPool[i+7].q3, ipPool[i+7].q4
                    );

                    Vector256<byte> EqualsMask = Avx2.CompareEqual(ipVector, comparables);

                    int mask = Avx2.MoveMask(EqualsMask);

                    /* // this is wasteful and slower than the switch statement below
                    if (Convert.ToString(mask, 16).Contains("f"))
                    {
                        containsFlag = true;
                    }
                    */

                    switch(mask)
                    {
                        case (int)0x7777777F:  //ipPool[i]
                        case (int)0x777777F7: //ipPool[i+1]
                        case (int)0x77777F77: //ipPool[i+2]
                        case (int)0x7777F777: //ipPool[i+3]
                        case (int)0x777F7777: //ipPool[i+4]
                        case (int)0x77F77777: //ipPool[i+5]
                        case (int)0x7F777777: //ipPool[i+6]
                        case unchecked((int)0xF7777777): //ipPool[i+7]
                            containsFlag = true;
                            break;

                        case (int)0x00000000: //not found
                            continue;
                        
                        default:
                            //Console.WriteLine($"here's the bitmask: { mask }");
                            break;
                            //throw new Exception("Something is wrong because it was found multiple times");
                    }
                }

                for (; i < ipPool.Length; i++)
                {
                    bool tempFlag = CompareIP(ipCandidate, ipPool[i]);
                    if (tempFlag)
                    {
                        containsFlag = true;
                    }
                }
            }
            else
            {
                throw new Exception("AVX2 is not supported");
            }

            return containsFlag;
        }

        public unsafe bool FindIPAVX2Parallel(ByteIP ipCandidate, ByteIP[] ipPool, ParallelOptions numThreads)
        {
            bool containsFlag = false;
            int numLoops = ipPool.Length/8;
            int countRemaining = ipPool.Length%8;

            if (Avx2.IsSupported)
            {
                Vector256<byte> ipVector = Vector256.Create(
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4
                );

                Parallel.For(0, numLoops-1, numThreads, (loopCount, state) =>{
                    int i = loopCount * 8;

                    Vector256<byte> comparables = Vector256.Create(
                        ipPool[i].q1, ipPool[i].q2, ipPool[i].q3, ipPool[i].q4,
                        ipPool[i+1].q1, ipPool[i+1].q2, ipPool[i+1].q3, ipPool[i+1].q4,
                        ipPool[i+2].q1, ipPool[i+2].q2, ipPool[i+2].q3, ipPool[i+2].q4,
                        ipPool[i+3].q1, ipPool[i+3].q2, ipPool[i+3].q3, ipPool[i+3].q4,
                        ipPool[i+4].q1, ipPool[i+4].q2, ipPool[i+4].q3, ipPool[i+4].q4,
                        ipPool[i+5].q1, ipPool[i+5].q2, ipPool[i+5].q3, ipPool[i+5].q4,
                        ipPool[i+6].q1, ipPool[i+6].q2, ipPool[i+6].q3, ipPool[i+6].q4,
                        ipPool[i+7].q1, ipPool[i+7].q2, ipPool[i+7].q3, ipPool[i+7].q4
                    );

                    Vector256<byte> EqualsMask = Avx2.CompareEqual(ipVector, comparables);

                    int mask = Avx2.MoveMask(EqualsMask);

                    switch(mask)
                    {
                        case (int)0x7777777F:  //ipPool[i]
                        case (int)0x777777F7: //ipPool[i+1]
                        case (int)0x77777F77: //ipPool[i+2]
                        case (int)0x7777F777: //ipPool[i+3]
                        case (int)0x777F7777: //ipPool[i+4]
                        case (int)0x77F77777: //ipPool[i+5]
                        case (int)0x7F777777: //ipPool[i+6]
                        case unchecked((int)0xF7777777): //ipPool[i+7]
                            containsFlag = true;
                            break;

                        case (int)0x00000000: //not found
                            break;
                        
                        default:
                            //Console.WriteLine($"here's the bitmask: { mask }");
                            break;
                            //throw new Exception("Something is wrong because it was found multiple times");
                    }
                });

                if (countRemaining > 0)
                {
                    for (int i = ipPool.Length - countRemaining; i < ipPool.Length; i++)
                    {
                        bool tempFlag = CompareIP(ipCandidate, ipPool[i]);
                        if (tempFlag)
                        {
                            containsFlag = true;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("AVX2 is not supported");
            }

            return containsFlag;
        }

        public unsafe bool FindIPAdvSimdSerial(ByteIP ipCandidate, ByteIP[] ipPool)
        {
            bool containsFlag = false;
            int i = 0;

            if (AdvSimd.IsSupported)
            {
                Vector128<byte> ipVector = Vector128.Create(
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4
                );

                for (i = 0; i + 3 < ipPool.Length; i = i + 4)
                {
                    Vector128<byte> comparables = Vector128.Create(
                        ipPool[i].q1, ipPool[i].q2, ipPool[i].q3, ipPool[i].q4,
                        ipPool[i+1].q1, ipPool[i+1].q2, ipPool[i+1].q3, ipPool[i+1].q4,
                        ipPool[i+2].q1, ipPool[i+2].q2, ipPool[i+2].q3, ipPool[i+2].q4,
                        ipPool[i+3].q1, ipPool[i+3].q2, ipPool[i+3].q3, ipPool[i+3].q4
                    );

                    Vector128<byte> mask = AdvSimd.CompareEqual(ipVector, comparables);

                    bool tempFlag = CheckVector128(mask);
                    if (tempFlag)
                    {
                        containsFlag = true;
                    }
                }

                for (; i < ipPool.Length; i++)
                {
                    bool tempFlag = CompareIP(ipCandidate, ipPool[i]);
                    if (tempFlag)
                    {
                        containsFlag = true;
                    }
                }
            }
            else
            {
                throw new Exception("AdvSimd is not supported");
            }

            return containsFlag;
        }

        public unsafe bool FindIPAdvSimdParallel(ByteIP ipCandidate, ByteIP[] ipPool, ParallelOptions numThreads)
        {
            bool containsFlag = false;
            int numLoops = ipPool.Length/8;
            int countRemaining = ipPool.Length%8;


            if (AdvSimd.IsSupported)
            {
                Vector128<byte> ipVector = Vector128.Create(
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4,
                    ipCandidate.q1, ipCandidate.q2, ipCandidate.q3, ipCandidate.q4
                );

                Parallel.For(0, numLoops-1, numThreads, (loopCount, state) =>{
                    int i = loopCount * 4;

                    Vector128<byte> comparables = Vector128.Create(
                        ipPool[i].q1, ipPool[i].q2, ipPool[i].q3, ipPool[i].q4,
                        ipPool[i+1].q1, ipPool[i+1].q2, ipPool[i+1].q3, ipPool[i+1].q4,
                        ipPool[i+2].q1, ipPool[i+2].q2, ipPool[i+2].q3, ipPool[i+2].q4,
                        ipPool[i+3].q1, ipPool[i+3].q2, ipPool[i+3].q3, ipPool[i+3].q4
                    );

                    Vector128<byte> mask = AdvSimd.CompareEqual(ipVector, comparables);

                    bool tempFlag = CheckVector128(mask);
                    if (tempFlag)
                    {
                        containsFlag = true;
                    }
                });

                if (countRemaining > 0)
                {
                    for (int i = ipPool.Length - countRemaining; i < ipPool.Length; i++)
                    {
                        bool tempFlag = CompareIP(ipCandidate, ipPool[i]);
                        if (tempFlag)
                        {
                            containsFlag = true;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("AdvSimd is not supported");
            }

            return containsFlag;
        }


        public bool CheckVector128(Vector128<byte> mask)
        {
            bool containsFlag = false;

            for(int i = 0; i < 16; i = i + 4)
            {
                bool q1Equal = Vector128.GetElement(mask, i) == 255;
                bool q2Equal = Vector128.GetElement(mask, i+1) == 255;
                bool q3Equal = Vector128.GetElement(mask, i+2) == 255;
                bool q4Equal = Vector128.GetElement(mask, i+3) == 255;

                if (q1Equal && q2Equal && q3Equal && q4Equal)
                {
                    containsFlag = true;
                }
            }

            return containsFlag;
        }

        public bool CompareIP(ByteIP ipCanidate, ByteIP tempIP)
        {
            bool containsFlag = false;

            bool q1Equal = ipCanidate.q1 == tempIP.q1;
            bool q2Equal = ipCanidate.q2 == tempIP.q2;
            bool q3Equal = ipCanidate.q3 == tempIP.q3;
            bool q4Equal = ipCanidate.q4 == tempIP.q4;

            if (q1Equal && q2Equal && q3Equal && q4Equal)
            {
                containsFlag = true;
            }

            return containsFlag;
        }
    }
}
