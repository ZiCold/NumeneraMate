using NumeneraMate.Libs.NumeneraObjects.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumeneraMate.Support.NUnitTests
{
    public static class ArtefactsExample
    {
        public static List<Artefact> List
        {
            get
            {
                var artefactsList = new List<Artefact>();

                artefactsList.Add(
                    new Artefact()
                    {
                        Name = "Projectile Drone",
                        Level = "1d6 + 3",
                        Form = @"A 3-foot-tall (1 m) collapsible tripod with a metallic projectile weapon mounted on top",
                        Effect = @"It takes two rounds to assemble and set up this device. Once set up, it takes an action to activate. When activated, this device follows whoever activated it around for one hour on its tripod legs (movement: short). If the user comes under attack, the drone fires one shot per round at attackers within long range, inflicting damage equal to the artifact level.",
                        Depletion = "1 in 1d20",
                        Source = "Discovery"
                    });

                artefactsList.Add(
                    new Artefact()
                    {
                        Name = "Protection Amulet",
                        Level = "1d6",
                        Form = "A stylized amulet worn on a chain",
                        Effect = "The amulet reduces one type of damage by an amount equal to the artifact level. Roll a d20 to determine the kind of damage the amulet protects against.",
                        Depletion = "1 in 1d6 (check each time the amulet reduces the damage)",
                        Source = "Discovery",

                        RollTable = new RollTable()
                        {
                            RollTableRows = new List<RollTableRow>()
                                {
                                    new RollTableRow()
                                    {
                                        Roll = "1-4",
                                        Result = "Cold"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "5-8",
                                        Result = "Electrocution and shock"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "9-12",
                                        Result = "Fire"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "13-16",
                                        Result = "Poison"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "17-20",
                                        Result = "Radiation"
                                    }
                                }
                        }
                    });

                artefactsList.Add(
                    new Artefact()
                    {
                        Name = "Amber Casement",
                        Level = "1d6 + 4",
                        Form = @"Series of short, rounded tubes and hoses about 12 inches (30 cm) long",
                        Effect = @"When activated, it solidifes the air in a 10-foot (3 m) cube of space, the center of which must be within short range of the device. The air is turned into an amberlike substance, and those trapped in it will likely suffocate or starve.",
                        Depletion = "1-4 in 1d6",
                        Source = "Discovery"
                    });

                artefactsList.Add(
                    new Artefact()
                    {
                        Name = "Amulet Of Safety",
                        Level = "1d6",
                        Form = @"Plain metallic disk on a chain",
                        Effect = @"Once the amulet is keyed to a specific numenera weapon, the weapon cannot activate to harm the wearer. The amulet’s level must be at least as high as the weapon’s level.",
                        Depletion = "-",
                        Source = "Discovery"
                    });

                return artefactsList;
            }
        }

    }
}
