using Kraken.WebSockets.Messages;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Kraken.WebSockets.Tests.Messages
{
    [ExcludeFromCodeCoverage]
    public class BookSnapshotMessageTests
    {
        #region CreateFromString()

        [Fact]
        public void CreateFromString_Test()
        {
            var rawBookSnapshotMessage = @"[0,{""as"":[[""5541.30000"",""2.50700000"",""1534614248.123678""],
      [
          ""5541.80000"",
          ""0.33000000"",
          ""1534614098.345543""
      ],
      [
          ""5542.70000"",
          ""0.64700000"",
          ""1534614244.654432""
      ]
    ],
    ""bs"": [
      [
          ""5541.20000"",
          ""1.52900000"",
          ""1534614248.765567""
      ],
      [
          ""5539.90000"",
          ""0.30000000"",
          ""1534614241.769870""
      ],
      [
          ""5539.50000"",
          ""5.00000000"",
          ""1534613831.243486""
      ]
    ]
  }
]";
            var bookSnapshotMessage = BookSnapshotMessage.CreateFromString(rawBookSnapshotMessage);

            Assert.NotNull(bookSnapshotMessage);
            Assert.Equal(0, bookSnapshotMessage.ChannelId);
            Assert.NotNull(bookSnapshotMessage.Asks);
            Assert.Equal(3, bookSnapshotMessage.Asks.Length);
            Assert.NotNull(bookSnapshotMessage.Bids);
            Assert.Equal(3, bookSnapshotMessage.Bids.Length);
        }

        #endregion
    }
}
