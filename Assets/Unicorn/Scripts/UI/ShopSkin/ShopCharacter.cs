using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn.UI.Shop
{
    public class ShopCharacter : UICanvas
    {
        [SerializeField] private List<ShopContent> contents;
        [SerializeField] private List<ShopTab> tabs;
        [SerializeField] private List<PageIndicator> listObjTabBottom;
        [SerializeField] private SkinChanger skinChanger;

        [FoldoutGroup("Persistence")] [SerializeField]
        private DataShopSkin dataShopSkin;

        [FoldoutGroup("Persistence")]
        [SerializeField] private CanvasGroup notEnoughGold;

        private Dictionary<TypeEquipment, ShopContent> shopContentsDictionary = new Dictionary<TypeEquipment, ShopContent>();


        public IDataSkin DataSkin { get; private set; }

        public SkinChanger SkinChanger => skinChanger;

        public DataShopSkin DataShopSkin => dataShopSkin;
        public DataTextureSkin DataTextureSkin { get; private set; }

        public void Configure(IDataSkin dataSkin, DataTextureSkin dataTextureSkin)
        {
            DataSkin = dataSkin;
            DataTextureSkin = dataTextureSkin;
            InitContents();
        }
        
        private void InitContents()
        {
            for (int i = 0; i < contents.Count; i++)
            {
                InitContent(contents[i]);
                shopContentsDictionary.Add(contents[i].TypeEquipment, contents[i]);
            }

            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].Init(this);
            }

            ActiveContentTab(0);
        }

        private void InitContent(ShopContent shopContent)
        {
            var type = shopContent.TypeEquipment;
            shopContent.ShopCharacter = this;
            shopContent.InitContent(dataShopSkin.GetDataShop(type));
        }

        private void Init()
        {
            ActiveContentTab(0);
            skinChanger.Init();
        }

        public void ReloadLayout(TypeEquipment typeEquipment)
        {
            shopContentsDictionary[typeEquipment].InitContent(dataShopSkin.GetDataShop(typeEquipment), false);
        }

        public override void Show(bool _isShown, bool isHideMain = true)
        {
            base.Show(_isShown, isHideMain);
            if (isShow)
            {
                Init();
            }
        }

        public void ActiveContentTab(int idTab)
        {
            ShopTab shopTab = tabs[idTab];
            if (shopTab.IsActive)
            {
                return;
            }

            for (int i = 0; i < contents.Count; i++)
            {
                contents[i].gameObject.SetActive(false);
                tabs[i].DisableTab();
            }

            ShopContent shopContent = contents[idTab];
            shopContent.gameObject.SetActive(true);
            shopContent.Reset();

            shopTab.ActiveTab();
        }

        public void SetupTabBottom(int currentTab, int countTab)
        {
            for (int i = 0; i < listObjTabBottom.Count; i++)
            {
                listObjTabBottom[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < countTab; i++)
            {
                listObjTabBottom[i].gameObject.SetActive(true);
                listObjTabBottom[i].Setup(false);
            }

            listObjTabBottom[currentTab].Setup(true);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SoundManager.Instance.PlaySoundButton();
            // TODO: Thoát ra ngoài thì update lại skin player
        }

        public void NotifyNotEnoughGold(Transform trans)
        {
            SetParentNotiNotEnoughGold(trans);
            notEnoughGold.alpha = 1;
            notEnoughGold.DOKill();
            notEnoughGold.DOFade(0, 0.75f).SetDelay(0.75f).SetEase(Ease.OutQuad);
            notEnoughGold.transform.DOKill();
            notEnoughGold.transform.DOLocalMoveY(50, 1.5f).SetEase(Ease.OutSine);
        }

        private void SetParentNotiNotEnoughGold(Transform trans)
        {
            notEnoughGold.transform.SetParent(trans);
            notEnoughGold.transform.localPosition = Vector3.zero;
        }
    }
}
