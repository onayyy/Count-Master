using System;
using System.Collections.Generic;

namespace Data.ValueObject
{
    [Serializable]
    public class LevelData
    {
        public List<GateData> GateList = new List<GateData>();
        public List<EnemyData> EnemyList = new List<EnemyData>();
    }
}