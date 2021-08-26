``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.1165 (20H2/October2020Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 5.0.9 (5.0.921.35908), X64 RyuJIT
  DefaultJob : .NET 5.0.9 (5.0.921.35908), X64 RyuJIT


```
|                       Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |      Gen 0 | Gen 1 | Gen 2 |     Allocated |
|----------------------------- |----------:|---------:|---------:|------:|--------:|-----:|-----------:|------:|------:|--------------:|
|           IPComparisonSerial |  48.57 ms | 0.204 ms | 0.191 ms |  1.00 |    0.00 |    5 |          - |     - |     - |             - |
|      IPComparisonMaxParallel |  25.39 ms | 0.174 ms | 0.163 ms |  0.52 |    0.00 |    1 |          - |     - |     - |       4,718 B |
|     IPComparisonHalfParallel |  28.37 ms | 0.313 ms | 0.293 ms |  0.58 |    0.01 |    2 |          - |     - |     - |       3,016 B |
|      IPComparisonTwoParallel |  66.26 ms | 0.220 ms | 0.172 ms |  1.36 |    0.01 |    7 |          - |     - |     - |       2,857 B |
|      IPComparisonOneParallel | 130.35 ms | 0.345 ms | 0.306 ms |  2.68 |    0.01 |    8 |          - |     - |     - |       1,616 B |
|        IPComparisonCoroutine | 299.33 ms | 5.012 ms | 4.443 ms |  6.17 |    0.10 |    9 | 96000.0000 |     - |     - | 805,306,424 B |
|       IPComparisonAVX2Serial |  45.84 ms | 0.487 ms | 0.456 ms |  0.94 |    0.01 |    4 |          - |     - |     - |             - |
|  IPComparisonAVX2MaxParallel |  25.36 ms | 0.055 ms | 0.052 ms |  0.52 |    0.00 |    1 |          - |     - |     - |       5,442 B |
| IPComparisonAVX2HalfParallel |  25.13 ms | 0.233 ms | 0.218 ms |  0.52 |    0.01 |    1 |          - |     - |     - |       3,360 B |
|  IPComparisonAVX2TwoParallel |  30.92 ms | 0.162 ms | 0.151 ms |  0.64 |    0.00 |    3 |          - |     - |     - |       1,809 B |
|  IPComparisonAVX2OneParallel |  56.45 ms | 0.363 ms | 0.340 ms |  1.16 |    0.01 |    6 |          - |     - |     - |       2,652 B |
