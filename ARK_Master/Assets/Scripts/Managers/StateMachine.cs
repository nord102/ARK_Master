using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class StateMachine : MonoBehaviour {

	public static StateMachine instance = null;
	public ShipInfo sInfo;
	public PlayerInfo pInfo;
	public List<Skills> AllAvailableSkills;
	
	private bool InventoryOpen = false;
	private bool CharacterSheetOpen = false;
	private bool MainMenuOpen = false;
	private bool MiniMapOpen = false;
    private bool BuildingMenuOpen = false;

    public GameObject Module1;
    public GameObject Module2;
    public GameObject Module3;

    public GameObject BuildingMenu;

    private Dictionary<GameObject, bool> UIModules = new Dictionary<GameObject,bool>();
    private List<object[]> UIModules2 = new List<object[]>();
    public Canvas UI;

    public enum RoomType { Basic=1, MedBay=2, Engineering=3 };
    public enum RoomShape { OneByOne=1, OneByTwo=2, TwoByOne=3, OneByThree=4 };

	private string PreviousPlayerFilePath = "Sinister.txt";
	public List<PlayerInfo> PreviousPlayers;

	public Sprite FireImage = new Sprite();
    

	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		Setup ();
	}

	#region File Loading/Saving
	void LoadDeadCharacters()
	{
		//Read the previous players file and load information about dead characters
		if (File.Exists (PreviousPlayerFilePath)) {
			StreamReader sr = File.OpenText (PreviousPlayerFilePath);
			string s = sr.ReadToEnd ();
			sr.Close ();
			//Now parse out the relevant information into PreviousPlayers
			char[] delimiterChars = { '|'};
			string[] players = s.Split(delimiterChars);
			
			foreach (string player in players)
			{
				char[] delChars = { '\n'};
				string[] lines = player.Split(delChars);
				PlayerInfo p = new PlayerInfo(int.Parse(lines[0]));
				p.PlayerName = lines[1];
				if (lines.Length > 2)
				{
					for (int i = 2; i < lines.Length -1; ++i)
					{
						char[] dChar = {':'};
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
		try{
			if (!File.Exists (PreviousPlayerFilePath)) {
				File.Create(PreviousPlayerFilePath);
			}
			StreamWriter sw = File.AppendText (PreviousPlayerFilePath);
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
			foreach(PlayerInfo p in PreviousPlayers)
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
		SaveDeadCharacters ();
	}

	public void ActivateSinisterEvent()
	{

	}

	void LoadSettings()
	{
		LoadDeadCharacters ();
	}

    void UpdateUI()
    {
        //Update Modules in wrench  
        foreach (object[] obj in UIModules2)
        {
            GameObject g = (GameObject)obj[0];
            obj[1] = false;
            Image temp = (Image)g.GetComponent<Image>();
            temp.sprite = null;
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
                        temp.sprite = s.symbol;
                        obj[1] = true;
                        added = true;
                    }
                }

            }
        }

        //Update Info Panel

        
    }

	void Setup() {
		//PlayerInfo.InitializePlayerInfo (0);
		pInfo = new PlayerInfo (0);
		sInfo = new ShipInfo ();
		GenerateAvailableSkills ();
		LoadSettings ();

        UIModules2.Add(new object[] { Module1, false });
        UIModules2.Add(new object[] { Module2, false });
        UIModules2.Add(new object[] { Module3, false });

		//Get Player to input their name
		//pInfo.PlayerName = GetPlayerName();

		//Generate Rooms
		//Start Intro Tutorial
        
        //Generate Room

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.I)) {
			InventoryOpen = !InventoryOpen;
			if (InventoryOpen)
			{
				//ShowInventory();

			}
			else
			{
				//HideInventory();

			}
		}
		else if (Input.GetKeyDown(KeyCode.C)) {
			CharacterSheetOpen = !CharacterSheetOpen;
			if (CharacterSheetOpen)
			{
				//ShowCharacterSheet();
			}
			else
			{
				//HideCharacterSheet();
			}
		}
		else if (Input.GetKeyDown(KeyCode.M)) {
			MiniMapOpen = !MiniMapOpen;
			if (MiniMapOpen)
			{
				//ShowMiniMap();
			}
			else
			{
				//HideMiniMap();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Escape)) {
			MainMenuOpen = !MainMenuOpen;
			if (MainMenuOpen)
			{
				//ShowMainMenu();
			}
			else
			{
				//HideMainMenu();
			}
		}
	}

	void GenerateAvailableSkills()
	{
		AllAvailableSkills = new List<Skills> ();
		AllAvailableSkills.Add (new Skills (0,FireImage, "Extinguisher", 10, 2));
		AllAvailableSkills.Add (new Skills (1,FireImage, "Laser", 20, 2));
	}

	//Determine event difficulty - based on rooms explored, strength of main character...?
	public int DetermineEventDifficulty()
	{
		//For Testing Only - Return 1
		return 1;
	}
}
