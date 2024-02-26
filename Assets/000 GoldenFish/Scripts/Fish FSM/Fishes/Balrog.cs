using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class Balrog : BaseFish
    {
        private float speed;
        private bool isAttacking;
        [SerializeField] private float attackArea;
        public override void Init()
        {
            base.Init();
            fireRate = 1.5f;
            timeToAttack = 1.5f;
            speed = 10;
            // stateMachine.ChangeState(attackState);
        }
            
        
        
        public override void OnMoveUpdate()
        {
            base.OnMoveUpdate();
            DetectTarget("Fish");
            MoveToTarget();
            CheckDistanceAndRotation();

        }

        public override void OnMoveFixedUpdate()
        {
            // base.OnMoveFixedUpdate();
        }

        public override void OnAttackUpdate()
        {
            base.OnAttackUpdate();
            DetectTarget("Fish");
            Attack();
            CheckDistanceAndRotation();
        }

        protected override void Update()
        {
            base.Update();
            // Debug.Log(stateMachine.currentState.Equals(attackState));
        }


        public override void DetectTarget(string layer)
        {
            if(nearestEnemy != null )
            {
                if (!nearestEnemy.gameObject.activeInHierarchy)
                {
                    nearestEnemy = null;
                }
                return;
            }
            base.DetectTarget(layer);
        }

        public void MoveToTarget()
        {
            if(nearestEnemy == null) return;
            transform.position = Vector3.MoveTowards(transform.position, nearestEnemy.position, speed * Time.deltaTime);

            var dir = nearestEnemy.transform.position - transform.position;
            transform.localScale = dir.x > 0 ? Vector3.one : new Vector3(-1, 1, 1);

            // transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        }
        
        
        public void CheckDistanceAndRotation()
        {
            if (nearestEnemy == null || isAttacking) return;
            var dir = nearestEnemy.transform.position - transform.position;
            // var scaler = transform.localScale;
            transform.localScale = dir.x >= 0 ? Vector3.one : new Vector3(-1, 1, 1);
            // Debug.Log(stateMachine.currentState.Equals(attackState));
            
            if (stateMachine.currentState.Equals(moveState))
            {
                if(Vector3.Distance(transform.position,nearestEnemy.position) < 2f) 
                    stateMachine.ChangeState(attackState);
            }
            else if (stateMachine.currentState.Equals(attackState))
            {
                if(Vector3.Distance(transform.position,nearestEnemy.position) >= 2f) 
                    stateMachine.ChangeState(moveState);
            }
        }

        public void Attack()
        {
            timeToAttack += Time.deltaTime;
            if (timeToAttack > fireRate)
            {
                isAttacking = true;
                timeToAttack = 0;
                StartCoroutine(SetAttackAnimation());
                // SetAttackAnimation();
                DealDamamge();
            }
        }

        public void DealDamamge()
        {
            if(nearestEnemy == null) return;
            // nearestEnemy.GetComponent<BaseFish>().OnTakenDame(atk);
            var enemiesFishColliders  = Physics2D.OverlapCircleAll(transform.position,attackArea ,LayerMask.GetMask("Fish"));
            if(enemiesFishColliders.Length == 0)
            {
                nearestEnemy = null;
                return;
            }
            
            foreach (var obj in enemiesFishColliders)
            {
                var fish = obj.GetComponent<BaseFish>();
                if (fish is not Balrog)
                {
                    fish.OnTakenDame(atk);
                }
            }
        }

        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                
                var bullet = other.GetComponent<Bullet>();
                if(bullet is PowerOrb) return;
                
                OnTakenDame(bullet.damamge);
                var currentSpeed = speed;
                if (bullet is IceShard && currentSpeed == 10f) 
                {
                    StartCoroutine(OnSlowEffect(3f, speed));
                }
            }
        }

        private IEnumerator OnSlowEffect(float timeSlow,float currentSpeed)
        {
            speed = currentSpeed / 2;
            yield return Yielders.Get(timeSlow);
            speed = currentSpeed;
        }
        
        
        public IEnumerator SetAttackAnimation()
        {
            _animator.SetTrigger("Attack");
            yield return Yielders.Get(0.9f);
            _animator.ResetTrigger("Attack");
            isAttacking = false;
        }
    }
}
