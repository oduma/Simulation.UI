using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sciendo.Core.Providers;
using Sciendo.Core;
using Simulation.DataAccess;
using Sciendo.Core.Providers.DataTypes;

namespace Tests
{
    [TestFixture]
    public class TopBuildingTests
    {
        [Test]
        public void GetTopProcessed_NoRecords_Ok()
        {
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            var results = trp.GetTopProcessed();
            Assert.IsEmpty(results);
        }

        [Test]
        public void GetTopProcessed_SomeRecords_ForOneItemType_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            sim.SaveRecordedWeeks(new List<WeekSummary>{ new WeekSummary{ItemType=ItemType.Artist,WeekNo=1,TopProcessed=true},
            new WeekSummary{ItemType=ItemType.Artist,WeekNo=3,TopProcessed=true},
            new WeekSummary{ItemType=ItemType.Artist,WeekNo=5,TopProcessed=true},
            new WeekSummary{ItemType=ItemType.Artist,WeekNo=7,TopProcessed=true}});
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            var results = trp.GetTopProcessed();
            Assert.IsNotEmpty(results);
            Assert.AreEqual(4, results.Count());
            Assert.AreEqual(4, results.Count(r => r.ItemType == ItemType.Artist));
        }

        [Test]
        public void GetTopProcessed_SomeRecords_ForSomeItemTypes_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            sim.SaveRecordedWeeks(new List<WeekSummary>{ new WeekSummary{ItemType=ItemType.Artist,WeekNo=1,TopProcessed=true},
            new WeekSummary{ItemType=ItemType.Track,WeekNo=3,TopProcessed=true},
            new WeekSummary{ItemType=ItemType.Track,WeekNo=5,TopProcessed=true},
            new WeekSummary{ItemType=ItemType.Artist,WeekNo=7,TopProcessed=true}});
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            var results = trp.GetTopProcessed();
            Assert.IsNotEmpty(results);
            Assert.AreEqual(4, results.Count());
            Assert.AreEqual(2, results.Count(r => r.ItemType == ItemType.Artist));
            Assert.AreEqual(2, results.Count(r => r.ItemType == ItemType.Track));
        }

        [Test]
        public void SaveTotalsForItems_AllNewItems_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            IScoreAlgorythm sal= ClientFactory.GetClients<IScoreAlgorythm>().First(a=>a.NoOfItemsConsidered==3);
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            trp.ClearAll(ItemType.Track);
            trp.SaveTotalForItems(new WeeklyTop
            {
                ItemType = ItemType.Track,
                WeekNo = 1,
                TopItems = new List<TopItem>{new TopItem{ItemName="Item1",ItemType=ItemType.Track,NumberOfPlays=10,Rank=1},
                new TopItem{ItemName="Item2",ItemType=ItemType.Track,NumberOfPlays=8,Rank=2},
                new TopItem{ItemName="Item3",ItemType=ItemType.Track,NumberOfPlays=7,Rank=3}}
            }, sal.ScoreRule);

            var savedTops = sim.ListTotalItems(ItemType.Track);
            Assert.IsNotEmpty(savedTops);
            Assert.AreEqual(3, savedTops.Count());
            Assert.AreEqual("Item1", savedTops[0].ItemName);
            Assert.AreEqual(ItemType.Track, savedTops[0].ItemType);
            Assert.AreEqual(10, savedTops[0].NumberOfPlays);
            Assert.AreEqual(1, savedTops[0].Rank);
            Assert.AreEqual(3, savedTops[0].Score);
            Assert.AreEqual(1, savedTops[0].EntryWeek);

            var savedRecords = sim.GetTopProcessed();

            Assert.IsNotEmpty(savedRecords);
            Assert.AreEqual(1, savedRecords.Count(r=>r.ItemType==ItemType.Track));
        }

        [Test]
        public void SaveTotalsForItems_SomeUpdates_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            IScoreAlgorythm sal = ClientFactory.GetClients<IScoreAlgorythm>().First(a => a.NoOfItemsConsidered == 3);
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            trp.ClearAll(ItemType.Track);
            trp.SaveTotalForItems(new WeeklyTop
            {
                ItemType = ItemType.Track,
                WeekNo = 1,
                TopItems = new List<TopItem>{new TopItem{ItemName="Item1",ItemType=ItemType.Track,NumberOfPlays=10,Rank=1},
                new TopItem{ItemName="Item2",ItemType=ItemType.Track,NumberOfPlays=8,Rank=2},
                new TopItem{ItemName="Item3",ItemType=ItemType.Track,NumberOfPlays=7,Rank=3}}
            }, sal.ScoreRule);

            trp.SaveTotalForItems(new WeeklyTop
            {
                ItemType = ItemType.Track,
                WeekNo = 2,
                TopItems = new List<TopItem>{new TopItem{ItemName="Item3",ItemType=ItemType.Track,NumberOfPlays=10,Rank=1},
                new TopItem{ItemName="Item4",ItemType=ItemType.Track,NumberOfPlays=9,Rank=2},
                new TopItem{ItemName="Item5",ItemType=ItemType.Track,NumberOfPlays=7,Rank=3}}
            }, sal.ScoreRule);

            var savedTops = sim.ListTotalItems(ItemType.Track);
            Assert.IsNotEmpty(savedTops);
            Assert.AreEqual(5, savedTops.Count());
            Assert.AreEqual("Item3", savedTops[0].ItemName);
            Assert.AreEqual(ItemType.Track, savedTops[0].ItemType);
            Assert.AreEqual(17, savedTops[0].NumberOfPlays);
            Assert.AreEqual(1, savedTops[0].Rank);
            Assert.AreEqual(4, savedTops[0].Score);
            Assert.AreEqual(1, savedTops[0].EntryWeek);

            Assert.AreEqual("Item4", savedTops[2].ItemName);
            Assert.AreEqual(ItemType.Track, savedTops[2].ItemType);
            Assert.AreEqual(9, savedTops[2].NumberOfPlays);
            Assert.AreEqual(3, savedTops[2].Rank);
            Assert.AreEqual(2, savedTops[2].Score);
            Assert.AreEqual(2, savedTops[2].EntryWeek);

            var savedRecords = sim.GetTopProcessed();

            Assert.IsNotEmpty(savedRecords);
            Assert.AreEqual(2, savedRecords.Count(r=>r.ItemType==ItemType.Track));
        }

        [Test]
        public void GetTotalItems_NoRecords_Ok()
        {
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            var results = trp.GetTotalItems(ItemType.Artist);
            Assert.IsEmpty(results);
            
        }

        [Test]
        public void GetTotalItems_Ok()
        {
            IScoreAlgorythm sal = ClientFactory.GetClients<IScoreAlgorythm>().First(a => a.NoOfItemsConsidered == 3);
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            trp.SaveTotalForItems(new WeeklyTop
            {
                ItemType = ItemType.Track,
                WeekNo = 1,
                TopItems = new List<TopItem>{new TopItem{ItemName="Item1",ItemType=ItemType.Track,NumberOfPlays=10,Rank=1},
                new TopItem{ItemName="Item2",ItemType=ItemType.Track,NumberOfPlays=8,Rank=2},
                new TopItem{ItemName="Item3",ItemType=ItemType.Track,NumberOfPlays=7,Rank=3}}
            }, sal.ScoreRule);

            trp.SaveTotalForItems(new WeeklyTop
            {
                ItemType = ItemType.Track,
                WeekNo = 2,
                TopItems = new List<TopItem>{new TopItem{ItemName="Item3",ItemType=ItemType.Track,NumberOfPlays=10,Rank=1},
                new TopItem{ItemName="Item4",ItemType=ItemType.Track,NumberOfPlays=9,Rank=2},
                new TopItem{ItemName="Item5",ItemType=ItemType.Track,NumberOfPlays=7,Rank=3}}
            }, sal.ScoreRule);

            var savedTops = trp.GetTotalItems(ItemType.Track);
            Assert.IsNotEmpty(savedTops);
            Assert.AreEqual(5, savedTops.Count());
            Assert.AreEqual("Item3", savedTops[0].ItemName);
            Assert.AreEqual(ItemType.Track, savedTops[0].ItemType);
            Assert.AreEqual(17, savedTops[0].NumberOfPlays);
            Assert.AreEqual(1, savedTops[0].Rank);
            Assert.AreEqual(4, savedTops[0].Score);
            Assert.AreEqual(1, savedTops[0].EntryWeek);

            Assert.AreEqual("Item4", savedTops[2].ItemName);
            Assert.AreEqual(ItemType.Track, savedTops[2].ItemType);
            Assert.AreEqual(9, savedTops[2].NumberOfPlays);
            Assert.AreEqual(3, savedTops[2].Rank);
            Assert.AreEqual(2, savedTops[2].Score);
            Assert.AreEqual(2, savedTops[2].EntryWeek);
        }

        [Test]
        public void ClearAll_NothingToClear_Ok()
        {
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            trp.ClearAll(ItemType.Artist);
        }

        [Test]
        public void ClearAll_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            IScoreAlgorythm sal = ClientFactory.GetClients<IScoreAlgorythm>().First(a => a.NoOfItemsConsidered == 3);
            ITopRecordProvider trp = ClientFactory.GetClient<ITopRecordProvider>();
            trp.SaveTotalForItems(new WeeklyTop
            {
                ItemType = ItemType.Track,
                WeekNo = 1,
                TopItems = new List<TopItem>{new TopItem{ItemName="Item1",ItemType=ItemType.Track,NumberOfPlays=10,Rank=1},
                new TopItem{ItemName="Item2",ItemType=ItemType.Track,NumberOfPlays=8,Rank=2},
                new TopItem{ItemName="Item3",ItemType=ItemType.Track,NumberOfPlays=7,Rank=3}}
            }, sal.ScoreRule);

            trp.SaveTotalForItems(new WeeklyTop
            {
                ItemType = ItemType.Track,
                WeekNo = 2,
                TopItems = new List<TopItem>{new TopItem{ItemName="Item3",ItemType=ItemType.Track,NumberOfPlays=10,Rank=1},
                new TopItem{ItemName="Item4",ItemType=ItemType.Track,NumberOfPlays=9,Rank=2},
                new TopItem{ItemName="Item5",ItemType=ItemType.Track,NumberOfPlays=7,Rank=3}}
            }, sal.ScoreRule);

            trp.ClearAll(ItemType.Track);
            var savedTops = trp.GetTotalItems(ItemType.Track);
            Assert.IsEmpty(savedTops);

            var savedRecords = sim.GetTopProcessed();

            Assert.IsEmpty(savedRecords);
        }

    }
}
