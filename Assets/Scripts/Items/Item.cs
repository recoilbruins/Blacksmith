using UnityEngine;
using UnityEngine.UI;

public class Item: MonoBehaviour
{
    public Image uiSprite;
    public string itemName { get; set; } = string.Empty;

    public virtual void UseItem()
    {

    }
}
