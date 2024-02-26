using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Unicorn.UI
{
    public class UiTop : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtGold;
        [SerializeField] private Button btnSetting;
        [SerializeField] private Transform transCoin;
        [SerializeField] private List<Image> listImgKeys;
        [SerializeField] private RectTransform objKey;

        private void Start()
        {
            btnSetting.onClick.AddListener(OnClickBtnSetting);

            GameManager.Instance.PlayerDataManager.actionUITop += UpdateUIHaveAnim;

            UpdateUiGold(0);
            UpdateLayoutKey();
        }

        private void OnDestroy()
        {
            GameManager.Instance.PlayerDataManager.actionUITop -= UpdateUIHaveAnim;
        }

        private void OnClickBtnSetting() { }


        private void UpdateUiGold(int _type)
        {
            switch (_type)
            {
                case 0:
                    txtGold.text = GameManager.Instance.Profile.GetGold().ToString();
                    break;

            }

        }

        private void UpdateUIHaveAnim(TypeItem _type)
        {
            switch (_type)
            {
                case TypeItem.Coin:
                {
                    SetTextCoin(GameManager.Instance.Profile.GetGold());
                    PlayAnimationScale(transCoin);
                    break;
                }
                case TypeItem.Key:
                {
                    UpdateLayoutKey(true);
                    break;
                }
            }
        }

        private void UpdateLayoutKey(bool isAni = false)
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

            if (isAni)
                objKey.DOAnchorPos3DY(-50, 1f).OnComplete(() => { objKey.DOAnchorPos3DY(135, 1f).SetDelay(1f); });
        }

        private Tweener tweenCoin;
        private int tmpCoin;

        private void SetTextCoin(int _coin)
        {
            tweenCoin = tweenCoin ?? DOTween.To(() => tmpCoin, x =>
            {
                tmpCoin = x;
                txtGold.text = tmpCoin.ToString();
            }, _coin, 0.3f).SetAutoKill(false).OnComplete(() =>
            {
                tmpCoin = GameManager.Instance.Profile.GetGold();
                txtGold.text = tmpCoin.ToString();
            });
            tweenCoin.ChangeStartValue(tmpCoin);
            tweenCoin.ChangeEndValue(_coin);
            tweenCoin.Play();
        }

        private void PlayAnimationScale(Transform transformScale)
        {
            transformScale.DOScale(1.4f, 0.1f).OnComplete(() => { transformScale.DOScale(1, 0.05f); });
        }
    }


}