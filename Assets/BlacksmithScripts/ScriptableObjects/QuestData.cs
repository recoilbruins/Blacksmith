using UnityEngine;

public enum QuestType { Kill, Craft, Deliver }

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest")]
public class QuestData : ScriptableObject
{
    public string questName;
    public QuestType questType;
    public string description;

    public ItemData requiredItem;      // For craft/deliver
    public int requiredAmount;

    public GameObject requiredEnemy;   // For kill
    public int killCount;

    public int rewardGold;
    public ItemData[] rewardItems;
    public int xpReward;

    public bool isRepeatable;
}