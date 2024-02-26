using System;
using System.Collections;
using System.Collections.Generic;
using DamageNumbersPro;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Unicorn
{
    public class BaseFish : Entity
    {
        //State
        public MoveState moveState;
        public FallState fallState;
        public AttackState attackState;
        public DieState dieState;

        //physic
        [Title("Physics editor")] 
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] public Collider2D _collider;
        [SerializeField] public Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SkeletonMecanim _skeleton;
        [SerializeField] private Image healthBar;
        
        //counting var
        private float timeToTurn;
        private float turnPeriod;

        [HideInInspector]public float timeToAttack;
        [HideInInspector]public float fireRate;
        
        [Title("Fish properties")]
        [SerializeField]public float maxHP;
        [SerializeField]public float currentHP;
        [SerializeField]public float atk;
        
        [Title("Fish target's properties")]
        public float closestEnemyDistance;
        public Transform nearestEnemy;

        [Title("Others")] 
        [SerializeField] private DamageNumberMesh damageNumber;
        [SerializeField] private Transform damageSpawnPos;
        
        // private Vector3 direction;
        protected override void Awake()
        {
            base.Awake();
            moveState = new MoveState(this, stateMachine);
            fallState = new FallState(this, stateMachine);
            attackState = new AttackState(this,stateMachine);
            dieState = new DieState(this, stateMachine);
            stateMachine.Initialize(fallState);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            // Gizmos.DrawWireSphere(transform.position,18f);
        }
#endif

        protected override void Start()
        {
            base.Start();
        }
        
        private void OnEnable()
        {
            Init();
        }

        public virtual void OnFallUpdate()
        {
            
        }

        public virtual void OnFallFixedUpdate()
        {
            
        }

        public virtual void OnMoveUpdate()
        {
            
        }

        public virtual void OnMoveFixedUpdate()
        {
            
        }

        public virtual void OnAttackUpdate()
        {
            
        }

        public virtual void OnAttackFixUpdate()
        {
            
        }
        
        public virtual void Init()
        {
            _collider.enabled = false;
            turnPeriod = Random.Range(3f, 6f);
            timeToTurn = turnPeriod;
            healthBar.fillAmount = 1;
            currentHP = maxHP;
            _spriteRenderer.color = new Color(255, 255, 255);
            stateMachine.ChangeState(fallState);
            LevelController.Instance.AddFish(this);
        }

        public void Fall()
        {
            var fallSpeed = -50f;
            _rigidbody2D.velocity = new Vector2(0, fallSpeed);
        }
        public void CheckFallQuitState()
        {
            if(transform.position.y <= 0)
            {
                stateMachine.ChangeState(moveState);
                _rigidbody2D.velocity = Vector3.zero;
            };
        }

        public void NatureMove(Vector2 dir, float speed)
        {
            _rigidbody2D.velocity = speed * dir;
            _spriteRenderer.flipX = _rigidbody2D.velocity.x >= 0;
        }

        public Vector2 SetDirection()
        {
            var dirX = Mathf.Cos(Random.Range(0, 360f) * Mathf.Deg2Rad);
            var dirY = Mathf.Sin(Random.Range(0, 360f) * Mathf.Deg2Rad);

            var dir = new Vector3(dirX, dirY);
            dir = dir.normalized;
            return dir;
        }
        
        public void SetMovement()
        {
            timeToTurn += Time.fixedDeltaTime;
            if (timeToTurn >= turnPeriod)
            {
                var dir = SetDirection();
                var speed = Random.Range(3f,5f);
                NatureMove(dir,speed);
                timeToTurn = 0;
                turnPeriod = Random.Range(3f, 6f);
            }
        }

        public void ResetMovement()
        {
            timeToTurn = 1;
            turnPeriod = Random.Range(3f, 6f);
            var dir = -_rigidbody2D.velocity.normalized;
            var speed = Random.Range(3f,5f);
            NatureMove(dir,speed);
        }

        public virtual void DetectTarget(string layer)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 30f,LayerMask.GetMask(layer));
            if(colliders.Length == 0)
            {
                nearestEnemy = null;
                return;
            }
            foreach (var obj in colliders)
            {
                if (obj.gameObject.CompareTag(layer))
                {
                    nearestEnemy = obj.transform;
                    nearestEnemy = CheckDistance(obj.transform);
                }
            }
        }
        
        public virtual void SetTarget(Transform position, TypeOfTarget type)
        {
            
        }

        public virtual Transform CheckDistance(Transform target)
        {
            if (Vector3.Distance(transform.position, target.position) <= closestEnemyDistance)
            {
                closestEnemyDistance = Vector3.Distance(transform.position, target.position);
                nearestEnemy = target;
            }
            
            return nearestEnemy;
        }
        public virtual void Shoot()
        {
            
        }
        
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Tank"))
            {
                ResetMovement();
            }
        }

        public virtual void SetHealthBarPercentage()
        {
            var percent = currentHP / maxHP;
            
            healthBar.fillAmount = percent;
        }
        
        public virtual void OnTakenDame(float dmgTaken)
        {
            currentHP -= dmgTaken;
            if (currentHP <= 0)
            {
                currentHP = 0;
                // stateMachine.ChangeState(dieState);
                LevelController.Instance.CheckLose(this);
                SimplePool.Despawn(gameObject);
                return;
            }
            SetHealthBarPercentage();
            StartCoroutine(OnTakenDameSpriteSelection());
            damageNumber.Spawn(damageSpawnPos.position, dmgTaken);
        }

        private IEnumerator OnTakenDameSpriteSelection()
        {
            _spriteRenderer.color = new Color(255,0,18);
            yield return Yielders.Get(0.1f);
            _spriteRenderer.color = new Color(255,255,255);

        }
        // private void OnTriggerEnter2D(Collision2D other)
        // {
        //     if (other.gameObject.CompareTag("Tank"))
        //     {
        //         ResetMovement();
        //     }
        // }
    }
}
