using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unicorn
{
    public class Timer : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] public int levelTime;
        [SerializeField] public LevelController levelController;
        [HideInInspector]public int currentTime;
        
        private float secondCount;
        void Start()
        {
            currentTime = levelTime;
            secondCount = 0;
        }

        private void CountDown()
        {
            if(!levelController.isPlaying) return;
            
            secondCount += Time.deltaTime;
            if (secondCount >= 1)
            {
                secondCount = 0;
                currentTime -= 1;
                EventBroker.Instance.EmitTimeChange(currentTime);
            }

            if (currentTime == 0)
            {
                EventBroker.Instance.EmitWin();
                levelController.isPlaying = false;
            }
        }
        
        // Update is called once per frame
        void Update()
        {
            CountDown();
        }
    }
}
