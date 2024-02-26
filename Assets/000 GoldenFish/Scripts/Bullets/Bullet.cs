using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Unicorn
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        public float damamge;
        public float speed;
        public Vector3 direction;

        [SerializeField] private GameObject explosion;
        
        public virtual void Init(float damage, Transform target, float speed)
        {
            this.damamge = damage;
            this.direction = target.position - transform.position;
            this.speed = speed;
            
        }
        
        public void Shoot()
        {
            _rigidbody2D.velocity = speed * direction;
        }

        private void OnBecameInvisible()
        {
            SimplePool.Despawn(gameObject);
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.CompareTag("Tank") || other.gameObject.CompareTag("Enemy"))
            {
                if(explosion!=null)
                    SimplePool.Spawn(explosion,transform.position,quaternion.identity);
                SimplePool.Despawn(gameObject);
            }
        }
    }
}
