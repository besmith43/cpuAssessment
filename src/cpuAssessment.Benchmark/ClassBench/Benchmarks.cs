using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Order;
using cpuAssessment.Class;

namespace  cpuAssessment.Benchmark.Benchmarks
{
    [MemoryDiagnoser]
    [RankColumn]
    public class ClassBenchmarks
    {
        public int a = 2;
        public int b = 3;

        public readonly ByteIP testIP = new ByteIP(149, 149, 140, 151);
        public ByteIPRange testIPRangeList;

        public ByteIP[] testIPRangeArray;

        public int maxNumThreads = Environment.ProcessorCount;

        public int halfNumThreads = Environment.ProcessorCount / 2;

        public ParallelOptions FullThreadCountOptions;

        public ParallelOptions HalfThreadCountOptions;

        public ParallelOptions TwoThreadCountOptions;

        public ParallelOptions OneThreadCountOptions;

        public static Class1 testClass = new Class1();

        [GlobalSetup]
        public void Setup()
        {
            testIPRangeList = new ByteIPRange{
                q1Start = 149,
                q1End = 150,
                q2Start = 0,
                q2End = 255,
                q3Start = 0,
                q3End = 255,
                q4Start = 0,
                q4End = 255
            };

            testIPRangeArray = testIPRangeList.GenerateList();

            FullThreadCountOptions = new ParallelOptions{
                MaxDegreeOfParallelism = maxNumThreads
            };

            HalfThreadCountOptions = new ParallelOptions{
                MaxDegreeOfParallelism = halfNumThreads
            };

            TwoThreadCountOptions = new ParallelOptions{
                MaxDegreeOfParallelism = 2
            };

            OneThreadCountOptions = new ParallelOptions{
                MaxDegreeOfParallelism = 1
            };
        }
/*
        [Benchmark]
        public void AddBenchmark()
        {
            testClass.Add(a, b);
        }

        [Benchmark]
        public void AVXBenchmark()
        {
            testClass.RunAVX2();
        }
*/
        [Benchmark(Baseline = true)]
        public void IPComparisonSerial()
        {
            testClass.FindIPSerial(testIP, testIPRangeArray);
        }

        [Benchmark]
        public void IPComparisonMaxParallel()
        {
            testClass.FindIPParallel(testIP, testIPRangeArray, FullThreadCountOptions);
        }

        [Benchmark]
        public void IPComparisonHalfParallel()
        {
            testClass.FindIPParallel(testIP, testIPRangeArray, HalfThreadCountOptions);
        }

        [Benchmark]
        public void IPComparisonTwoParallel()
        {
            testClass.FindIPParallel(testIP, testIPRangeArray, TwoThreadCountOptions);
        }

        [Benchmark]
        public void IPComparisonOneParallel()
        {
            testClass.FindIPParallel(testIP, testIPRangeArray, OneThreadCountOptions);
        }

        [Benchmark]
        public void IPComparisonCoroutine()
        {
            foreach(ByteIP tempIP in testIPRangeList.Coroutine())
            {
                testClass.CompareIP(testIP, tempIP);
            }
        }

        [Benchmark]
        public void IPComparisonAVX2Serial()
        {
            testClass.FindIPAVX2Serial(testIP, testIPRangeArray);
        }

        [Benchmark]
        public void IPComparisonAVX2MaxParallel()
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray, FullThreadCountOptions);
        }

        [Benchmark]
        public void IPComparisonAVX2HalfParallel()
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray, HalfThreadCountOptions);
        }

        [Benchmark]
        public void IPComparisonAVX2TwoParallel()
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray, TwoThreadCountOptions);
        }

        [Benchmark]
        public void IPComparisonAVX2OneParallel()
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray, OneThreadCountOptions);
        }
 
    }
}