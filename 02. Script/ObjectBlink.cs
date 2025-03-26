using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectBlink : MonoBehaviour
{
    private Image logoImage;
    private float blinkDuration = 1f;

    void Start()
    {
        logoImage = this.gameObject.GetComponent<Image>();
        Blink();
    }

    void Blink()
    {
        logoImage.DOFade(0f, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
