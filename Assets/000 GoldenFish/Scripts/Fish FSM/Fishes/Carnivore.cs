using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class Carnivore : BaseFish
    {
        //BẮN CHẬM DAME TO
        // Start is called before the first frame update
        [SerializeField] private GameObject bulletPrefab;
        public override void Init()
        {
            base.Init();
            fireRate = 3f;
            timeToAttack = 0f;
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
                var powerOrb = SimplePool.Spawn(bulletPrefab, transform.position, Quaternion.identity).GetComponent<PowerOrb>();
                powerOrb.Init(atk,nearestEnemy,2f);
            }
            
           
            
        }
    }
}
