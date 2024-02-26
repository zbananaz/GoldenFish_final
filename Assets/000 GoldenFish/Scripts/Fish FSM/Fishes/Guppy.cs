using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace Unicorn
{
    public class Guppy : BaseFish
    {
        [SerializeField] private GameObject Coin;
        [SerializeField] private Transform[] tranformlist;

        private float timeToSpawnCoin;
         [SerializeField] private float preiod;
        private Vector2[] path;

        public override void Init()
        {
            base.Init();
            timeToSpawnCoin = 0;
            // preiod = 3f;

            for (int i = 0; i < tranformlist.Length; i++)
            {
                path[i] = tranformlist[i].position;
            }
        }

        // Start is called before the first frame update
        public override void OnMoveFixedUpdate()
        {
            base.OnMoveFixedUpdate();
            SetMovement();
        }

        public override void OnMoveUpdate()
        {
            base.OnMoveUpdate();
            SpawnCoin();
        }

        private void SpawnCoin()
        {
            
            if(!LevelController.Instance.isPlaying) return;
            timeToSpawnCoin += Time.deltaTime;

            if (timeToSpawnCoin > preiod)
            {
                timeToSpawnCoin = 0;
                var coin =  SimplePool.Spawn(Coin, transform.position, quaternion.identity).GetComponent<Coin>();
                coin.Init();
                // coin.transform.DOMove(new Vector3(coin.transform.position.x+4f,coin.transform.position.y+4f), 1f).SetEase(Ease.Linear).OnComplete(() =>
                // {
                //     
                // });
            }
        }
    }
}
