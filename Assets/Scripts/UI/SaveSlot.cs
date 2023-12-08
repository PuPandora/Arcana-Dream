using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    public byte slotIndex;
    public bool isExistData;

    [Title("Exist Group")]
    public GameObject existGroup;
    [SerializeField] TextMeshProUGUI positionText;
    [SerializeField] TextMeshProUGUI playedStageCountText;
    [SerializeField] TextMeshProUGUI averageLevelText;
    [SerializeField] TextMeshProUGUI lastestPlayedTimeText;

    [Title("Empty Group")]
    public GameObject emptyGroup;

    void Start()
    {
        isExistData = File.Exists(DataManager.instance.path + DataManager.instance.saveFileName + $"{slotIndex}" + ".json");

        if (isExistData)
        {
            SaveData data = DataManager.instance.LoadSaveData(slotIndex);
            positionText.text = $"위치 : {data.position}";
            playedStageCountText.text = $"꿈 탐색 횟수 : {data.playedStageCount}";
            averageLevelText.text = $"평균 레벨 : {data.playerStateData.averageLevel}";
            lastestPlayedTimeText.text = $"마지막 플레이 : {data.lastPlayedDate}";

            existGroup.SetActive(true);
            emptyGroup.SetActive(false);
        }
        else
        {
            existGroup.SetActive(false);
            emptyGroup.SetActive(true);
        }

        // 임시 코드
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        // To do
        // 새 게임을 누른 상태에서 데이터가 있는 세이브 슬롯을 누르면 삭제 확인 뜨게 하기
        DataManager.instance.saveSlotIndex = slotIndex;

        // 새 게임은 스테이지부터 시작
        if (GameManager.instance.isNewGame)
        {
            GameManager.instance.EnterStage();
        }
        // 이어하기
        else
        {
            // 데이터가 있다면
            if (isExistData)
            {
                GameManager.instance.EnterLobby();
                // + 해당 슬롯 데이터 불러오기
            }
            // 없다면 새 게임
            else
            {
                GameManager.instance.EnterStage();
                GameManager.instance.isNewGame = true;
            }
        }
    }
}
