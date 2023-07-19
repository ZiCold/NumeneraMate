using NUnit.Framework;
using NumeneraMate.Support.Parsers;
using NumeneraMate.Libs.Devices;
using System.IO;
using System.Collections.Generic;
using FluentAssertions;
using System;

namespace NumeneraMate.Support.NUnitTests
{
    public class DevicesTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCalculatedProperties()
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());

            var d10cypherXML = @"  <Cypher>
    <Name>Analysis Scanner</Name>
    <Level>1d10</Level>
    <MinimumCraftingLevel>1</MinimumCraftingLevel>
    <Wearable>Bracelet</Wearable>
    <Usable>Handheld device</Usable>
    <Effect>This device scans and records everything within short range for one round and then conveys the level and nature of all creatures, objects, and energy sources it scanned. This information can be accessed for 28 hours after the scan.</Effect>
    <Source>Compendium</Source>
  </Cypher>";
            var d10cypher = NumeneraXML.DeserializeCypherFromXMLString(d10cypherXML);

            var d10baseLevel = d10cypher.LevelBaseDice;
            Assert.AreEqual(10, d10baseLevel);

            var d10levelTerm = d10cypher.LevelIncrease;
            Assert.AreEqual(0, d10levelTerm);

            var d10minCraftingLevel = d10cypher.MinimumCraftingLevel;
            Assert.AreEqual(1, d10minCraftingLevel);

            var d10randomLevel = rand.Next(1, d10cypher.LevelBaseDice) + d10cypher.LevelIncrease;
            Assert.IsTrue(d10randomLevel >= 1 && d10randomLevel <= 10);


            var d6cypherXML = @"  <Cypher>
    <Name>Amplification Parasite</Name>
    <Level>1d6 + 4</Level>
    <MinimumCraftingLevel>4</MinimumCraftingLevel>
    <Internal>Living fish, beetle, or worm that must be ingested</Internal>
    <Effect>Upon eating the parasite, the user chooses one stat and the GM chooses a different stat. The difficulty of any roll related to the user’s chosen stat is reduced by two steps, and the difficulty of any roll involving the GM’s chosen stat is increased by two steps. The parasite dies after 1d6 hours, and the effect ends when the user violently expels it from her body.</Effect>
    <Source>Compendium</Source>
  </Cypher>";
            var d6cypher = NumeneraXML.DeserializeCypherFromXMLString(d6cypherXML);

            var d6baseLevel = d6cypher.LevelBaseDice;
            Assert.AreEqual(6, d6baseLevel);

            var d6levelTerm = d6cypher.LevelIncrease;
            Assert.AreEqual(4, d6levelTerm);

            var d6minCraftingLevel = d6cypher.MinimumCraftingLevel;
            Assert.AreEqual(5, d6minCraftingLevel);

            var d6randomLevel = rand.Next(1, d6cypher.LevelBaseDice) + d6cypher.LevelIncrease;
            Assert.IsTrue(d6randomLevel >= 5 && d6randomLevel <= 10);


            var staticLevelCypherXML = @"  <Cypher>
    <Name>Detonation (singularity)</Name>
    <Level>10</Level>
    <Effect>Explodes and creates a momentary singularity that tears at the fabric of the universe. Inflicts 20 points of damage to all within short range, drawing them (or their remains) together to immediate range (if possible). Player characters in the radius move one step down the damage track if they fail a Might defense roll.</Effect>
    <Source>Discovery</Source>
    <Usable>Explosive device or ceramic sphere (thrown, short range) or handheld projector (long range)</Usable>
  </Cypher>";
            var staticLevelCypher = NumeneraXML.DeserializeCypherFromXMLString(staticLevelCypherXML);

            var staticLevel = staticLevelCypher.LevelBaseDice;
            Assert.AreEqual(0, staticLevel);

            var staticLevelTerm = staticLevelCypher.LevelIncrease;
            Assert.AreEqual(10, staticLevelTerm);

            var staticLevelMinCraftingLevel = staticLevelCypher.MinimumCraftingLevel;
            Assert.AreEqual(10, staticLevelMinCraftingLevel);

            var staticLevelRandom = (staticLevelCypher.LevelBaseDice == 0 ? 0 : rand.Next(1, d6cypher.LevelBaseDice)) + staticLevelCypher.LevelIncrease;
            Assert.IsTrue(staticLevelRandom == 10);
        }

        [Test]
        public void TestPDFParsing_Cyphers()
        {
            var directory = @"..\..\..\ExampleFiles";
            var name = "Test_Cyphers.txt";
            var fileName = Path.Combine(directory, name);
            var fileNameXml = Path.Combine(directory, Path.GetFileNameWithoutExtension(fileName) + ".xml");
            var deviceParser = new DevicesParser();
            var cyphers = deviceParser.GetCyphersListFromRawText(fileName, "Discovery");

            cyphers.Should().BeEquivalentTo(CyphersExample.List);
        }

        [Test]
        public void TestPDFParsing_Artefacts()
        {
            var directory = @"..\..\..\ExampleFiles";
            var name = "Test_Artefacts.txt";
            var fileName = Path.Combine(directory, name);
            var fileNameXml = Path.Combine(directory, Path.GetFileNameWithoutExtension(fileName) + ".xml");
            var deviceParser = new DevicesParser();
            var artefacts = deviceParser.GetArtefactsListFromRawText(fileName, "Discovery");

            System.Diagnostics.Debug.WriteLine(artefacts[0].Name);
            artefacts.Should().BeEquivalentTo(ArtefactsExample.List);
            //artefacts[1].Should().BeEquivalentTo(ArtefactsExample.List[1]);
        }
    }
}