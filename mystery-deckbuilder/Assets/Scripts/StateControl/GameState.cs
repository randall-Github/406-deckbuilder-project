/*
 * author(s): Gabriel LePoudre
 * 
 * This script stores the static "GameState" which serves as a static list of values you need to keep track of
 */

using System.Collections.Generic;


/*
 * The GameState static class is used to track all things state about our game. Because our genre has to \
 *  keep track of what the player knows, and descisions they have made 
 */
public static class GameState
{
    // stores the new values that have been made. Whenever you define a new GameStateValue, use this as arg2
    private static List<IGameStateValue> _gameStateValues = new();


    public static GameStateValue<int> currentDay = new(0, _gameStateValues); // The current game day, as an example


    /* GameStateValue holder class for Meta data about the game. Could be what "phase" or "mode" of gameplay */
    public class Meta 
    {
        /* Current Game "phase", or "mode" as a state machine enum */
        public enum GameplayPhases
        {
            Tutorial,
            Phase_1, // this is just "normal gameplay"
        }
        public static GameStateValue<GameplayPhases> currentGameplayPhase = 
            new(GameplayPhases.Tutorial, _gameStateValues);

        
        public static GameStateValue<int> currentAct = new(1, _gameStateValues);


        public static GameStateValue<Encounter> activeEncounter = new(null, _gameStateValues);

        public static GameStateValue<bool> notepadActive = new(false, _gameStateValues);

        //public static GameStateValue<bool> lastEncounterWin = new(false, _gameStateValues);
        
    }


    /* GameStateValue holder class for Player data. Could be what they know for use in Dialogue trees */
    public class Player
    {
        public static GameStateValue<List<int>> fullDeck;

        // tutorial/testing TODO remove
        static int[] startingDeck = { 1, 5, 9, 1, 5, 9, 1, 5, 9, 1, 5, 9 };
        public static GameStateValue<List<int>> dailyDeck = new(new List<int>(startingDeck), _gameStateValues);


        public enum Locations
        {
            Motel,
            Bar,
            Boxcar
        }

        //NOTE: be sure to update this. the state listeners of certain NPCs (like Rat Prince) that are placed in multiple locations rely on this
        public static GameStateValue<Locations> location = new(Locations.Motel, _gameStateValues);
    }


    /* GameState holder class for NPCs data. Could be their current location 
     * NOTE: be sure to update encountersCompleted and encountersWon for every NPC because they have dialogue
     * that is dependent on these
    */
    public class NPCs
    {

        //NOTE: updates automatically in NPCdialoguetrigger
        public static string lastNPCSpokenTo = "";

        
        //so we can access the met value with the name of the NPC
        // yes very grotesque i know
        public static Dictionary<string, GameStateValue<bool>> npcNameToMet = new(){{"Nibbles", Nibbles.met}, 
        {"Austin", Austin.met}, {"Austyn", Austyn.met}, {"Alan", Alan.met}, 
        {"Mark", Mark.met}, {"Samuel", Samuel.met}, {"Doug", Doug.met}, 
        {"Elk Secretary", Elk.met}, {"Rat Leader", Rat_Leader.met}, {"Rat Prince", Rat_Prince.met}, 
        {"Rat Mob", Rat_Mob.met}, {"Bee", Bee.met}, {"Marry", Marry.met}, 
        {"Wolverine", Wolverine.met}, {"Black Bear", Black_Bear.met}, {"Crouton", Crouton.met}, 
        {"Nina", Nina.met}, {"Mike", Mike.met}, {"Speck", Speck.met}, 
        {"Oslow", Oslow.met}, {"Clay", Clay.met}};

        //we'll be switching scenes so we have to statically store NPC dialogue keys
        public static Dictionary<string, string> currentNPCDialogueKeys = new();
        
        public static class Nibbles
        {
           
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Austin
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Austyn
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Alan
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Mark
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Samuel
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Doug
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Elk
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Rat_Leader
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Rat_Prince
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
            
        }

        public static class Rat_Mob
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Bee
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Marry
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Wolverine
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Black_Bear
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Crouton
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Nina
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Mike
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Speck
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Oslow
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        public static class Clay
        {
            public static GameStateValue<int> encountersCompleted = new(0, _gameStateValues);
            public static GameStateValue<int> encountersWon = new(0, _gameStateValues);
            public static GameStateValue<bool> met = new(false, _gameStateValues);
        }

        
        

    }

    /* GameState holder class for ongoing card and deck information*/
    public class CardInfo
    {
        //these are lists because they have to be in this context. just be mindfull of list length weirdness
        static int[] startingDeck = {1, 5, 9, 1, 5, 9, 1, 5, 9, 1, 5, 9};

        static int[] startingDiscard = { };
        public static GameStateValue<List<int>> currentDiscard = new(new List<int>(startingDiscard), _gameStateValues);
    }


    /* Sets all tracked GameStateValues to their default values. WARNING: Irreversible */
    public static void ResetCurrentGameState()
    {
        foreach (IGameStateValue gameState in _gameStateValues)
        {
            gameState.Reset();
        }
    }

}