using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private float nextLeechTime = 0;
    private float timeOnPlatform = 0;
    private bool batteryOnPlatform = false;
    private bool IsFull => currentEnergyAmount >= maxEnergyToAbsorb;

    [SerializeField] private Image energySlider;

    public UnityEvent onPlatformFull;
    [SerializeField] private Battery battery;
    [SerializeField] private int maxEnergyToAbsorb;
    [SerializeField] private int currentEnergyAmount;
    [SerializeField] private int energyToReceive = 0;
    [SerializeField] private float currencyMultiplier = 1;
    
    [SerializeField] private float leechInterval;
    [SerializeField] private float timeBeforeStartLeeching;

    private void Update()
    {
        ChargeUp();
    }

    private void ChargeUp()
    {
        energySlider.fillAmount = (float)currentEnergyAmount / maxEnergyToAbsorb;

        if (!batteryOnPlatform || IsFull)
            return;

        timeOnPlatform += Time.deltaTime;

        if (timeOnPlatform >= timeBeforeStartLeeching && Time.time >= nextLeechTime)
        {
            if (battery.TryRemoveEnergy(energyToReceive))
            {
                currentEnergyAmount += energyToReceive;
                nextLeechTime = Time.time + leechInterval;

                int currencyEarned = (int)(energyToReceive * currencyMultiplier);
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
        if (other.CompareTag("Battery"))
        {
            batteryOnPlatform = true;
            timeOnPlatform = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            batteryOnPlatform = false;
            timeOnPlatform = 0f;
        }
    }
}

