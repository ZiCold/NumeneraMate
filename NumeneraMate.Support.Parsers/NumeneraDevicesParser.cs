using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Support.Parsers
{
    /// <summary>
    /// Creates xml/csv from from PDF extracted text
    /// </summary>
    public class NumeneraDevicesParser
    {
        public string FileName { get; set; }
        public string Source { get; set; }
        public List<string> KeywordsList { get; set; }
        
        // Input - fileName, source, keywords
        public NumeneraDevicesParser(string fileName, string sourceBook, List<string> keywordsList)
        {
            FileName = fileName;
            Source = sourceBook;
            KeywordsList = keywordsList;
        }


        public void TestArtefacts()
        {
            var testArtefact = @"Projectile Drone
Level: 1d6 + 3
Form: A 3-foot-tall (1 m) collapsible tripod
with a metallic projectile weapon mounted on
top
Effect: It takes two rounds to assemble and set
up this device. Once set up, it takes an action
to activate. When activated, this device follows
whoever activated it around for one hour on
its tripod legs (movement: short). If the user
comes under attack, the drone fires one shot
per round at attackers within long range,
inflicting damage equal to the artifact level.
Depletion: 1 in 1d20
Protection Amulet
Level: 1d6
Form: A stylized amulet worn on a chain
Effect: The amulet reduces one type of
damage by an amount equal to the artifact
level. Roll a d20 to determine the kind of
damage the amulet protects against.
Table:
1–4 Cold
5–8 Electrocution and shock
9–12 Fire
13–16 Poison
17–20 Radiation
Depletion: 1 in 1d6 (check each time the
amulet reduces the damage)
AMBER CASEMENT
Level: 1d6 + 4
Form: Series of short, rounded tubes and
hoses about 12 inches (30 cm) long
Effect: When activated, it solidifes the air in a
10-foot (3 m) cube of space, the center of
which must be within short range of the
device. The air is turned into an amberlike
substance, and those trapped in it will
likely suffocate or starve.
Depletion: 1–4 in 1d6
AMULET OF SAFETY
Level: 1d6
Form: Plain metallic disk on a chain
Effect: Once the amulet is keyed to a specific
numenera weapon, the weapon cannot
activate to harm the wearer. The amulet’s
level must be at least as high as the
weapon’s level.
Depletion: —";
            Console.WriteLine(testArtefact);
        }

        public void TestCyphers()
        {
            var testCyphers = @"DETONATION
Level: 1d6 + 2
Wearable: Wristband projector (long range)

Effect: Explodes in an immediate radius, 
Roll for the type of damage:
Table:
01–10 Cell-disrupting (harms only flesh)
11–30 Corrosive
31–40 Electrical discharge
41–50 Heat drain (cold)
51–75 Fire
76–00 Shrapnel
Usable: Explosive device (thrown, short range)
or handheld projector (long range)

DENSITY NODULE
Level: 1d6
Usable: Crystal nodule affixed to a melee
weapon
Effect: For the next 28 hours, each time the
weapon the nodule is attached to strikes
a solid creature or object, the weapon
suddenly increases dramatically in weight,
causing the blow to inflict an additional 2
points of damage (3 points if the cypher is
level 4 or higher).

SKILL BOOST
Level: 1d6

Usable: Injector
Effect: Dramatically but temporarily alters the
user’s mind and body so that one specific
physical action they can perform is eased by
three steps. Once activated, this boost can be
used a number of times equal to the cypher’s
level, but only within a 28-hour period. The
boost takes effect each time the action is
performed, so a level 3 cypher boosts the
first three times the action is attempted. The
action can be one of a number of possibilities:
Table:
01–15 Melee attack
16–30 Ranged attack
31–40 Speed defense
41–50 Might defense
51–60 Intellect defense
61–68 Jumping
69–76 Climbing
77–84 Running
85–92 Swimming
93–94 Sneaking
95–96 Balancing
97–98 Perceiving
99 Carrying
00 Escaping
Internal: Pill, ingestible liquid
Smthmore here

DENSITY NODULE
Level: 1d6
Usable: Crystal nodule affixed to a melee
weapon
Effect: For the next 28 hours, each time the
weapon the nodule is attached to strikes
a solid creature or object, the weapon
suddenly increases dramatically in weight,
causing the blow to inflict an additional 2
points of damage (3 points if the cypher is
level 4 or higher).";
            Console.WriteLine(testCyphers);
        }
    }
}
