using UnityEditor;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int currency = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        // Update UI here
    }

    public bool TryRemoveCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            return true;
        }
        return false;
    }
}
