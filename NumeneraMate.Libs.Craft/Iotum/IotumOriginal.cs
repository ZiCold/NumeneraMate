using System;
using System.Collections.Generic;
using System.Text;

namespace NumeneraMate.Libs.Craft
{
	public class IotumOriginal : Iotum
	{
		public IotumOriginal()
		{ }

		public IotumOriginal(string strToParse, string delimeter)
			: base(strToParse, delimeter)
		{ }

		public uint Io { get; set; }        // it's d6 make it Attribute?
		public uint ResponsiveSynth { get; set; }
		public uint AptClay { get; set; }
		public uint BioCircuitry { get; set; }
		public uint Synthsteel { get; set; }
		public uint PliableMetal { get; set; }
		public uint AzureSteel { get; set; }
		public uint MimeticGel { get; set; }
		public uint Quantium { get; set; }
		public uint AmberCrystal { get; set; }
		public uint Protomatter { get; set; }
		public uint ThaumDust { get; set; }
		public uint SmartTissue { get; set; }
		public uint Psiranium { get; set; }
		public uint KaonDot { get; set; }
		public uint Monopole { get; set; }
		public uint MidnightStone { get; set; }
		public uint Oraculum { get; set; }
		public uint VirtuonParticle { get; set; }
		public uint TamedIron { get; set; }
		public uint Philosophine { get; set; }
		public uint DataOrb { get; set; }
		public uint ScalarBosonRod { get; set; }
		public uint CosmicFoam { get; set; }
	}
}
