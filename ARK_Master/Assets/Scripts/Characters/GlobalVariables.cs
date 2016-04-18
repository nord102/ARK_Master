using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;

public static class GlobalVariables
{
    public static DataTable unlockedCharacters = new DataTable();
    public static List<int> unlockedBuildings = new List<int>();
    public static List<int> unlockedSkills = new List<int>();
    public static DataTable roomAvailability = new DataTable();
    public static int selectedCharacter = -1;      

    //Do this immediately on opening the game!
    public static void LoadGlobalUnlocks()
    {
        //Read in unlock text file?

        //Load lists with the values
        Database db = new Database(Application.dataPath);

        GlobalVariables.roomAvailability = db.SelectTable("SELECT * FROM RoomAvailability");
        GlobalVariables.unlockedCharacters = db.SelectTable("SELECT * FROM CharacterAvailability");
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
