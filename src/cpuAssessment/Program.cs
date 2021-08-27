using System;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.Intrinsics;
using cpuAssessment.cmd;
using cpuAssessment.Class;

namespace cpuAssessment
{
    class Program
    {
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
            if (System.Runtime.Intrinsics.X86.Avx2.IsSupported)
            {
                RunX86();
            }
            else if (System.Runtime.Intrinsics.Arm.AdvSimd.IsSupported)
            {
                RunArm();
            }
            else
            {
                RunBase();
            }
        }

        public static unsafe void RunBase()
        {
            Class1 classLib = new Class1();

            Int64 before_testIP = GC.GetTotalMemory(false);
            ByteIP testIP = new ByteIP(149, 149, 140, 151);
            Int64 after_testIP = GC.GetTotalMemory(false);

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

            Int64 before_Array = GC.GetTotalMemory(false);
            ByteIP[] testIPRangeArray = testIPRangeList.GenerateList();
            Int64 after_Array = GC.GetTotalMemory(false);

            bool found = classLib.FindIPParallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });

            Console.WriteLine($"the size of an instance of the byteIP class is { after_testIP - before_testIP }");

            Console.WriteLine($"the size of the IP range array of ByteIP's is { after_Array - before_Array } bytes or { (after_Array - before_Array) /1024 /1024 } MB");

            Console.WriteLine($"Number of objects in ByteIP Array: { testIPRangeArray.Length }");

            Console.WriteLine($"That is { (after_Array - before_Array) / testIPRangeArray.Length } bytes per IP Address object");

            Console.WriteLine("Running the Coroutine version");

            bool foundCoroutine = false;

            foreach(ByteIP tempIP in testIPRangeList.Coroutine())
            {
                foundCoroutine = classLib.CompareIP(testIP, tempIP);
            }
        }

        public static unsafe void RunX86()
        {
            Class1 classLib = new Class1();

            Console.WriteLine($"2 + 3 = { classLib.Add(2,3) }");
            
            classLib.RunAVX2();

            Int64 before_testIP = GC.GetTotalMemory(false);
            ByteIP testIP = new ByteIP(149, 149, 140, 151);
            Int64 after_testIP = GC.GetTotalMemory(false);

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

            Int64 before_Array = GC.GetTotalMemory(false);
            ByteIP[] testIPRangeArray = testIPRangeList.GenerateList();
            Int64 after_Array = GC.GetTotalMemory(false);

            bool found = classLib.FindIPParallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });

            Console.WriteLine($"the size of an instance of the byteIP class is { after_testIP - before_testIP }");

            Console.WriteLine($"the size of the IP range array of ByteIP's is { after_Array - before_Array } bytes or { (after_Array - before_Array) /1024 /1024 } MB");

            Console.WriteLine($"Number of objects in ByteIP Array: { testIPRangeArray.Length }");

            Console.WriteLine($"That is { (after_Array - before_Array) / testIPRangeArray.Length } bytes per IP Address object");

            Console.WriteLine("Running the Coroutine version");

            bool foundCoroutine = false;

            foreach(ByteIP tempIP in testIPRangeList.Coroutine())
            {
                foundCoroutine = classLib.CompareIP(testIP, tempIP);
            }

            ByteIPRange testAVX2RangeList = new ByteIPRange{
                q1Start = 149,
                q1End = 149,
                q2Start = 149,
                q2End = 149,
                q3Start = 140,
                q3End = 140,
                q4Start = 150,
                q4End = 157
            };

            bool foundAVX2 = classLib.FindIPAVX2Serial(testIP, testAVX2RangeList.GenerateList());
            bool foundAVX22 = classLib.FindIPAVX2Serial(testIP, testIPRangeArray);

            Console.WriteLine($"Answer from the small avx2 run: { foundAVX2 }");
            Console.WriteLine($"Answer from the long run: { foundAVX22 }");

            bool foundAVX2Parallel = classLib.FindIPAVX2Parallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });

            Console.WriteLine($"Answer from AVX2 Parallel: { foundAVX2Parallel }");
        }

        public static unsafe void RunArm()
        {
            Class1 classLib = new Class1();

            Int64 before_testIP = GC.GetTotalMemory(false);
            ByteIP testIP = new ByteIP(149, 149, 140, 151);
            Int64 after_testIP = GC.GetTotalMemory(false);

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

            Int64 before_Array = GC.GetTotalMemory(false);
            ByteIP[] testIPRangeArray = testIPRangeList.GenerateList();
            Int64 after_Array = GC.GetTotalMemory(false);

            bool found = classLib.FindIPParallel(testIP, testIPRangeArray, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });

            Console.WriteLine($"the size of an instance of the byteIP class is { after_testIP - before_testIP }");

            Console.WriteLine($"the size of the IP range array of ByteIP's is { after_Array - before_Array } bytes or { (after_Array - before_Array) /1024 /1024 } MB");

            Console.WriteLine($"Number of objects in ByteIP Array: { testIPRangeArray.Length }");

            Console.WriteLine($"That is { (after_Array - before_Array) / testIPRangeArray.Length } bytes per IP Address object");

            Console.WriteLine("Running the Coroutine version");

            bool foundCoroutine = false;

            foreach(ByteIP tempIP in testIPRangeList.Coroutine())
            {
                foundCoroutine = classLib.CompareIP(testIP, tempIP);
            }

            bool foundAdvSimdSerial = classLib.FindIPAdvSimdSerial(testIP, testIPRangeArray);

            bool foundAdvSimdParallel = classLib.FindIPAdvSimdParallel(testIP, testIPRangeArray, new ParallelOptions{ MaxDegreeOfParallelism = Environment.ProcessorCount });

            Console.WriteLine($"Answer from the AdvSimd Serial: { foundAdvSimdSerial }");
            Console.WriteLine($"Answer from the AdvSimd Parallel: { foundAdvSimdParallel }");
        }
    }
}
