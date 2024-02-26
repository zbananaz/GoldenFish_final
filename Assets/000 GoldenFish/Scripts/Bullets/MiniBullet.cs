using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class MiniBullet : Bullet
    {
        public override void Init(float damage, Transform target, float speed)
        {
            base.Init(damage, target, speed);
            var offset = Random.Range(-5f, 5f);
            direction = new Vector3(direction.x, direction.y + offset);
            transform.Rotate(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            // transform.Rotate(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Shoot();

        }
    }
}
