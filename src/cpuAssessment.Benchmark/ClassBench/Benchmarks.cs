using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Order;
using cpuAssessment.Class;

namespace cpuAssessment.Benchmark.Benchmarks;

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
        testIPRangeList = new ByteIPRange
        {
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

        FullThreadCountOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxNumThreads
        };

        HalfThreadCountOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = halfNumThreads
        };

        TwoThreadCountOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 2
        };

        OneThreadCountOptions = new ParallelOptions
        {
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
    public void IPComparisonScalarSerial()
    {
        testClass.FindIPSerial(testIP, testIPRangeArray);
    }

    [Benchmark]
    public void IPComparisonScalarMaxParallel()
    {
        testClass.FindIPParallel(testIP, testIPRangeArray, FullThreadCountOptions);
    }

    [Benchmark]
    public void IPComparisonScalarHalfParallel()
    {
        testClass.FindIPParallel(testIP, testIPRangeArray, HalfThreadCountOptions);
    }

    [Benchmark]
    public void IPComparisonScalarTwoParallel()
    {
        testClass.FindIPParallel(testIP, testIPRangeArray, TwoThreadCountOptions);
    }

    [Benchmark]
    public void IPComparisonScalarOneParallel()
    {
        testClass.FindIPParallel(testIP, testIPRangeArray, OneThreadCountOptions);
    }

    [Benchmark]
    public void IPComparisonScalarCoroutine()
    {
        foreach (ByteIP tempIP in testIPRangeList.Coroutine())
        {
            testClass.CompareIP(testIP, tempIP);
        }
    }

    [Benchmark]
    public void IPComparisonVectorSerial()
    {
        if (System.Runtime.Intrinsics.X86.Avx2.IsSupported)
        {
            testClass.FindIPAVX2Serial(testIP, testIPRangeArray);
        }
        else if (System.Runtime.Intrinsics.Arm.AdvSimd.IsSupported)
        {
            testClass.FindIPAdvSimdSerial(testIP, testIPRangeArray);
        }
    }

    [Benchmark]
    public void IPComparisonVectorMaxParallel()
    {
        if (System.Runtime.Intrinsics.X86.Avx2.IsSupported)
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray, FullThreadCountOptions);
        }
        else if (System.Runtime.Intrinsics.Arm.AdvSimd.IsSupported)
        {
            testClass.FindIPAdvSimdParallel(testIP, testIPRangeArray, FullThreadCountOptions);
        }
    }

    [Benchmark]
    public void IPComparisonVectorHalfParallel()
    {
        if (System.Runtime.Intrinsics.X86.Avx2.IsSupported)
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray, HalfThreadCountOptions);
        }
        else if (System.Runtime.Intrinsics.Arm.AdvSimd.IsSupported)
        {
            testClass.FindIPAdvSimdParallel(testIP, testIPRangeArray, HalfThreadCountOptions);
        }
    }

    [Benchmark]
    public void IPComparisonVectorTwoParallel()
    {
        if (System.Runtime.Intrinsics.X86.Avx2.IsSupported)
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray, TwoThreadCountOptions);
        }
        else if (System.Runtime.Intrinsics.Arm.AdvSimd.IsSupported)
        {
            testClass.FindIPAdvSimdParallel(testIP, testIPRangeArray, TwoThreadCountOptions);
        }
    }

    [Benchmark]
    public void IPComparisonVectorOneParallel()
    {
        if (System.Runtime.Intrinsics.X86.Avx2.IsSupported)
        {
            testClass.FindIPAVX2Parallel(testIP, testIPRangeArray, OneThreadCountOptions);
        }
        else if (System.Runtime.Intrinsics.Arm.AdvSimd.IsSupported)
        {
            testClass.FindIPAdvSimdParallel(testIP, testIPRangeArray, OneThreadCountOptions);
        }
    }

    [Benchmark()]
    public void IPComparisonAsync()
    {
        Class1.FindIPAsync(testIP, testIPRangeArray);
    }
}