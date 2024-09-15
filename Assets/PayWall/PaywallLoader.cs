using System.Collections;
using UnityEngine;

public class PaywallLoader : MonoBehaviour
{
    [SerializeField] private Paywall paywallPrefab;
    private Paywall paywallInstance;

    private float pastTimeScale;

    private void Awake()
    {
        pastTimeScale = Time.timeScale;
        DontDestroyOnLoad(gameObject);

        string subStatus = PlayerPrefs.GetString("subscription");
        
        if (string.IsNullOrEmpty(subStatus) || subStatus == "false")
        {
            CreatePaywall();
        }
    }

    private void CreatePaywall()
    {
        paywallInstance = Instantiate(paywallPrefab, gameObject.transform);

        paywallInstance.onPaywallClosed += () =>
        {
            Debug.Log("Closing paywal...");
            Time.timeScale = pastTimeScale;
            StartCoroutine(Timer());
            paywallInstance = null;
        };
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(120);

        pastTimeScale = Time.timeScale;
        Time.timeScale = 0;
        CreatePaywall();
    }
}