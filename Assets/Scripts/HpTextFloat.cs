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
        transform.localScale = Vector3.one*.8f;
        transform.localPosition = Vector3.zero + Vector3.left*Random.Range(-10,10)*2f ;
        Color cl = textHp.color;
        cl.a = 1f;
        textHp.color = cl;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOLocalMoveY(100, 1f))
            .Insert(.5f, textHp.DOFade(0f, mySequence.Duration()))
            .Insert(0.3f,transform.DOScale(1.3f,.3f).SetEase(Ease.OutBounce));
        mySequence.Play();
    }
}
