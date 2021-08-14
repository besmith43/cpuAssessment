using System;
using BenchmarkDotNet.Running;
using cpuAssessment.Benchmark.Benchmarks;

namespace cpuAssessment.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ClassBenchmarks>();
        }
    }
}
