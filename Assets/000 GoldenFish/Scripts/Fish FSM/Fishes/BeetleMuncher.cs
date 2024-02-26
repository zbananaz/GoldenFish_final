using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class BeetleMuncher : BaseFish
    {
        [SerializeField] private GameObject bulletPrefab;
        private Coroutine shootCoroutine;
        public override void Init()
        {
            base.Init();
            fireRate = 1.5f;
            timeToAttack = 1.4f; // atk = 0.3f;
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
                shootCoroutine = StartCoroutine(MultipleShoot());
            }
        }

        private IEnumerator MultipleShoot()
        {
            var miniFireRate = Random.Range(0.05f, 0.1f);
            var numberOfShoot = Random.Range(4, 7);
            for (int i = 0; i < numberOfShoot; i++)
            {
                var miniBullet = SimplePool.Spawn(bulletPrefab, transform.position, Quaternion.identity).GetComponent<MiniBullet>();
                miniBullet.Init(atk,nearestEnemy,2f);
                miniFireRate = Random.Range(0.05f, 0.1f);
                yield return Yielders.Get(miniFireRate);

            }
        }
        
        
    }
}
