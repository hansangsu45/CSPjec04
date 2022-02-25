using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;
    public Text statusText;
    private float deltaTime;


    #region UNITY MONOBEHAVIOR METHODS

    public void Start()
    {
        /* 전체 화면 광고가 표시되는 동안 Unity 앱을 일시 중지 */
        //MobileAds.SetiOSAppPauseOnBackground(true);

        /* Simulator */
        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        /* Add some test device IDs (replace with your own device IDs).*/
#if UNITY_IPHONE
                            deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
        deviceIds.Add("75EF8D155528C04DACBBA6F36F433035");
#endif

        /*아동 및 가족을 포함한 전체 사용자를 타겟팅하는 앱 설정 및  test device IDs. 어뷰징으로 걸림 반드시 해줄것*/
        /* 어플 "Device ID"를 받아서 실행 https://play.google.com/store/apps/details?id=com.evozi.deviceid */
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
        /* MobileAds 정적 메서드를 통해 전역으로 적용될 타겟팅 정보를 수집하는 객체 : 아동용으로 처리하도록 설정 */
        MobileAds.SetRequestConfiguration(requestConfiguration);
        // Google 모바일 광고 SDK를 초기화합니다.
        MobileAds.Initialize(HandleInitCompleteAction);

    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        //sdk 초기화 여부
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            statusText.text = "Initialization complete";
            RequestBannerAd();
        });
    }
    #endregion

    #region HELPER METHODS
    /*admob를 설정했으며 이제는 내가 설정 한 키워드를 기반으로 관련 광고가 게재되기를 원합니다. */
    private AdRequest CreateAdRequest()
    {
        // .AddKeyword("unity-admob-sample")
        //
        return new AdRequest.Builder()
            .AddKeyword("game")
            .Build();
    }


    #endregion

    #region BANNER ADS

    /*베너 광고 호출 */
    public void RequestBannerAd()
    {
        statusText.text = "Requesting Banner Ad.";
        // 광고 ad units  id
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // 기존에 불러온 광고 정리
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // 베너 크기 : 테스트 아이디는 애드몹 테스트 아이디로 검색
        // 베너 광고 종류 : https://developers.google.com/admob/android/banner?hl=ko
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load a banner ad : 실제로 불러옴
        bannerView.LoadAd(CreateAdRequest());
    }

    /* 베너 광고 삭제 */
    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion


    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        statusText.text = "Requesting Interstitial Ad.";

#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // 전면광고 초기화 있으면 삭제
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
        interstitialAd = new InterstitialAd(adUnitId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        interstitialAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        interstitialAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        interstitialAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // 전면광고 로드
        interstitialAd.LoadAd(CreateAdRequest());
    }

    //전면광고 준비여부
    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            statusText.text = "Interstitial ad is not ready yet";
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion


    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        statusText.text = "Requesting Rewarded Ad.";
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        // 새로운 보상광고 객체 형성
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
        rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
        // 리워드 광고 보상
        //rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();
        rewardedAd.OnUserEarnedReward += (sender, args) =>
        {
            OnUserEarnedRewardEvent.Invoke();
            statusText.text = "reward get !!";
        };

        // 광고 초기화 하기
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        //보상형 광고가 아직 준비여부
        if (rewardedAd != null)
        {
            rewardedAd.Show();
            //다음 광고를 빨리 가지고 오기위해
            RequestAndLoadRewardedAd();
        }
        else
        {
            statusText.text = "Rewarded ad is not ready yet.";
        }
    }


    #endregion
}
