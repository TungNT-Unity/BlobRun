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
        cloneHpText.GetComponentInChildren<TextMeshProUGUI>().text = amount.ToString();
        cloneHpText.localScale = Vector3.one;
        cloneHpText.position = mainCam.WorldToScreenPoint(pos + Vector3.up * 3f);
        cloneHpText.gameObject.SetActive(true);
    }
}
