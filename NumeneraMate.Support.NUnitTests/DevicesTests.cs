using NUnit.Framework;
using NumeneraMate.Support.Parsers;
using NumeneraMate.Libs.Devices;
using System.IO;
using System.Collections.Generic;
using FluentAssertions;

namespace NumeneraMate.Support.NUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCalculatedProperties()
        {
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

            var d10baseLevel = d10cypher.LevelBase;
            Assert.AreEqual(10, d10baseLevel);

            var d10levelTerm = d10cypher.LevelTerm;
            Assert.AreEqual(0, d10levelTerm);

            var d10minCraftingLevel = d10cypher.MinimumCraftingLevel;
            Assert.AreEqual(1, d10minCraftingLevel);

            var d6cypherXML = @"  <Cypher>
    <Name>Amplification Parasite</Name>
    <Level>1d6 + 4</Level>
    <MinimumCraftingLevel>4</MinimumCraftingLevel>
    <Internal>Living fish, beetle, or worm that must be ingested</Internal>
    <Effect>Upon eating the parasite, the user chooses one stat and the GM chooses a different stat. The difficulty of any roll related to the user’s chosen stat is reduced by two steps, and the difficulty of any roll involving the GM’s chosen stat is increased by two steps. The parasite dies after 1d6 hours, and the effect ends when the user violently expels it from her body.</Effect>
    <Source>Compendium</Source>
  </Cypher>";
            var d6cypher = NumeneraXML.DeserializeCypherFromXMLString(d6cypherXML);

            var d6baseLevel = d6cypher.LevelBase;
            Assert.AreEqual(6, d6baseLevel);

            var d6levelTerm = d6cypher.LevelTerm;
            Assert.AreEqual(4, d6levelTerm);

            var d6minCraftingLevel = d6cypher.MinimumCraftingLevel;
            Assert.AreEqual(5, d6minCraftingLevel);
        }

        [Test]
        public void TestPDFParsing()
        {
            var directory = @"..\..\..\ExampleFiles";
            var name = "Test_Cyphers.txt";
            var fileName = Path.Combine(directory, name);
            var fileNameXml = Path.Combine(directory, Path.GetFileNameWithoutExtension(fileName) + ".xml");
            var deviceParser = new DevicesParser("Discovery", DeviceType.Cypher);
            deviceParser.CreateXMLFromRawCyphersText(fileName, fileNameXml);
            var cyphers = NumeneraXML.DeserializeCyphersListFromXML(fileNameXml);

            cyphers[0].Should().BeEquivalentTo(CyphersExample.List[0]);
            cyphers[1].Should().BeEquivalentTo(CyphersExample.List[1]);

            //cyphers[2].RollTable.Should().BeEquivalentTo(CyphersExample.List[2].RollTable);

            //cyphers.Should().BeEquivalentTo(CyphersExample.List);
        }
    }
}