using System;
using System.Collections.Generic;
using System.Text;

namespace NumeneraMate.Libs.NumeneraObjects.Craft
{
	public class IotumHomebrew : Iotum
	{
		public IotumHomebrew()
		{ }

		public IotumHomebrew(string strToParse, string delimeter)
			: base(strToParse, delimeter)
		{ }

		/// <summary>
		/// Ctor for transmorf of homebrew Iotum
		/// </summary>
		/// <param name="origin"></param>
		public IotumHomebrew(IotumOriginal origin)
		{
			Io = origin.Io;
			ResponsiveSynth = origin.ResponsiveSynth;
			AptClay = origin.AptClay;

			PliableMetal = origin.PliableMetal;
			PliableMetal += origin.BioCircuitry;
			PliableMetal += origin.Synthsteel;

			MimeticGel = origin.MimeticGel;
			MimeticGel += origin.AzureSteel;
			MimeticGel += origin.Quantium;

			AmberCrystal = origin.AmberCrystal;
			AmberCrystal += origin.ThaumDust;
			AmberCrystal += 10 * origin.Protomatter;

			Psiranium = origin.Psiranium;
			Psiranium += origin.KaonDot;
			Psiranium += origin.Monopole;
			Psiranium += origin.SmartTissue;

			Oraculum = origin.Oraculum;
			Oraculum += origin.MidnightStone;
			Oraculum += origin.VirtuonParticle;

			TamedIron = origin.TamedIron;
			TamedIron += origin.Philosophine;
			TamedIron += origin.ScalarBosonRod;
			TamedIron += origin.DataOrb;

			CosmicFoam = origin.CosmicFoam;
		}

		public uint Io { get; set; }        // it's d6 - to Attribute?
		public uint ResponsiveSynth { get; set; }
		public uint AptClay { get; set; }
		public uint PliableMetal { get; set; }
		public uint MimeticGel { get; set; }
		public uint AmberCrystal { get; set; }
		public uint Psiranium { get; set; }
		public uint Oraculum { get; set; }
		public uint TamedIron { get; set; }
		public uint CosmicFoam { get; set; }
	}
}
