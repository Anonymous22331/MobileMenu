using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonus : MonoBehaviour
{
   [SerializeField] private int[] rewards = new int[7];

    private bool isRewardAvailable;
    private int maxStreak = 7;
    private int currentStreak { get => PlayerPrefs.GetInt("currentStreak", 0); set => PlayerPrefs.SetInt("currentStreak", value); }
    private DateTime? lastClaimTime { 
        get 
        { 
            string data = PlayerPrefs.GetString("lastClaimTime", null);
            if (!string.IsNullOrEmpty(data))
            {
                return DateTime.Parse(data);
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                PlayerPrefs.SetString("lastClaimTime", value.ToString());
            }
            else
            {
                PlayerPrefs.DeleteKey("lastClaimTime");
            }
        }
    }
    [SerializeField] private float dailyCooldown = 24f;
    private float loseStreakTime = 48f;

    private void Start()
    {
        StartCoroutine(RewardTimeUpdater());
        UpdateRewards();
        GameObject.Find("DailyMenu").SetActive(false);
        GameObject.Find("DailyMenuGet").SetActive(false);
    }

    private void Update()
    {
        if (GameObject.Find("DailyMenu") != null)
        {
            UpdateUI();
        }
    }

    private void UpdateRewards()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject.Find("TicketDay" + (i + 1).ToString() + "/TicketImage/TicketAmount").GetComponent<Text>().text = "X" + rewards[i];
        }

    }

    private IEnumerator RewardTimeUpdater()
    {
        while (true)
        {
            if (GameObject.Find("DailyMenu") != null)
            {
                UpdateRewardStatus();
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void UpdateRewardStatus()
    {
        isRewardAvailable = true;
        if (lastClaimTime.HasValue)
        {
            var deltaTime = DateTime.UtcNow - lastClaimTime.Value;
            if (deltaTime.TotalHours > loseStreakTime)
            {
                lastClaimTime = null;
                currentStreak = 0;
            } 
            else if (deltaTime.TotalHours < dailyCooldown)
            {
                isRewardAvailable = false;
            }
        }
    }

    private void UpdateUI()
    {
        GameObject timerField = GameObject.Find("DailyBonusTimer");

        if (isRewardAvailable)
        {
            timerField.GetComponent<Text>().text = "Available";
        }
        else
        {
            var nextDailyBonus = lastClaimTime.Value.AddHours(dailyCooldown);

            timerField.GetComponent<Text>().text = "Next in: " + ((nextDailyBonus - DateTime.UtcNow).Hours).ToString()+ "Hours";
        }
        GameObject.Find("WeekSlider").GetComponent<Slider>().value = currentStreak;
        GameObject.Find("DaysCountText").GetComponent<Text>().text = currentStreak.ToString() + "/7";
        for (int i = 1; i <= currentStreak; i++)
        {
            GameObject.Find("TicketDay" + (i).ToString()).GetComponent<Image>().color = new Color(0, 0.8f, 0, 1);
        }
    }

    public void TryClaimReward()
    {
        if (!isRewardAvailable)
        {
            GameObject.Find("DailyMenuGet").SetActive(false);
            return;
        }
        if (currentStreak == 6)
        {
            for (int i = 1; i <= currentStreak; i++)
            {
                GameObject.Find("TicketDay" + (i).ToString()).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
        GameObject.Find("DailyMenu").SetActive(false);
        GameObject.Find("GiftText").GetComponent<Text>().text = "Day" + (currentStreak+1).ToString();
        GameObject.Find("TicketsGetAmount").GetComponent<Text>().text = rewards[currentStreak].ToString();
        int reward = rewards[currentStreak];
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money")+reward);
        lastClaimTime = DateTime.UtcNow;
        
        currentStreak = (currentStreak + 1) % maxStreak;
    }
}
