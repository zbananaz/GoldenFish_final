using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Unicorn
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] public Rigidbody2D _rigidbody2D;
        [SerializeField] public float timeToDisapear;
        public int value = 10;
        private Coroutine disapearCourutine;
        public void Init()
        {
            transform.localScale = Vector3.one;
            disapearCourutine = StartCoroutine(DelayToDisappear());
            transform.DOMoveY(transform.position.y + 2f, .3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _rigidbody2D.velocity = new Vector2(0, -2f);

            });
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Floor"))
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }

        public void OnClick()
        {
            //bien mat
            LevelController.Instance.AddCoin(value);
            StopCoroutine(disapearCourutine);
            gameObject.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                SimplePool.Despawn(gameObject);
            });
            // add coin
        }

        private IEnumerator DelayToDisappear()
        {
            yield return Yielders.Get(timeToDisapear);
            gameObject.transform.DOScale(0, 0.5f).OnComplete(() =>
            {
                SimplePool.Despawn(gameObject);
            });
        }
    }
}
