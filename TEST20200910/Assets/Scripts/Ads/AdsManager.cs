using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{

    private const string appId = "ca-app-pub-9893767872798094~7765342202";
    private const string appIdTest = "ca-app-pub-3940256099942544~3347511713";
    private const string adId = "ca-app-pub-9893767872798094/5392645523";
    private const string adIdTest = "ca-app-pub-3940256099942544/2934735716";

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        MobileAds.Initialize(appId);

        RequestBanner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RequestBanner()
    {
#if DEBUG
        // Create a 320x50 banner at the top of the screen.
        BannerView bannerView = new BannerView(adIdTest, AdSize.Banner, AdPosition.Top);
#else
        // Create a 320x50 banner at the top of the screen.
        BannerView bannerView = new BannerView(adId, AdSize.Banner, AdPosition.Top);
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);

    }

    public void DisplayUnityAds()
    {
        if (Advertisement.IsReady())
            Advertisement.Show();
    }

    public bool isRewardAdReady()
    {
        return Advertisement.IsReady();
    }

    public bool isRewardAdShowing()
    {
        return Advertisement.isShowing;
    }
}
