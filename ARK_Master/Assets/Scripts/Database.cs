using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Collections.Generic;

//http://answers.unity3d.com/questions/743400/database-sqlite-setup-for-unity.html
//64 bit dll http://blog.synopse.info/post/2013/03/23/Latest-version-of-sqlite3.dll-for-Windows-64-bit
public class Database
{
    private string conn; //Path to database.
    private string lootColumnsNoId;
    private string lootColumns;

    public Database(string applicationDataPath)
    {
        conn = "URI=file:" + applicationDataPath + "/Databases/DB.db"; //Path to database.
        lootColumnsNoId = " RewardName, RewardImagePath, RewardTimer, HPChange, EnergyChange, ShieldChange, ShipResourcesFound, SkillFound, CharacterUnlocked, BuildingUnlocked, LootTableValue, EventType ";
        lootColumns = " Id," + lootColumnsNoId;

        /* Usage example
        Database db = new Database(Application.dataPath);

        Loot hi = db.Find(1);
        db.Delete(3);
        List<Loot> list = db.Search("h");
        foreach (Loot z in list)
        {
            db.Insert(z);
        }


        Loot a = new Loot();
        db.Insert(a);

        List<Loot> random = db.GetRandomRows(5, 4);
        Loot r = db.GetRandomRow(5);*/
    }

    public Rewards GetRandomRow(int lootTableValue, int eventType = 0)
    {
        Rewards ret = null;

        List<Rewards> lootList = GetRandomRows(lootTableValue, 1, eventType);
        if (lootList.Count != 0)
        {
            ret = lootList[0];
        }

        return ret;
    }

    public List<Rewards> GetRandomRows(int lootTableValue, int numRows = 1, int eventType = -1, int roomType = -1)
    {
        bool includeEventType = eventType == -1 ? false : true;
        bool includeRoomType = roomType == -1 ? false : true;
        string extraSearch = "";

        if(includeEventType)
        {
            extraSearch += " AND EventType = @EventType OR EventType = -1";
        }
        if (includeRoomType)
        {
            extraSearch += " AND RoomType = @RoomType OR RoomType = -1";
        }

        string sqlQuery = "SELECT " + lootColumns + " FROM Reward WHERE LootTableValue = @LootTableValue OR LootTableValue = -1 " + extraSearch + " ORDER BY RANDOM() LIMIT @NumRows;";

        List<DBParameter> parameters = new List<DBParameter>();
        parameters.Add(new DBParameter("@LootTableValue", lootTableValue));
        parameters.Add(new DBParameter("@NumRows", numRows));
        if(includeEventType)
        {
            parameters.Add(new DBParameter("@EventType", eventType));
        }
        if (includeRoomType)
        {
            parameters.Add(new DBParameter("@RoomType", roomType));
        }

        DBResults results = ExecuteReader(sqlQuery, parameters);

        List<Rewards> loot = GetList(results);

        return loot;
    }

    /// <summary
    /// Searches with LIKE %RewardName%
    /// </summary>
    /// <param name="RewardName"></param>
    /// <returns></returns>
    public List<Rewards> Search(string RewardName)
    {
        string sqlQuery = "SELECT " + lootColumns + " FROM Reward WHERE RewardName LIKE @RewardName";
        List<DBParameter> parameters = new List<DBParameter>();
        parameters.Add(new DBParameter("@RewardName", "%" + RewardName + "%"));

        DBResults results = ExecuteReader(sqlQuery, parameters);

        return GetList(results);
    }

    public void Insert(Rewards loot)
    {
        string sqlQuery = @"
            INSERT INTO Reward(" + lootColumnsNoId + @") VALUES 
            (@Id, @RewardName, @RewardImagePath, @RewardTimer, @HPChange, @EnergyChange, @ShieldChange, @ShipResourcesFound, @SkillFound, @CharacterUnlocked, @LootTableValue)";

        List<DBParameter> parameters = new List<DBParameter>();
        parameters.Add(new DBParameter("@Id", loot.Id));
        parameters.Add(new DBParameter("@RewardName", loot.RewardName));
        parameters.Add(new DBParameter("@RewardImagePath", loot.RewardImagePath));
        parameters.Add(new DBParameter("@RewardTimer", loot.RewardTimer));
        parameters.Add(new DBParameter("@HPChange", loot.HPChange));
        parameters.Add(new DBParameter("@EnergyChange", loot.EnergyChange));
        parameters.Add(new DBParameter("@ShieldChange", loot.ShieldChange));
        parameters.Add(new DBParameter("@ShipResourcesFound", loot.ShipResourcesFound));
        parameters.Add(new DBParameter("@SkillFound", loot.SkillFound));
        parameters.Add(new DBParameter("@CharacterUnlocked", loot.CharacterUnlocked));
        parameters.Add(new DBParameter("@LootTableValue", loot.LootTableValue));

        ExecuteNonQuery(sqlQuery, parameters);
    }

    public void Delete(int Id)
    {
        string sqlQuery = "DELETE FROM Reward WHERE Id = @Id";

        List<DBParameter> parameters = new List<DBParameter>();
        parameters.Add(new DBParameter("@Id", Id));

        ExecuteNonQuery(sqlQuery, parameters);
    }

