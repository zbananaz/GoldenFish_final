using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using Unicorn.Utilities;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Unicorn.UI
{
    public class UiMainLobby : UICanvas
    {
        [Title("Button")] [SerializeField] private Button btnHide;
        [SerializeField] private Button btnLuckySpin;
        [SerializeField] private Button btnSkin;
        [SerializeField] private Button btnNoAds;
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnGift;

        [Title("Others")] [SerializeField] private List<Image> listImgKeys;
        [SerializeField] private TextMeshProUGUI txtCurentLevel;

        public Button BtnNoAds
        {
            get => btnNoAds;
        }

        public Button BtnPlay => btnHide;
        private bool isFistOpen;

        // Start is called before the first frame update
        void Start()
        {
            btnHide.onClick.AddListener(OnClickBtnPlay);
            btnLuckySpin.onClick.AddListener(OnClickBtnSpin);
            btnSkin.onClick.AddListener(OnClickShopSkin);
            btnNoAds.onClick.AddListener(OnClickBtnNoAds);
            btnSetting.onClick.AddListener(OnClickBtnSetting);
            btnGift.onClick.AddListener(OnClickBtnGift);
            Init();
        }

        private void Init()
        {
            SetLayoutKey();
            var dataLevel = GameManager.Instance.DataLevel;
            txtCurentLevel.text = $"Level {dataLevel.DisplayLevel}";
            BtnPlay.gameObject.SetActive(true);
            if (PlayerDataManager.Instance.IsNoAds())
            {
                btnNoAds.interactable = false;
            }
        }


        private void SetLayoutKey()
        {
            for (int i = 0; i < listImgKeys.Count; i++)
            {
                listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(false);
            }

            int countKey = GameManager.Instance.Profile.GetKey() > 3 ? 3 : GameManager.Instance.Profile.GetKey();
            for (int i = 0; i < countKey; i++)
            {
                listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(true);
            }
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (IsShow)
            {
                Init();
                btnGift.gameObject.SetActive(IsShowGift());
            }
            else
            {
                BtnPlay.gameObject.SetActive(false);
            }
        }


        private void OnClickBtnPlay()
        {
            GameManager.Instance.StartLevel();
            ShowAniHide();

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnSpin()
        {
            GameManager.Instance.UiController.OpenLuckyWheel();

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickShopSkin()
        {
            GameManager.Instance.UiController.OpenShopCharacter();

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnNoAds()
        {
            GameManager.Instance.IapController.PurchaseProduct((int)IdPack.NO_ADS_BASIC);
            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnSetting()
        {
            SoundManager.Instance.PlaySoundButton();
        }

        public void ShowAniHide()
        {
            Show(false);
        }

        public void ActiveMainLobby()
        {
            Show(true);
        }

        public void Hack()
        {
            GameManager.Instance.Profile.AddGold(100000, "");
        }

        private void OnClickBtnGift()
        {
#if UNITY_EDITOR
            OnRewardGift();

#else
       
        WaitingCanvas.Instance.Show("");
        Observable.FromCoroutine(ActionWatchVideo).Subscribe().AddTo(this.gameObject);
#endif
        }

        private IEnumerator ActionWatchVideo()
        {
            float _tmp1 = 0;
            float _tmp2 = 1;
            RewardAdStatus _rewardAdStatus = RewardAdStatus.NoVideoNoInterstitialReward;
            while (_tmp1 < 2)
            {
                _tmp1 += Time.deltaTime;
                _tmp2 += Time.deltaTime;
                if (_tmp2 > 0.5f)
                {
                    _tmp2 = 0;
                    _rewardAdStatus =
                        AdManager.Instance.ShowAdsReward(OnRewardGift, Helper.video_reward_gift_box, false);
                    switch (_rewardAdStatus)
                    {
                        case RewardAdStatus.NoInternet:
                            WaitingCanvas.Instance.Hide();
                            PopupDialogCanvas.Instance.Show("No Internet!");

                            Analytics.LogEventByName("Monetize_reward_no_internet");
                            Analytics.LogEventByName("Monetize_interstitial_no_internet");
                            yield break;
                        case RewardAdStatus.NoVideoNoInterstitialReward:
                            break;
                        default:
                            WaitingCanvas.Instance.Hide();
                            yield break;
                    }
                }

                yield return null;
            }

            if (_rewardAdStatus == RewardAdStatus.NoVideoNoInterstitialReward)
            {
                WaitingCanvas.Instance.Hide();
                PopupDialogCanvas.Instance.Show("No Video!");
                Analytics.LogEventByName("Monetize_no_reward");
                Analytics.LogEventByName("Monetize_no_reward_no_interstitial");
            }
        }

        private void OnRewardGift()
        {
            PlayerDataManager.Instance.SetTimeLoginOpenGift(DateTime.Now.ToString());
            btnGift.gameObject.SetActive(false);
            if (isFistOpen)
            {
                isFistOpen = false;
                int idSkin = GetIdSkin();
                int idHat = GetIdHat();
                if (idSkin != -1)
                {
                    RewardEndGame reward = new RewardEndGame();
                    reward.Type = TypeEquipment.Skin;
                    reward.Id = idSkin;

                    GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                    PlayerDataManager.Instance.SetUnlockSkin(TypeEquipment.Skin, idSkin);
                }
                else if (idHat != -1)
                {
                    RewardEndGame reward = new RewardEndGame();
                    reward.Type = TypeEquipment.Hat;
                    reward.Id = idHat;

                    GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                    PlayerDataManager.Instance.SetUnlockSkin(TypeEquipment.Hat, reward.Id);
                }
                else
                {
                    int gold = Random.Range(200, 1000);
                    GameManager.Instance.Profile.AddGold(gold, "gift_box");

                    SoundManager.Instance.PlaySoundReward();
                }


            }
            else
            {
                var rd = Random.Range(0, 100);
                if (rd < 80)
                {
                    int gold = Random.Range(200, 1000);
                    GameManager.Instance.Profile.AddGold(gold, "gift_box");

                    SoundManager.Instance.PlaySoundReward();
                }
                else
                {
                    int idSkin = GetIdSkin();
                    int idHat = GetIdHat();
                    if (idSkin != -1)
                    {
                        RewardEndGame reward = new RewardEndGame();
                        reward.Type = TypeEquipment.Skin;
                        reward.Id = idSkin;

                        GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                        PlayerDataManager.Instance.SetUnlockSkin(TypeEquipment.Skin, reward.Id);
                    }
                    else if (idHat != -1)
                    {
                        RewardEndGame reward = new RewardEndGame();
                        reward.Type = TypeEquipment.Hat;
                        reward.Id = idHat;

                        GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                        PlayerDataManager.Instance.SetUnlockSkin(TypeEquipment.Hat, idHat);
                    }
                    else
                    {
                        int gold = Random.Range(200, 1000);
                        GameManager.Instance.Profile.AddGold(gold, "gift_box");

                        SoundManager.Instance.PlaySoundReward();
                    }
                }
            }

        }

        private int GetIdSkin()
        {
            var data = GameManager.Instance.UiController.ShopCharacter.DataShopSkin.GetDataShop(TypeEquipment.Skin);
            List<int> listId = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                if (!PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.Skin, data[i].idSkin) &&
                    !data[i].typeUnlock.HasFlag(TypeUnlockSkin.SPIN))
                {
                    listId.Add(data[i].idSkin);
                }
            }

            if (listId.Count > 0)
            {
                int index = Random.Range(0, listId.Count);
                return listId[index];


            }
            else
            {
                return -1;
            }
        }

        private int GetIdHat()
        {
            var data = GameManager.Instance.UiController.ShopCharacter.DataShopSkin.GetDataShop(TypeEquipment.Hat);
            List<int> listId = new List<int>();
            for (int i = 0; i < data.Count; i++)
            {
                if (!PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.Hat, data[i].idSkin) &&
                    !data[i].typeUnlock.HasFlag(TypeUnlockSkin.SPIN))
                {
                    listId.Add(data[i].idSkin);
                }
            }

            if (listId.Count > 0)
            {
                int index = Random.Range(0, listId.Count);
                return listId[index];


            }
            else
            {
                return -1;
            }
        }

        private bool IsShowGift()
        {

            string time = PlayerDataManager.Instance.GetTimeLoginOpenGift();
            if (string.IsNullOrEmpty(time))
            {
                isFistOpen = true;

                return true;
            }


            DateTime timeLogin = DateTime.Parse(time);

            long tickTimeNow = DateTime.Now.Ticks;
            long tickTimeOld = timeLogin.Ticks;

            long elapsedTicks = tickTimeNow - tickTimeOld;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            float totalSeconds = (float)elapsedSpan.TotalSeconds;

            var totalTimeReset = 600 - totalSeconds;
            if (totalTimeReset <= 0)
            {
                PlayerDataManager.Instance.SetTimeLoginOpenGift(DateTime.Now.ToString());
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
