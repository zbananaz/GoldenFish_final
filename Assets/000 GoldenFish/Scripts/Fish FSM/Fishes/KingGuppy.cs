using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Unicorn
{
    public class KingGuppy : BaseFish
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject bulletPrefab;
        public override void Init()
        {
            base.Init();
            fireRate = 1.5f;
            timeToAttack = 0f;
            // atk = 5f;
            closestEnemyDistance = 0f;
        }

        public override void OnMoveUpdate()
        {
            base.OnMoveUpdate();
            DetectTarget("Enemy");
            Shoot();
        }

        public override void OnMoveFixedUpdate()
        {
            base.OnMoveFixedUpdate();
            SetMovement();
        }
        
        public override void DetectTarget(string layer)
        {
            base.DetectTarget(layer);
        }
    

        public override void Shoot()
        {
            base.Shoot();
            
            if(nearestEnemy == null) return;
 
            timeToAttack += Time.deltaTime;

            if (timeToAttack > fireRate)
            {
                timeToAttack = 0;
                var iceShard = SimplePool.Spawn(bulletPrefab, transform.position, quaternion.identity).GetComponent<IceShard>();
                iceShard.Init(atk,nearestEnemy,2f);
            }
            
           
            
        }
    }
}
