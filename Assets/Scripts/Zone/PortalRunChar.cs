using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRunChar : MonoBehaviour
{
    SpriteRenderer spriter;
    ParticleSystem particle;

    public float glowDuration = 1f;
    public Ease glowEase = Ease.Linear;

    void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
    }

    public void OnGlow()
    {
        spriter.DOColor(Color.white, glowDuration)
            .SetEase(glowEase);
        particle.Play();
        
    }

    public void OffGlow()
    {
        spriter.DOColor(new Color(1, 1, 1, 0), glowDuration)
            .SetEase(glowEase);
        particle.Stop();
    }
}
