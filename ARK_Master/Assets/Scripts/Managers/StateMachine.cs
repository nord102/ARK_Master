﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;
using System.Data;
using System.Linq;

public class StateMachine : MonoBehaviour
{
    public static StateMachine instance = null;
    public ShipInfo sInfo;
    public PlayerInfo pInfo;
    public List<Skills> AllAvailableSkills;

    private bool InventoryOpen = false;
    private bool CharacterSheetOpen = false;
    private bool MainMenuOpen = false;
    private bool MiniMapOpen = false;
    private bool BuildingMenuOpen = false;
    public bool PlayerControl = false;
    public bool eventActive = false;

    public Player player;
    public RuntimeAnimatorController AnimatorMechanic;
    public RuntimeAnimatorController AnimatorFirefighter;
    public RuntimeAnimatorController AnimatorSoldier;

    #region Modules
    public GameObject Module1;
    public GameObject Module2;
    public GameObject Module3;
    public GameObject Module1Selected;
    public GameObject Module2Selected;
    public GameObject Module3Selected;
    #endregion

    public Camera mainCamera;

    public GameObject BuildingMenu;

    private Dictionary<GameObject, bool> UIModules = new Dictionary<GameObject, bool>();
    private List<object[]> UIModules2 = new List<object[]>();
    public Canvas UI;

    public enum RoomType { Basic = 1, MedBay = 2, Engineering = 3 };
    public enum RoomShape { OneByOne = 1, OneByTwo = 2, TwoByOne = 3, OneByThree = 4 };

    private string PreviousPlayerFilePath = "Sinister.txt";
    public List<PlayerInfo> PreviousPlayers = new List<PlayerInfo>();

    #region UI 
    public Image playerHealthBar;
    public Image playerShieldBar;
    public Text shipResources;
    #endregion

    public GameObject DialogueBox;
    public Database db;
    public string appPath;
    public string ImagePath;

    public GameObject EventInfo;

    public GameObject alien;
    public GameObject fire;
	public GameObject hole;
    public GameObject sinisterEnemy;

    //Short - 30
    //Medium - 60
    //Long - 90
    public int numMaxRooms = 30;
    public int sinisterEventNum = 0;

    #region RoomSelect Variables
    private int roomShapeSelectRow = 1;
    public GameObject RoomShapeSelectMenu_1;
    public GameObject RoomShapeSelectMenu_2;
    public GameObject RoomShapeSelectMenu_3;
    private int roomTypeSelectRow = 1;
    public GameObject RoomTypeSelectMenu_1;
    public GameObject RoomTypeSelectMenu_2;
    private int roomTypeSelected = 0;
    #endregion

    public Canvas gameOverMenu;
    public Canvas winMenu;
	
	public GameObject GoInventory;
    public GameObject RewardsWon;

