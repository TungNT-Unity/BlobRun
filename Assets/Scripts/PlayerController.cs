using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Slider = UnityEngine.UI.Slider;

public class PlayerController : MonoBehaviour,IHealth
{
    public static PlayerController Instance;
    public float itemCollectRange = 5f;
    public int hp;
    [SerializeField] private Animator anim;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private CamPosInfo[] camPosInfos;
    [SerializeField] private CinemachineVirtualCamera runningCam;
    [SerializeField] private CinemachineVirtualCamera battleCam;
    public Transform transformFollowPlayer;
    public CharacterMovementController playerMovement;
    private bool _isRunningCam = true;
    
    private int currentHp;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _isRunningCam = true;
        currentHp = hp;
    }

    public void ChangeCam()
    {
        _isRunningCam = !_isRunningCam;
        runningCam.gameObject.SetActive(_isRunningCam);
        battleCam.gameObject.SetActive(!_isRunningCam);
    }

    public void OnDamage(int amount)
    {
        if (currentHp <= 0)
            return;
        currentHp -= amount;
        UIManager.Instance.CreateHpTextFloat(amount, transform.position + Vector3.up);
        if (currentHp <= 0)
        {
            currentHp = 0;
            StartCoroutine(DelayDeath());
        }

        hpSlider.DOComplete();
        float sliderValue = (float) currentHp / (float) hp;
        hpSlider.DOValue(sliderValue, .3f);
    }
    

    IEnumerator DelayDeath()
    {
        anim.SetBool("IsDeath",true);
        yield return new WaitForSeconds(1f);
    }

    public void OnHealth(int amount)
    {
        
    }
}

public struct CamPosInfo
{
    public float x;
    public float y;
    public float z;
}
