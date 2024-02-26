using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using Logger = Unicorn.Utilities.Logger;

namespace Unicorn.Examples
{
    public class SkinChangerRoblox : SkinChanger
    {
        public static readonly int MAIN_COLOR_ID = Shader.PropertyToID("_BaseColor");
        public static readonly int EMISSION_MAP_ID = Shader.PropertyToID("_EmissionMap");
        public static readonly int EMISSION_COLOR = Shader.PropertyToID("_EmissionColor");
        public static readonly int MAIN_TEXTURE_ID = Shader.PropertyToID("_BaseMap");

        [SerializeField] private bool canHavePet = true;
        [SerializeField] protected DataTextureSkin dataTextureSkin;
        [SerializeField] private Renderer[] renderers;
        
        [FoldoutGroup("Pet")] [SerializeField] private Vector3 petOffset;
        [FoldoutGroup("Pet")] [SerializeField] private float petMaxDistance = 5;

        private Transform maskTransform;
        private GameObject mask;
        private MeshRenderer faceRenderer;
        private Transform hatTransform;
        private GameObject hat;
        private Pet pet;
        private Character character;
        private MaterialPropertyBlock blockMaterial;

        public event Action<SkinChangerRoblox, Pet> OnNewPetSpawned;

        public DataTextureSkin DataTextureSkin => dataTextureSkin;

        public Renderer[] Renderers => renderers;

        public int TypeSkin { get; protected set; }

        public int TypeHat { get; private set; }

        public int Number { get; private set; } = -1;

        public int PetId { get; private set; } = -1;

        public Pet Pet => pet;

        private MaterialPropertyBlock BlockMaterial => blockMaterial ??= new MaterialPropertyBlock();

        private void Awake()
        {
            hatTransform = transform.GetChild(0).Find("Armature/body_d/body_u/neck/head");
            maskTransform = transform.GetChild(0).Find("Armature/body_d/body_u/neck/head");
            if (dataTextureSkin.faceRenderer)
            {
                faceRenderer = Instantiate(dataTextureSkin.faceRenderer, maskTransform);
                if (faceRenderer.transform.parent == null)
                {
                    faceRenderer.gameObject.SetActive(false);
                }
            }
        }

        public override void Init()
        {
            Init(GetComponent<Character>());
        }
        
        public virtual void Init(Character character)
        {
            this.character = character;
            InitFace();
            InitSkin();
            InitHat();
            InitPet();
            InitMask();
        }

        private void InitFace()
        {
            var count = dataTextureSkin.faceTextures.Count;
            var index = Random.Range(0, count);
            ChangeTexture(faceRenderer, dataTextureSkin.faceTextures[index]);
        }

        private void InitSkin()
        {
            if (character.IsPlayer)
            {
                var id = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Skin);
                ChangeSkin(id);
            }
            else
            {
                ChangeSkin(Random.Range(0, dataTextureSkin.skinTextures.Count));
            }
        }

