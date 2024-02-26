using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class IceShard : Bullet
    {
        public override void Init(float damage, Transform target, float speed)
        {
            base.Init(damage, target, speed);
            transform.Rotate(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Shoot();
        }
    }
}
