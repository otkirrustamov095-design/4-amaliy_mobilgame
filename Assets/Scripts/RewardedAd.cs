using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Rewarded Ads (mukofotli reklama) ning oddiy simulyatsiyasi.
// Haqiqiy loyihada bu yerda Unity Ads / AdMob chaqiriladi,
// tamoyil esa bir xil: reklama TO'LIQ ko'rilsa -> bonus beriladi.
public class RewardedAd : MonoBehaviour
{
    public Button adButton;       // "Reklama ko'rish" tugmasi
    public GameObject adPanel;    // reklama oynasi
    public Text adText;           // reklama oynasidagi matn
    public float adDuration = 3f; // reklama davomiyligi (soniya)
    public int bonusCoins = 50;

    void Start()
    {
        adPanel.SetActive(false);
        adButton.onClick.AddListener(ShowAd);
    }

    public void ShowAd()
    {
        StartCoroutine(PlayAd());
    }

    IEnumerator PlayAd()
    {
        adButton.interactable = false;
        adPanel.SetActive(true);
        Time.timeScale = 0f; // reklama vaqtida o'yin to'xtaydi

        float timeLeft = adDuration;
        while (timeLeft > 0)
        {
            adText.text = "REKLAMA\n\nBu yerda reklama ko'rsatiladi...\n" + Mathf.CeilToInt(timeLeft) + " soniya";
            timeLeft -= Time.unscaledDeltaTime; // timeScale=0 bo'lgani uchun unscaled
            yield return null;
        }

        adPanel.SetActive(false);
        Time.timeScale = 1f;

        // Reklama tugadi -> bonus beramiz
        GameManager.Instance.GiveAdReward(bonusCoins);
        adButton.interactable = true;
    }
}
