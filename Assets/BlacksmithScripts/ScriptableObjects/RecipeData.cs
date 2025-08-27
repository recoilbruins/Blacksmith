using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Crafting/Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public ItemData outputItem;
    public int outputAmount = 1;

    [System.Serializable]
    public struct Ingredient
    {
        public ItemData item;
        public int amount;
    }

    public Ingredient[] ingredients;
    public float craftingTime = 2f;
    public int requiredToolLevel = 0;
}
