using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unicorn
{

    [Serializable]
    public class DifficultInfo
    {
        public int difficulty;
        public List<int> timeSpawnPeriod;
        public List<int> timeSpawnAmount;
    }
    
    [CreateAssetMenu(fileName = "DifficultyAsset")]
    public class DifficultyAsset : SerializedScriptableObject
    {
        public List<DifficultInfo> DifficultInfos;
    }
}
