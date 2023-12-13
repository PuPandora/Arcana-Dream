using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStageData", menuName = "Scriptable Object/Stage Data")]
public class StageData : ScriptableObject
{
    [Title("Info")]
    [PreviewField(100, ObjectFieldAlignment.Left, FilterMode = FilterMode.Point)]
    public Sprite stageThumbnail;
    public byte stageIndex;
    public string stageName;
    public int recommendLevel;
    public string stageGoal;
    public bool isShow;
    public string stageUnlockCondition;
    public bool isOpen = true;

    [Title("Data")]
    public float stageGlobalLightIntensity = 1f;
    public AudioClip bgm;
    public AudioClip bossBgm;
    public RuleTile stageFloor;
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
