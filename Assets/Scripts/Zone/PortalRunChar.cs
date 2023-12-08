using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRunChar : MonoBehaviour
{
    // 나중에 LobbyManager 만드는 것이 좋을듯
    PortalZone portal;
    SpriteRenderer spriter;
    ParticleSystem particle;

    void Awake()
    {
        portal = GetComponentInParent<PortalZone>();
        spriter = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
    }

    public void OnGlow()
    {
        spriter.DOColor(Color.white, portal.glowDuration)
            .SetEase(portal.glowEase);
        particle.Play();
        
    }

    // 다 도달하면 0.9 ~ 1 정도로 반짝반짝 빛나는 반복 넣기

    public void OffGlow()
    {
        spriter.DOColor(new Color(1, 1, 1, 0), portal.glowDuration)
            .SetEase(portal.glowEase);
        particle.Stop();
    }
}