        private void InitHat()
        {
            if (character.IsPlayer)
            {
                var hat = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Hat);
                ChangeHat(hat);
            }
            else
            {
                var randomChance = Random.Range(0, 3);
                if (randomChance < 2) return;

                var randomHat = Random.Range(0, dataTextureSkin.Hats.Length);
                ChangeHat(randomHat);
            }
        }

        private void InitMask()
        {
            if (character.IsPlayer)
            {
                var mask = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Mask);
                ChangeMask(mask);
            }
            else
            {
                var randomChance = Random.Range(0, 3);
                if (randomChance < 2) return;

                var randomMask = dataTextureSkin.masks.Length;
                randomMask = Random.Range(0, randomMask);
                ChangeMask(randomMask);
            }
        }

        private void InitPet()
        {
            if (character.IsPlayer)
            {
                var petId = PlayerDataManager.Instance.GetIdEquipSkin(TypeEquipment.Pet);
                ChangePet(petId);
            }
            else
            {
                var hasPet = Random.value < .25f;
                if (!hasPet) return;
                var petId = Random.Range(0, dataTextureSkin.pets.Length);
                ChangePet(petId);
            }
        }

        public override void Change(TypeEquipment typeEquipment, int id)
        {
            switch (typeEquipment)
            {
                case TypeEquipment.Hat:
                    ChangeHat(id);
                    break;
                case TypeEquipment.Skin:
                    ChangeSkin(id);
                    break;
                case TypeEquipment.Pet:
                    ChangePet(id);
                    break;
                case TypeEquipment.Mask:
                    ChangeMask(id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeEquipment), typeEquipment, null);
            }
        }

        public void CopyFrom(SkinChangerRoblox skinChangerRoblox)
        {
            ChangeSkin(skinChangerRoblox.TypeSkin);
            ChangeHat(skinChangerRoblox.TypeHat);
            ChangeNumber(skinChangerRoblox.Number);
        }

        public virtual void ChangeSkin(int id)
        {
            if (id < 0) id = 0;
            TypeSkin = id;

            ChangeNumber(0);
            foreach (var renderer in renderers)
            {
                if (DataTextureSkin.skinTextures.Count >= id)
                {
                    ChangeTexture(renderer, DataTextureSkin.skinTextures[id]);
                }
                else
                {
                    Logger.LogError($"{name} has invalid id skin {id}");
                }
            }
        }

        public void ChangeNumber(int num)
        {
            if (!DataTextureSkin) return;

            var texture = dataTextureSkin.numberTextures[0];
            ChangeTexture(Renderers[0], texture, 1);
            Number = num;
        }

        public void ChangeToWinTexture()
        {
            ChangeTexture(faceRenderer, dataTextureSkin.winTexture);
        }

        public void ChangeToDieTexture()
        {
            ChangeTexture(faceRenderer, dataTextureSkin.loseTexture);
        }

        public virtual void ChangeTexture(Renderer renderer, Texture texture, int materialId = 0)
        {
            if (renderer.materials.Length <= materialId)
            {
                materialId = 0;
                Logger.Log("Material Id is out of range, using material index 0");
            }

            renderer.GetPropertyBlock(BlockMaterial, materialId);
            BlockMaterial.SetTexture(MAIN_TEXTURE_ID, texture);
            renderer.SetPropertyBlock(BlockMaterial, materialId);
        }

        public virtual void ChangeHat(int id)
        {
            var hats = dataTextureSkin.Hats;
            if (hats.Length == 0)
            {
                Logger.LogError($"{name} has no hats!");
                return;
            }

            if (!hatTransform)
            {
                Logger.LogWarning($"{name} does not have a transform to wear hat!");
                return;
            }


            if (id >= hats.Length)
            {
                Logger.LogError($"{gameObject.name} has invalid hat id {id}");
                id = -1;
            }

            if (hat != null)
            {
                hat.SetActive(false);
            }

            TypeHat = id;

            if (id < 0)
            {
                if (!character.IsPlayer)
                    return;

                id = 11; // hair
                TypeHat = 11;
                ;
            }

            hat = hatTransform.GetChild(id).gameObject;
            hat.SetActive(true);
        }

        public virtual void ChangePet(int id)
        {
            if (pet)
            {
                SimplePool.Despawn(pet.gameObject);
            }

            if (!canHavePet) return;

            if (id >= dataTextureSkin.pets.Length)
            {
                Logger.LogError($"{gameObject.name} has invalid pet id {id}");
                id = -1;
            }

            PetId = id;
            if (id == -1)
            {
                return;
            }

            pet = Pet.Spawn(character, petOffset, petMaxDistance, id);
            OnNewPetSpawned?.Invoke(this, pet);
        }

        private void ChangeMask(int id)
        {
            var masks = dataTextureSkin.masks;
            if (masks.Length == 0)
            {
                Logger.LogError($"{name} has no hats!");
                return;
            }

            if (!maskTransform)
            {
                Logger.LogWarning($"{name} does not have a transform to wear mask!");
                return;
            }


            if (id >= masks.Length)
            {
                Logger.LogError($"{gameObject.name} has invalid wear id {id}");
                id = -1;
            }

            if (mask != null)
            {
                SimplePool.Despawn(mask);
            }

            TypeHat = id;

            if (id < 0)
            {
                return;

            }

            mask = SimplePool.Spawn(masks[id], false);
            mask.transform.SetParent(maskTransform, false);
            mask.SetActive(true);
        }
    }

}