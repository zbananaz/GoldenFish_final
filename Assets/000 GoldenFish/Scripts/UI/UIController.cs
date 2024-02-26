using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unicorn
{
    public class UIController : UICanvas
    {
        [Title("Buttons")] 
        [SerializeField] private Button btnTouchToPlay;
        [SerializeField] private Button btnGuppy;
        [SerializeField] private Button btnKingGuppy;
        [SerializeField] private Button btnCanivore;
        [SerializeField] private Button btnBetleMuncher;
        [SerializeField] private Button btnHome;
        [SerializeField] private Button btnResume;
        [SerializeField] private Button btnHomWin;
        [SerializeField] private Button btnHomeLose;
        
        
        [SerializeField] private Button btnSetting;
        // Start is called before the first frame update

        [Title("UI Objects")] 
        [SerializeField] private GameObject topLeftUI;
        [SerializeField] private GameObject topRightUI;
        [SerializeField] private GameObject settingScreen;
        [SerializeField] private GameObject settingTab;
        [SerializeField] private GameObject winScreen;
        [SerializeField] private GameObject loseScreen;

        [Title("Others")] 
        [SerializeField] private TextMeshProUGUI txtCoin;
        [SerializeField] private TextMeshProUGUI txtTime;
        [SerializeField] private TextMeshProUGUI txtTotalCoin;

        [Title("Price")] [SerializeField] private TextMeshProUGUI guppyPriceText;
        [SerializeField] private TextMeshProUGUI kingGuppyPriceText;
        [SerializeField] private TextMeshProUGUI beetleMuncherPriceText;
        [SerializeField] private TextMeshProUGUI carnivorePriceText;

        [SerializeField] private int guppyPrice;
        [SerializeField] private int kingGuppyPrice;
        [SerializeField] private int beetleMuncherPrice;
        [SerializeField] private int carnivorePrice;
        
        void Start()
        {
            btnTouchToPlay.onClick.AddListener(TouchToPlayOnClick);
            btnGuppy.onClick.AddListener(SpawnGuppyOnClick);
            btnKingGuppy.onClick.AddListener(SpawnKingGuppyOnClick);
            btnCanivore.onClick.AddListener(SpawnCarnivoreOnClick);
            btnBetleMuncher.onClick.AddListener(SpawnBetleMuncherOnClick);
            btnSetting.onClick.AddListener(SettingButtonOnClick);
            btnHome.onClick.AddListener(HomeOnClick);
            btnResume.onClick.AddListener(ResumeOnClick);
            btnHomWin.onClick.AddListener(HomeOnClick);
            btnHomeLose.onClick.AddListener(HomeOnClick);
            EventBroker.Instance.OnTimeChange.AddListener(SetTime);
            EventBroker.Instance.OnCoinChange.AddListener(SetCoin);
            EventBroker.Instance.OnWin.AddListener(ShowWin);
            EventBroker.Instance.OnLose.AddListener(ShowLose);

            guppyPriceText.text = guppyPrice.ToString();
            kingGuppyPriceText.text = kingGuppyPrice.ToString();
            beetleMuncherPriceText.text = beetleMuncherPrice.ToString();
            carnivorePriceText.text = carnivorePrice.ToString();
        }

        public void ShowWin()
        {
            winScreen.gameObject.SetActive(true);
            txtTotalCoin.text = LevelController.Instance.totalBank.ToString();
            topLeftUI.gameObject.SetActive(false);
            topRightUI.gameObject.SetActive(false);
        }

        public void ShowLose()
        {
            loseScreen.gameObject.SetActive(true);
            topLeftUI.gameObject.SetActive(false);
            topRightUI.gameObject.SetActive(false);
        }
        
        private void ResumeOnClick()
        {
            Time.timeScale = 1;
            settingTab.transform.DOScale(0.3f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                settingScreen.gameObject.SetActive(false);
            });

        }

        private void HomeOnClick()
        {
            SceneManager.LoadScene(3);
            Time.timeScale = 1;
        }

        private void SettingButtonOnClick()
        {
            settingScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        private void TouchToPlayOnClick()
        {
            btnTouchToPlay.gameObject.SetActive(false);
            topRightUI.SetActive(true);
            topLeftUI.SetActive(true);
            LevelController.Instance.isPlaying = true;
        }

        private void SpawnBetleMuncherOnClick()
        {
            if (LevelController.Instance.Bank >=beetleMuncherPrice )
            {
                SetCoin(50);
                LevelController.Instance.AddCoin(-beetleMuncherPrice);
                LevelController.Instance.SpawnBeetleMuncher();

            }
                
        }

        private void SpawnCarnivoreOnClick()
        {
            if (LevelController.Instance.Bank >= carnivorePrice)
            {
                LevelController.Instance.AddCoin(-carnivorePrice);
                LevelController.Instance.SpawnCarnivore();

            }
        }

        private void SpawnKingGuppyOnClick()
        {
            if (LevelController.Instance.Bank >= kingGuppyPrice)
            {
                LevelController.Instance.AddCoin(-kingGuppyPrice);
                LevelController.Instance.SpawnKingGuppy();

            }
        }

        private void SpawnGuppyOnClick()
        {
            if (LevelController.Instance.Bank >= guppyPrice)
            {
                LevelController.Instance.AddCoin(-guppyPrice);
                LevelController.Instance.SpawnGuppy();

            }
        }

        private void SetTime(int totalTime)
        {
            var timeSpan = TimeSpan.FromSeconds(totalTime);
            txtTime.text = Helper.FormatTime(timeSpan.Minutes, timeSpan.Seconds);
        }

        private void SetCoin(int coin)
        {
            txtCoin.text = coin.ToString();
        }
    }
}
