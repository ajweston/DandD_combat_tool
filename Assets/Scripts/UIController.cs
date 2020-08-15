using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using NReco.Csv;

public class UIController : MonoBehaviour
{
    public int pageCount;
    public int slotsPerPage;


    Transform controllerTransform;
    GameObject[] pages;
    Transform[] pageTransforms;
    int activePage;
    Button[] exitButtons;
    GameObject[,] slotObjects;
    Transform[,] slotTransforms;
    Dropdown[,] profileDropdowns;
    Text[,] profileLabels;
    InputField[,] dexterityInputs;
    InputField[,] acInputs;
    Dropdown[,] weaponDropdowns;
    Text[,] weaponLabels;
    InputField[,] speedFactorInputs;
    InputField[,] slotAttackInputs;
    UnityEngine.UI.Text[,] attackTexts;
    UnityEngine.UI.Text[,] acAdjustmentsTexts;
    int[,] pageIndex;
    int[,] slotIndex;
    System.Collections.ArrayList profiles;
    
    enum weapons
    {
        nullWeapon,
        battleAxe
    };

    struct profile 
    {
        public string name;
        public int dexterity;
        public int ac;
        public weapons defaultWeaponID;
        public int speedFactor;
    };

    public static GameObject GetChildWithName(Transform trans, string name)
    {
        try
        {
            Transform childTrans = trans.Find(name);
            if (childTrans != null)
            {
                return childTrans.gameObject;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            UnityEngine.Debug.Log("Could not find child with name " + name);
            return null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //initialize profiles list
        profiles = new ArrayList();

        //Get Controller transform
        controllerTransform = GetComponent<Transform>();

        //Get Page Objects
        pages = new GameObject[pageCount];
        pages[0] = GetChildWithName(controllerTransform, "Page0");
        pages[1] = GetChildWithName(controllerTransform, "Page1");

        pages[0].SetActive(true);
        for(int i = 1; i < pageCount; i++)
        {
            pages[i].SetActive(false);
        }

        //Get Page Transforms
        pageTransforms = new Transform[pageCount];
        for (int i = 0; i < pageCount; i++)
        {
            pageTransforms[i] = pages[i].GetComponent<Transform>();
        }

        //Get Slot objects
        slotObjects = new GameObject[pageCount, slotsPerPage];
        pageIndex = new int[pageCount, slotsPerPage];
        slotIndex = new int[pageCount, slotsPerPage];
        //Page0
        for(int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                pageIndex[i, j] = i;
                slotIndex[i, j] = j;
                string name = System.String.Format("Slot{0}", j);
                slotObjects[i, j] = GetChildWithName(pageTransforms[i], name);
                if (slotObjects[i,j] == null)
                {
                    UnityEngine.Debug.Log("Issue getting slot objects");
                }
            }
        }



        //Get transforms of slot objects
        slotTransforms = new Transform[pageCount, slotsPerPage];

        for(int i = 0; i < pageCount; i++)
        {
            for(int j = 0; j < slotsPerPage; j++)
            {
                slotTransforms[i, j] = slotObjects[i, j].GetComponent<Transform>();

            }
        }

        //Get all slot profile dropdowns and their labels
        profileDropdowns = new Dropdown[pageCount, slotsPerPage];
        profileLabels = new Text[pageCount, slotsPerPage];
        for(int i = 0; i < pageCount; i++)
        {
            for(int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                profileDropdowns[i, j] = GetChildWithName(slotTransforms[i, j], "profileDropdown").GetComponent<Dropdown>();
                profileLabels[i, j] = GetChildWithName(GetChildWithName(slotTransforms[i, j], "profileDropdown").GetComponent<Transform>(), "Label").GetComponent<Text>();
                profileDropdowns[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
            }
        }
        //Load all profiles
        loadProfiles();

        //Get all dexterity input fields
        dexterityInputs = new InputField[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                dexterityInputs[i, j] = GetChildWithName(slotTransforms[i, j], "dexterityInput").GetComponent<InputField>();
                dexterityInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
            }
        }

        //Get all the AC input fields
        acInputs = new InputField[pageCount, slotsPerPage];
        for(int i = 0; i < pageCount; i++)
        {
            for(int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                acInputs[i, j] = GetChildWithName(slotTransforms[i, j], "baseAC").GetComponent<InputField>();
                acInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
            }
        }

        //Get and populate the weapons dropdowns
        weaponDropdowns = new Dropdown[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                weaponDropdowns[i, j] = GetChildWithName(slotTransforms[i, j], "weaponDropdown").GetComponent<Dropdown>();
                weaponDropdowns[i, j].onValueChanged.AddListener(delegate { try { updateWeaponDropdown(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
            }
        }

        //Get the speed factor input fields
        speedFactorInputs = new InputField[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                speedFactorInputs[i, j] = GetChildWithName(slotTransforms[i, j], "speedFactorInput").GetComponent<InputField>();
                speedFactorInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
            }
        }

        //Get the the slot to attack input field
        slotAttackInputs = new InputField[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                slotAttackInputs[i, j] = GetChildWithName(slotTransforms[i, j], "SlotAttacking").GetComponent<InputField>();
                slotAttackInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
            }
        }

        //Get the attack text
        attackTexts = new UnityEngine.UI.Text[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                attackTexts[i, j] = GetChildWithName(slotTransforms[i, j], "Attacks").GetComponent<Text>();
            }
        }

        //Get the AC Adjustment text
        acAdjustmentsTexts = new UnityEngine.UI.Text[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                acAdjustmentsTexts[i, j] = GetChildWithName(slotTransforms[i, j], "acAdjustment").GetComponent<Text>();
            }
        }

        //Setup exit buttons with listener functions
        exitButtons = new Button[pageCount];
        for (int i = 0; i < pageCount; i++)
        {
            exitButtons[i] = GetChildWithName(pageTransforms[i], "exitButton").GetComponent<Button>();
            exitButtons[i].onClick.AddListener(exitButtonPressed);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (activePage != 0)
            {
                activePage--;
                pages[activePage + 1].SetActive(false);
                pages[activePage].SetActive(true);
            }
        }else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(activePage != pageCount-1)
            {
                activePage++;
                pages[activePage - 1].SetActive(false);
                pages[activePage].SetActive(true);
            }
        }
    }

    void exitButtonPressed()
    {
        Application.Quit();
    }

    //Fill the profiles array and adds all options to the profile dropdown
    void loadProfiles()
    {
        //parse profiles
        string fileName = "./config/profiles.csv";
        StreamReader reader = new StreamReader(fileName);
        var csvReader = new CsvReader(reader, ",");
        //read out column header
        csvReader.Read();
        while (csvReader.Read())
        {
            if (csvReader[0] == "") break;
            profile newProfile;
            newProfile.name = csvReader[0];
            newProfile.dexterity = int.Parse(csvReader[1]);
            newProfile.ac = int.Parse(csvReader[2]);
            if (csvReader[3] != "")
            {
                newProfile.defaultWeaponID = (weapons)int.Parse(csvReader[3]);
            }
            else newProfile.defaultWeaponID = weapons.nullWeapon;
            if (csvReader[4] != "")
            {

                newProfile.speedFactor = int.Parse(csvReader[4]);
            }
            else newProfile.speedFactor = 0;

            profiles.Add(newProfile);
        }


        List<string> m_DropOptions = new List<string> { "-" };
        for(int i = 0; i < profiles.Count; i++)
        {
            profile _profile = (profile)profiles[i];
            m_DropOptions.Add(_profile.name);
        }

        for (int i = 0; i < pageCount; i++)
        {
            for(int j = 0; j < slotsPerPage; j++)
            {
                profileDropdowns[i, j].ClearOptions();
                profileDropdowns[i, j].AddOptions(m_DropOptions);
            }
        }
    }

    void updateSlot(int page, int slot)
    {
        try
        {
            acAdjustmentsTexts[page, slot].text = "Test";
        }
        catch
        {
            UnityEngine.Debug.Log("Cannot update for page: " + page + " and slot: " + slot);
        }
    }

    void updateWeaponDropdown(int page, int slot)
    {

    }

}
