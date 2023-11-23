using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("ViewArea")) return;

        Player player = GameManager.instance.player;
        Vector2 playerPos = player.transform.position;
        Vector2 myPos = transform.position;
        float viewAreaSize = GameManager.instance.viewArea.size.x;

        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        switch (gameObject.tag)
        {
            case "Floor":
                float dirX = player.moveInput.x < 0 ? -1 : 1;
                float dirY = player.moveInput.y < 0 ? -1 : 1;

                if (diffX > diffY)
                {
                    transform.Translate(Vector2.right * dirX * viewAreaSize * 2);
                }
                else
                {
                    transform.Translate(Vector2.up * dirY * viewAreaSize * 2);
                }
                break;

            case "Enemy":
                Vector2 offset = player.moveInput * viewAreaSize * 0.5f;
                if (diffX > diffY)
                {
                    offset.x += Random.Range(-2f, 2f);
                    offset.y += Random.Range(-8f, 8f);
                }
                else
                {
                    offset.x += Random.Range(-8f, 8f);
                    offset.y += Random.Range(-2f, 2f);
                }

                transform.position = playerPos + offset;
                break;
        }
    }
}