    // Use this for initialization
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        Setup();
    }

    #region File Loading/Saving
    void LoadDeadCharacters()
    {
        //Read the previous players file and load information about dead characters
        if (File.Exists(PreviousPlayerFilePath))
        {
            StreamReader sr = File.OpenText(PreviousPlayerFilePath);
            string s = sr.ReadToEnd();
            sr.Close();


            //Now parse out the relevant information into PreviousPlayers
            char[] delimiterChars = { '|' };
            string[] players = s.Split(delimiterChars);

            //Debug.Log (players.Length);

            foreach (string player in players)
            {
                if (player.Equals(""))
                    continue;
                char[] delChars = { '\n' };
                string[] lines = player.Split(delChars);
                PlayerInfo p = new PlayerInfo(int.Parse(lines[0]));
                p.PlayerName = lines[1];
                if (lines.Length > 2)
                {
                    for (int i = 2; i < lines.Length - 1; ++i)
                    {
                        char[] dChar = { ':' };
                        int id = int.Parse(lines[i].Split(dChar)[0]);
                        p.CurrentSkills.Add(AllAvailableSkills[id]);
                    }
                }
                PreviousPlayers.Add(p);
            }
        }
    }

    void SaveDeadCharacters()
    {
        //Part of losing the game is saving the information from the current player into the PreviousPlayers file.
        //We should save their name, their number, and their equipped skills?
        try
        {
            if (!File.Exists(PreviousPlayerFilePath))
            {
                File.Create(PreviousPlayerFilePath);
            }
            StreamWriter sw = File.AppendText(PreviousPlayerFilePath);
            StringBuilder sb = new StringBuilder();

            //Save current character
            sb.AppendLine(pInfo.CurrentPlayer.ToString());
            sb.AppendLine(pInfo.PlayerName);
            foreach (Skills i in AllAvailableSkills) //(int i = 0; i < AllAvailableSkills.Count; i ++)
            {
                if (i.isActive)
                {
                    sb.AppendLine(i.skillID + ": " + i.skillName);
                }
            }
            //Extra line to separate different characters
            sb.AppendLine("|");

            //Save all previous dead characters that are still.... alive...?
            foreach (PlayerInfo p in PreviousPlayers)
            {
                sb.AppendLine(p.CurrentPlayer.ToString());
                sb.AppendLine(p.PlayerName);

                //Add the skills somehow?
                foreach (Skills s in p.CurrentSkills)
                {
                    sb.AppendLine(s.skillID + ": " + s.skillName);
                }


                //Extra line to separate different characters
                sb.AppendLine("|");
            }

            sw.Write(sb.ToString());
            sw.Close();
        }
        catch
        {
            //Issue opening or creating the file? don't worry about it then.
        }
    }
    #endregion

    //Call this when the player's Health reaches 0, or other events that end the game
    public void GameOver()
    {
        gameOverMenu.enabled = true;
        PlayerControl = false;

        try
        {
            //SaveDeadCharacters();
        }
        catch
        {
            //Forget it then
        }

        
    }

    public void WinGame()
    {
        winMenu.enabled = true;
        PlayerControl = false;
    }

    public void ActivateSinisterEvent()
    {
        sinisterEventNum++;
    }

    void LoadSettings()
    {
        try
        {
            LoadDeadCharacters();
        }
        catch
        {
            //Not implemented anyway
        }
    }

    public void UpdateUI()
    {
        #region Skills
        //Update Modules in wrench  
        foreach (object[] obj in UIModules2)
        {
            GameObject g = (GameObject)obj[0];
            obj[1] = false;
            Image temp = (Image)g.GetComponent<Image>();
            temp.sprite = null;
        }

        Module1Selected.SetActive(false);
        Module2Selected.SetActive(false);
        Module3Selected.SetActive(false);

        switch (player.equiped)
        {
            case 1:
                Module1Selected.SetActive(true);
                break;
            case 2:
                Module2Selected.SetActive(true);
                break;
            case 3:
                Module3Selected.SetActive(true);
                break;
        }

        foreach (Skills s in AllAvailableSkills)
        {
            if (s.isActive)
            {
                bool added = false;
                foreach (object[] obj in UIModules2)
                {
                    GameObject g = (GameObject)obj[0];
                    Image temp = (Image)g.GetComponent<Image>();
                    if (!(bool)obj[1] && !added)
                    {
                        g.SetActive(true);
                        temp.sprite = s.symbol;
                        obj[1] = true;
                        added = true;
                    }
                }

            }
        }
        #endregion

        //Update Room Shape Lists with Available Rooms
            switch (roomShapeSelectRow)
            {
                case 1:                    
                    RoomSelector(GlobalVariables.roomAvailability, RoomShapeSelectMenu_1);
                    break;
                case 2:
                    RoomSelector(GlobalVariables.roomAvailability, RoomShapeSelectMenu_2);
                    break;
                case 3:
                    RoomSelector(GlobalVariables.roomAvailability, RoomShapeSelectMenu_3);
                    break;
            }

        //Update Info Panel


    }

    

    void Setup()
    {
        //--
        winMenu.enabled = false;
        gameOverMenu.enabled = false;
        //--


        BuildingMenu.SetActive(false);

        //HAD TO CHANGE THIS APPLICATION DATA PATH///////////////////////////////////////////////////////////////////////
        appPath = Application.dataPath;
        //appPath = "C:/Users/nord102/Desktop";


        db = new Database(Application.dataPath);

        ImagePath = appPath + "/Images/Rewards/";
        PreviousPlayers = new List<PlayerInfo>();

        //PlayerInfo.InitializePlayerInfo (0);
        pInfo = new PlayerInfo(0);
        sInfo = new ShipInfo();
        GenerateAvailableSkills();
        LoadSettings();

        UIModules2.Add(new object[] { Module1, false });
        UIModules2.Add(new object[] { Module2, false });
        UIModules2.Add(new object[] { Module3, false });

        PlayerControl = true;

        switch( GlobalVariables.selectedCharacter)
        {
            case 1:
                player.animator.runtimeAnimatorController = AnimatorMechanic;
                break;
            case 2:
                player.animator.runtimeAnimatorController = AnimatorFirefighter;
                break;
            case 3:
                player.animator.runtimeAnimatorController = AnimatorSoldier;
                break;
        }

		GoInventory.GetComponent<Inventory>().SetupInventory();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !eventActive)
        {
            //Inventory inv = GoInventory.GetComponent<Inventory>();
            //InventoryOpen = !InventoryOpen;
            //if (InventoryOpen)
            //{
            //    inv.Show();
            //    //sInfo.SetResources(10);
            //}
            //else
            //{
            //    inv.Hide();

            //}
        }
        else if (Input.GetKeyDown(KeyCode.C) && !eventActive)
        {
            CharacterSheetOpen = !CharacterSheetOpen;
            if (CharacterSheetOpen)
            {
                //ShowCharacterSheet();
                BuildingMenuOpen = true;
                PlayerControl = false;
                BuildingMenu.SetActive(true);
                SwitchRoomType("Storage");
                mainCamera.orthographicSize = 18;

            }
            else
            {
                //Unable to stop building until the room being
                //dragged is placed
                if (!Dragging.instance.draggingMode)
                {
                    //HideCharacterSheet();
                    BuildingMenuOpen = false;
                    PlayerControl = true;
                    BuildingMenu.SetActive(false);
                    mainCamera.orthographicSize = 5;

                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            //MiniMapOpen = !MiniMapOpen;
            //if (MiniMapOpen)
            //{
            //    //ShowMiniMap();
            //}
            //else
            //{
            //    //HideMiniMap();
            //}
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Pressed X");
            StateMachine.instance.EndEvent(Generate.instance.currentRoom.roomEvent);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            EnableAllRooms();
            Dragging.instance.freeRooms = true;
        }

    }

    #region Skill Related Functions
    void GenerateAvailableSkills()
    {
        //IMAGES
        //Put in Images/Resources/Inventory
        AllAvailableSkills = new List<Skills>();
        AllAvailableSkills.Add(new Skills(1, Resources.Load<Sprite>("Inventory/t_laser"), "Laser", 20, 2));
        AllAvailableSkills.Add(new Skills(0, Resources.Load<Sprite>("Inventory/t_extinguisher"), "Extinguisher", 10, 2));//Resource.Load
        AllAvailableSkills.Add(new Skills(2, Resources.Load<Sprite>("Inventory/t_torch"), "Welder", 30, 2));
        AllAvailableSkills[0].isActive = true;
        AllAvailableSkills[1].isActive = true;
        AllAvailableSkills[2].isActive = true;
        AllAvailableSkills[0].isOwned = true;
        AllAvailableSkills[1].isOwned = true;
        AllAvailableSkills[2].isOwned = true;  
    }
	
	public void SetSkillActive(SkillType skillType, bool active = true)
    {
        Skills skill = AllAvailableSkills.Where(x => x.skillID == (int)skillType).FirstOrDefault();

        skill.isActive = active;
    }
    #endregion

    #region Event Related Functions
    //Determine event difficulty
    //Based on Number of Rooms placed, Number of Components in the Event Room, and Room Type
    public int DetermineEventDifficulty(int roomType, int numComponents)
    {
        int eventDifficulty = Generate.instance.GetRoomGameObjectList().Count * numComponents + (roomType * 10);

        int quotient = (int)Mathf.Floor(eventDifficulty / (numMaxRooms / 5));

        int remainder = eventDifficulty % (numMaxRooms / 5);

        if (remainder > 0)
        {
            quotient += 1;
        }

        return quotient;
    }

    public void FireEvent(Events myEvent)
    {
        PlayerControl = false;
        eventActive = true;

        MyCanvas canvasScript = DialogueBox.GetComponent<MyCanvas>();

        canvasScript.StartEvent(myEvent);
    }

    //After user has clicked to confirm the event
    public void StartEvent()
    {
        MyCanvas canvasScript = DialogueBox.GetComponent<MyCanvas>();
        canvasScript.Close();
        PlayerControl = true;

        #region Put Player in Event Room
        float playerPosX = Player.instance.gameObject.transform.position.x;
        float playerPosY = Player.instance.gameObject.transform.position.y;
        float doorPosX = Generate.instance.currentDoor.posX;
        float doorPosY = Generate.instance.currentDoor.posY;

        //Player Going Left (Is Right of Door)
        if ((playerPosX > doorPosX) && (playerPosY > (doorPosY - 0.9) && playerPosY < (doorPosY + 0.9)))
        {
            Player.instance.gameObject.transform.position = new Vector3(doorPosX - 1.25f, doorPosY, 0f); ;
        }

        //Player Going Right (Is Left of Door)
        else if ((playerPosX < doorPosX) && (playerPosY > (doorPosY - 0.9) && playerPosY < (doorPosY + 0.9)))
        {
            Player.instance.gameObject.transform.position = new Vector3(doorPosX + 1.25f, doorPosY, 0f); ;
        }

        //Player Going Down (Is Above Door)
        else if ((playerPosX > doorPosX - 0.9 && playerPosX < doorPosX + 0.9) && (playerPosY > doorPosY))
        {
            Player.instance.gameObject.transform.position = new Vector3(doorPosX, doorPosY - 1.25f, 0f); ;
        }

        //Player Going Up (Is Below Door)
        else if ((playerPosX > doorPosX - 0.9 && playerPosX < doorPosX + 0.9) && (playerPosY < doorPosY))
        {
            Player.instance.gameObject.transform.position = new Vector3(doorPosX, doorPosY + 1.25f, 0f); ;
        }
        #endregion

        //Spawn Enemies
        InstantiateEnemy.spawnEnemy(Generate.instance.currentRoom.roomEvent.Enemies, Generate.instance.currentRoom);
    }

    public void EndEvent(Events myEvent)
    {
        //eventActive = false;
        PlayerControl = false;
        Generate.instance.RemoveDoors();
        Generate.instance.currentDoor = null;

        ///First, get the time that the Player took to complete the event
        ///Next, make a list of the rewards that the Player gets based on that time
        ///ergo, comparing the times
        ///then apply those rewards to the Player
        ///and display them to the screen
        ///Why is this difficult?
        ///

        //--
        

        //--


        #region Rewards
       
        //Hides the Event Display that was at the top of the screen
        EventInfo eventInfo = EventInfo.GetComponent<EventInfo>();
        eventInfo.EndEventInfo();
    
        List<Rewards> rewardsWon = eventInfo.GetRewardsWon();
        List<int> irewardsWon = rewardsWon.Select(x => x.Id).ToList();
        //Show rewards on RewardsWon
        MyCanvas cScript = DialogueBox.GetComponent<MyCanvas>();
        GameObject r = Instantiate(RewardsWon);
        cScript.PlaceRewards(r);

        List<int> iallRewards = cScript.MyEvent.SuccessRewards.Select(x => x.Id).ToList();

        List<int> rewardsNotWon = iallRewards.Intersect(irewardsWon).ToList();
        foreach(Transform child in r.transform)
        {
            try
            {
                Data d = child.GetComponent<Data>();
                try
                {
                    int skillId = child.GetComponent<Data>().MySkill.skillID;
                    if (rewardsNotWon.Contains(skillId))
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                catch
                {
                    //You never set the skill id!?
                }
            }
            catch
            {
                //Ignore it?
            }
        }
        r.SetActive(true);
        //Destroy(r);
        //r.transform.parent.gameObject.SetActive(true);
        //--Rewards/
        #endregion

        //Max Rooms have been placed and the last Sinister Event has been completed
        if (Generate.instance.GetRoomGameObjectList().Count == numMaxRooms && sinisterEventNum == numMaxRooms / 10)
        {
            WinGame();
        }
    }
    #endregion 

    #region Room Type / Shape Select Functions
    public void EnableAllRooms()
    {
        foreach (DataRow row in GlobalVariables.roomAvailability.Rows)
        {
            row["LocalUnlocked"] = 1;
        }
    }

    public void RoomSelector(DataTable availibilty, GameObject RoomSelectMenu)
    {
        foreach (Transform child in RoomSelectMenu.transform)
        {
            try
            {
                DataRow[] availibiltyRow = availibilty.Select("Name = '" + child.name + "' AND RoomType = " + roomTypeSelected + " AND RowNum = " + roomShapeSelectRow);
                string teststr = (string)availibiltyRow[0][4];
                if (teststr == "1")
                {
                    child.gameObject.SetActive(true);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }

            }
            catch
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    //Room Shape Select Scrolling
    public void UpdateRoomSelect(string direction)
    {
        if (roomShapeSelectRow == 1 && direction == "down")
        {
            roomShapeSelectRow++;
            RoomShapeSelectMenu_1.SetActive(false);
            RoomShapeSelectMenu_2.SetActive(true);

        }
        else if (roomShapeSelectRow == 2 && direction == "down")
        {
            roomShapeSelectRow++;
            RoomShapeSelectMenu_2.SetActive(false);
            RoomShapeSelectMenu_3.SetActive(true);
        }
        else if (roomShapeSelectRow == 2 && direction == "up")
        {
            roomShapeSelectRow--;
            RoomShapeSelectMenu_2.SetActive(false);
            RoomShapeSelectMenu_1.SetActive(true);
        }
        else if (roomShapeSelectRow == 3 && direction == "up")
        {
            roomShapeSelectRow--;
            RoomShapeSelectMenu_3.SetActive(false);
            RoomShapeSelectMenu_2.SetActive(true);
        }

        UpdateUI();
    }
    
    //Room Type Select Scrolling
    public void UpdateRoomTypeSelect(string direction)
    {
        Vector3 transform_Temp;
        bool roomChange = false;

        if (roomTypeSelectRow == 1 && direction == "down")
        {
            roomTypeSelectRow++;
            roomChange = true;
        }
        else if (roomTypeSelectRow == 2 && direction == "up")
        {
            roomTypeSelectRow--;
            roomChange = true;
        }

        if (roomChange)
        {
            transform_Temp = RoomTypeSelectMenu_1.transform.position;
            RoomTypeSelectMenu_1.transform.position = RoomTypeSelectMenu_2.transform.position;
            RoomTypeSelectMenu_2.transform.position = transform_Temp;
            roomChange = false;
        }        
    }

    //Changing Room Select Type
    public void SwitchRoomType(string roomType)
    {
        if (roomType == "Storage")
        {
            roomTypeSelected = 1;
        }
        else if (roomType == "Medical")
        {
            roomTypeSelected = 2;
        }
        else if (roomType == "Engineering")
        {
            roomTypeSelected = 3;
        }
        else if (roomType == "Labratory")
        {
            roomTypeSelected = 4;
        }

        foreach (Transform child in RoomShapeSelectMenu_1.transform)
        {
            foreach (Transform smallerChild in child.transform)
            {
                if (smallerChild.name == roomType)
                {
                    smallerChild.gameObject.SetActive(true);
                }
                else
                {
                    smallerChild.gameObject.SetActive(false);
                }
            }
        }

        foreach (Transform child in RoomShapeSelectMenu_2.transform)
        {
            foreach (Transform smallerChild in child.transform)
            {
                if (smallerChild.name == roomType)
                {
                    smallerChild.gameObject.SetActive(true);
                }
                else
                {
                    smallerChild.gameObject.SetActive(false);
                }
            }
        }

        foreach (Transform child in RoomShapeSelectMenu_3.transform)
        {
            foreach (Transform smallerChild in child.transform)
            {
                if (smallerChild.name == roomType)
                {
                    smallerChild.gameObject.SetActive(true);
                }
                else
                {
                    smallerChild.gameObject.SetActive(false);
                }
            }
        }

        UpdateUI();
    }
    #endregion 
}
