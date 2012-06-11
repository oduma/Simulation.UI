using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tests.Mocks;
using Sciendo.Core;
using Sciendo.Core.CacheManager;

namespace Tests
{
    [TestFixture]
    public class CacheProviderTests
    {
        MockCacheManager cm;

        [SetUp]
        public void SetUp()
        {
            cm = (MockCacheManager)ClientFactory.GetClient<ICacheManager>();
            cm.MyOwnFakeCache = new SortedList<string, object>();
        }

        [Test]
        public void WithFullKeyOnly_CachePresent()
        {
            IMockClassUsingCache mc = ClientFactory.GetClient<IMockClassUsingCache>();

            cm.MyOwnFakeCache.Add("CachedMethodForFullKeyOnlyCachePresent-abc", "abc - Processed!!!");
            var result = mc.CachedMethodForFullKeyOnlyCachePresent("abc");

            Assert.IsNotNull(result);

            Assert.AreEqual(cm.MyOwnFakeCache["CachedMethodForFullKeyOnlyCachePresent-abc"].ToString(), result);
        }

        [Test]
        public void WithFullKeyOnly_CacheNotPresent()
        {
            IMockClassUsingCache mc = ClientFactory.GetClient<IMockClassUsingCache>();

            var result = mc.CachedMethodForFullKeyOnly("abc");

            Assert.IsNotNull(result);

            Assert.AreEqual("abc - Processed!!!", result);

            Assert.IsTrue(cm.MyOwnFakeCache.ContainsKey("CachedMethodForFullKeyOnly-abc"));

            Assert.AreEqual(cm.MyOwnFakeCache["CachedMethodForFullKeyOnly-abc"].ToString(),result);
        }

        [Test]
        public void FullAndMinimalKey_FullKeyCachePresent()
        {
            IMockClassUsingCache mc = ClientFactory.GetClient<IMockClassUsingCache>();
            cm.MyOwnFakeCache.Add("CacheMethodForFullAndMinimalKeyCachePresent-abc-def", "abcdef - Processed!!!");
            var result = mc.CacheMethodForFullAndMinimalKeyCachePresent("abc", "def");

            Assert.IsNotNull(result);

            Assert.AreEqual(cm.MyOwnFakeCache["CacheMethodForFullAndMinimalKeyCachePresent-abc-def"].ToString(), result);
        }

        [Test]
        public void FullAndMinimalKey_MinimalKeyCachePresentOnly()
        {
            IMockClassUsingCache mc = ClientFactory.GetClient<IMockClassUsingCache>();
            if (!cm.MyOwnFakeCache.ContainsKey("CacheMethodForFullAndMinimalKeyMinimalCachePresent-feg-ghi"))
                cm.MyOwnFakeCache.Add("CacheMethodForFullAndMinimalKeyMinimalCachePresent-feg-ghi", "some other old string - Processed!!!");
            var result = mc.CacheMethodForFullAndMinimalKeyMinimalCachePresent("abc", "def");

            Assert.IsNotNull(result);

            Assert.AreEqual(cm.MyOwnFakeCache["CacheMethodForFullAndMinimalKeyMinimalCachePresent-feg-ghi"].ToString(), result);
        }

        [Test]
        public void FullAndMinimalKey_FullAndMinimalKeyPresent()
        {
            IMockClassUsingCache mc = ClientFactory.GetClient<IMockClassUsingCache>();
            cm.MyOwnFakeCache.Add("CacheMethodForFullAndMinimalKeyMinimalCachePresent-feg-ghi", "some other old string - Processed!!!");
            cm.MyOwnFakeCache.Add("CacheMethodForFullAndMinimalKeyMinimalCachePresent-abc-def", "some new string here - processed!!!");
            var result = mc.CacheMethodForFullAndMinimalKeyMinimalCachePresent("abc", "def");

            Assert.IsNotNull(result);

            Assert.AreEqual(cm.MyOwnFakeCache["CacheMethodForFullAndMinimalKeyMinimalCachePresent-abc-def"].ToString(), result);
        }

        [Test]
        public void FullAndMinimalKey_NoKeyPresent()
        {
            IMockClassUsingCache mc = ClientFactory.GetClient<IMockClassUsingCache>();
            var result = mc.CacheMethodForFullAndMinimalKey("abc", "def");

            Assert.IsNotNull(result);

            Assert.AreEqual("abcdef - Processed!!!", result);

            Assert.IsTrue(cm.MyOwnFakeCache.ContainsKey("CacheMethodForFullAndMinimalKey-abc-def"));

            Assert.AreEqual(cm.MyOwnFakeCache["CacheMethodForFullAndMinimalKey-abc-def"].ToString(), result);
        }

        [Test]
        public void FullAndMinimalKey_NoKeyPresent_NullReturn()
        {
            IMockClassUsingCache mc = ClientFactory.GetClient<IMockClassUsingCache>();
            var result = mc.CacheMethodForFullAndMinimalKeyMinimalCachePresent("abc", "def");

            Assert.IsNull(result);

            Assert.IsFalse(cm.MyOwnFakeCache.ContainsKey("CacheMethodForFullAndMinimalKeyMinimalCachePresent-abc-def"));

            Assert.IsFalse(cm.MyOwnFakeCache.ContainsKey("CacheMethodForFullAndMinimalKeyMinimalCachePresent"));

            Assert.AreEqual(cm.MyOwnFakeCache.Count, 0);
        }
    }
}
