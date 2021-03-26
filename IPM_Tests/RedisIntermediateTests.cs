using System.Data;
using NUnit.Framework;

namespace IPM_Project
{
    [TestFixture]
    public class RedisIntermediateTests {

        private RedisIntermediate RedisIntermediate;

        [SetUp]
        public void Setup() {
            this.RedisIntermediate = new RedisIntermediate();
        }

        [Test]
        public void SendTestRequest() {
            this.RedisIntermediate.SendRequest("Test request", CommandType.COMMENT);
        }

    }
}