``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.1165 (20H2/October2020Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100-preview.7.21379.14
  [Host]     : .NET 5.0.9 (5.0.921.35908), X64 RyuJIT
  DefaultJob : .NET 5.0.9 (5.0.921.35908), X64 RyuJIT


```
|                   Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |      Gen 0 | Gen 1 | Gen 2 |     Allocated |
|------------------------- |----------:|---------:|---------:|------:|--------:|-----:|-----------:|------:|------:|--------------:|
|       IPComparisonSerial |  51.87 ms | 0.495 ms | 0.463 ms |  1.00 |    0.00 |    3 |          - |     - |     - |             - |
|     IPComparisonParallel |  25.44 ms | 0.118 ms | 0.105 ms |  0.49 |    0.00 |    1 |          - |     - |     - |       4,894 B |
|    IPComparisonCoroutine | 297.85 ms | 3.351 ms | 2.799 ms |  5.75 |    0.06 |    4 | 96000.0000 |     - |     - | 805,306,424 B |
|   IPComparisonAVX2Serial |  45.73 ms | 0.398 ms | 0.372 ms |  0.88 |    0.01 |    2 |          - |     - |     - |             - |
| IPComparisonAVX2Parallel |  25.37 ms | 0.053 ms | 0.050 ms |  0.49 |    0.00 |    1 |          - |     - |     - |       5,642 B |