    public Rewards Find(int Id)
    {
        Rewards found = null;
        string sqlQuery = "SELECT " + lootColumns + " FROM Reward WHERE Id = @Id ";

        DBParameter pId = new DBParameter("@Id", Id);
        List<DBParameter> parameters = new List<DBParameter>();
        parameters.Add(pId);

        DBResults results = ExecuteReader(sqlQuery, parameters);

        List<Rewards> list = GetList(results);
        if(list.Count != 0)
        {
            found = list[0];
        }

        results.Close();

        return found;
    }

    private DBResults ExecuteReader(string commandText, List<DBParameter> parameters)
    {
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = commandText;

        AddParameters(dbcmd, parameters);
        
        IDataReader reader = dbcmd.ExecuteReader();

        DBResults ret = new DBResults(dbconn, dbcmd, reader);
        return ret;
    }

    public DataTable SelectTable(string commandText)
    {
        DataTable data = new DataTable();
        SqliteConnection dbconn;
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        SqliteCommand comm = new SqliteCommand(commandText, dbconn);
        SqliteDataAdapter adapt = new SqliteDataAdapter(comm);
        adapt.Fill(data);

        return data;
    }

    private int ExecuteNonQuery(string commandText, List<DBParameter> parameters)
    {
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        dbcmd.CommandText = commandText;

        AddParameters(dbcmd, parameters);

        return dbcmd.ExecuteNonQuery();
    }

    private void AddParameters(IDbCommand dbcmd, List<DBParameter> parameters)
    {
        IDbDataParameter parameter = null;
        foreach (DBParameter param in parameters)
        {
            parameter = dbcmd.CreateParameter();
            parameter.ParameterName = param.Name;
            parameter.Value = param.Value;
            dbcmd.Parameters.Add(parameter);
        }
    }

    private List<Rewards> GetList(DBResults results)
    {
        Rewards found = null;
        List<Rewards> ret = new List<Rewards>();
        while (results.reader.Read())
        {
            int Id = results.reader.GetInt32(0);
            string RewardName = null;
            if (!results.reader.IsDBNull(1))
            {
                RewardName = results.reader.GetString(1);
            }
            string RewardImagePath = null;
            if (!results.reader.IsDBNull(2))
            {
                RewardImagePath = results.reader.GetString(2);
            }
            int RewardTimer = results.reader.GetInt32(3);
            int HPChange = results.reader.GetInt32(4);
            int EnergyChange = results.reader.GetInt32(5);
            int ShieldChange = results.reader.GetInt32(6);
            int ShipResourcesFound = results.reader.GetInt32(7);
            int SkillFound = results.reader.GetInt32(8);
            int CharacterUnlocked = results.reader.GetInt32(9);
            int BuildingUnlocked = results.reader.GetInt32(10);
            int LootTableValue = results.reader.GetInt32(11);
            int EventType = results.reader.GetInt32(12);

            found = new Rewards(Id, RewardName, RewardImagePath, RewardTimer, HPChange, EnergyChange, ShieldChange, ShipResourcesFound, SkillFound, CharacterUnlocked, BuildingUnlocked, LootTableValue, EventType);

            ret.Add(found);
            Debug.Log("id= " + Id + "  rewardName =" + RewardName);
        }
        results.Close();
        return ret;
    }
}

//Easier that writing all this out again when using multiple queries
public class DBResults
{
    public IDbConnection dbconn;
    public IDbCommand dbcmd;
    public IDataReader reader;
    private bool closed;

    public DBResults(IDbConnection dbconn, IDbCommand dbcmd, IDataReader reader)
    {
        this.dbconn = dbconn;
        this.dbcmd = dbcmd;
        this.reader = reader;

        closed = false;
    }

    public void Close()
    {
        try // Will fail if its a NonQuery. Not a big deal just need to make sure cleanup code after this runs
        {
            reader.Close();
            reader = null;
        }
        catch(Exception) { }

        try
        {
            dbcmd.Dispose();
            dbcmd = null;
            dbconn.Close();
            dbconn = null;
        }
        catch(Exception) { }

        closed = true;
    }

    ~DBResults()
    {
        if (!closed)
        {
            Close();
        }
    }
}

/// <summary>
/// Couldn't find a way to init an IDbCommand parameter without initializing through IDbCommand.CreateParameter
/// </summary>
public class DBParameter
{
    public string Name;
    public object Value;

    public DBParameter(string Name, object Value)
    {
        this.Name = Name;
        this.Value = Value;
    }
}

/*public class Loot
{
    public int Id { get; set; }
    public string RewardName { get; set; }
    public int RewardType { get; set; }
    public int RewardValue { get; set; }
    public string StatAffected { get; set; }
    public int RewardTimer { get; set; }
    public string RewardImage { get; set; }
    public int LootTableValue { get; set; }

    public Loot()
    {

    }

    public Loot(int id, string rewardName, int rewardType, int rewardValue, string statAffected, int rewardTimer, string rewardImage, int lootTableValue)
    {
        Id = id;
        RewardName = rewardName;
        RewardType = rewardType;
        RewardValue = rewardValue;
        StatAffected = statAffected;
        RewardTimer = rewardTimer;
        RewardImage = rewardImage;
        LootTableValue = lootTableValue;
    }
}*/