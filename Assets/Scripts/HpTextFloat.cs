using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HpTextFloat : MonoBehaviour
{
    private Sequence mySequence;
    [SerializeField] private TextMeshProUGUI textHp;
    private void OnEnable()
    {
        transform.DOComplete();
        textHp.DOComplete();
        transform.localPosition = Vector3.zero + Vector3.left*Random.Range(-50,50)*2f ;
        Color cl = textHp.color;
        cl.a = 1f;
        textHp.color = cl;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOLocalMoveY(300, 1.5f))
            .Insert(.7f, textHp.DOFade(0f, mySequence.Duration()));
        mySequence.Play();
    }
}
