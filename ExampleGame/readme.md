# Cycle of the Simulacra (WIP)
## The Example Game using both SadConsole v9 and GoRogue v3

Plot: You come to awareness at the shrine in the center of the map. The only things nearby are an alter, a dagger, a tome, and an hourglass.
 
The tome contains the instructions to create a _Simulacrum_, sentient clone, out of Snow, Slime, and Bones. 
You need to collect 100 units of each on the alter and place a weapon into its hand to bring it to life. 
Get Snow from Frost Giants, get Slime from Cyborgs, and get Bones from Death-Worshippers.
You have one turn of the hourglass, or 100 turns, to complete the Simularcum. 
There is no mention of reward or punishment.

As your attention returns to the room, the hourglass turns by it's own volition!

| Key | Action | Description | 
| --- | --- | --- |
| Arrow Keys | Move in direction | 
| A | Attack | telegraph & select squares |
| F | Feint | telegraph, but don't attack | 
| R | Rush | Telegraph, then move an additional square before damaging next round
| B | Block | take half damage, stuns attacker for one round |
| I | Inventory | Swaps the currently stored item with whichever slot makes most sense (Body or Wielded) |

## How to play

Characters have health (the player starts with 3), and they can gain additional hit points by finding Fountains littered throughout the dungeon. 
All characters have 1 base damage, and all weapons (except the dagger) grant an additional 1 damage. 
Characters can only possess three items: a wielded item, a worn item, and a stored item.

Combat is turn based. 
To __Attack__, Characters walk into an enemy.

Characters who feel defensive may choose to __Block__. 
Blocking characters take no damage for a round, and stun their attacker for a round. 

Many enemies have special abilities which make them very difficult to fight. 
There are two bosses which hold magic items that may make your quest easier or harder.

## Code Organization (WIP)
The namespaces match the directory structure:

```
Solution/
 +- ExampleGame/ 
 |   +- Program.cs                              # creates & starts the game
 |   +- GameUi.cs                               # manages the screen space 
 |   +- MapGeneration/
 |   |   +- MapGenerator.cs                     # coordinates GenerationSteps
 |   |   +- GenerationSteps/
 |   |       +- BackroomsGenerationStep.cs      # produces a cluster of rectangular rooms
 |   |       +- CaveGenerationStep.cs           # a small cellular automata 
 |   |       +- CaveSeedingStep.cs              # initialize the map for the cave generation to work
 |   |       +- CryptGenerationStep.cs          # create a brick pattern of rooms with thick walls 
 |   |       +- SpiralGenerationStep.cs         # carves a spiral out of a solid area