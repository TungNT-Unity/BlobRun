using System;
using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Camera mainCam;
    [SerializeField] private Transform hpFloatPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateHpTextFloat(int amount,Vector3 pos)
    {
        Transform cloneHpText = PoolManager.Pools["Pool"].Spawn(hpFloatPrefab);
        cloneHpText.SetParent(transform);
        cloneHpText.SetAsLastSibling();
        cloneHpText.GetComponentInChildren<TextMeshProUGUI>().text = amount.ToString();
        cloneHpText.localScale = Vector3.one;
        cloneHpText.position = mainCam.WorldToScreenPoint(pos + Vector3.up * 3f);
        RectTransform rectTransform = cloneHpText.GetComponent<RectTransform>();
        Vector3 posOnRectTrans = rectTransform.anchoredPosition3D;
        posOnRectTrans.z = 0f;
        rectTransform.anchoredPosition3D = posOnRectTrans;
        cloneHpText.gameObject.SetActive(true);
    }
}
