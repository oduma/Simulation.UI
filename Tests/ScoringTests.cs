using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sciendo.Core.Providers;
using Sciendo.Core;
using Sciendo.Core.Providers.DataTypes;
using Simulation.DataAccess;

namespace Tests
{
    [TestFixture]
    public class ScoringTests
    {
        [Test]
        public void GetAllAvailableScoringAlgorythms_NothingInUse_Ok()
        {
            IAlgorythmPoolProvider app = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            var allAlgs = app.GetAvailableScoreAlgorythms(ItemType.Artist);
            Assert.IsNotNull(allAlgs);
            Assert.AreEqual(2, allAlgs.Count());
            Assert.AreEqual(2, allAlgs.Count(a => !a.InUse));
        }

        [Test]
        public void GetAllAvailableScoringAlgorythm_OneInUse_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            sim.SaveCurrentScoreAlgorythm(new CurrentScoreAlgorythm { ItemType = ItemType.Artist, Name = "F1" });
            IAlgorythmPoolProvider app = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            var allAlgs = app.GetAvailableScoreAlgorythms(ItemType.Artist);
            Assert.IsNotNull(allAlgs);
            Assert.AreEqual(2, allAlgs.Count());
            Assert.AreEqual(1, allAlgs.Count(a => !a.InUse));
            Assert.AreEqual("F1", allAlgs.Where(a => a.InUse).Select(a => a.Name).First());
        }

        [Test]
        public void SetRule_First_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            IAlgorythmPoolProvider app = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            app.SetRule(new CurrentScoreAlgorythm { Name = "F1", ItemType = ItemType.Artist });
            var allAlgs = app.GetAvailableScoreAlgorythms(ItemType.Artist);
            Assert.IsNotNull(allAlgs);
            Assert.AreEqual(2, allAlgs.Count());
            Assert.AreEqual(1, allAlgs.Count(a => !a.InUse));
            Assert.AreEqual("F1", allAlgs.Where(a => a.InUse).Select(a => a.Name).First());
            Assert.AreEqual(1, sim.ListCurrentScoreAlgorythms().Count);
        }

        [Test]
        public void SetRule_UpdateExisting_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            sim.SaveCurrentScoreAlgorythm(new CurrentScoreAlgorythm { Name = "Top 3", ItemType = ItemType.Artist });
            IAlgorythmPoolProvider app = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            app.SetRule(new CurrentScoreAlgorythm { Name = "F1", ItemType = ItemType.Artist });
            var allAlgs = app.GetAvailableScoreAlgorythms(ItemType.Artist);
            Assert.IsNotNull(allAlgs);
            Assert.AreEqual(2, allAlgs.Count());
            Assert.AreEqual(1, allAlgs.Count(a => !a.InUse));
            Assert.AreEqual("F1", allAlgs.Where(a => a.InUse).Select(a => a.Name).First());
            Assert.AreEqual(1, sim.ListCurrentScoreAlgorythms().Count);
            Assert.AreEqual("F1", sim.ListCurrentScoreAlgorythms()[0].Name);
        }

        [Test]
        public void GetCurrentAlgorythm_FoundOne_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            sim.SaveCurrentScoreAlgorythm(new CurrentScoreAlgorythm { Name = "F1", ItemType = ItemType.Artist });
            IAlgorythmPoolProvider app = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            var currentAlg = app.GetCurrentAlgorythm(ItemType.Artist);
            Assert.IsNotNull(currentAlg);
            Assert.AreEqual("F1", currentAlg.Name);
            Assert.AreEqual(8, currentAlg.NoOfItemsConsidered);
            Assert.True(currentAlg.InUse);
            Assert.IsNotNull(currentAlg.ScoreRule);
        }

        [Test]
        public void GetCurrentAlgorythm_FoundNone_Ok()
        {
            ISimulationCRUD sim = ClientFactory.GetClient<ISimulationCRUD>();
            sim.SaveCurrentScoreAlgorythm(new CurrentScoreAlgorythm { Name = "F1", ItemType = ItemType.Artist });
            IAlgorythmPoolProvider app = ClientFactory.GetClient<IAlgorythmPoolProvider>();
            var currentAlg = app.GetCurrentAlgorythm(ItemType.Track);
            Assert.IsNotNull(currentAlg);
            Assert.AreEqual("Top 3 (3,2,1)", currentAlg.Name);
            Assert.AreEqual(3, currentAlg.NoOfItemsConsidered);
            Assert.False(currentAlg.InUse);
            Assert.IsNotNull(currentAlg.ScoreRule);
        }
    }
}
