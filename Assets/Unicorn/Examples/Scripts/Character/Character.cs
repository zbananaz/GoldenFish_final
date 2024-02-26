using UnityEngine;

namespace Unicorn.Examples
{
    public class Character : MonoBehaviour
    {
        private SkinChangerRoblox skinCharacter;

        public SkinChangerRoblox SkinCharacter
        {
            get => skinCharacter;
            protected set => skinCharacter = value;
        }

        [field: SerializeField]
        public bool IsPlayer { get; set; }

        protected virtual void Awake()
        {
            skinCharacter = GetComponent<SkinChangerRoblox>();
        }

        protected virtual void Start()
        {
            SkinCharacter.Init(this);
        }
    }
}