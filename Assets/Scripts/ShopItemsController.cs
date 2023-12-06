using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class ShopItemsController : MonoBehaviour
{
    [SerializeField] ShopItem[] items;
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject containerBlock;

    private bool wasInitialized = false;
    private int levelsCompleted;
    private List<GameObject> shopBlocks = new List<GameObject>();

    private void Start()
    {
        LoadStore();
        GameObject.Find("ShopMenu").SetActive(false);
    }

    public void LoadStore()
    {
        levelsCompleted = GameObject.Find("LevelController").GetComponent<LevelController>().levelsCompleted;
        GameObject.Find("TicketsAmount").GetComponent<Text>().text = PlayerPrefs.GetInt("money").ToString();
        int shopBlockIndex = 0;
        foreach (ShopItem item in items)
        {
            if (!wasInitialized)
            {
                FirstInitialization(item);
            }
            // Set Currency of item
            string currency = ReturnCurrency(item);
            // Настройка IAP
            SetItemPreferences(item, shopBlocks[shopBlockIndex], currency);
            shopBlockIndex++;
        }
        wasInitialized = true;
    }

    private void FirstInitialization(ShopItem item)
    {
        GameObject shopBlock = Instantiate(blockPrefab);
        shopBlock.transform.SetParent(containerBlock.transform);
        shopBlock.transform.Find("ItemName").GetComponent<Text>().text = item.itemName;
        if (item.itemName == "Epic chest")
        {
            shopBlock.transform.Find("CostBackground").GetComponent<CodelessIAPButton>().productId = "epicChest";
            shopBlock.transform.Find("CostBackground").GetComponent<CodelessIAPButton>().enabled = true;
        }
        else if (item.itemName == "Lucky chest")
        {
            shopBlock.transform.Find("CostBackground").GetComponent<CodelessIAPButton>().productId = "luckyChest";
            shopBlock.transform.Find("CostBackground").GetComponent<CodelessIAPButton>().enabled = true;
        }
        else
        {
            shopBlock.transform.Find("CostBackground").GetComponent<Button>().onClick.AddListener(delegate { ItemPurchase(item, shopBlock); });
        }
        shopBlock.transform.localScale = new Vector3(1, 1, 1);
        shopBlocks.Add(shopBlock);
    }

    private string ReturnCurrency(ShopItem item)
    {
        if (item.isForTickets)
        {
            return "T";
        }
        else
        {
            return "$";
        }
    }

    private void SetItemPreferences(ShopItem item, GameObject shopBlock, string currency)
    {
        if (item.isBought)
        {
            shopBlock.transform.Find("IconBackground/ItemIcon").GetComponent<Image>().sprite = item.itemSprite;
            shopBlock.transform.Find("CostBackground/CostText").GetComponent<Text>().enabled = false;
            shopBlock.transform.Find("IconBackground/LevelRequiredText").GetComponent<Text>().enabled = false;
            shopBlock.transform.Find("CostBackground/IsBought").GetComponent<Image>().enabled = true;
        }
        else if (item.isAbleForPurchase || levelsCompleted >= item.levelRequired)
        {
            shopBlock.transform.Find("IconBackground/ItemIcon").GetComponent<Image>().sprite = item.itemSprite;
            shopBlock.transform.Find("IconBackground/LevelRequiredText").GetComponent<Text>().enabled = false;
            shopBlock.transform.Find("CostBackground/CostText").GetComponent<Text>().text = item.itemCost.ToString() + currency;
        }
        else
        {
            shopBlock.transform.Find("IconBackground/ItemIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/locked");
            shopBlock.transform.Find("IconBackground/LevelRequiredText").GetComponent<Text>().enabled = true;
            shopBlock.transform.Find("IconBackground/LevelRequiredText").GetComponent<Text>().text = "Level: " + item.levelRequired.ToString();
            shopBlock.transform.Find("CostBackground/CostText").GetComponent<Text>().text = item.itemCost.ToString() + currency;
        }
    }

    public void OnPurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "epicChest":
                {
                    AddTickets(500);
                    break;
                }
            case "luckyChest":
                {
                    AddTickets(1500);
                    break;
                }
        }
    }

    private void AddTickets(int value)
    {
        PlayerPrefs.SetInt("money", Convert.ToInt32(PlayerPrefs.GetInt("money") + value));
    }
    private void ItemPurchase(ShopItem item, GameObject shopBlock)
    {
        if (PlayerPrefs.GetInt("money") > item.itemCost && (item.isAbleForPurchase || levelsCompleted >= item.levelRequired))
        {
            PlayerPrefs.SetInt("money", Convert.ToInt32(PlayerPrefs.GetInt("money") - item.itemCost));
            shopBlock.transform.Find("CostBackground").GetComponent<Button>().enabled = false;
            shopBlock.transform.Find("CostBackground/CostText").GetComponent<Text>().enabled = false;
            shopBlock.transform.Find("CostBackground/IsBought").GetComponent<Image>().enabled = true;
        }
    }

}
