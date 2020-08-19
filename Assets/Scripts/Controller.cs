using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Transform trans;
    GameObject CombatUI;
    GameObject OptionsUI;
    System.Collections.Generic.List<GameObject> menus;

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

        trans = GetComponent<Transform>();
        CombatUI = GetChildWithName(trans, "CombatUI");
        OptionsUI = GetChildWithName(trans, "OptionsUI");
        OptionsUI.SetActive(false);
        CombatUI.SetActive(true);
        activeMenu = 1;
        menus = new System.Collections.Generic.List<GameObject>();
        menus.Add(OptionsUI);
        menus.Add(CombatUI);

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
}
