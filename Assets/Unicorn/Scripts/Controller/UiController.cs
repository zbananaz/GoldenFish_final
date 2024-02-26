using Unicorn.UI.Shop;
using UnityEngine;

namespace Unicorn.UI
{
    public class UiController : MonoBehaviour
    {
        public UiMainLobby UiMainLobby;
        public UiLose UiLose;
        public UiWin UiWin;
        public ShopCharacter ShopCharacter;

        public PopupRewardEndGame PopupRewardEndGame;
        public PopupChestKey PopupChestKey;
        public LuckyWheel LuckyWheel;
        public GameObject Loading;

        public void Init()
        {
            ShopCharacter.Configure(
                PlayerDataManager.Instance,
                PlayerDataManager.Instance.DataTextureSkin);
        }

        public void OpenUiLose()
        {
            UiLose.Show(true);
        }

        public void OpenUiWin(int gold)
        {
            var UiWin = GameManager.Instance.UiController.UiWin;
            UiWin.Show(true);
            UiWin.Init(gold);
        }

        public void OpenShopCharacter()
        {
            ShopCharacter.Show(true);
        }

        public void OpenPopupReward(RewardEndGame reward, TypeDialogReward type)
        {
            if (PopupRewardEndGame.IsShow)
                return;

            PopupRewardEndGame.Show(true);
            PopupRewardEndGame.Init(reward, type);
        }

        public void OpenPopupChestKey(RewardEndGame reward)
        {
            PopupChestKey.Show(true);
            PopupChestKey.Init(reward);
        }

        public void OpenLuckyWheel()
        {
            LuckyWheel.Show(true);
        }

        public void OpenLoading(bool isLoading)
        {
            Loading.SetActive(isLoading);
        }
    }
}

