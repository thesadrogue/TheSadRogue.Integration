using System.Collections.Generic;
using ExampleGame.Components;
using ExampleGame.Items;
using SadRogue.Primitives;
using TheSadRogue.Integration;

namespace ExampleGame
{
    public class CreatureFactory
    {
        private static Dictionary<Factions, string> _minionNames;
        private static Dictionary<Factions, string> _infantryNames;
        private static Dictionary<Factions, string> _cavalryNames;
        private static Dictionary<Factions, string> _artilleryNames;
        private static Dictionary<Factions, string> _bossNames;

        public CreatureFactory()
        {
            _minionNames = new Dictionary<Factions, string>()
            {
                { Factions.Aliens, "Small Grey Alien" },
                { Factions.Angels, "Cherub" },
                { Factions.AngelsFallen, "Fallen Cherub" },
                { Factions.AtomicHorrors, "Tiny Terror" },
                { Factions.Beasts, "Wild Boar" },
                { Factions.Cultists, "Neophyte" },
                { Factions.Cyborgs, "Cyberdog" },
                { Factions.Demons, "Imp" },
                { Factions.Dragons, "Dragon Shaman" },
                { Factions.ElderThings, "Small Otherworldly Creature" },
                { Factions.Elementals, "Elemental Mephit" },
                { Factions.Fey, "Pixie" },
                { Factions.FrostGiants, "Adolescent Frost Giant" },
                { Factions.Golems, "Flesh Golem" },
                { Factions.Illusionists, "Shadow Beast" },
                { Factions.Invokers, "Novice Invoker" },
                { Factions.Killbots, "Recon Rover" },
                { Factions.Lycanthropes, "Were-rat" },
                { Factions.Mutants, "One-Eyed Humanoid" },
                { Factions.Necromancers, "Budding Necromancer" },
                { Factions.Psychics, "Lesser Telekine" },
                { Factions.Summoners, "Summoned Brute" },
                { Factions.UndeadCadaverous, "Zombie" },
                { Factions.UndeadSkeletal, "Skeleton" },
                { Factions.UndeadSpectral, "Ghost" },
                { Factions.Vampires, "Vampire Spawn" },
                { Factions.WinterWitches, "Frozern Watcher" },
                { Factions.WorshippersOfAnubis, "Desicated Husk" },
                { Factions.WorshippersOfHades, "Ferryman" },
                { Factions.WorshippersOfThor, "Viking" },
            };
            
            _infantryNames = new Dictionary<Factions, string>()
            {
                { Factions.Aliens, "Tall Grey Alien" },
                { Factions.Angels, "Nomadic Deva" },
                { Factions.AngelsFallen, "Erinyes" },
                { Factions.AtomicHorrors, "Beggar-For-Death" },
                { Factions.Beasts, "Mama Boar" },
                { Factions.Cultists, "Nouveau Acolyte" },
                { Factions.Cyborgs, "Malfunctioning Cyborg" },
                { Factions.Demons, "Quasit" },
                { Factions.Dragons, "Dragon Disciple" },
                { Factions.ElderThings, "Strange Otherworldly Creature" },
                { Factions.Elementals, "Lesser Elemental" },
                { Factions.Fey, "Satyr" },
                { Factions.FrostGiants, "Young Adult Frost Giant" },
                { Factions.Golems, "Clay Golem" },
                { Factions.Illusionists, "Projected Image" },
                { Factions.Invokers, "Invoker" },
                { Factions.Killbots, "Lesser Killbot" },
                { Factions.Lycanthropes, "Werewolf" },
                { Factions.Mutants, "Three-Armed Humanoid" },
                { Factions.Necromancers, "Student Necromancer" },
                { Factions.Psychics, "Telekine" },
                { Factions.Summoners, "Summoned Lieutenant" },
                { Factions.UndeadCadaverous, "Wight" },
                { Factions.UndeadSkeletal, "Giant Skeleton" },
                { Factions.UndeadSpectral, "Wraith" },
                { Factions.Vampires, "Young Vampire" },
                { Factions.WinterWitches, "Winter Wight" },
                { Factions.WorshippersOfAnubis, "Lesser Mummy" },
                { Factions.WorshippersOfHades, "Watcher-of-the-Styx" },
                { Factions.WorshippersOfThor, "Viking Captain" },
            };
            
            
            _cavalryNames = new Dictionary<Factions, string>()
            {
                { Factions.Aliens, "Reptilian" },
                { Factions.Angels, "Astral Deva" },
                { Factions.AngelsFallen, "Excellent Erinyes" },
                { Factions.AtomicHorrors, "Thriving Sub-Atomic Horror" },
                { Factions.Beasts, "Bear" },
                { Factions.Cultists, "Acolyte" },
                { Factions.Cyborgs, "Cyborg" },
                { Factions.Demons, "Bearded Devil" },
                { Factions.Dragons, "Dragonborn" },
                { Factions.ElderThings, "Adult Elder Thing" },
                { Factions.Elementals, "Elemental" },
                { Factions.Fey, "Centaur" },
                { Factions.FrostGiants, "Mature Frost Giant" },
                { Factions.Golems, "Shale Golem" },
                { Factions.Illusionists, "Lesser Illusionist" },
                { Factions.Invokers, "Scholarly Invoker" },
                { Factions.Killbots, "Killbot" },
                { Factions.Lycanthropes, "Werebear" },
                { Factions.Mutants, "Three-Armed Giant" },
                { Factions.Necromancers, "Graduate Necromancer" },
                { Factions.Psychics, "Practiced Manifester" },
                { Factions.Summoners, "Novice Summoner" },
                { Factions.UndeadCadaverous, "Ghoul" },
                { Factions.UndeadSkeletal, "Bear Skeleton" },
                { Factions.UndeadSpectral, "Dread Wrait" },
                { Factions.Vampires, "Vampire" },
                { Factions.WinterWitches, "Ice Sculpture" },
                { Factions.WorshippersOfAnubis, "Mummy" },
                { Factions.WorshippersOfHades, "Petitioner to Cerberus" },
                { Factions.WorshippersOfThor, "Legendary Viking" },
            };
            
            
            _artilleryNames = new Dictionary<Factions, string>()
            {
                { Factions.Aliens, "Mantid" },
                { Factions.Angels, "Planetary Deva" },
                { Factions.AngelsFallen, "Peerless Erinyes" },
                { Factions.AtomicHorrors, "Glowing One" },
                { Factions.Beasts, "Elephant" },
                { Factions.Cultists, "One-Who-Knows" },
                { Factions.Cyborgs, "Fine-Tuned Cyborg" },
                { Factions.Demons, "Pit Fiend" },
                { Factions.Dragons, "Young Dragon" },
                { Factions.ElderThings, "Incomprehensible Thing" },
                { Factions.Elementals, "Elder Elemental" },
                { Factions.Fey, "Genius Loci" },
                { Factions.FrostGiants, "Elder Frost Giant" },
                { Factions.Golems, "Iron Golem" },
                { Factions.Illusionists, "Illusionist" },
                { Factions.Invokers, "Pre-eminent Invoker" },
                { Factions.Killbots, "Brand-New Killbot" },
                { Factions.Lycanthropes, "Giant Were-Alligator" },
                { Factions.Mutants, "Two-Headed Giant" },
                { Factions.Necromancers, "Master Necromancer" },
                { Factions.Psychics, "Psion" },
                { Factions.Summoners, "Summoner" },
                { Factions.UndeadCadaverous, "Ghast" },
                { Factions.UndeadSkeletal, "T-Rex Skeleton" },
                { Factions.UndeadSpectral, "Spectre" },
                { Factions.Vampires, "Head Vampire" },
                { Factions.WinterWitches, "Ice-Sheathed Treant" },
                { Factions.WorshippersOfAnubis, "Priest Of Anubis" },
                { Factions.WorshippersOfHades, "Guardian of Hell's Halls" },
                { Factions.WorshippersOfThor, "Valkyrie" },
            };
            
            _bossNames = new Dictionary<Factions, string>()
            {
                { Factions.Aliens, "Alien Overlord" },
                { Factions.Angels, "Solar Deva" },
                { Factions.AngelsFallen, "Keres" },
                { Factions.AtomicHorrors, "Post-Evolutionary Mass" },
                { Factions.Beasts, "Giant Tyrannosaurus Rex" },
                { Factions.Cultists, "'It'" },
                { Factions.Cyborgs, "Multitudinal Cybor-Being" },
                { Factions.Demons, "Queen Marilith" },
                { Factions.Dragons, "Dragon" },
                { Factions.ElderThings, "Truly Elder Thing" },
                { Factions.Elementals, "Elemental Prince" },
                { Factions.Fey, "Earth Itself" },
                { Factions.FrostGiants, "Frost Giant Chieftain" },
                { Factions.Golems, "Steel Golem" },
                { Factions.Illusionists, "Master Illusionist" },
                { Factions.Invokers, "Master Invoker" },
                { Factions.Killbots, "Killbot mk II 'Supreme'" },
                { Factions.Lycanthropes, "Giant Were-Tyrannosaurus" },
                { Factions.Mutants, "Mutant Committee Creature" },
                { Factions.Necromancers, "Liche" },
                { Factions.Psychics, "Master Kineticist" },
                { Factions.Summoners, "Master Summoner" },
                { Factions.UndeadCadaverous, "Morgh" },
                { Factions.UndeadSkeletal, "Massive Bone-Wurm" },
                { Factions.UndeadSpectral, "Lord of Shades" },
                { Factions.Vampires, "Dracula" },
                { Factions.WinterWitches, "The Winter Witch" },
                { Factions.WorshippersOfAnubis, "Anubis" },
                { Factions.WorshippersOfHades, "Hades" },
                { Factions.WorshippersOfThor, "Thor" },
            };
        }
        
