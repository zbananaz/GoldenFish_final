﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unicorn;
using Unicorn.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Unicorn.UI
{
    public class UiLose : UICanvas
    {
        [SerializeField] private TextMeshProUGUI txtRetry;
        [SerializeField] private Button btnRetry;
        [SerializeField] private Button btnSkipLevel;
        [SerializeField] private TextMeshProUGUI txtSkipLevel;
        [SerializeField] private Image imgSkipLevelBg;
        private Tween tween;

        private void Start()
        {
            btnRetry.onClick.AddListener(OnClickBtnRetry);
            btnSkipLevel.onClick.AddListener(OnClickBtnRevive);
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);

            if (!isShow)
            {
                return;
            }

            if (tween != null)
                tween.Kill();

            btnRetry.interactable = false;
            txtRetry.color = new Color(1, 1, 1, 0);
            txtRetry.DOFade(0.8f, 0.5f).SetDelay(3).OnComplete(() => { btnRetry.interactable = true; });

            btnSkipLevel.gameObject.SetActive(true);

            imgSkipLevelBg.fillAmount = 0;
            tween = imgSkipLevelBg.DOFillAmount(1, Constants.REVIVE_CHOOSING_TIME)
                .SetEase(Ease.Linear)
                .OnComplete(() => { btnSkipLevel.gameObject.SetActive(false); });
        }

        private void OnClickBtnRetry()
        {
            OnBackPressed();

            //UnicornAdManager.ShowInterstitial(Helper.inter_end_game_lose);
            GameManager.Instance.LoadLevel();

            SoundManager.Instance.PlaySoundButton();
        }

        private void OnClickBtnRevive()
        {
            if (imgSkipLevelBg.fillAmount == 1)
            {
                return;
            }

            UnicornAdManager.ShowAdsReward(OnReward, Helper.video_reward_revive);
            SoundManager.Instance.PlaySoundButton();
        }

        private void OnReward()
        {
            if (!isShow)
                return;

            StartCoroutine(IEWaitRevive());

        }

        private IEnumerator IEWaitRevive()
        {
            yield return new WaitForSeconds(0.2f);
            OnBackPressed();
            GameManager.Instance.Revive();
        }
    }

}