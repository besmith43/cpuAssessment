%title: Types of Parallelism
%author: Blake Smith
%date: 2024-04-02


-> # Types of Parallelism <-
============================

```
Parallelism:
a type of computation
in which more than one calculation or process
is carried out simulateously
```

[Wikipedia](https://en.wikipedia.org/wiki/Parallel_computing)

----------------------------------------------------


-> # Various Types of Parallelism <-

## Single line of Execution
- Scalar
- Pipelining
- Out-of-order Execution
- SuperScalar
- SIMD

## Multiply lines of Execution
- Threading
- Parallel
- Async/Await

----------------------------------------------------


-> # Scalar <-

- Pure Procedural
- Top Down Style Execution
- Step by Step

```csharp
public void main() {
	Console.WriteLine("What is your favorite color?");
	var answer = Console.ReadLine();
	Console.WriteLine($"Your favorite color is { answer }");
}
```

----------------------------------------------------


-> # Pipelining <-

-> allowing instructions to start executing at each stage of the cpu core <-

-> # Out-of-order Execution <-

-> creates a queue for instructions to be pulled from <-
-> such that any instruction that is ready to be run can be run <-
-> regardless of the program counter <-

-> # SuperScalar <-

-> when a single CPU core has multiple execution units that are independent of one another <-
-> but rely on the same pre and post processing stages <-

----------------------------------------------------


-> # SIMD Programming <-

-> SIMD stands for Single Instruction/Multiple Data <-

-> It is the use of special machine instructions within your code <-
-> and execution units outside of a CPU core's regular ALU's <-
-> to process multiple data values with a single instruction <-

-> This project makes use of x86's AVX2 and Arm's Neon <-

----------------------------------------------------

-> # AVX2 <- 

- 256 bit memory address
- makes use of XMM and YMM registers
- handles 8 32-bit single precision floating point numbers or
- 4 64-bit double precision floating point numbers
- handles vector math

----------------------------------------------------

-> # AVX2 <- 

```csharp
    public unsafe bool FindIPAVX2Parallel(ByteIP ipCandidate, ByteIP[] ipPool, ParallelOptions numThreads)
    {
        bool containsFlag = false;
        int numLoops = ipPool.Length / 8;
        int countRemaining = ipPool.Length % 8;
        if (Avx2.IsSupported)
        {
            Vector256<byte> ipVector = Vector256.Create(...);
            Parallel.For(0, numLoops - 1, numThreads, (loopCount, state) => {
                int i = loopCount * 8;
                Vector256<byte> comparables = Vector256.Create(
                    ipPool[i].q1, ipPool[i].q2, ipPool[i].q3, ipPool[i].q4,
					...
                    ipPool[i + 7].q1, ipPool[i + 7].q2, ipPool[i + 7].q3, ipPool[i + 7].q4
                );
                Vector256<byte> EqualsMask = Avx2.CompareEqual(ipVector, comparables);
```

----------------------------------------------------

-> # AVX2 <- 

```csharp
                int mask = Avx2.MoveMask(EqualsMask);
                switch (mask)
                {
                    case (int)0x7777777F:  //ipPool[i]
                    case (int)0x777777F7: //ipPool[i+1]
                    case (int)0x77777F77: //ipPool[i+2]
                    case (int)0x7777F777: //ipPool[i+3]
                    case (int)0x777F7777: //ipPool[i+4]
                    case (int)0x77F77777: //ipPool[i+5]
                    case (int)0x7F777777: //ipPool[i+6]
                    case unchecked((int)0xF7777777): //ipPool[i+7]
                        containsFlag = true;
                        break;
                    case (int)0x00000000: //not found
                        break;
                    default:
                        break;
                }
            });
            if (countRemaining > 0) {
                for (int i = ipPool.Length - countRemaining; i < ipPool.Length; i++) {
                    bool tempFlag = CompareIP(ipCandidate, ipPool[i]);
                    if (tempFlag) { containsFlag = true; }
                }
            }
        }
		...
		return containsFlag;
    }
```

----------------------------------------------------


-> # Threading <-

-> Threads are separate execution paths with shared memory <-

```csharp
var players = new List<Players>();
var mutex = new Mutex();
public Player GetPlayer(int id)
{
	return players.Where(x => x.ID == id);
}
public Player UpdatePlayerPostion(int id, int x, int y)
{
	mutex.WaitOne();
	var player = players.Where(x => x.ID == id);
	player.x = x;
	player.y = y;
	mutex.ReleaseMutex();
	return player;
}
```

----------------------------------------------------

-> # Parallel Programming <-

-> Separate Paths of execution without shared memory <-

```csharp
void main()
{
	...
	Parallel.ForEach(array, numThreads, (tempArray) =>
    {
		doWork(tempArray);
    });
	...
}
```

----------------------------------------------------

-> Async / Await <-

-> syntactic feature of many programming languages that allows for an asynchronous, non-blocking function <-
-> to be structured within and run by an ordinary synchronous function <-

-> basically it means that the runtime is using a finite state machines under the hood <-
-> to handle the coroutines that are often implemented to run separate logic on the cpu <-
-> while an I/O operation is being waited on <-

-> this can be seen in C#, Javascript, and Python to name a few <-

```csharp
void main()
{
	Task<string> fileContentsTask = ReadFile("text.txt");
	... do other work on the cpu while the hard drive gets the requested file's contents ...
	string contents = await fileContentsTask;
	... perform operations on the file's contents ...
}
async Task<string> ReadFile(string filename)
{
	return await File.GetAllBytes(filename).ToString();
}
```

----------------------------------------------------

-> # Benchmark Run Results <-

``` ini

BenchmarkDotNet=v0.13.0, OS=macOS 14.4.1 (23E224) [Darwin 23.4.0]
Apple M1 Max, 1 CPU, 10 logical and 10 physical cores
.NET SDK=8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), Arm64 RyuJIT
  DefaultJob : .NET 8.0.1 (8.0.123.58001), Arm64 RyuJIT


```

|                         Method |      Mean |    Error |   StdDev | Ratio | RatioSD | Rank |     Allocated |
|------------------------------- |----------:|---------:|---------:|------:|--------:|-----:|--------------:|
|       IPComparisonScalarSerial |  41.28 ms | 0.355 ms | 0.332 ms |  1.00 |    0.00 |    6 |          52 B |
|  IPComparisonScalarMaxParallel |  30.57 ms | 0.608 ms | 0.791 ms |  0.74 |    0.02 |    3 |       3,862 B |
| IPComparisonScalarHalfParallel |  39.13 ms | 1.075 ms | 3.171 ms |  0.91 |    0.07 |    5 |       2,766 B |
|  IPComparisonScalarTwoParallel |  59.83 ms | 1.194 ms | 1.674 ms |  1.45 |    0.06 |    7 |       2,113 B |
|  IPComparisonScalarOneParallel | 114.52 ms | 1.000 ms | 1.265 ms |  2.77 |    0.03 |   10 |       1,984 B |
|    IPComparisonScalarCoroutine | 325.90 ms | 1.379 ms | 1.076 ms |  7.88 |    0.05 |   11 | 805,307,096 B |
|       IPComparisonVectorSerial | 104.04 ms | 0.269 ms | 0.252 ms |  2.52 |    0.02 |    9 |         134 B |
|  IPComparisonVectorMaxParallel |  12.17 ms | 0.204 ms | 0.181 ms |  0.29 |    0.00 |    1 |       4,186 B |
| IPComparisonVectorHalfParallel |  17.04 ms | 0.325 ms | 0.304 ms |  0.41 |    0.01 |    2 |       2,840 B |
|  IPComparisonVectorTwoParallel |  37.48 ms | 0.189 ms | 0.168 ms |  0.91 |    0.01 |    4 |       2,102 B |
|  IPComparisonVectorOneParallel |  72.36 ms | 0.283 ms | 0.265 ms |  1.75 |    0.01 |    8 |       1,912 B |
|              IPComparisonAsync |  41.69 ms | 0.827 ms | 1.238 ms |  1.00 |    0.04 |    6 | 268,437,024 B |

