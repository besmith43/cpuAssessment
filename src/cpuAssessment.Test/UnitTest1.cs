using System;
using Xunit;
using cpuAssessment.Class;

namespace cpuAssessment.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Class1 testClass = new Class1();
            int result = testClass.Add(2,3);
            Assert.Equal(5, result);
        }

        [Fact]
        public void TestIPComparisonSerial()
        {
            Class1 testClass = new Class1();

            ByteIP testIP = new ByteIP(149, 149, 140, 151);

            ByteIPRange testIPRangeList = new ByteIPRange{
                q1Start = 149,
                q1End = 149,
                q2Start = 149,
                q2End = 149,
                q3Start = 0,
                q3End = 50,
                q4Start = 0,
                q4End = 255
            };

            ByteIP[] testIPRangeArray = testIPRangeList.GenerateList();

            bool found = testClass.FindIPSerial(testIP, testIPRangeArray);

            Assert.Equal(false, found);
        }

        [Fact]
        public void TestIPComparisonParallel()
        {
            Class1 testClass = new Class1();

            ByteIP testIP = new ByteIP(149, 149, 140, 151);

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

            ByteIP[] testIPRangeArray = testIPRangeList.GenerateList();

            bool found = testClass.FindIPParallel(testIP, testIPRangeArray);

            Assert.Equal(true, found);
        }
    }
}
