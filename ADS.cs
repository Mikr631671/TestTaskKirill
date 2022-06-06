using UnityEngine;
using GoogleMobileAds.Api;

public class ADS : MonoBehaviour
{
    private InterstitialAd interstitialAdBlock;
    private string interstitialAdID = "ca-app-pub-3940256099942544/8691691433"; //test ID so that Google does not ban the account

    private void Awake()
    {
        gameObject.GetComponent<CubeManager>().TimeToShowAds += ShowAd;
    }

    private void OnEnable()
    {
        interstitialAdBlock = new InterstitialAd(interstitialAdID);
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAdBlock.LoadAd(adRequest);
    } //banner initialization

    private void ShowAd()
    {
        if (interstitialAdBlock.IsLoaded()) interstitialAdBlock.Show();
    }
}
