# Crawler-10774743
Welcome to the CMD project! This Readme file will outline:
- How the user should interact with the executable
- Algorithms the software is using or based on
- Advanced behaviours
- Video link/Video iframe

## How the user should interact with the executable
To simply call the executable the user should:
1. Open the command prompt by typing in 'cmd' into their searchbar.
2. Type in to get the executable's path directory
```
cd [path for individual user]\Dungeon-10774743.zip\2022-comp1000-coursework-BungeeDragon\Software\bin\Release\netcoreapp3.1
```
3. The finally call the executable by typing in:
```
start Dungeon.exe
```
And the crawler should begin!

Some features to keep in mind:
- To begin the crawler the user should either type in one of the three commands corresponding to the maps avaliable
```
load Simple.map
load Simple2.map
load Advanced.map
```
- The map should display on screen, then the user with be prompted to type in commands such as 'start' or 'advanced'
- There will be continuous prompts during the execution of the program to help the user interact with the game!
- If the player has selected advanced mode and has either died or finished the map, there is the avaliability to replay the map they just completed

## Algorithms the software is using or based on
The software itself uses a range of algorithms for it to function.
### Basics
- Loops:
Example below shows how the map.txt file was converted into a multidimentional array
```
 originalMap = new char[10][];
 for (int i = 0; i < originalMap.Length; i++)
 {
     char[] charArray = mapXcoord[i].ToCharArray();
     originalMap[i] = charArray;
 }
```
- If statements:
This is an essential part of the software to check for conditions. The example below largely controls the attack behaviour of the player and monster using and if statement, else statement and nested if statement.
```
 PlayerAttack = true;
 MonsterHealth -= PlayerDamage;
 if (MonsterHealth <= 0 )
 {
     MonsterDead = true;
 }
 else
 {
     MonsterAttack = true;
     Random random = new Random();
     MonsterDamage = random.Next(0, 4);
     PlayerHealth -= MonsterDamage;

     if (PlayerHealth <= 0)
     {
         ReplayOption = true;
     }
 }
```
- While loop:
As this is a less flexible and stable structure to use, its only been used once in the program to initiate a continous loop until an if statement is met.
```
while (i == 0)
{
    int randDirectionY = random.Next(-1, 2);
    int randDirectionX = random.Next(-1, 2);
    if (workingMap[my - 1][mx] == '@' || workingMap[my][mx - 1] == '@' || workingMap[my + 1][mx] == '@' || workingMap[my][mx + 1] == '@')
    {
        currentMonster[0] = my;
        currentMonster[1] = mx;
        workingMap[my][mx] = 'M';
        AttackMode = true;
        i++;
    }
    else if (workingMap[my + randDirectionY][mx + randDirectionX] == '.')
    {
        workingMap[my + randDirectionY][mx + randDirectionX] = 'M';
        i++;
    }
    else if (workingMap[my + randDirectionY][mx + randDirectionX] == 'C')
    {
        workingMap[my][mx] = '.';
        workingMap[my + randDirectionY][mx + randDirectionX] = 'M';
        MonsterHealth++;
        MonsterStartingHealth++;
        MonsterCoins++;
        i++;
    }
}
```
- Arrays: 
This was the main data structure used without the program, to store the characters of the text files and allow methods the maipulate the data.
```
if (workingMap[directionY][directionX] == 'D' && advanced == false)
{
    this.status = GameState.STOP;
    workingMap[y][x] = '.';
    workingMap[directionY][directionX] = '@';
    return false;
}
```
### More advanced
- Functions with parameters are the main structures used to improve readibility and minimize the repeatition of code with minimal changes
- The software uses 3 functions independant of the given ones in the exercise, that are:
  - ResetGame()
  - Movement(int directionY, int directionX)
  - MonsterBehaviour(int my, int mx)
- The parameters passed through are particulary useful in sharing variable values from one function to another, for example below shows how the player actions call upon the movement function and the values they pass through the parameters:
```
if (action == PlayerActions.NORTH)
{
    Movement(y - 1, x);
}
if (action == PlayerActions.WEST)
{
    Movement(y, x - 1);
}
if (action == PlayerActions.SOUTH)
{
    Movement(y + 1, x);
}
if (action == PlayerActions.EAST)
{
    Movement(y, x + 1);
}
```
## Advanced behaviours
All the advanced behaviours have been added to the software
- Using the command “advanced” before typing in play is used to enabled the advanced
mode only then are advanced features working
- Using the “Q” key the player is able to attack a position dealing 1 damage to a Monster
- The advanced map can be loaded and completed by the user.
- When player moves over a tile the tile is hidden and the player symbol is rendered until player leaves the tile.
  - Monsters can move over empty spaces
  - '#' are walls that cannot be passes
  - 'C' are gold pieces which can be collected or walked over
### Extras
- Monster may have 3 damage point
  - Random from 0 to 3 inclusive damage on player
- An attack from the player only deals 1 damage
  - This damage is increased from obtaining coins
- Players heal one damage when they collect a coin and start with 2 health.
  - a coin is equivelant to +1 health and +1 attack damage
- Monster may attack
  - There is a chance the monster deals 0 damage
- Monsters can eat coins to get stronger 
  - If a monster walks over a coin, all monsters receive a +1 attack damage
- When Monsters die they drop their coins
  - Represented by 'B' on the map, as a bag on coins
- Upon Entering the exit players get displayed a status message and can either reply or quit the game
- Allow for a Replay of the map using the command: “replay”
  - Likewise "quit" quits the game
> Most details taken from COMP1000 module handbook for this assigment 
## Video link/Video iframe
The link below contains a video that briefly describes the functions and development of the software
https://youtu.be/Qq0QvsFcD_Y
