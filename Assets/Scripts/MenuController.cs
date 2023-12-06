using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    private void Start()
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach(GameObject button in buttons)
        {
            button.GetComponent<Button>().onClick.AddListener(GameObject.Find("ClickSound").GetComponent<AudioSource>().Play);
        }
        GameObject.Find("SettingsMenu").SetActive(false);
    }

    public void Update()
    {
        if (GameObject.Find("TicketsAmount") != null)
        {
            GameObject.Find("TicketsAmount").GetComponent<Text>().text = PlayerPrefs.GetInt("money").ToString();
        }
        }

    public void ToggleColor(GameObject button)
    {
        Color buttonColor = button.GetComponentInChildren<Image>().color;
        bool needToEnable;
        if (buttonColor.r == 1)
        {
            button.GetComponentInChildren<Image>().color = new Color(0, 1, 0, 0.2f);
            needToEnable = true;
        }
        else
        {
            button.GetComponentInChildren<Image>().color = new Color(1, 0, 0, 0.2f);
            needToEnable = false;
        }
        if (button.name == "SoundButton")
        {
                GameObject.Find("BackGroundMusic").GetComponent<AudioSource>().enabled = needToEnable; 
        }
        else if (button.name == "VibrationButton")
        {
                GameObject.Find("ClickSound").GetComponent<AudioSource>().enabled = needToEnable;
                GameObject.Find("CompleteLevelSound").GetComponent<AudioSource>().enabled = needToEnable;
        }
        
    }
}
