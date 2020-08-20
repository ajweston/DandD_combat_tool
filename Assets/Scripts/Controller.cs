using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IniParser;
using IniParser.Model;
using System.Diagnostics;

public class Controller : MonoBehaviour
{
    Transform trans;
    GameObject CombatUI;
    GameObject OptionsUI;

    //configuration structs
    OptionsController.configStruct optionsConfig;

    System.Collections.Generic.List<GameObject> menus;

    FileIniDataParser parser;
    IniData data;

    int activeMenu;
    int returnMenu;

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
        parser = new FileIniDataParser();
        data = parser.ReadFile("./config/config.ini");

        fillConfigStructs();

        trans = GetComponent<Transform>();
        CombatUI = GetChildWithName(trans, "CombatUI");
        OptionsUI = GetChildWithName(trans, "OptionsUI");
        menus = new System.Collections.Generic.List<GameObject>();
        menus.Add(OptionsUI);
        menus.Add(CombatUI);

        CombatUI.GetComponent<CombatController>().Initialize();
        OptionsUI.GetComponent<OptionsController>().Initialize(optionsConfig);

        OptionsUI.SetActive(false);
        CombatUI.SetActive(true);
        activeMenu = 1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(activeMenu == 0)
            {
                activeMenu = returnMenu;
                menus[0].SetActive(false);
                menus[activeMenu].SetActive(true);
            }
            else
            {
                returnMenu = activeMenu;
                menus[activeMenu].SetActive(false);
                activeMenu = 0;
                menus[activeMenu].SetActive(true);
            }
        }else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            menus[activeMenu--].SetActive(false);
            if(activeMenu < 1)
            {
                activeMenu = menus.Count-1;
            }
            menus[activeMenu].SetActive(true);
        }else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            menus[activeMenu++].SetActive(false);
            if (activeMenu > menus.Count-1)
            {
                activeMenu = 1;
            }
            menus[activeMenu].SetActive(true);
        }
    }

    void fillConfigStructs()
    {
        optionsConfig.width = int.Parse(data["Display"]["width"]);
        optionsConfig.height = int.Parse(data["Display"]["height"]);
        optionsConfig.fullscreen = bool.Parse(data["Display"]["fullscreen"]);
        optionsConfig.textSize = int.Parse(data["CombatUI"]["textSize"]);
        optionsConfig.textSize = int.Parse(data["CombatUI"]["smallTextSize"]);
    }

    public void shutdown()
    {
        //Update config ini
        optionsConfig = GetChildWithName(transform,"OptionsUI").GetComponent<OptionsController>().config;
        UnityEngine.Debug.Log(optionsConfig.width + "x" + optionsConfig.height);
        data["Display"]["width"] = optionsConfig.width.ToString();
        data["Display"]["height"] = optionsConfig.height.ToString();
        data["Display"]["fullscreen"] = optionsConfig.fullscreen.ToString();
        data["CombatUI"]["textSize"] = optionsConfig.textSize.ToString();
        data["CombatUI"]["smallTextSize"] = optionsConfig.smallTextSize.ToString();

        parser.WriteFile("./config/config.ini", data);
        Application.Quit();
    }
}
