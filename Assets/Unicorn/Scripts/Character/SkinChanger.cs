using UnityEngine;

namespace Unicorn
{
    /// <summary>
    /// Base class cho hệ thống thay skin
    /// </summary>
    public abstract class SkinChanger : MonoBehaviour
    {
        public abstract void Init();
        
        public abstract void Change(TypeEquipment typeEquipment, int id);
    }
}