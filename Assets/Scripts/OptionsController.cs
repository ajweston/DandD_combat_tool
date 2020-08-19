using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{

    Dropdown screenSizeDropdown;
    UnityEngine.UI.Text screenSizeLabel;
    UnityEngine.UI.Button exitButton;
    bool fullscreen;
    int width, height;

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
        screenSizeDropdown = GetChildWithName(transform, "screenSizeDropdown").GetComponent<Dropdown>();
        screenSizeLabel = GetChildWithName(GetChildWithName(transform, "screenSizeDropdown").GetComponent<Transform>(), "Label").GetComponent<Text>();

        screenSizeDropdown.onValueChanged.AddListener(delegate { updateScreen(); });
        fullscreen = false;

        width = 800;
        height = 600;
        Screen.SetResolution(width, height, fullscreen);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateScreen()
    {
        if (screenSizeLabel.text == "-") return;
        if (screenSizeLabel.text == "800x600")
        {
            Screen.SetResolution(800, 600, fullscreen);
            GetChildWithName(transform.parent.transform, "CombatUI").GetComponent<CombatController>().ChangeTextSize(12,8);
            return;
        }
        if (screenSizeLabel.text == "1024x768")
        {
            Screen.SetResolution(1024, 768, fullscreen);
            GetChildWithName(transform.parent.transform, "CombatUI").GetComponent<CombatController>().ChangeTextSize(12,8);
            return;
        }
        if (screenSizeLabel.text == "1280x720")
        {
            Screen.SetResolution(1280, 720, fullscreen);
            GetChildWithName(transform.parent.transform, "CombatUI").GetComponent<CombatController>().ChangeTextSize(12,8);
            return;
        }
        if (screenSizeLabel.text == "1440x1080")
        {
            Screen.SetResolution(1440, 1080, fullscreen);
            GetChildWithName(transform.parent.transform, "CombatUI").GetComponent<CombatController>().ChangeTextSize(10,10);
            return;
        }

    }
}
