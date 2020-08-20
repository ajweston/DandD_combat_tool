using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using NReco.Csv;
using System.Globalization;

public class CombatController : MonoBehaviour
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
        "Awl Pike",                           //1
        "Axe, Battle",                        //2
        "Axe, Hand",                          //3
        "Bardiche",                           //4
        "Bec de Corbin",                      //5
        "Bill-Guisarme",                      //6
        "Bo Stick",                           //7
        "Club",                               //8
        "Dagger",                             //9
        "Falchion",                           //10
        "Fauchard",                           //11
        "Fauchard-Fork",                      //12
        "Fist or Open Hand",                  //13
        "Fist, mailed",                       //14
        "Flail, Footman's",                   //15
        "Flail, Horseman's",                  //16
        "Foot, bare or soft boot",            //17
        "Foot, hard boot",                    //18
        "Fork, Military",                     //19
        "Glaive",                             //20
        "Glaive-Guisarme",                    //21
        "Guisarme",                           //22
        "Guisarme-Voulge",                    //23
        "Halberd",                            //24
        "Hammer, Lucern",                     //25
        "Hammer",                             //26
        "Hook Fauchard",                      //27
        "Javelin",                            //28
        "Jo Stick",                           //29
        "Lance (Heavy Horse)",                //30
        "Lance (Light Horse)",                //31
        "Lance (Medium Horse)",               //32
        "Mace, Footman's",                    //33
        "Mace, Horseman's",                   //34
        "Maul",                               //35
        "Morning Star",                       //36
        "Partisan",                           //37
        "Pick, Military, Footman's",          //38
        "Pick, Military, Horseman's",         //39
        "Pick, Awl",                          //40
        "Quarter Staff",                      //41
        "Ranseur",                            //42
        "Scimitar",                           //43
        "Spear",                              //44
        "Septum",                             //45
        "Sword, Bastard",                     //46
        "Sword, Broad",                       //47
        "Sword, Long",                        //48
        "Sword, Short",                       //49
        "Sword, Two-Handed",                  //50
        "Trident",                            //51
        "Voulge",                             //52
        "Thrown Hand-Axe",                    //53
        "Bow, Composite, Long",               //54
        "Bow, Composite, Short",              //55
        "Bow, Long",                          //56
        "Bow, Short",                         //57
        "Thrown Club",                        //58
        "Crossbow, Heavy",                    //59
        "Crossbow, Light",                    //60
        "Thrown Dagger",                      //61
        "Dart",                               //62
        "Thrown Hammer",                      //63
        "Thrown Javelin",                     //64
        "Sling, Bullet",                      //65
        "Sling, Stone",                       //66
        "Thrown Spear"                        //67
    };

    int[] weaponSpeedFactors =
    {
        -1, //"-",
        13, //"Awl Pike",
        7,  //"Axe, Battle",
        4,  //"Axe, Hand",
        9,  //"Bardiche",
        9,  //"Bec de Corbin",
        10, //"Bill-Guisarme",
        3,  //"Bo Stick",
        4,  //"Club",
        2,  //"Dagger",
        5,  //"Falchion",
        8,  //"Fauchard",
        8,  //"Fauchard-Fork",
        1,  //"Fist or Open Hand",
        1,  //"Fist, mailed",
        7,  //"Flail, Footman's",
        6,  //"Flail, Horseman's",
        3,  //"Foot, bare or soft boot",
        3,  //"Foot, hard boot"
        7,  //"Fork, Military",
        8,  //"Glaive",
        9,  //"Glaive-Guisarme",
        8,  //"Guisarme",
        10, //"Guisarme-Voulge",
        9,  //"Halberd",
        9,  //"Hammer, Lucern",
        4,  //"Hammer",
        9,  //"Hook Fauchard",
        3,  //"Javelin",
        2,  //"Jo Stick",
        8,  //"Lance (Heavy Horse)",
        7,  //"Lance (Light Horse)",
        6,  //"Lance (Medium Horse)",
        7,  //"Mace, Footman's",
        6,  //"Mace, Horseman's",
        8,  //"Maul",
        7,  //"Morning Star",
        9,  //"Partisan",
        7,  //"Pick, Military, Footman's",
        5,  //"Pick, Military, Horseman's",
        13, //"Pick, Awl",
        4,  //"Quarter Staff",
        8,  //"Ranseur",
        4,  //"Scimitar",
        6,  //"Spear",
        8,  //"Septum",
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
        1,  //"Thrown Javelin",
        1,  //"Sling, Bullet",
        1,  //"Sling, Stone",
        1   //"Thrown Spear"

    };

    int[,] weaponACAdjustments = {
        {0,0,0,0,0,0,0,0,0,},                           //"-",
        {-1 ,0 ,0 ,0 ,0 ,0 ,0 ,-1 ,-2 },                //Awl Pike
        {-3,-2,-1,-1,0,0,1,1,2 },                       //"Axe, Battle",
        {-3,-2,-2,-1,0,0,1,1,1 },                       //"Axe, Hand",
        {-2,-1,0,0,1,1,2,2,3 },                         //"Bardiche",
        {2,2,2,0,0,0,0,0,-1 },                          //"Bec de Corbin",
        {0,0,0,0,0,0,1,0,0 },                           //"Bill-Guisarme",
        {-9,-7,-5,-3,-1,0,1,0,3 },                      //"Bo Stick",
        {-5,-4,-3,-2,-1,-1,0,0,1 },                     //"Club",
        {-3,-3,-2,-2,0,0,1,1,3 },                       //"Dagger",
        {-2, -1, 0, 1, 1, 1, 1, 0, 0 },                 //Falchion,
        {-1,-1,-1,0,0,0,1,0,1 },                        //"Fauchard",
        {-1, -1, -1, 0, 0, 0, 1, 0, 1 },                //"Fauchard-Fork",
        {-7, -5, -3, -1, 0, 0, 2, 0, 4 },               //"Fist or Open Hand",
        {-6, -4, -3, 0, 0, 0, 2, 0, 3 },                //"Fist, mailed",
        {2, 2, 1, 2, 1, 1, 1, 1, -1 },                  //"Flail, Footman's",
        {0, 0, 0, 0, 0, 1, 1, 1, 0  },                  //"Flail, Horseman's",
        {-6 ,-4 ,-3 ,0 ,0 ,0 ,+3 ,+3 ,+3 },             //"Foot, bare or soft boot",
        {-5 ,-3 ,-2 ,0 ,0 ,0 ,+3 ,+3 ,+3  },            //"Foot, hard boot"
        {-2, -2, -1, 0, 0, +1, +1, 0, +1 },             //"Fork, Military",
        {-1 ,-1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },                 //"Glaive",
        { -1 ,-1 ,0 ,0 ,0 ,0 ,0 ,0 ,0},                 //"Glaive-Guisarme",
        {-2 ,-2 ,-1 ,-1 ,0 ,0 ,0 ,-1 ,-1  },            //"Guisarme",
        {-1 ,-1 ,0 ,+1 ,+1 ,+1 ,0 ,0 ,0 },              //"Guisarme-Voulge",
        { +1 ,+1 ,+1 ,+2 ,+2 ,+2 ,+1 ,+1 ,0},           //"Halberd",
        {+1 ,+1 ,+2 ,+2 ,+2 ,+1 ,+1 ,0 ,0  },           //"Hammer, Lucern",
        {0 ,+1 ,0 ,+1 ,0 ,0 ,0 ,0 ,0 },                 //"Hammer",
        {-2 ,-2 ,-1 ,-1 ,0 ,0 ,0 ,0 ,-1  },             //"Hook Fauchard",
        {-3 ,-2 ,-2 ,-1 ,0 ,0 ,+1 ,0 ,+2  },            //"Javelin",
        { 9 ,-7 ,-5 ,-3 ,-1 ,0 ,+1 ,0 ,+3 },            //"Jo Stick",
        {+3 ,+3 ,+2 ,+2 ,+2 ,+1 ,+1 ,0 ,0 },            //"Lance (Heavy Horse)",
        {-2 ,-2 ,-1 ,0 ,0 ,0 ,0 ,0 ,0  },               //"Lance (Light Horse)",
        { 0 ,+1 ,+1 ,+1 ,+1 ,0 ,0 ,0 ,0},               //"Lance (Medium Horse)",
        { +1 ,+1 ,0 ,0 ,0 ,0 ,0 ,+1 ,-1},               //"Mace, Footman's",
        { +1 ,+1 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },                //"Mace, Horseman's",
        {+1 ,+1 ,+1 ,+1 ,+1 ,+1 ,0 ,0 ,-2 },            //"Maul",
        { 0 ,+1 ,+1 ,+1 ,+1 ,+1 ,+1 ,+2 ,+2},           //"Morning Star",
        { 0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 },                  //"Partisan",
        { +2 ,+2 ,+1 ,+1 ,0 ,-1 ,-1 ,-1 ,-2},           //"Pick, Military, Footman's",
        { +1 ,+1 ,+1 ,+1 ,0 ,0 ,-1 ,-1 ,-1},            //"Pick, Military, Horseman's",
        { -1,0,0,0,0,0,0,-1,-2},                        //"Pick, Awl",
        { -7 ,-5 ,-3 ,-1 ,0 ,0 ,+1 ,+1 ,+1 },           //"Quarter Staff",
        { -2 ,-1 ,-1 ,0 ,0 ,0 ,0 ,0 ,+1 },              //"Ranseur",
        { -3 ,-2 ,-2 ,-1 ,0 ,0 ,+1 ,+1 ,+3},            //"Scimitar",
        { -2 ,-1 ,-1 ,-1 ,0 ,0 ,0 ,0 ,0 },              //"Spear",
        { -2 ,-1 ,0 ,0 ,0 ,0 ,0 ,+1 ,+2 },              //"Septum",
        {0 ,0 ,+1 ,+1 ,+1 ,+1 ,+1 ,+1 ,0 },             //"Sword, Bastard",
        { -3 ,-2 ,-1 ,0 ,0 ,+1 ,+1 ,+1 ,+2 },           //"Sword, Broad",
        {-2 ,-1 ,0 ,0 ,0 ,0 ,0 ,+1 ,+2  },              //"Sword, Long",
        { -3 ,-2 ,-2 ,-1 ,0 ,0 ,+1 ,0 ,+2 },            //"Sword, Short",
        { +2 ,+2 ,+2 ,+2 ,+3 ,+3 ,+3 ,+1 ,0},           //"Sword, Two-Handed",
        { -3 ,-2 ,-1 ,-1 ,0 ,0 ,+1 ,0 ,+1 },            //"Trident",
        { -1 ,-1 ,0 ,+1 ,+1 ,+1 ,0 ,0 ,0},              //"Voulge",
        { -4 ,-3 ,-2 ,-1 ,-1 ,0 ,0 ,0 ,+1},             //"Thrown Hand-Axe",
        { -2 ,-1 ,0 ,0 ,+1 ,+2 ,+2 ,+3 ,+3},            //"Bow, Composite, Long",
        { -3 ,-3 ,-1 ,0 ,+1 ,+2 ,+2 ,+2 ,+3},           //"Bow, Composite, Short",
        { -1 ,0 ,0 ,+1 ,+2 ,+3 ,+3 ,+3 ,+3},            //"Bow, Long",
        { -5 ,-4 ,-1 ,0 ,0 ,+1 ,+2 ,+2 ,+2},            //"Bow, Short",
        { -7 ,-5 ,-3 ,-2 ,-1 ,-1 ,-1 ,0 ,0},            //"Thrown Club",
        { -1 ,0 ,+1 ,+2 ,+3 ,+3 ,+4 ,+4 ,+4},           //"Crossbow, Heavy",
        {-2 ,-1 ,0 ,0 ,+1 ,+2 ,+3 ,+3 ,+3 },            //"Crossbow, Light",
        { -5 ,-4 ,-3 ,-2 ,-1 ,-1 ,0 ,0 ,+1},            //"Thrown Dagger",
        { -5 ,-4 ,-3 ,-2 ,-1 ,0 ,+1 ,0 ,+1 },           //"Dart",
        { -2 ,-1 ,0 ,0 ,0 ,0 ,0 ,0 ,+1},                //"Thrown Hammer",
        { -5 ,-4 ,-3 ,-2 ,-1 ,0 ,+1 ,0 ,+1 },           //"Thrown Javelin",
        { -2 ,-2 ,-1 ,0 ,0 ,0 ,+2 ,+1 ,+3 },            //"Sling, Bullet",
        { -5 ,-4 ,-2 ,-1 ,0 ,0 ,+2 ,+1 ,+3},            //"Sling, Stone",
        { -3 ,-3 ,-2 ,-2 ,-1 ,0 ,0 ,0 ,0}               //"Thrown Spear"
    };

    int[,] clericDruidMonkToHit = {
        {25,25,24,23,23,22,21,21,20,20,20,20,20,20,20,20,20,20,19 },    //-10
        {24,24,23,22,22,21,20,20,20,20,20,20,20,20,20,20,19,19,18 },    //-9
        {23,23,22,21,21,20,20,20,20,20,20,20,20,20,19,19,18,18,17 },    //-8
        {22,22,21,20,20,20,20,20,20,20,20,20,19,19,18,18,17,17,16 },    //-7
        {21,21,20,20,20,20,20,20,20,20,20,19,18,18,17,17,16,16,15 },    //-6
        {20,20,20,20,20,20,20,20,20,19,19,18,17,17,16,16,15,15,14 },    //-5
        {20,20,20,20,20,20,20,20,19,18,18,17,16,16,15,15,14,14,13 },    //-4
        {20,20,20,20,20,20,19,19,18,17,17,16,15,15,14,14,13,13,12 },    //-3
        {20,20,20,20,20,19,18,18,17,16,16,15,14,14,13,13,12,12,11 },    //-2
        {20,20,20,19,19,18,17,17,16,15,15,14,13,13,12,12,11,11,10 },    //-1
        {20,20,19,18,18,17,16,16,15,14,14,13,12,12,11,11,10,10,9 },    //0
        {19,19,18,17,17,16,15,15,14,13,13,12,11,11,10,10,9,9,8 },    //1
        {18,18,17,16,16,15,14,14,13,12,12,11,10,10,9,9,8,8,7 },    //2
        {17,17,16,15,15,14,13,13,12,11,11,10,9,9,8,8,7,7,6 },    //3
        {16,16,15,14,14,13,12,12,11,10,10,9,8,8,7,7,6,6,5 },    //4
        {15,15,14,13,13,12,11,11,10,9,9,8,7,7,6,6,5,5,4 },    //5
        {14,14,13,12,12,11,10,10,9,8,8,7,6,6,5,5,4,4,3 },    //6
        {13,13,12,11,11,10,9,9,8,7,7,6,5,5,4,4,3,3,2 },    //7
        {12,12,11,10,10,9,8,8,7,6,6,5,4,4,3,3,2,2,1 },    //8
        {11,11,10,9,9,8,7,7,6,5,5,4,3,3,2,2,1,1,0 },    //9
        {10,10,9,8,8,7,6,6,5,4,4,3,2,2,1,1,0,0,-1 }     //10
    };

    int[,] theifAssassinToHit = {
        {26,26,25,25,24,24,23,22,21,21,20,20,20,20,20,20,20,20,20,20,20 },    //-10
        {25,25,24,24,23,23,22,21,20,20,20,20,20,20,20,20,20,20,20,20,19 },    //-9
        {24,24,23,23,22,22,21,20,20,20,20,20,20,20,20,20,20,20,19,19,18 },    //-8
        {23,23,22,22,21,21,20,20,20,20,20,20,20,20,20,20,19,19,18,18,17 },    //-7
        {22,22,21,21,20,20,20,20,20,20,20,20,20,20,19,19,18,18,17,17,16 },    //-6
        {21,21,20,20,20,20,20,20,20,20,20,20,19,19,18,18,17,17,16,16,15 },    //-5
        {20,20,20,20,20,20,20,20,20,20,19,19,18,18,17,17,16,16,15,15,14 },    //-4
        {20,20,20,20,20,20,20,20,19,19,18,18,17,17,16,16,15,15,14,14,13 },    //-3
        {20,20,20,20,20,20,20,19,18,18,17,17,16,16,15,15,14,14,13,13,12 },    //-2
        {20,20,20,20,20,20,19,18,17,17,16,16,15,15,14,14,13,13,12,12,11 },    //-1
        {20,20,20,20,19,19,18,17,16,16,15,15,14,14,13,13,12,12,11,11,10 },    //0
        {20,20,19,19,18,18,17,16,15,15,14,14,13,13,12,12,11,11,10,10,9 },    //1
        {19,19,18,18,17,17,16,15,14,14,13,13,12,12,11,11,10,10,9,9,8 },    //2
        {18,18,17,17,16,16,15,14,13,13,12,12,11,11,10,10,9,9,8,8,7 },    //3
        {17,17,16,16,15,15,14,13,12,12,11,11,10,10,9,9,8,8,7,7,6 },    //4
        {16,16,15,15,14,14,13,12,11,11,10,10,9,9,8,8,7,7,6,6,5 },    //5
        {15,15,14,14,13,13,12,11,10,10,9,9,8,8,7,7,6,6,5,5,4 },    //6
        {14,14,13,13,12,12,11,10,9,9,8,8,7,7,6,6,5,5,4,4,3 },    //7
        {13,13,12,12,11,11,10,9,8,8,7,7,6,6,5,5,4,4,3,3,2 },    //8
        {12,12,11,11,10,10,9,8,7,7,6,6,5,5,4,4,3,3,2,2,1 },    //9
        {11,11,10,10,9,9,8,7,6,6,5,5,4,4,3,3,2,2,1,1,0 }     //10
    };

    int[,] fighterPaladinRangerBardToHit = {
        {26,25,24,23,22,21,20,20,20,20,20,20,19,18,17,16,15,14 },    //-10
        {25,24,23,22,21,20,20,20,20,20,20,19,18,17,16,15,14,13 },    //-9
        {24,23,22,21,20,20,20,20,20,20,19,18,17,16,15,14,13,12 },    //-8
        {23,22,21,20,20,20,20,20,20,19,18,17,16,15,14,13,12,11 },    //-7
        {22,21,20,20,20,20,20,20,19,18,17,16,15,14,13,12,11,10 },    //-6
        {21,20,20,20,20,20,20,19,18,17,16,15,14,13,12,11,10,9 },    //-5
        {20,20,20,20,20,20,19,18,17,16,15,14,13,12,11,10,9,8 },    //-4
        {20,20,20,20,20,19,18,17,16,15,14,13,12,11,10,9,8,7 },    //-3
        {20,20,20,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6 },    //-2
        {20,20,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5 },    //-1
        {20,20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4 },    //0
        {20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3 },    //1
        {19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2 },    //2
        {18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1 },    //3
        {17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0 },    //4
        {16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0,-1 },    //5
        {15,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0,-1,-2 },    //6
        {14,13,12,11,10,9,8,7,6,5,4,3,2,1,0,-1,-2,-3 },    //7
        {13,12,11,10,9,8,7,6,5,4,3,2,1,0,-1,-2,-3,-4 },    //8
        {12,11,10,9,8,7,6,5,4,3,2,1,0,-1,-2,-3,-4,-5 },    //9
        {11,10,9,8,7,6,5,4,3,2,1,0,-1,-2,-3,-4,-5,-6 }     //10
    };

    int[,] magicUserIllusionistToHit = {
        {26,26,26,25,25,24,24,23,23,22,21,21,20,20,20,20,20,20,20,20,20 },    //-10
        {25,25,25,24,24,23,23,22,22,21,20,20,20,20,20,20,20,20,20,20,20 },    //-9
        {24,24,24,23,23,22,22,21,21,20,20,20,20,20,20,20,20,20,20,20,19 },    //-8
        {23,23,23,22,22,21,21,20,20,20,20,20,20,20,20,20,20,20,19,19,18 },    //-7
        {22,22,22,21,21,20,20,20,20,20,20,20,20,20,20,20,19,19,18,18,17 },    //-6
        {21,21,21,20,20,20,20,20,20,20,20,20,20,20,19,19,18,18,17,17,16 },    //-5
        {20,20,20,20,20,20,20,20,20,20,20,20,19,19,18,18,17,17,16,16,15 },    //-4
        {20,20,20,20,20,20,20,20,20,20,19,19,18,18,17,17,16,16,15,15,14 },    //-3
        {20,20,20,20,20,20,20,20,20,19,18,18,17,17,16,16,15,15,14,14,13 },    //-2
        {20,20,20,20,20,20,20,19,19,18,17,17,16,16,15,15,14,14,13,13,12 },    //-1
        {20,20,20,20,20,19,19,18,18,17,16,16,15,15,14,14,13,13,12,12,11 },    //0
        {20,20,20,19,19,18,18,17,17,16,15,15,14,14,13,13,12,12,11,11,10 },    //1
        {19,19,19,18,18,17,17,16,16,15,14,14,13,13,12,12,11,11,10,10,9 },    //2
        {18,18,18,17,17,16,16,15,15,14,13,13,12,12,11,11,10,10,9,9,8 },    //3
        {17,17,17,16,16,15,15,14,14,13,12,12,11,11,10,10,9,9,8,8,7 },    //4
        {16,16,16,15,15,14,14,13,13,12,11,11,10,10,9,9,8,8,7,7,6 },    //5
        {15,15,15,14,14,13,13,12,12,11,10,10,9,9,8,8,7,7,6,6,5 },    //6
        {14,14,14,13,13,12,12,11,11,10,9,9,8,8,7,7,6,6,5,5,4 },    //7
        {13,13,13,12,12,11,11,10,10,9,8,8,7,7,6,6,5,5,4,4,3 },    //8
        {12,12,12,11,11,10,10,9,9,8,7,7,6,6,5,5,4,4,3,3,2 },    //9
        {11,11,11,10,10,9,9,8,8,7,6,6,5,5,4,4,3,3,2,2,1 }     //10
    };

    int[,] monsterToHit = {
        {26,25,24,23,22,21,20,20,20,20,20,20,20,20,19,19,18,18,17 },    //-10
        {25,24,23,22,21,20,20,20,20,20,20,20,20,19,18,18,17,17,16 },    //-9
        {24,23,22,21,20,20,20,20,20,20,20,20,19,18,17,17,16,16,15 },    //-8
        {23,22,21,20,20,20,20,20,20,20,19,19,18,17,16,16,15,15,14 },    //-7
        {22,21,20,20,20,20,20,20,20,19,18,18,17,16,15,15,14,14,13 },    //-6
        {21,20,20,20,20,20,20,20,19,18,17,17,16,15,14,14,13,13,12 },    //-5
        {20,20,20,20,20,20,19,19,18,17,16,16,15,14,13,13,12,12,11 },    //-4
        {20,20,20,20,20,19,18,18,17,16,15,15,14,13,12,12,11,11,10 },    //-3
        {20,20,20,20,19,18,17,17,16,15,14,14,13,12,11,11,10,10,9 },    //-2
        {20,20,20,19,18,17,16,16,15,14,13,13,12,11,10,10,9,9,8 },    //-1
        {20,20,19,18,17,16,15,15,14,13,12,12,11,10,9,9,8,8,7 },    //0
        {20,19,18,17,16,15,14,14,13,12,11,11,10,9,8,8,7,7,6 },    //1
        {19,18,17,16,15,14,13,13,12,11,10,10,9,8,7,7,6,6,5 },    //2
        {18,17,16,15,14,13,12,12,11,10,9,9,8,7,6,6,5,5,4 },    //3
        {17,16,15,14,13,12,11,11,10,9,8,8,7,6,5,5,4,4,3 },    //4
        {16,15,14,13,12,11,10,10,9,8,7,7,6,5,4,4,3,3,2 },    //5
        {15,14,13,12,11,10,9,9,8,7,6,6,5,4,3,3,2,2,1 },    //6
        {14,13,12,11,10,9,8,8,7,6,5,5,4,3,2,2,1,1,0 },    //7
        {13,12,11,10,9,8,7,7,6,5,4,4,3,2,1,1,0,0,-1 },    //8
        {12,11,10,9,8,7,6,6,5,4,3,3,2,1,0,0,-1,-1,-2 },    //9
        {11,10,9,8,7,6,5,5,4,3,2,2,1,0,-1,-1,-2,-2,-3 }     //10
    };



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
    public void Initialize()
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
                profileDropdowns[i, j].onValueChanged.AddListener(delegate {  profileUpdate(pageIndex[m, n], slotIndex[m, n]);  });
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
                classDropdowns[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]);  });
                targetClassDropdowns[i, j] = GetChildWithName(slotTransforms[i, j], "targetClassDropdown").GetComponent<Dropdown>();
                targetClassLabels[i, j] = GetChildWithName(GetChildWithName(slotTransforms[i, j], "targetClassDropdown").GetComponent<Transform>(), "Label").GetComponent<Text>();
                targetClassDropdowns[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]);  });

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
                levelOrHitDieInputs[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]);  });
                targetLevelOrHitDieInputs[i, j] = GetChildWithName(slotTransforms[i, j], "targetLevelOrHitDie").GetComponent<InputField>();
                targetLevelOrHitDieInputs[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]);  });
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
                acTypeInputs[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]);  });
                targetAcTypeInputs[i, j] = GetChildWithName(slotTransforms[i, j], "targetACType").GetComponent<InputField>();
                targetAcTypeInputs[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]);  });
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
                acInputs[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]);  });
                targetAcInputs[i, j] = GetChildWithName(slotTransforms[i, j], "targetAC").GetComponent<InputField>();
                targetAcInputs[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]);  });
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
                weaponDropdowns[i, j].onValueChanged.AddListener(delegate {  updateWeaponDropdown(pageIndex[m, n], slotIndex[m, n],false); });
                targetWeaponDropdowns[i, j] = GetChildWithName(slotTransforms[i, j], "targetWeaponDropdown").GetComponent<Dropdown>();
                targetWeaponDropdowns[i, j].onValueChanged.AddListener(delegate {  updateWeaponDropdown(pageIndex[m, n], slotIndex[m, n],true);  });
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
                speedFactorInputs[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]); });
                targetSpeedFactorInputs[i, j] = GetChildWithName(slotTransforms[i, j], "targetSpeedFactorInput").GetComponent<InputField>();
                targetSpeedFactorInputs[i, j].onValueChanged.AddListener(delegate {  updateSlot(pageIndex[m, n], slotIndex[m, n]); });
                
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

        //Set default values
        for (int i = 0; i < pageCount; i++)
        {
            for(int j = 0; j < slotsPerPage; j++)
            {
                speedFactorInputs[i, j].text = "-";
                targetSpeedFactorInputs[i, j].text = "-";

                acInputs[i, j].text = "-";
                targetAcInputs[i, j].text = "-";

                acTypeInputs[i, j].text = "-";
                targetAcTypeInputs[i, j].text = "-";

                levelOrHitDieInputs[i, j].text = "-";
                targetLevelOrHitDieInputs[i, j].text = "-";
            }
        }

        //Setup exit buttons with listener functions
        exitButtons = new Button[pageCount];
        for (int i = 0; i < pageCount; i++)
        {
            exitButtons[i] = GetChildWithName(pageTransforms[i], "exitButton").GetComponent<Button>();
            exitButtons[i].onClick.AddListener(exitButtonPressed);
        }

        ChangeTextSize(12,10);

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
        transform.parent.GetComponent<Controller>().shutdown();
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
            minToHitTexts[page, slot].text = "-";
            attackTexts[page, slot].text = "-";
            targetMinToHitTexts[page, slot].text = "-";
            targetAttackTexts[page, slot].text = "-";

        }
        else
        {
            //Determine Number of attacks
            float targetAttacks = (float)(float.Parse(speedFactorInputs[page, slot].text)/ float.Parse(targetSpeedFactorInputs[page,slot].text));
            float playerAttacks = (float)(float.Parse(targetSpeedFactorInputs[page, slot].text) / float.Parse(speedFactorInputs[page, slot].text));
            if (playerAttacks < 1)
            {
                targetAttacks = 1/playerAttacks;
                playerAttacks = 1;
            }else if(targetAttacks < 1)
            {
                playerAttacks = 1/targetAttacks;
                targetAttacks = 1;
            }

            attackTexts[page,slot].text = ((int)playerAttacks).ToString();
            targetAttackTexts[page, slot].text = ((int)targetAttacks).ToString();

            //Determine Min to Hit for slot
            string classLabel = classLabels[page, slot].text;
            int weaponID = 0;
            for (int i = 0; i < weaponNames.Length; i++)
            {
                if (weaponLabels[page, slot].text == weaponNames[i])
                {
                    weaponID = i;
                    break;
                }

            }

            int targetAcType = int.Parse(targetAcTypeInputs[page, slot].text);

            int targetAC = int.Parse(targetAcInputs[page, slot].text);
            targetAC += weaponACAdjustments[weaponID, targetAcType - 2];
            if (targetAC > 10) targetAC = 10;
            if (targetAC < -10) targetAC = -10;

            int tabelVal = 0;
            int level = 0;
            if(classLabel == "Cleric" || classLabel == "Druid" || classLabel == "Monk")
            {
                level = int.Parse(levelOrHitDieInputs[page, slot].text) - 1;
                if (level > 18) level = 18;

                tabelVal = clericDruidMonkToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }else if (classLabel == "Magic-User" || classLabel == "Illusionist")
            {
                level = int.Parse(levelOrHitDieInputs[page, slot].text) - 1;
                if (level > 20) level = 20;
                tabelVal = magicUserIllusionistToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }else if (classLabel == "Fighter" || classLabel == "Paladin" || classLabel == "Ranger" || classLabel == "Bard")
            {
                level = int.Parse(levelOrHitDieInputs[page, slot].text);
                if (level > 17) level = 17;
                tabelVal = fighterPaladinRangerBardToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }else if (classLabel == "Theif" || classLabel == "Assassin")
            {
                level = int.Parse(levelOrHitDieInputs[page, slot].text)-1;
                if (level > 20) level = 20;
                tabelVal = theifAssassinToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }
            else
            {
                level = int.Parse(levelOrHitDieInputs[page, slot].text) + 2;
                if (level > 16) level = 16;
                tabelVal = monsterToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }



            minToHitTexts[page, slot].text = tabelVal.ToString();

            //Deterimine Min to hit for target
            classLabel = targetClassLabels[page, slot].text;
            weaponID = 0;
            for (int i = 0; i < weaponNames.Length; i++)
            {
                if (targetWeaponLabels[page, slot].text == weaponNames[i])
                {
                    weaponID = i;
                    break;
                }

            }

            targetAcType = int.Parse(acTypeInputs[page, slot].text);

            targetAC = int.Parse(acInputs[page, slot].text);
            targetAC += weaponACAdjustments[weaponID, targetAcType - 2];
            if (targetAC > 10) targetAC = 10;
            if (targetAC < -10) targetAC = -10;

            tabelVal = 0;
            level = 0;
            if (classLabel == "Cleric" || classLabel == "Druid" || classLabel == "Monk")
            {
                level = int.Parse(targetLevelOrHitDieInputs[page, slot].text) - 1;
                if (level > 18) level = 18;

                tabelVal = clericDruidMonkToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }
            else if (classLabel == "Magic-User" || classLabel == "Illusionist")
            {
                level = int.Parse(targetLevelOrHitDieInputs[page, slot].text) - 1;
                if (level > 20) level = 20;
                tabelVal = magicUserIllusionistToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }
            else if (classLabel == "Fighter" || classLabel == "Paladin" || classLabel == "Ranger" || classLabel == "Bard")
            {
                level = int.Parse(targetLevelOrHitDieInputs[page, slot].text);
                if (level > 17) level = 17;
                tabelVal = fighterPaladinRangerBardToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }
            else if (classLabel == "Theif" || classLabel == "Assassin")
            {
                level = int.Parse(targetLevelOrHitDieInputs[page, slot].text) - 1;
                if (level > 20) level = 20;
                tabelVal = theifAssassinToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }
            else
            {
                level = int.Parse(targetLevelOrHitDieInputs[page, slot].text) +2;
                if (level > 16) level = 16;
                tabelVal = monsterToHit[targetAC + 10, level];
                UnityEngine.Debug.Log("min to hit: " + tabelVal);
            }



            targetMinToHitTexts[page, slot].text = tabelVal.ToString();
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
                    GetChildWithName(GetChildWithName(slotTransforms[page, slot], "targetSpeedFactorInput").GetComponent<Transform>(), "Text").GetComponent<Text>().text = weaponSpeedFactors[i].ToString();
                    if (weaponNames[i] == "-")
                    {

                        targetSpeedFactorInputs[page, slot].text = "-";
                    }
                    break;
                }

            }
            else
            {
                if(weaponLabels[page,slot].text == weaponNames[i])
                {
                    speedFactorInputs[page,slot].text = weaponSpeedFactors[i].ToString();
                    GetChildWithName(GetChildWithName(slotTransforms[page, slot], "speedFactorInput").GetComponent<Transform>(), "Text").GetComponent<Text>().text = weaponSpeedFactors[i].ToString();
                    if (weaponNames[i] == "-")
                    {

                        speedFactorInputs[page, slot].text = "-";
                    }
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

    public void ChangeTextSize(int textSize, int smallText)
    {
        for(int i = 0; i < pageCount; i++)
        {
            for(int j = 0; j < slotsPerPage; j++)
            {
                profileLabels[i, j].fontSize = textSize;
                classLabels[i, j].fontSize = smallText;
                GetChildWithName(levelOrHitDieInputs[i, j].transform, "Placeholder").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(levelOrHitDieInputs[i, j].transform, "Text").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(acTypeInputs[i, j].transform, "Placeholder").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(acTypeInputs[i, j].transform, "Text").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(acInputs[i, j].transform, "Placeholder").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(acInputs[i, j].transform, "Text").GetComponent<Text>().fontSize = textSize;
                weaponLabels[i, j].fontSize = smallText;
                GetChildWithName(speedFactorInputs[i, j].transform, "Placeholder").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(speedFactorInputs[i, j].transform, "Text").GetComponent<Text>().fontSize = textSize;
                attackTexts[i, j].fontSize = textSize;
                minToHitTexts[i, j].fontSize = textSize;
                targetClassLabels[i, j].fontSize = smallText;
                GetChildWithName(targetLevelOrHitDieInputs[i, j].transform, "Placeholder").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(targetLevelOrHitDieInputs[i, j].transform, "Text").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(targetAcTypeInputs[i, j].transform, "Placeholder").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(targetAcTypeInputs[i, j].transform, "Text").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(targetAcInputs[i, j].transform, "Placeholder").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(targetAcInputs[i, j].transform, "Text").GetComponent<Text>().fontSize = textSize;
                targetWeaponLabels[i, j].fontSize = smallText;
                GetChildWithName(targetSpeedFactorInputs[i, j].transform, "Placeholder").GetComponent<Text>().fontSize = textSize;
                GetChildWithName(targetSpeedFactorInputs[i, j].transform, "Text").GetComponent<Text>().fontSize = textSize;
                targetAttackTexts[i, j].fontSize = textSize;
                targetMinToHitTexts[i, j].fontSize = textSize;
            }
        }
        
    }

}
