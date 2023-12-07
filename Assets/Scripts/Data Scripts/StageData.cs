using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageData", menuName = "Scriptable Object/Stage Data")]
public class StageData : ScriptableObject
{
    public string stageName;
    public byte stageIndex;
    public GameObject stageFloor;
    public StageTable[] stageTable;
}

[Serializable]
public class StageTable
{
    public SpawnTable[] spawnTable;

    [Tooltip("적 스폰 시간")]
    public float spawnDelay;

    [Tooltip("다음 테이블로 넘어갈 시간")]
    public float nextTableTime;

    [Serializable]
    public class SpawnTable
    {
        [Tooltip("적 데이터")]
        public EnemyData enemyData;

        [HideInInspector]
        public short enemyId;

        [Tooltip("적 스폰 가중치")]
        public float spawnWeight;

        public void InitEnemyId()
        {
            if (enemyData == null)
            {
                Debug.LogWarning("enemyData가 비어있습니다.");
                return;
            }
            enemyId = enemyData.id;
        }

        public void InitEnemyData()
        {
            enemyData = Utils.GetEnemyDataWithId(enemyId);
        }
    }
}
