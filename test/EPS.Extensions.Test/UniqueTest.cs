using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EPS.Extensions.Test
{
    public class UniqueLoadTest: TheoryData<int,int>
    {
        public UniqueLoadTest(int[] a,int[] b)
        {
                foreach (var i1 in a)
                {
                    foreach (var i2 in b)
                    {
                        var u = Unique.Unique.Generate(i1, i2);
                        var punc = 0;
                        var aph = 0;
                        foreach (var i in u.Select(c => (int) c))
                        {
                            if (Unique.Unique.Punctuation.FirstOrDefault(x => x == i) > 0) punc++;
                            if (Unique.Unique.AlphaNumerics.FirstOrDefault(x => x == i) > 0) aph++;
                        }

                        Assert.InRange(i1, i1 - i2, i1);
                        Assert.InRange(i2,0,i2);
                    }
                }
        }

    }
    public class UniqueTest
    {
        public static readonly int[] alphas = {6, 7, 8, 9, 10};
        public static readonly int[] puncs = {1, 2, 3, 4, 5};
        public static UniqueLoadTest ult = new UniqueLoadTest(alphas,puncs);

        private readonly ITestOutputHelper output;

        public UniqueTest(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
        }

        // [Theory]
        // [MemberData(nameof(ult))]
        // public void TestLoad(int i1, int i2)
        // {
        //     Assert.Equal(i1, i1);
        //     Assert.Equal(i2, i2);
        // }

        [Fact]
        public void TestUnique()
        {
            var hs = new HashSet<string>();
            for (int i = 0; i < 100; i++)
            {
                var u = Unique.Unique.Generate(8, 0);
                hs.Add(u);
                output.WriteLine(u);
            }
        }
    }
}
