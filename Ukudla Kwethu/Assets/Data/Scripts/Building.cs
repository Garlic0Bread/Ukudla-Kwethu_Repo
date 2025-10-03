using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private float nextLeechTime = 0;
    private float timeOnPlatform = 0;
    private bool basketOnPlatform = false;
    private bool IsFull => currentFoodAmount >= maxFoodToCollect;

    [SerializeField] private Image foodCollectedSlider;

    public UnityEvent onPlatformFull;
    [SerializeField] private Battery battery;
    [SerializeField] private int maxFoodToCollect;
    [SerializeField] private int currentFoodAmount;
    [SerializeField] private int foodToReceive = 0;
    [SerializeField] private float currencyMultiplier = 1;
    
    [SerializeField] private float leechInterval;
    [SerializeField] private float timeBeforeStartLeeching;

    private void Update()
    {
        ChargeUp();
    }

    private void ChargeUp()
    {
        //foodCollectedSlider.fillAmount = (float)currentFoodAmount / maxFoodToCollect;

        if (!basketOnPlatform || IsFull)
            return;

        timeOnPlatform += Time.deltaTime;

        if (timeOnPlatform >= timeBeforeStartLeeching && Time.time >= nextLeechTime)
        {
            if (battery.TryRemoveEnergy(foodToReceive))
            {
                currentFoodAmount += foodToReceive;
                nextLeechTime = Time.time + leechInterval;

                int currencyEarned = (int)(foodToReceive * currencyMultiplier);
                CurrencyManager.Instance.AddCurrency(currencyEarned);

                if (IsFull)
                {
                    onPlatformFull?.Invoke();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basket"))
        {
            basketOnPlatform = true;
            timeOnPlatform = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Basket"))
        {
            basketOnPlatform = false;
            timeOnPlatform = 0f;
        }
    }
}

