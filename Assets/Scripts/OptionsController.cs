using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{

    Dropdown screenSizeDropdown;
    UnityEngine.UI.Text screenSizeLabel;
    UnityEngine.UI.Button exitButton;
    UnityEngine.UI.Toggle fullscreenToggle;
    public configStruct config;

    public struct configStruct
    {
        public int width;
        public int height;
        public bool fullscreen;
        public int textSize;
        public int smallTextSize;
    }

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
    public void Initialize(configStruct _config)
    {
        config = _config;
        screenSizeDropdown = GetChildWithName(transform, "screenSizeDropdown").GetComponent<Dropdown>();
        screenSizeLabel = GetChildWithName(GetChildWithName(transform, "screenSizeDropdown").GetComponent<Transform>(), "Label").GetComponent<Text>();
        fullscreenToggle = GetChildWithName(transform, "fullscreenToggle").GetComponent<Toggle>();
        exitButton = GetChildWithName(transform, "exitButton").GetComponent<Button>();


        screenSizeDropdown.onValueChanged.AddListener(delegate { updateScreen(); });
        fullscreenToggle.onValueChanged.AddListener(delegate { updateFullscreen(); });
        Screen.SetResolution(config.width, config.height, config.fullscreen);
        string f = string.Format("{0}x{1}", config.width, config.height);
        screenSizeLabel.text = f;
        fullscreenToggle.isOn = config.fullscreen;
        exitButton.onClick.AddListener(exitButtonPressed);
        GetChildWithName(transform.parent.transform, "CombatUI").GetComponent<CombatController>().ChangeTextSize(config.textSize, config.smallTextSize);
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
            config.width = 800;
            config.height = 600;
            config.textSize = 12;
            config.smallTextSize = 8;
        }
        if (screenSizeLabel.text == "1024x768")
        {
            config.width = 1024;
            config.height = 768;
            config.textSize = 12;
            config.smallTextSize = 8;
        }
        if (screenSizeLabel.text == "960x720")
        {
            config.width = 960;
            config.height = 720;
            config.textSize = 12;
            config.smallTextSize = 8;
        }
        if (screenSizeLabel.text == "1440x1080")
        {
            config.width = 1440;
            config.height = 1080;
            config.textSize = 10;
            config.smallTextSize = 10;
        }
        GetChildWithName(transform.parent.transform, "CombatUI").GetComponent<CombatController>().ChangeTextSize(config.textSize, config.smallTextSize);
        Screen.SetResolution(config.width, config.height, config.fullscreen);
    }

    void updateFullscreen()
    {
        if (fullscreenToggle.isOn)
        {
            config.fullscreen = true;
        }
        else
        {
            config.fullscreen = false;
        }
        Screen.SetResolution(config.width, config.height, config.fullscreen);
    }
    void exitButtonPressed()
    {
        transform.parent.GetComponent<Controller>().shutdown();
    }

}
