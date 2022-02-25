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
        /* ��ü ȭ�� ���� ǥ�õǴ� ���� Unity ���� �Ͻ� ���� */
        //MobileAds.SetiOSAppPauseOnBackground(true);

        /* Simulator */
        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        /* Add some test device IDs (replace with your own device IDs).*/
#if UNITY_IPHONE
                            deviceIds.Add("96e23e80653bb28980d3f40beb58915c");
#elif UNITY_ANDROID
        deviceIds.Add("75EF8D155528C04DACBBA6F36F433035");
#endif

        /*�Ƶ� �� ������ ������ ��ü ����ڸ� Ÿ�����ϴ� �� ���� ��  test device IDs. ���¡���� �ɸ� �ݵ�� ���ٰ�*/
        /* ���� "Device ID"�� �޾Ƽ� ���� https://play.google.com/store/apps/details?id=com.evozi.deviceid */
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
        /* MobileAds ���� �޼��带 ���� �������� ����� Ÿ���� ������ �����ϴ� ��ü : �Ƶ������� ó���ϵ��� ���� */
        MobileAds.SetRequestConfiguration(requestConfiguration);
        // Google ����� ���� SDK�� �ʱ�ȭ�մϴ�.
        MobileAds.Initialize(HandleInitCompleteAction);

    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        //sdk �ʱ�ȭ ����
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            statusText.text = "Initialization complete";
            RequestBannerAd();
        });
    }
    #endregion

    #region HELPER METHODS
    /*admob�� ���������� ������ ���� ���� �� Ű���带 ������� ���� ���� ����Ǳ⸦ ���մϴ�. */
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

    /*���� ���� ȣ�� */
    public void RequestBannerAd()
    {
        statusText.text = "Requesting Banner Ad.";
        // ���� ad units  id
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // ������ �ҷ��� ���� ����
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // ���� ũ�� : �׽�Ʈ ���̵�� �ֵ�� �׽�Ʈ ���̵�� �˻�
        // ���� ���� ���� : https://developers.google.com/admob/android/banner?hl=ko
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        bannerView.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        bannerView.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        bannerView.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();

        // Load a banner ad : ������ �ҷ���
        bannerView.LoadAd(CreateAdRequest());
    }

    /* ���� ���� ���� */
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

        // ���鱤�� �ʱ�ȭ ������ ����
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

        // ���鱤�� �ε�
        interstitialAd.LoadAd(CreateAdRequest());
    }

    //���鱤�� �غ񿩺�
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

        // ���ο� ���󱤰� ��ü ����
        rewardedAd = new RewardedAd(adUnitId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) => OnAdLoadedEvent.Invoke();
        rewardedAd.OnAdFailedToLoad += (sender, args) => OnAdFailedToLoadEvent.Invoke();
        rewardedAd.OnAdOpening += (sender, args) => OnAdOpeningEvent.Invoke();
        rewardedAd.OnAdFailedToShow += (sender, args) => OnAdFailedToShowEvent.Invoke();
        rewardedAd.OnAdClosed += (sender, args) => OnAdClosedEvent.Invoke();
        // ������ ���� ����
        //rewardedAd.OnUserEarnedReward += (sender, args) => OnUserEarnedRewardEvent.Invoke();
        rewardedAd.OnUserEarnedReward += (sender, args) =>
        {
            OnUserEarnedRewardEvent.Invoke();
            statusText.text = "reward get !!";
        };

        // ���� �ʱ�ȭ �ϱ�
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        //������ ���� ���� �غ񿩺�
        if (rewardedAd != null)
        {
            rewardedAd.Show();
            //���� ���� ���� ������ ��������
            RequestAndLoadRewardedAd();
        }
        else
        {
            statusText.text = "Rewarded ad is not ready yet.";
        }
    }


    #endregion
}
