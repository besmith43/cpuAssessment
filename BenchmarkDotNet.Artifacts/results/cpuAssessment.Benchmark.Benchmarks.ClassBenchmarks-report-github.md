``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.22000
Microsoft SQ2 3.15 GHz, 1 CPU, 8 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host] : .NET 5.0.9 (5.0.921.35908), Arm64 RyuJIT


```
|                   Method | Mean | Error | Ratio | RatioSD | Rank |
|------------------------- |-----:|------:|------:|--------:|-----:|
| IPComparisonScalarSerial |   NA |    NA |     ? |       ? |    ? |

Benchmarks with issues:
  ClassBenchmarks.IPComparisonScalarSerial: DefaultJob
