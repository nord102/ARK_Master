using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalVariables
{
    public static List<int> unlockedCharacters = new List<int>();
    public static List<int> unlockedBuildings = new List<int>();
    public static List<int> unlockedSkills = new List<int>();
    public static int selectedCharacter = -1;      

    //Do this immediately on opening the game!
    public static void LoadGlobalUnlocks()
    {
        //Read in unlock text file?

        //Load lists with the values
    }

}

public static class ObjectType
{
    public const int Cabinet = 0;
    public const int Chair = 1;
    public const int Computer = 2;
    public const int Crate = 3;
    public const int Table = 4;
}
