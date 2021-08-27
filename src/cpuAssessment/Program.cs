using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.Intrinsics;
using cpuAssessment.cmd;
using cpuAssessment.Class;

namespace cpuAssessment
{
    class Program
    {
        public static Class1 classLib = new Class1();
        public static int numLoops = 100;
        public static long[] ScalarSerialStopWatch = new long[numLoops];
        public static long[] ScalarParallelAllStopWatch = new long[numLoops];
        public static long[] ScalarParallelHalfStopWatch = new long[numLoops];
        public static long[] ScalarParallel2StopWatch = new long[numLoops];
        public static long[] ScalarParallel1StopWatch = new long[numLoops];
        public static long[] ScalarCoRoutineStopWatch = new long[numLoops];
        public static long[] VectorSerialStopWatch = new long[numLoops];
        public static long[] VectorParallelAllStopWatch = new long[numLoops];
        public static long[] VectorParallelHalfStopWatch = new long[numLoops];
        public static long[] VectorParallel2StopWatch = new long[numLoops];
        public static long[] VectorParallel1StopWatch = new long[numLoops];
        public static Options cmdFlags;
        static void Main(string[] args)
        {
            cmdParser cmdP = new(args);

            cmdFlags = cmdP.Parse();  

            if (cmdFlags.versionFlag)
            {
                Console.WriteLine(cmdFlags.versionText);
                return;
            }

            if (cmdFlags.helpFlag)
            {
                Console.WriteLine(cmdFlags.helpText);
                return;
            }

            Run();
        }

        public static void Run()
        {
            ByteIP testIP = new ByteIP(149, 149, 140, 151);
            
            ByteIPRange testIPRangeList = new ByteIPRange{
                q1Start = 149,
                q1End = 150,
                q2Start = 0,
                q2End = 255,
                q3Start = 0,
                q3End = 255,
                q4Start = 0,
                q4End = 255
            };

            ByteIP[] testIPRangeArray = testIPRangeList.GenerateList();


            if (System.Runtime.Intrinsics.X86.Avx2.IsSupported)
            {
                RunBase(testIP, testIPRangeList, testIPRangeArray);
                RunX86(testIP, testIPRangeArray);
            }
            else if (System.Runtime.Intrinsics.Arm.AdvSimd.IsSupported)
            {
                RunBase(testIP, testIPRangeList, testIPRangeArray);
                RunArm(testIP, testIPRangeArray);
            }
            else
            {
                RunBase(testIP, testIPRangeList, testIPRangeArray);
            }
        }

        public static unsafe void RunBase(ByteIP testIP, ByteIPRange testIPRangeList, ByteIP[] testIPRangeArray)
        {
            Stopwatch Timer = new Stopwatch();

            for (int i = 0; i < numLoops; i++)
            {
                Timer.Start();
                bool foundSerial = classLib.FindIPSerial(testIP, testIPRangeArray);
                Timer.Stop();
                ScalarSerialStopWatch[i] = Timer.ElapsedMilliseconds;
                Timer.Reset();
            }

            Console.WriteLine($"Scalar Serial Find IP function took and average of { ScalarSerialStopWatch.Sum()/numLoops } ms to complete");

            Timer.Reset();

            for (int i = 0; i < numLoops; i++)
            {
                Timer.Start();
                bool foundAll = classLib.FindIPParallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });
                Timer.Stop();
                Timer.Reset();
            }

            Console.WriteLine($"Scalar Parallel w/{ Environment.ProcessorCount } threads Find IP function took an average of { ScalarParallelAllStopWatch.Sum()/numLoops } ms to complete");

            Timer.Reset();

            Timer.Start();
            bool foundHalf = classLib.FindIPParallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount/2 });
            Timer.Stop();

            Console.WriteLine($"Scalar Parallel w/{ Environment.ProcessorCount/2 } threads Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool found2 = classLib.FindIPParallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = 2 });
            Timer.Stop();

            Console.WriteLine($"Scalar Parallel w/2 threads Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool found1 = classLib.FindIPParallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = 1 });
            Timer.Stop();

            Console.WriteLine($"Scalar Parallel w/1 thread Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

        bool foundCoroutine = false;

            Timer.Start();
            foreach(ByteIP tempIP in testIPRangeList.Coroutine())
            {
                foundCoroutine = classLib.CompareIP(testIP, tempIP);
            }
            Timer.Stop();

            Console.WriteLine($"Scalar CoRoutine Find IP function took { Timer.Elapsed } to complete");
        }

        public static unsafe void RunX86(ByteIP testIP, ByteIP[] testIPRangeArray)
        {
            Stopwatch Timer = new Stopwatch();

            Timer.Start();
            bool foundAVX2 = classLib.FindIPAVX2Serial(testIP, testIPRangeArray);
            Timer.Stop();

            Console.WriteLine($"Vector Serial Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool foundAVX2ParallelAll = classLib.FindIPAVX2Parallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });
            Timer.Stop();

            Console.WriteLine($"Vector Parallel w/{ Environment.ProcessorCount } threads Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool foundAVX2ParallelHalf = classLib.FindIPAVX2Parallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount/2 });
            Timer.Stop();

            Console.WriteLine($"Vector Parallel w/{ Environment.ProcessorCount/2 } threads Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool foundAVX2Parallel2 = classLib.FindIPAVX2Parallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = 2 });
            Timer.Stop();

            Console.WriteLine($"Vector Parallel w/2 threads Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool foundAVX2Parallel1 = classLib.FindIPAVX2Parallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = 1 });
            Timer.Stop();

            Console.WriteLine($"Vector Parallel w/1 thread Find IP function took { Timer.Elapsed } to complete");

        }

        public static unsafe void RunArm(ByteIP testIP, ByteIP[] testIPRangeArray)
        {
            Stopwatch Timer = new Stopwatch();

            Timer.Start();
            bool foundAdvSimdSerial = classLib.FindIPAdvSimdSerial(testIP, testIPRangeArray);
            Timer.Stop();

            Console.WriteLine($"Vector Serial Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool foundAdvSimdParallelAll = classLib.FindIPAdvSimdParallel(testIP, testIPRangeArray, new ParallelOptions{ MaxDegreeOfParallelism = Environment.ProcessorCount });
            Timer.Stop();

            Console.WriteLine($"Vector Parallel w/{ Environment.ProcessorCount } threads Find IP function took { Timer.Elapsed } to complete");


            Timer.Reset();

            Timer.Start();
            bool foundAdvSimdParallelHalf = classLib.FindIPAdvSimdParallel(testIP, testIPRangeArray, new ParallelOptions{ MaxDegreeOfParallelism = Environment.ProcessorCount/2 });
            Timer.Stop();

            Console.WriteLine($"Vector Parallel w/{ Environment.ProcessorCount/2 } threads Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool foundAdvSimdParallel2 = classLib.FindIPAdvSimdParallel(testIP, testIPRangeArray, new ParallelOptions{ MaxDegreeOfParallelism = 2 });
            Timer.Stop();

            Console.WriteLine($"Vector Parallel w/2 threads Find IP function took { Timer.Elapsed } to complete");

            Timer.Reset();

            Timer.Start();
            bool foundAdvSimdParallel1 = classLib.FindIPAdvSimdParallel(testIP, testIPRangeArray, new ParallelOptions{ MaxDegreeOfParallelism = 1 });
            Timer.Stop();

            Console.WriteLine($"Vector Parallel w/1 thread Find IP function took { Timer.Elapsed } to complete");

        }
    }
}
