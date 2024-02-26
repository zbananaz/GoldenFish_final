using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace Unicorn
{
     public enum TypeOfTarget
    {
        Food,
        Enemy
    }
    public class LevelController : MonoBehaviour
    {
        public static LevelController Instance;
        
        [SerializeField] public List<Transform> fishSpawnPositions;
        [SerializeField] public List<Transform> enemySpawnPositions;

        [SerializeField] public List<BaseFish> allyFishes;
        [SerializeField] public List<BaseFish> enemyFish;

        [HideInInspector] public bool isPlaying;

        public List<BaseFish> currentFish;

        public int Bank = 200;
        public int totalBank = 200;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }

            isPlaying = false;
        }

        private void Start()
        {
            SpawnGuppy();
            SpawnGuppy();
            SpawnGuppy();

        }

        [Button]
        public void SpawnGuppy()
        {
            var index = Random.Range(0, fishSpawnPositions.Count);
            SimplePool.Spawn(allyFishes[0], fishSpawnPositions[index].position, quaternion.identity);
        }
        
        [Button]
        public void SpawnKingGuppy()
        {
            var index = Random.Range(0, fishSpawnPositions.Count);
            SimplePool.Spawn(allyFishes[1], fishSpawnPositions[index].position, quaternion.identity);
        }
        [Button]
        public void SpawnBeetleMuncher()
        {
            var index = Random.Range(0, fishSpawnPositions.Count);
            SimplePool.Spawn(allyFishes[2], fishSpawnPositions[index].position, quaternion.identity);
        }
        [Button]
        public void SpawnCarnivore()
        {
            var index = Random.Range(0, fishSpawnPositions.Count);
            SimplePool.Spawn(allyFishes[3], fishSpawnPositions[index].position, quaternion.identity);
        }
        
        [Button]
        public void SpawnBalrog()
        {
            var index = Random.Range(0, enemySpawnPositions.Count);
            SimplePool.Spawn(enemyFish[0], enemySpawnPositions[index].position, quaternion.identity);
        }
        
        public void SpawnShark()
        {
            var index = Random.Range(0, enemySpawnPositions.Count);
            SimplePool.Spawn(enemyFish[1], enemySpawnPositions[index].position, quaternion.identity);
        }

        public void AddCoin(int value)
        {
            Bank += value;
            EventBroker.Instance.EmitCoinChange(Bank);

            if (value > 0)
            {
                totalBank += value;
            }
        }


        public void AddFish(BaseFish fish)
        {
            if(fish is Balrog) return;
            currentFish.Add(fish);
        }

        public void CheckLose(BaseFish fish)
        {
            if(fish is Balrog) return;
            currentFish.Remove(fish);
            if (currentFish.Count == 0)
            {
                EventBroker.Instance.EmitLose();
                isPlaying = false;
            }
        }
        
        [Button]
        public void SetTimeScale()
        {
            Time.timeScale = 1;
        }
    }
}
