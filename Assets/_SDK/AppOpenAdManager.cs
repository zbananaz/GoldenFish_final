using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;

public class AppOpenAdManager
{
#if UNITY_ANDROID || UNITY_EDITOR
  
    private const string AD_UNIT_ID = "ca-app-pub-6336405384015455/1575816255";

#elif UNITY_IOS
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/5662855259";
#else
    private const string AD_UNIT_ID = "unexpected_platform";
#endif

    private static AppOpenAdManager instance;

    private AppOpenAd appOpenAd;

    private bool isShowingAd = false;
    private int numberRequest = 0;
    private bool isFirtShow = false;
    public bool IsShowAds = true;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable
    {
        get
        {
            return appOpenAd != null;
        }
    }

    public void LoadAd()
    {
        LoadAd(AD_UNIT_ID);
    }

    private void LoadAd(string id)
    {
        if (appOpenAd != null)
        {
            appOpenAd.Destroy();
            appOpenAd = null;
        }

        Debug.Log("Loading the app open ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        AppOpenAd.Load(id, adRequest,
         (AppOpenAd ad, LoadAdError error) =>
         {
             // if error is not null, the load request failed.
             if (error != null || ad == null)
             {
                 Debug.LogError("app open ad failed to load an ad " +
                                "with error : " + error);

              
                 return;
             }

             Debug.Log("App open ad loaded with response : "
                       + ad.GetResponseInfo());

             appOpenAd = ad;
           
         });


    }

    public void ShowAdIfAvailable()
    {
        if (PlayerDataManager.Instance.IsNoAds() || isShowingAd)
        {
            return;
        }

        if (!IsAdAvailable)
        {
            numberRequest = 0;
            LoadAd();

            return;
        }
        isFirtShow = true;

        appOpenAd.OnAdFullScreenContentClosed += HandleAdDidDismissFullScreenContent;
        appOpenAd.OnAdFullScreenContentFailed += HandleAdFailedToPresentFullScreenContent;
        appOpenAd.OnAdFullScreenContentOpened += HandleAdDidPresentFullScreenContent;
        appOpenAd.OnAdImpressionRecorded += HandleAdDidRecordImpression;
        appOpenAd.OnAdPaid += HandlePaidEvent;

        appOpenAd.Show();
    }

    private void HandleAdDidDismissFullScreenContent()
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        appOpenAd = null;
        isShowingAd = false;

        numberRequest = 0;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(AdError error)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", error.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        appOpenAd = null;

        numberRequest = 0;
        LoadAd();

    }

    private void HandleAdDidPresentFullScreenContent()
    {
        Debug.Log("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression()
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(AdValue adValue)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                adValue.CurrencyCode, adValue.Value);

        double rev = adValue.Value / 1000000f;
        // AnalyticsRevenueAds.SendEventAOA(rev);
    }

}
