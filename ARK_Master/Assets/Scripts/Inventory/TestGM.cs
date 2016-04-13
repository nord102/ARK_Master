using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGM : MonoBehaviour {
    public static TestGM instance = null;
    public GameObject GoInventory;
    public List<Skills> AllAvailableSkills;

    void Start ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        GenerateAvailableSkills();
        GoInventory.GetComponent<Inventory>().SetupInventory();
    }

    void GenerateAvailableSkills()
    {
        AllAvailableSkills = new List<Skills>();
        AllAvailableSkills.Add(new Skills(0, null, "Extinguisher", 10, 2));
        AllAvailableSkills.Add(new Skills(1, null, "Laser", 20, 2));
    }

    void Update ()
    {
	
	}
}
