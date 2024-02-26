using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unicorn.Examples;

namespace Unicorn
{
    /// <summary>
    /// Lưu dữ liệu về skin
    /// </summary>
    /// <remarks>
    /// Tuỳ mỗi game mà sẽ có thông tin về skin khác nhau. Mọi người sửa lại đúng với skin trong game nhé
    /// </remarks>
    [CreateAssetMenu(fileName = "DataTextureSkin", menuName = "ScriptableObjects/Data Texture Skin")]
    public class DataTextureSkin : SerializedScriptableObject
    {
        [HorizontalGroup("Skin")] [PreviewField(120)]
        public List<Sprite> skinIcons;

        [HorizontalGroup("Skin")] [PreviewField(120)]
        public List<Texture> skinTextures;

        public List<Texture> numberTextures;
        public List<Texture> faceTextures;
        public Texture winTexture;
        public Texture loseTexture;
        public MeshRenderer faceRenderer;

        [HorizontalGroup("Mask")] [PreviewField(100)]
        public List<Sprite> maskIcons;

        [HorizontalGroup("Mask")] [PreviewField(100)]
        public GameObject[] masks;

        [HorizontalGroup("Hat")] [PreviewField(100)] [ListDrawerSettings(ShowIndexLabels = true)]
        public List<Sprite> hatIcons;

        [HorizontalGroup("Hat")] [PreviewField(100)] [ListDrawerSettings(ShowIndexLabels = true)]
        public GameObject[] Hats;

        [HorizontalGroup("Pet")] public List<Sprite> petIcons;
        [HorizontalGroup("Pet")] public Pet[] pets;


        public Sprite GetIcon(TypeEquipment typeEquipment, int id)
        {
            switch (typeEquipment)
            {
                case TypeEquipment.Hat:
                    return hatIcons[id];
                case TypeEquipment.Skin:
                    return skinIcons[id];
                case TypeEquipment.Pet:
                    return petIcons[id];
                case TypeEquipment.Mask:
                    return maskIcons[id];
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeEquipment), typeEquipment, null);
            }
        }
    }



}