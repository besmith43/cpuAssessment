using System;
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
        public void IPComparisonParallel()
        {
            testClass.FindIPParallel(testIP, testIPRangeArray);
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
        public void IPComparisonAVX2Parallel()
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray);
        }
 
    }
}