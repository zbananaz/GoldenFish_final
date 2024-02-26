using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class FXController : MonoBehaviour
    {
        private void OnEnable()
        {
            throw new NotImplementedException();
        }

        private IEnumerator DelayToDeSpawnFX()
        {
            yield return Yielders.Get(1f);
            SimplePool.Despawn(gameObject);
        }
    }
}
