using NumeneraMate.Libs.NumeneraObjects.Craft;
using NUnit.Framework;

namespace NumeneraMate.Support.NUnitTests
{
	public class IotumTests
	{
		[SetUp]
		public void Setup()
        {
        }

		string inputStringFromPDF = "Io(1d6 units); responsive synth(50 units);" +
		"azure steel(15 units); amber crystal(15 units);" +
		"pliable metal(10 units); protomatter(1 unit);" +
		"monopole(4 units); philosophine(4 units);";

		string expectedCsvRow = "1;50;;;;10;15;;;15;1;;;;;4;;;;;4;;;;";

		string expectedCsvTitles = "Io;ResponsiveSynth;AptClay;BioCircuitry;" +
				"Synthsteel;PliableMetal;AzureSteel;MimeticGel;Quantium;AmberCrystal;" +
				"Protomatter;ThaumDust;SmartTissue;Psiranium;KaonDot;Monopole;" +
				"MidnightStone;Oraculum;VirtuonParticle;TamedIron;Philosophine;" +
				"DataOrb;ScalarBosonRod;CosmicFoam;";

		IotumOriginal expectedOriginUnit = new IotumOriginal()
		{
			Io = 1,
			ResponsiveSynth = 50,
			AzureSteel = 15,
			AmberCrystal = 15,
			PliableMetal = 10,
			Protomatter = 1,
			Monopole = 4,
			Philosophine = 4
		};

		IotumHomebrew expectedHomebrewUnit = new IotumHomebrew()
		{
			Io = 1,
			ResponsiveSynth = 50,
			PliableMetal = 10,
			MimeticGel = 15,
			AmberCrystal = 25,
			Psiranium = 4,
			TamedIron = 4
		};

		[Test]
		public void CreateOriginalIotumFromString()
		{
			var actualUnit = new IotumOriginal(inputStringFromPDF, ";");
			Assert.AreEqual(expectedOriginUnit, actualUnit);
		}

		[Test]
		public void CreateCsvRowFromOriginalIotum()
		{
			var actualString = expectedOriginUnit.GetCSV(";");
			Assert.AreEqual(expectedCsvRow, actualString);
		}

		[Test]
		public void CreateCsvTitlesFromOriginalIotum()
		{
			var actualString = expectedOriginUnit.GetCSVTitles(";");
			Assert.AreEqual(expectedCsvTitles, actualString);
		}

		[Test]
		public void CreateHomebrewFromString()
		{
			var iotumOriginal = new IotumOriginal(inputStringFromPDF, ";");
			var actualUnit = new IotumHomebrew(iotumOriginal);
			Assert.AreEqual(expectedHomebrewUnit, actualUnit);
		}

	}
}