        /// <summary>
        /// Basic Trash Mob
        /// </summary>
        /// <param name="position">Location at which to spawn</param>
        /// <param name="faction">The faction to which this creature belongs</param>
        /// <returns>A new minion creature</returns>
        public static RogueLikeEntity Minion(Point position, Factions faction)
        {
            
            int glyph = _minionNames[faction][0];
            RogueLikeEntity critter = new RogueLikeEntity(position, glyph);
            var combatComponent = new CombatComponent(2, 0, 1);
            critter.AddComponent(combatComponent);
            
            var inventory = new InventoryComponent();
            critter.AddComponent(inventory);

            return critter;

            
        }

        /// <summary>
        /// Creates an intelligent footsoldier
        /// </summary>
        /// <param name="position">Location at which to spawn</param>
        /// <param name="faction">The faction to which this creature belongs</param>
        /// <returns>A new creature</returns>
        public static RogueLikeEntity Infantry(Point position, Factions faction)
        {

            int glyph = _infantryNames[faction][0];
            RogueLikeEntity critter = new RogueLikeEntity(position, glyph);
            var combatComponent = new CombatComponent(4, 0, 1);
            critter.AddComponent(combatComponent);

            var inventory = new InventoryComponent();
            critter.AddComponent(inventory);

            //todo: special abilities
            return critter;
        }

