using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
//using Assets;

public enum SkillType {  Nothing = -1, Extinguisher = 0, Laser };

public class Inventory : MonoBehaviour//, IHasChanged
{
    //[SerializeField]
    //Transform slots;

    //[SerializeField]
    //Text inventoryText;

    public int Rows = 3;
    public int Columns = 5;
    private int SlotCount;

    //XYSize is a slot's size
    //XYStart is the top left of the panel
    //XYDisplace is the space in-between slots. XYSize is added
    public int XSize = 50;
    public int YSize = 50;
    public int XStart = -120;//Could be derived, but too lazy
    public int YStart = 60;
    public int XDisplace = 10;
    public int YDisplace = -10;

    public List<SkillType> Contents;
    //public List<GameObject> SlotList;

    public GameObject PrefabSlot;
    public GameObject PrefabSlotImage;
    public GameObject Items;
    public GameObject Closable;

    public Sprite Placeholder;

    //Makes typing a little more convenient
    private List<Skills> AllSkills
    {
        get
        {
            return StateMachine.instance.AllAvailableSkills;
            //return TestGM.instance.AllAvailableSkills;
        }
    }
    
    public void Show()
    {
        Closable.SetActive(true);
    }
    
    public void Hide()
    {
        Closable.SetActive(false);
    }

    public void CreateAndPlaceSlots()
    {
        GameObject newSlot = null;
        int x = 0;
        int y = 0;
        for(int i = 0; i < Rows; ++i)
        {
            for(int j = 0; j < Columns; ++j)
            {
                x = XStart + (XSize + XDisplace) * j;
                y = YStart + (-YSize + YDisplace) * i;

                newSlot = Instantiate(PrefabSlot) as GameObject;
                newSlot.transform.SetParent(Items.transform, false);

                //newSlot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                newSlot.transform.localPosition = new Vector3(x, y);
                newSlot.name = "Slot - " + i.ToString() + "-" + j.ToString();

                //SlotList.Add(newSlot);
                newSlot = null;
            }
        }
    }

	public void SetupInventory()
    {
        SlotCount = Rows * Columns;
        //XDisplace += XSize;
        //YDisplace += YSize;

        Contents = new List<SkillType>();
        foreach(Skills s in AllSkills)
        {
            //if (s.isOwned)
            {
                Contents.Add((SkillType)s.skillID);
            }
        }

        int SkillsVsSlots = SlotCount - AllSkills.Count;
        for(int i = 0; i < SkillsVsSlots; ++i)// Need to keep track of their order
        {
            Contents.Add(SkillType.Nothing);
        }

        //SlotList = new List<GameObject>();

        CreateAndPlaceSlots();

        InitItems();
    }

    public GameObject InitSmallSlot()
    {
        GameObject ret = Instantiate(PrefabSlot);//change later
        return ret;
    }

    public void InitItems()
    {
        //Wouldn't make sense since it's for every powerup
        if(Items.transform.childCount < Contents.Count)
        {
            throw new System.Exception("Make inventory bigger");
        }

        GameObject current = null;
        GameObject newImage = null;
        Skills newImageSkill = null;
        for (int i = 0; i < Contents.Count; ++i)
        {
            if (Contents[i] != SkillType.Nothing)
            {
                // Get parent, init new image and set parent
                current = Items.transform.GetChild(i).gameObject;

                newImage = Instantiate(PrefabSlotImage);
                newImage.transform.SetParent(current.transform, false);
                newImage.transform.localPosition = new Vector3(0, 0);

                //Set image based on Contents
                Image newImageImage = newImage.GetComponent<Image>();
                newImageImage.sprite = Placeholder;

                //Scale Image
                RectTransform rect = newImage.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(XSize, YSize);

                //Set data
                newImageSkill = AllSkills.Where(x => x.skillID == (int)Contents[i]).FirstOrDefault();
                newImage.GetComponent<Data>().MySkill = newImageSkill;

                //Set Image
                newImage.GetComponent<Image>().sprite = newImageSkill.symbol;
            }
        }
    }

    //Don't see a point in this
    //public void HasChanged()
    //{
    //    System.Text.StringBuilder builder = new System.Text.StringBuilder();
    //    builder.Append(" - ");

    //    foreach (Transform slotTransform in slots)
    //    {
    //        GameObject item = slotTransform.GetComponent<Slot>().item;

    //        if (item)
    //        {
    //            builder.Append(item.name);
    //            builder.Append(" - ");
    //        }
    //    }

    //    //inventoryText.text = builder.ToString();
    //}

    void Update ()
    {
	    
	}
}

//namespace UnityEngine.EventSystems
//{
//    public interface IHasChanged : UnityEngine.EventSystems.IEventSystemHandler
//    {
//        void HasChanged();
//    }
//}