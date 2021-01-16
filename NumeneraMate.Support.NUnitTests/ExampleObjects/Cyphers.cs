using NumeneraMate.Libs.Devices;
using System;
using System.Collections.Generic;
using System.Text;

namespace NumeneraMate.Support.NUnitTests
{
    public static class CyphersExample
    {
        public static List<Cypher> List
        {
            get
            {
                var cyphersList = new List<Cypher>();
                
                cyphersList.Add(
                    new Cypher()
                    {
                        Name = "Detonation",
                        Level = "1d6 + 2",
                        Effect = "Explodes in an immediate radius,  Roll for the type of damage:",
                        Source = "Discovery",
                        Wearable = "Wristband projector (long range)",
                        Usable = "Explosive device (thrown, short range) or handheld projector (long range)",
                        RollTable = new RollTable()
                        {
                            RollTableRows = new List<RollTableRow>()
                                {
                                    new RollTableRow()
                                    {
                                        Roll = "01-10",
                                        Result = "Cell-disrupting (harms only flesh)"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "11-30",
                                        Result = "Corrosive"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "31-40",
                                        Result = "Electrical discharge"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "41-50",
                                        Result = "Heat drain (cold)"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "51-75",
                                        Result = "Fire"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "76-00",
                                        Result = "Shrapnel"
                                    }
                                }
                        }
                    });
                
                cyphersList.Add(
                    new Cypher()
                    {
                        Name = "Density Nodule",
                        Level = "1d6",
                        Effect = "For the next 28 hours, each time the weapon the nodule " +
                        "is attached to strikes a solid creature or object, " +
                        "the weapon suddenly increases dramatically in weight, " +
                        "causing the blow to inflict an additional 2 points of damage " +
                        "(3 points if the cypher is level 4 or higher).",
                        Source = "Discovery",
                        Usable = "Crystal nodule affixed to a melee weapon"
                    });

                cyphersList.Add(
                    new Cypher()
                    {
                        Name = "Skill Boost",
                        Level = "1d6",

                        Usable = "Injector",
                        Effect = @"Dramatically but temporarily alters the user’s mind and body so that one specific physical action they can perform is eased by three steps. Once activated, this boost can be used a number of times equal to the cypher’s level, but only within a 28-hour period. The boost takes effect each time the action is performed, so a level 3 cypher boosts the first three times the action is attempted. The action can be one of a number of possibilities:",

                        RollTable = new RollTable()
                        {
                            RollTableRows = new List<RollTableRow>()
                                {
                                    new RollTableRow()
                                    {
                                        Roll = "01-15",
                                        Result = "Melee attack"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "16-30",
                                        Result = "Ranged attack"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "31-40",
                                        Result = "Speed defense"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "41-50",
                                        Result = "Might defense"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "51-60",
                                        Result = "Intellect defense"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "61-68",
                                        Result = "Jumping"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "69-76",
                                        Result = "Climbing"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "77-84",
                                        Result = "Running"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "85-92",
                                        Result = "Swimming"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "93-94",
                                        Result = "Sneaking"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "95-96",
                                        Result = "Balancing"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "97-98",
                                        Result = "Perceiving"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "99",
                                        Result = "Carrying"
                                    },
                                    new RollTableRow()
                                    {
                                        Roll = "00",
                                        Result = "Escaping"
                                    }
                                }

                        },
                        
                        Source = "Discovery",
                        Internal = @"Pill, ingestible liquid " +
                                "Smthmore here"
                    });

                cyphersList.Add(
                    new Cypher()
                    {
                        Name = "Density Nodule",
                        Level = "1d6",
                        Effect = @"For the next 28 hours, each time the weapon the nodule is attached to strikes a solid creature or object, the weapon suddenly increases dramatically in weight, causing the blow to inflict an additional 2 points of damage (3 points if the cypher is level 4 or higher).",
                        Source = "Discovery",
                        Usable = "Crystal nodule affixed to a melee weapon"
                    });

                return cyphersList;
            }
        }
    }
}
