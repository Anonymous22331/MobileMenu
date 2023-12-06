using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Asset/ShopItem")]
public class ShopItem : ScriptableObject
{
    public string itemName;
    public float itemCost;
    public int levelRequired;
    public Sprite itemSprite;
    public bool isForTickets;
    public bool isAbleForPurchase;
    public bool isBought;
}
