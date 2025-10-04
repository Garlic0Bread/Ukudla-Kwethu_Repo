using System.Collections.Generic;
using UnityEngine;

public class Reciper_Manager : MonoBehaviour
{
    public static Reciper_Manager Instance;

    [System.Serializable]
    public class Recipe
    {
        public string recipeName;
        public List<string> requiredCropIDs;
        public bool unlocked = false;
    }

    [SerializeField] private List<Recipe> recipes;

    private void Awake()
    {
        Instance = this;
    }

    public void TryUnlockRecipe(Dictionary<string, int> storedCrops)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.unlocked) continue;

            bool allPresent = true;
            foreach (string cropID in recipe.requiredCropIDs)
            {
                if (!storedCrops.ContainsKey(cropID) || storedCrops[cropID] <= 0)
                {
                    allPresent = false;
                    break;
                }
            }

            if (allPresent)
            {
                recipe.unlocked = true;
                Debug.Log($"Unlocked recipe: {recipe.recipeName}");
            }
        }
    }
}
