using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public int levelsCompleted = 0;

    private GameObject[] levels;

    private void Start()
    {
        levels = GameObject.FindGameObjectsWithTag("Level");
        int levelNumber = 1;
        foreach (GameObject level in levels)
        {
            level.GetComponent<Button>().onClick.AddListener(delegate { CompleteLevel(level); });
            level.GetComponent<Button>().onClick.AddListener(delegate { LevelCompleteSound(level); });
            level.transform.Find("LevelNumber").GetComponent<Text>().text = levelNumber.ToString();
            if (levelNumber > levelsCompleted+1)
            {
                level.transform.Find("LevelNumber").GetComponent<Text>().enabled = false;
                level.transform.Find("IsLocked").GetComponent<Image>().enabled = true;
            }
            levelNumber++;
        }
        GameObject.Find("LevelsMenu").SetActive(false);
    }

    private void LevelCompleteSound(GameObject levelNumber)
    {
        if (Convert.ToInt32(levelNumber.transform.Find("LevelNumber").GetComponent<Text>().text) == levelsCompleted)
        {
            GameObject.Find("CompleteLevelSound").GetComponent<AudioSource>().Play();
        }
    }

    private void CompleteLevel(GameObject button)
    {
        if (levelsCompleted + 1 == Convert.ToInt32(button.transform.Find("LevelNumber").GetComponent<Text>().text)) 
        {
            levelsCompleted += 1;
            button.GetComponent<Image>().color = new Color(0,0.8f,0,1);
            if (levelsCompleted < levels.Length)
            {
                levels[levelsCompleted].transform.Find("LevelNumber").GetComponent<Text>().enabled = true;
                levels[levelsCompleted].transform.Find("IsLocked").GetComponent<Image>().enabled = false;
            }
        }
    }
}
