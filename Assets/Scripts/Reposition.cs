using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform != GameManager.instance.viewArea.transform || !StageManager.instance.isLive) return;

        Player player = GameManager.instance.player;
        Vector2 playerPos = player.transform.position;
        Vector2 myPos = transform.position;

        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        switch (gameObject.tag)
        {
            // 플레이어 이동 방향으로 타일 이동
            // 나중에 기기에 따라 ViewArea 크기가 다르고
            // 바닥의 크기 만큼 이동하도록 수정
            // + 플레이어 moveInput 없이 밀려나는 경우에 대한 예외 필요
            case "Floor":
                float dirX = player.moveInput.x < 0 ? -1 : 1;
                float dirY = player.moveInput.y < 0 ? -1 : 1;

                if (diffX > diffY)
                {
                    transform.Translate(Vector2.right * dirX * 30 * 2);
                }
                else
                {
                    transform.Translate(Vector2.up * dirY * 30 * 2);
                }
                break;

            case "Enemy":
                // 여기도 ViewArea 크기에 따라 자동 할당 필요
                Vector2 offset = player.moveInput * 13f;
                offset.x += Random.Range(-3f, 3f);
                offset.y += Random.Range(-3f, 3f);

                transform.position = (playerPos + offset);
                break;
        }
    }
}
