using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Driver;

using Moq;

namespace Architecture.DataSource.MongoDb.Tests
{
    public static class TestHelper
    {
        public static void MockAsyncCursor<T>(Mock<IAsyncCursor<T>> mockAsyncCursor, IEnumerable<T> expected)
        {
            int expectedCount = expected.Count();

            mockAsyncCursor
                .Setup(c => c.MoveNext(default))
                .Returns(() => expectedCount-- > 0);
            mockAsyncCursor
                .SetupGet(c => c.Current)
                .Returns(expected.AsEnumerable());
        }
    }
}
