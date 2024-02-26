using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Ump.Api;

namespace Unicorn
{
    public class LoadingStartManager : MonoBehaviour
    {
        [SerializeField] CanvasGroup group;
        [SerializeField] private Image imgLoading;
        [SerializeField] private float timeLoading = 5;

        private AsyncOperation loadSceneAsync;
        private AppOpenAdManager appOpenAdManager;
        public static LoadingStartManager Instance { get; set; }

        [SerializeField] private PopupGDPR popupGDPR;

        private void Awake()
        {
            appOpenAdManager = AppOpenAdManager.Instance;
            Instance = this;
        }

        void Start()
        {
            MobileAds.RaiseAdEventsOnUnityMainThread = true;

            DontDestroyOnLoad(gameObject);
            LoadMasterLevel();

            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
            CheckGDPR();
        }

        public void Init()
        {
            Debug.Log("Init Splash");
            UnicornAdManager.Init();
            LoadAppOpen();
        
            RunLoadingBar();
        }

        private void LoadAppOpen()
        {
#if UNITY_EDITOR
            return;
#endif
            MobileAds.Initialize(initStatus => { appOpenAdManager.LoadAd(); });
        }

        private void LoadMasterLevel()
        {
            loadSceneAsync = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }

        private void RunLoadingBar()
        {
            imgLoading.DOFillAmount(0.9f, timeLoading)
                .SetEase(Ease.OutQuint)
                .OnComplete(() => { StartCoroutine(Fade()); });
        }

        private IEnumerator Fade()
        {
            appOpenAdManager.ShowAdIfAvailable();
            yield return new WaitForSeconds(1);

            yield return new WaitUntil(() => loadSceneAsync.isDone);
            imgLoading.DOFillAmount(1f, 0.1f);
            group.DOFade(0, 0.2f)
                .OnComplete(() => { Destroy(group.gameObject); });
        }



        private void OnAppStateChanged(AppState state)
        {
#if UNITY_EDITOR
            return;
#endif
            // Display the app open ad when the app is foregrounded.
            Debug.Log("App State is " + state);
            if (state == AppState.Foreground && group == null)
            {
                appOpenAdManager.ShowAdIfAvailable();
            }
        }

        #region  UMP SDK 
        private void CheckGDPR()
        {

            // Set tag for under age of consent.
            // Here false means users are not under age of consent.
            ConsentRequestParameters request = new ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,

            };

            Debug.Log("aaaaa ConsentInformation " + ConsentInformation.ConsentStatus.ToString());

            // Check the current consent information status.
            ConsentInformation.Update(request, OnConsentInfoUpdated);
        }

        void OnConsentInfoUpdated(FormError consentError)
        {
            if (consentError != null)
            {
                Init();
                // Handle the error.
                UnityEngine.Debug.LogError("UMP Error 1 " + consentError.Message);
                return;
            }

            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.
            ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
            {
                if (formError != null)
                {
                    Init();

                    // Consent gathering failed.
                    UnityEngine.Debug.LogError("UMP Error " + consentError.Message);
                    return;
                }

                // Consent has been gathered.

                if (ConsentInformation.CanRequestAds())
                {
                    Debug.Log("UMP Request Ads");
                    Init();

                  
                }
            });
        }

        #endregion

    }
}