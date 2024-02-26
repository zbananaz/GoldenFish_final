using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class PowerOrb : Bullet
    {
        public override void Init(float damage, Transform target, float speed)
        {
            base.Init(damage, target, speed);
            transform.Rotate(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            Shoot();
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            base.OnTriggerEnter2D(other);
            var enemiesFishColliders  = Physics2D.OverlapCircleAll(transform.position,3 ,LayerMask.GetMask("Enemy"));
            if(enemiesFishColliders.Length == 0)
            {
                return;
            }

            foreach (var obj in enemiesFishColliders)
            {
                obj.GetComponent<BaseFish>().OnTakenDame(damamge);
            }
            
        }
    }
}