        /// <summary>
        /// Creates a formidable foe 
        /// </summary>
        /// <param name="position">Location at which to spawn</param>
        /// <param name="faction">The faction to which this creature belongs</param>
        /// <returns>A new strong creature</returns>
        public static RogueLikeEntity Cavalry(Point position, Factions faction)
        {
            
            int glyph = _cavalryNames[faction][0];
            RogueLikeEntity critter = new RogueLikeEntity(position, glyph);
            var combatComponent = new CombatComponent(7, 0, 1);
            critter.AddComponent(combatComponent);
            
            var inventory = new InventoryComponent();
            critter.AddComponent(inventory);

            //todo: special abilities
            return critter;
            
        }
        
        /// <summary>
        /// Creates a truly terrifying enemy
        /// </summary>
        /// <param name="position">Location at which to spawn</param>
        /// <param name="faction">The faction to which this creature belongs</param>
        /// <returns>A new very strong creature</returns>
        public static RogueLikeEntity Artillery(Point position, Factions faction)
        {
            int glyph = _artilleryNames[faction][0];
            RogueLikeEntity critter = new RogueLikeEntity(position, glyph);
            var combatComponent = new CombatComponent(10, 0, 2);
            critter.AddComponent(combatComponent);
            
            var inventory = new InventoryComponent();
            critter.AddComponent(inventory);

            //todo: special abilities
            return critter;
            
        }
        
        /// <summary>
        /// Creates a super-duper enemy creature
        /// </summary>
        /// <param name="position">Location at which to spawn</param>
        /// <param name="faction">The faction to which this creature belongs</param>
        /// <returns>A new minion creature</returns>
        public static RogueLikeEntity Boss(Point position, Factions faction)
        {
            int glyph = _bossNames[faction][0];
            RogueLikeEntity critter = new RogueLikeEntity(position, glyph);
            var combatComponent = new CombatComponent(20, 1, 2);
            critter.AddComponent(combatComponent);
            
            var inventory = new InventoryComponent();
            critter.AddComponent(inventory);

            //todo: special abilities
            return critter;
        }
    }
}