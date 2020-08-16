using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using NReco.Csv;
using System.Globalization;

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
    Dropdown[,] classDropdowns;
    Text[,] classLabels;
    InputField[,] levelOrHitDieInputs;
    InputField[,] acTypeInputs;
    InputField[,] acInputs;
    Dropdown[,] weaponDropdowns;
    UnityEngine.UI.Text[,] weaponLabels;
    InputField[,] speedFactorInputs;
    UnityEngine.UI.Text[,] attackTexts;
    UnityEngine.UI.Text[,] minToHitTexts;
    Dropdown[,] targetClassDropdowns;
    Text[,] targetClassLabels;
    InputField[,] targetLevelOrHitDieInputs;
    InputField[,] targetAcTypeInputs;
    InputField[,] targetAcInputs;
    Dropdown[,] targetWeaponDropdowns;
    Text[,] targetWeaponLabels;
    InputField[,] targetSpeedFactorInputs;
    UnityEngine.UI.Text[,] targetAttackTexts;
    UnityEngine.UI.Text[,] targetMinToHitTexts;
    int[,] pageIndex;
    int[,] slotIndex;
    System.Collections.Generic.List<profile> profiles;

    string[] classes =
    {
        "-",
        "Cleric",
        "Druid",
        "Fighter",
        "Paladin",
        "Ranger",
        "Magic-User",
        "Illusionist",
        "Theif",
        "Assassin",
        "Monk",
        "Monster"
    };

    string[] weaponNames =
    {
        "-",                                  //0
        "Axe, Battle",                        //1
        "Axe, Hand",                          //2
        "Bardiche",                           //3
        "Bec de Corbin",                      //4
        "Bill-Guisarme",                      //5
        "Bo Stick",                           //6
        "Club",                               //7
        "Dagger",                             //8
        "Fauchard",                           //9
        "Fauchard-Fork",                      //10
        "Fist or Open Hand",                  //11
        "Flail, Footman's",                   //12
        "Flail, Horseman's",                  //13
        "Fork, Military",                     //14
        "Glaive",                             //15
        "Glaive-Guisarme",                    //16
        "Guisarme",                           //17
        "Guisarme-Voulge",                    //18
        "Halberd",                            //19
        "Hammer, Lucern",                     //20
        "Hammer",                             //21
        "Jo Stick",                           //22
        "Lance (Heavy Horse)",                //23
        "Lance (Light Horse)",                //24
        "Lance (Medium Horse)",               //25
        "Mace, Footman's",                    //26
        "Mace, Horseman's",                   //27
        "Morning Star",                       //28
        "Partisan",                           //29
        "Pick, Military, Footman's",          //30
        "Pick, Military, Horseman's",         //31
        "Pick, Awl",                          //32
        "Ranseur",                            //33
        "Scimitar",                           //34
        "Spear",                              //35
        "Septum",                             //36
        "Staff, Quarter",                     //37
        "Sword, Bastard",                     //38
        "Sword, Broad",                       //39
        "Sword, Long",                        //40
        "Sword, Short",                       //41
        "Sword, Two-Handed",                  //42
        "Trident",                            //43
        "Voulge",                             //44
        "Thrown Hand-Axe",                    //45
        "Bow, Composite, Long",               //46
        "Bow, Composite, Short",              //47
        "Bow, Long",                          //48
        "Bow, Short",                         //49
        "Thrown Club",                        //50
        "Crossbow, Heavy",                    //51
        "Crossbow, Light",                    //52
        "Thrown Dagger",                      //53
        "Dart",                               //54
        "Thrown Hammer",                      //55
        "Javelin",                            //56
        "Sling, Bullet",                      //57
        "Sling, Stone",                       //58
        "Thrown Spear"                        //59
    };

    int[] weaponSpeedFactors =
    {
        -1, //"-",
        7,  //"Axe, Battle",
        4,  //"Axe, Hand",
        9,  //"Bardiche",
        9,  //"Bec de Corbin",
        10, //"Bill-Guisarme",
        3,  //"Bo Stick",
        4,  //"Club",
        2,  //"Dagger",
        8,  //"Fauchard",
        8,  //"Fauchard-Fork",
        1,  //"Fist or Open Hand",
        7,  //"Flail, Footman's",
        6,  //"Flail, Horseman's",
        7,  //"Fork, Military",
        8,  //"Glaive",
        9,  //"Glaive-Guisarme",
        8,  //"Guisarme",
        10, //"Guisarme-Voulge",
        9,  //"Halberd",
        9,  //"Hammer, Lucern",
        4,  //"Hammer",
        2,  //"Jo Stick",
        8,  //"Lance (Heavy Horse)",
        7,  //"Lance (Light Horse)",
        6,  //"Lance (Medium Horse)",
        7,  //"Mace, Footman's",
        6,  //"Mace, Horseman's",
        7,  //"Morning Star",
        9,  //"Partisan",
        7,  //"Pick, Military, Footman's",
        5,  //"Pick, Military, Horseman's",
        13, //"Pick, Awl",
        8,  //"Ranseur",
        4,  //"Scimitar",
        6,  //"Spear",
        8,  //"Septum",
        4,  //"Staff, Quarter",
        6,  //"Sword, Bastard",
        5,  //"Sword, Broad",
        5,  //"Sword, Long",
        3,  //"Sword, Short",
        10, //"Sword, Two-Handed",
        6,  //"Trident",
        10, //"Voulge",
        1,  //"Thrown Hand-Axe",
        2,  //"Bow, Composite, Long",
        2,  //"Bow, Composite, Short",
        2,  //"Bow, Long",
        2,  //"Bow, Short",
        1,  //"Thrown Club",
        0,  //"Crossbow, Heavy",
        1,  //"Crossbow, Light",
        2,  //"Thrown Dagger",
        3,  //"Dart",
        1,  //"Thrown Hammer",
        1,  //"Javelin",
        1,  //"Sling, Bullet",
        1,  //"Sling, Stone",
        1   //"Thrown Spear"

    };

    int[,] weaponACAdjustments = {
        {0,0,0,0,0,0,0,0,0 },          //"-",
        {-3,-2,-1,-1,0,0,1,1,2 }//,      //"Axe, Battle",
        //{ },                           //"Axe, Hand",
        //{ },                           //"Bardiche",
        //{ },                           //"Bec de Corbin",
        //{ },                           //"Bill-Guisarme",
        //{ },                           //"Bo Stick",
        //{ },                           //"Club",
        //{ },                           //"Dagger",
        //{ },                           //"Fauchard",
        //{ },                           //"Fauchard-Fork",
        //{ },                           //"Fist or Open Hand",
        //{ },                           //"Flail, Footman's",
        //{ },                           //"Flail, Horseman's",
        //{ },                           //"Fork, Military",
        //{ },                           //"Glaive",
        //{ },                           //"Glaive-Guisarme",
        //{ },                           //"Guisarme",
        //{ },                           //"Guisarme-Voulge",
        //{ },                           //"Halberd",
        //{ },                           //"Hammer, Lucern",
        //{ },                           //"Hammer",
        //{ },                           //"Jo Stick",
        //{ },                           //"Lance (Heavy Horse)",
        //{ },                           //"Lance (Light Horse)",
        //{ },                           //"Lance (Medium Horse)",
        //{ },                           //"Mace, Footman's",
        //{ },                           //"Mace, Horseman's",
        //{ },                           //"Morning Star",
        //{ },                           //"Partisan",
        //{ },                           //"Pick, Military, Footman's",
        //{ },                           //"Pick, Military, Horseman's",
        //{ },                           //"Pick, Awl",
        //{ },                           //"Ranseur",
        //{ },                           //"Scimitar",
        //{ },                           //"Spear",
        //{ },                           //"Septum",
        //{ },                           //"Staff, Quarter",
        //{ },                           //"Sword, Bastard",
        //{ },                           //"Sword, Broad",
        //{ },                           //"Sword, Long",
        //{ },                           //"Sword, Short",
        //{ },                           //"Sword, Two-Handed",
        //{ },                           //"Trident",
        //{ },                           //"Voulge",
        //{ },                           //"Thrown Hand-Axe",
        //{ },                           //"Bow, Composite, Long",
        //{ },                           //"Bow, Composite, Short",
        //{ },                           //"Bow, Long",
        //{ },                           //"Bow, Short",
        //{ },                           //"Thrown Club",
        //{ },                           //"Crossbow, Heavy",
        //{ },                           //"Crossbow, Light",
        //{ },                           //"Thrown Dagger",
        //{ },                           //"Dart",
        //{ },                           //"Thrown Hammer",
        //{ },                           //"Javelin",
        //{ },                           //"Sling, Bullet",
        //{ }                            //"Sling, Stone",
    };                                 //"Thrown Spear"


    struct profile 
    {
        public string name;
        public int classId;
        public int level;
        public int acType;
        public int ac;
        public int defaultWeaponID;
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
        profiles = new List<profile>();

        //Get Controller transform
        controllerTransform = GetComponent<Transform>();

        //Get Page Objects
        pages = new GameObject[pageCount];
        for (int i = 0; i < pageCount; i++)
        {
            string name = System.String.Format("Page{0}", i);
            pages[i] = GetChildWithName(controllerTransform, name);
            if (pages[i] == null)
            {
                UnityEngine.Debug.Log("Issue getting page objects");
            }
        }


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

        //Change number labels
        UnityEngine.UI.Text label;
        Transform trans;
        for (int i = 1; i < pageCount; i++)
        {
            trans = GetChildWithName(pageTransforms[i], "Labels").GetComponent<Transform>();
            label = GetChildWithName(trans, "Label1").GetComponent<Text>();
            label.text = (1 + 5 * i).ToString();
            label = GetChildWithName(trans, "Label2").GetComponent<Text>();
            label.text = (2 + 5 * i).ToString();
            label = GetChildWithName(trans, "Label3").GetComponent<Text>();
            label.text = (3 + 5 * i).ToString();
            label = GetChildWithName(trans, "Label4").GetComponent<Text>();
            label.text = (4 + 5 * i).ToString();
            label = GetChildWithName(trans, "Label5").GetComponent<Text>();
            label.text = (5 + 5 * i).ToString();

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
                profileDropdowns[i, j].onValueChanged.AddListener(delegate { try { profileUpdate(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
            }
        }
        //Load all profiles
        loadProfiles();

        //Get all slot class dropdowns and their labels
        classDropdowns = new Dropdown[pageCount, slotsPerPage];
        targetClassDropdowns = new Dropdown[pageCount, slotsPerPage];
        classLabels = new Text[pageCount, slotsPerPage];
        targetClassLabels = new Text[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                classDropdowns[i, j] = GetChildWithName(slotTransforms[i, j], "classDropdown").GetComponent<Dropdown>();
                classLabels[i, j] = GetChildWithName(GetChildWithName(slotTransforms[i, j], "classDropdown").GetComponent<Transform>(), "Label").GetComponent<Text>();
                classDropdowns[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                targetClassDropdowns[i, j] = GetChildWithName(slotTransforms[i, j], "targetClassDropdown").GetComponent<Dropdown>();
                targetClassLabels[i, j] = GetChildWithName(GetChildWithName(slotTransforms[i, j], "targetClassDropdown").GetComponent<Transform>(), "Label").GetComponent<Text>();
                targetClassDropdowns[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });

                loadClasses(classDropdowns[i, j]);
                loadClasses(targetClassDropdowns[i, j]);
            }
        }

        //Get all levelOrHitDie input fields
        levelOrHitDieInputs = new InputField[pageCount, slotsPerPage];
        targetLevelOrHitDieInputs = new InputField[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                levelOrHitDieInputs[i, j] = GetChildWithName(slotTransforms[i, j], "levelOrHitDie").GetComponent<InputField>();
                levelOrHitDieInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                targetLevelOrHitDieInputs[i, j] = GetChildWithName(slotTransforms[i, j], "targetLevelOrHitDie").GetComponent<InputField>();
                targetLevelOrHitDieInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                levelOrHitDieInputs[i, j].text = "-";
                targetLevelOrHitDieInputs[i, j].text = "-";
            }
        }

        //Get all acType input fields
        acTypeInputs = new InputField[pageCount, slotsPerPage];
        targetAcTypeInputs = new InputField[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                acTypeInputs[i, j] = GetChildWithName(slotTransforms[i, j], "ACType").GetComponent<InputField>();
                acTypeInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                targetAcTypeInputs[i, j] = GetChildWithName(slotTransforms[i, j], "targetACType").GetComponent<InputField>();
                targetAcTypeInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                acTypeInputs[i, j].text = "-";
                targetAcTypeInputs[i, j].text = "-";
            }
        }

        //Get all the AC input fields
        acInputs = new InputField[pageCount, slotsPerPage];
        targetAcInputs = new InputField[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for(int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                acInputs[i, j] = GetChildWithName(slotTransforms[i, j], "AC").GetComponent<InputField>();
                acInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                targetAcInputs[i, j] = GetChildWithName(slotTransforms[i, j], "targetAC").GetComponent<InputField>();
                targetAcInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                acInputs[i, j].text = "-";
                targetAcInputs[i, j].text = "-";
            }
        }

        //Get and populate the weapons dropdowns, get the weapon labels
        weaponDropdowns = new Dropdown[pageCount, slotsPerPage];
        targetWeaponDropdowns = new Dropdown[pageCount, slotsPerPage];
        weaponLabels = new UnityEngine.UI.Text[pageCount, slotsPerPage];
        targetWeaponLabels = new UnityEngine.UI.Text[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                weaponDropdowns[i, j] = GetChildWithName(slotTransforms[i, j], "weaponDropdown").GetComponent<Dropdown>();
                weaponDropdowns[i, j].onValueChanged.AddListener(delegate { try { updateWeaponDropdown(pageIndex[m, n], slotIndex[m, n],false); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                targetWeaponDropdowns[i, j] = GetChildWithName(slotTransforms[i, j], "targetWeaponDropdown").GetComponent<Dropdown>();
                targetWeaponDropdowns[i, j].onValueChanged.AddListener(delegate { try { updateWeaponDropdown(pageIndex[m, n], slotIndex[m, n],true); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                populateWeaponDropdown(weaponDropdowns[i, j]);
                populateWeaponDropdown(targetWeaponDropdowns[i, j]);

                weaponLabels[i,j] = GetChildWithName(GetChildWithName(slotTransforms[i,j].GetComponent<Transform>(),"weaponDropdown").GetComponent<Transform>(),"Label").GetComponent<Text>();
                targetWeaponLabels[i, j] = GetChildWithName(GetChildWithName(slotTransforms[i, j].GetComponent<Transform>(), "targetWeaponDropdown").GetComponent<Transform>(), "Label").GetComponent<Text>();

            }
        }

        //Get the speed factor input fields
        speedFactorInputs = new InputField[pageCount, slotsPerPage];
        targetSpeedFactorInputs = new InputField[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                int m = i;
                int n = j;
                speedFactorInputs[i, j] = GetChildWithName(slotTransforms[i, j], "speedFactorInput").GetComponent<InputField>();
                speedFactorInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                targetSpeedFactorInputs[i, j] = GetChildWithName(slotTransforms[i, j], "targetSpeedFactorInput").GetComponent<InputField>();
                targetSpeedFactorInputs[i, j].onValueChanged.AddListener(delegate { try { updateSlot(pageIndex[m, n], slotIndex[m, n]); } catch { UnityEngine.Debug.Log(m + " " + n); } });
                speedFactorInputs[i, j].text = "-";
                targetSpeedFactorInputs[i, j].text = "-";
            }
        }


        //Get the attack text
        attackTexts = new UnityEngine.UI.Text[pageCount, slotsPerPage];
        targetAttackTexts = new UnityEngine.UI.Text[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                attackTexts[i, j] = GetChildWithName(slotTransforms[i, j], "Attacks").GetComponent<Text>();
                targetAttackTexts[i, j] = GetChildWithName(slotTransforms[i, j], "targetAttacks").GetComponent<Text>();
            }
        }

        //Get the AC Adjustment text
        minToHitTexts = new UnityEngine.UI.Text[pageCount, slotsPerPage];
        targetMinToHitTexts = new UnityEngine.UI.Text[pageCount, slotsPerPage];
        for (int i = 0; i < pageCount; i++)
        {
            for (int j = 0; j < slotsPerPage; j++)
            {
                minToHitTexts[i, j] = GetChildWithName(slotTransforms[i, j], "minToHit").GetComponent<Text>();
                targetMinToHitTexts[i, j] = GetChildWithName(slotTransforms[i, j], "targetMinToHit").GetComponent<Text>();
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
            newProfile.name = "-";
            newProfile.classId = 0;
            newProfile.level = 0;
            newProfile.ac = 0;
            newProfile.acType = 0;
            newProfile.defaultWeaponID = 0;
            newProfile.speedFactor = 0;

            newProfile.name = csvReader[0];
            newProfile.classId = int.Parse(csvReader[1]);
            newProfile.level = int.Parse(csvReader[2]);
            newProfile.acType = int.Parse(csvReader[3]);
            newProfile.ac = int.Parse(csvReader[4]);
            if (csvReader[5] != "")
            {
                newProfile.defaultWeaponID = int.Parse(csvReader[5]);
                newProfile.speedFactor = weaponSpeedFactors[newProfile.defaultWeaponID];
            }
            if (csvReader[6] != "")
            {
                newProfile.defaultWeaponID = 0;
                newProfile.speedFactor = int.Parse(csvReader[6]);
            }

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

    void populateWeaponDropdown(Dropdown dropdown)
    {
        dropdown.ClearOptions();
        List<string> m_DropOptions = new List<string> {};
        for (int i = 0; i < weaponNames.Length; i++)
        {
            m_DropOptions.Add(weaponNames[i]);
        }
        dropdown.AddOptions(m_DropOptions);
    }


    void updateSlot(int page, int slot)
    {
        if (acTypeInputs[page, slot].text == "-" || acInputs[page, slot].text == "-" || speedFactorInputs[page, slot].text == "-" || targetAcInputs[page, slot].text == "-" ||
            targetAcTypeInputs[page, slot].text == "-" || targetSpeedFactorInputs[page, slot].text == "-" || classLabels[page,slot].text == "-" || targetClassLabels[page,slot].text == "-" ||
            levelOrHitDieInputs[page,slot].text == "-" || targetLevelOrHitDieInputs[page,slot].text == "-")
        {
            return;
        }
        else
        {

            minToHitTexts[page, slot].text = "Test";
        }

    }

    void updateWeaponDropdown(int page, int slot, bool target)
    {

        for(int i = 0; i < weaponNames.Length; i++)
        {
            if (target)
            {
                if (targetWeaponLabels[page, slot].text == weaponNames[i])
                {
                    targetSpeedFactorInputs[page,slot].text = weaponSpeedFactors[i].ToString();
                    GetChildWithName(GetChildWithName(slotTransforms[page, slot], "targetSpeedFactorInput").GetComponent<Transform>(), "Placeholder").GetComponent<Text>().text = weaponSpeedFactors[i].ToString();
                    break;
                }

            }
            else
            {
                if(weaponLabels[page,slot].text == weaponNames[i])
                {
                    speedFactorInputs[page,slot].text = weaponSpeedFactors[i].ToString();
                    GetChildWithName(GetChildWithName(slotTransforms[page, slot], "speedFactorInput").GetComponent<Transform>(), "Placeholder").GetComponent<Text>().text = weaponSpeedFactors[i].ToString();
                    break;
                }
            }
        }
        updateSlot(page, slot);
    }

    void profileUpdate(int page, int slot)
    {
        for (int i = 0; i < profiles.Count; i++)
        {
            if(profiles[i].name == profileLabels[page, slot].text)
            {
                UnityEngine.Debug.Log(profiles[i].name);
                UnityEngine.Debug.Log(weaponNames[profiles[i].defaultWeaponID]);
                UnityEngine.Debug.Log(profiles[i].level.ToString());
                weaponLabels[page, slot].text = weaponNames[profiles[i].defaultWeaponID];
                classLabels[page, slot].text = classes[profiles[i].classId];
                levelOrHitDieInputs[page, slot].text = profiles[i].level.ToString();
                acTypeInputs[page,slot].text = profiles[i].acType.ToString();
                acInputs[page, slot].text = profiles[i].ac.ToString();
                speedFactorInputs[page, slot].text = profiles[i].speedFactor.ToString();
                break;
            }
        }
        updateSlot(page, slot);
    }

    void loadClasses(Dropdown dropdown)
    {
        dropdown.ClearOptions();
        List<string> m_DropOptions = new List<string> { };
        for (int i = 0; i < classes.Length; i++)
        {
            m_DropOptions.Add(classes[i]);
        }
        dropdown.AddOptions(m_DropOptions);
    }

}
