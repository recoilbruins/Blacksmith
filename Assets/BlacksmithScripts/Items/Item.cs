using UnityEngine;
using UnityEngine.UI;

public class Item: MonoBehaviour
{
    public Image uiSprite;
    public string itemName { get; set; } = string.Empty;

    public float itemWeight { get; set; } = 0f;

    public virtual void UseItem()
    {

    }

    public virtual void PickUpItem()
    {

    }

}
