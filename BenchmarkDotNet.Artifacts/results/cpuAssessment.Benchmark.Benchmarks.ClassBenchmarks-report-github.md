``` ini

BenchmarkDotNet=v0.13.0, OS=macOS 14.4.1 (23E224) [Darwin 23.4.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK=8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), Arm64 RyuJIT
  DefaultJob : .NET 8.0.1 (8.0.123.58001), Arm64 RyuJIT


```
|                         Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |       Gen 0 | Gen 1 | Gen 2 |     Allocated |
|------------------------------- |----------:|---------:|---------:|------:|--------:|-----:|------------:|------:|------:|--------------:|
|       IPComparisonScalarSerial |  41.28 ms | 0.355 ms | 0.332 ms |  1.00 |    0.00 |    6 |           - |     - |     - |          52 B |
|  IPComparisonScalarMaxParallel |  30.57 ms | 0.608 ms | 0.791 ms |  0.74 |    0.02 |    3 |           - |     - |     - |       3,862 B |
| IPComparisonScalarHalfParallel |  39.13 ms | 1.075 ms | 3.171 ms |  0.91 |    0.07 |    5 |           - |     - |     - |       2,766 B |
|  IPComparisonScalarTwoParallel |  59.83 ms | 1.194 ms | 1.674 ms |  1.45 |    0.06 |    7 |           - |     - |     - |       2,113 B |
|  IPComparisonScalarOneParallel | 114.52 ms | 1.000 ms | 1.265 ms |  2.77 |    0.03 |   10 |           - |     - |     - |       1,984 B |
|    IPComparisonScalarCoroutine | 325.90 ms | 1.379 ms | 1.076 ms |  7.88 |    0.05 |   11 | 128000.0000 |     - |     - | 805,307,096 B |
|       IPComparisonVectorSerial | 104.04 ms | 0.269 ms | 0.252 ms |  2.52 |    0.02 |    9 |           - |     - |     - |         134 B |
|  IPComparisonVectorMaxParallel |  12.17 ms | 0.204 ms | 0.181 ms |  0.29 |    0.00 |    1 |           - |     - |     - |       4,186 B |
| IPComparisonVectorHalfParallel |  17.04 ms | 0.325 ms | 0.304 ms |  0.41 |    0.01 |    2 |           - |     - |     - |       2,840 B |
|  IPComparisonVectorTwoParallel |  37.48 ms | 0.189 ms | 0.168 ms |  0.91 |    0.01 |    4 |           - |     - |     - |       2,102 B |
|  IPComparisonVectorOneParallel |  72.36 ms | 0.283 ms | 0.265 ms |  1.75 |    0.01 |    8 |           - |     - |     - |       1,912 B |
|              IPComparisonAsync |  41.69 ms | 0.827 ms | 1.238 ms |  1.00 |    0.04 |    6 |           - |     - |     - | 268,437,024 B |
