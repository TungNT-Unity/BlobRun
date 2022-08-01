using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectFollow3DObj : MonoBehaviour
{
    public Transform target;
    [SerializeField] float offsetY = .5f;
    [SerializeField] bool isUIObject = true;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
            return;
        if (isUIObject)
        {
            transform.position = UIManager.Instance.mainCam.WorldToScreenPoint(target.position + Vector3.up * offsetY);
        }
        else
            transform.position = target.position + Vector3.up * offsetY;
    }
}
