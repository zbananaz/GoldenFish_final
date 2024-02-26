using System.Collections;
using System.Collections.Generic;
using Unicorn;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class UISetting : UICanvas
    {
        [SerializeField] private Button btnMusic;
        [SerializeField] private Text txtMusic;
        [SerializeField] private Button btnSound;
        [SerializeField] private Text txtSound;
        [SerializeField] private Button btnRateUs;
        [SerializeField] private Button btnRestorePurchase;

        protected override void Awake()
        {
            btnMusic.onClick.AddListener(ToggleMusic);
            btnSound.onClick.AddListener(ToggleSound);
            btnRateUs.onClick.AddListener(Rate);
            btnRestorePurchase.onClick.AddListener(RestorePurchase);

            RestoreSettings();
        }

        private void RestoreSettings()
        {
            txtMusic.text = PlayerDataManager.Instance.GetMusicSetting() ? "ON" : "OFF";
            txtSound.text = PlayerDataManager.Instance.GetSoundSetting() ? "ON" : "OFF";
        }

        private void ToggleMusic()
        {
            bool isOn = !PlayerDataManager.Instance.GetMusicSetting();
            txtMusic.text = isOn ? "ON" : "OFF";
            PlayerDataManager.Instance.SetMusicSetting(isOn);

            SoundManager.Instance.PlaySoundButton();

            SoundManager.Instance.SettingMusic(isOn);
        }

        private void ToggleSound()
        {
            bool isOn = !PlayerDataManager.Instance.GetSoundSetting();
            txtSound.text = isOn ? "ON" : "OFF";
            PlayerDataManager.Instance.SetSoundSetting(isOn);

            SoundManager.Instance.PlaySoundButton();

            SoundManager.Instance.SettingFxSound(isOn);
        }

        private void Rate()
        {
            Application.OpenURL(RocketConfig.OPEN_LINK_RATE);
        }

        private void RestorePurchase()
        {
#if UNITY_IOS
        GameManager.Instance.IapController.RestoreButtonClick();
#endif
        }

        public void Open()
        {
            gameObject.SetActive(true);
            GameManager.Instance.Pause();

            SoundManager.Instance.PlaySoundButton();
        }

        public void Close()
        {
            gameObject.SetActive(false);
            GameManager.Instance.Resume();

            SoundManager.Instance.PlaySoundButton();
        }
    }

}