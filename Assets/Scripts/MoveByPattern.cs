using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByPattern : MonoBehaviour
{
    [SerializeField] private MoveNode[] moveNodes;

    private float _currentTimeDelay;

    private int _curMoveNode;
    private float _startTime;
    int _lastNode;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentTimeDelay = 0f;
        _curMoveNode = 1;
        _lastNode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTimeDelay += Time.deltaTime;
        if (_currentTimeDelay >= moveNodes[_curMoveNode].delay)
        {
            // Set our position as a fraction of the distance between the markers.
            Vector3 currentPlayerPos = PlayerController.Instance
                .transform.position;
            currentPlayerPos.x = 0f;
            currentPlayerPos.y = 0f;
            Vector3 localPosOfPlayer =
                PlayerController.Instance.transformFollowPlayer.InverseTransformPoint(currentPlayerPos);
            transform.localPosition = Vector3.Lerp(transform.localPosition, moveNodes[_curMoveNode].targetPos + localPosOfPlayer, moveNodes[_curMoveNode].moveSpeed);
            if (Vector3.Distance(transform.localPosition, moveNodes[_curMoveNode].targetPos + localPosOfPlayer) < .2f)
            {
                _curMoveNode = (_curMoveNode + 1 >= moveNodes.Length) ? 0 : _curMoveNode + 1;
                _currentTimeDelay = 0;
                _lastNode = (_curMoveNode - 1 < 0) ? moveNodes.Length - 1 : _curMoveNode - 1;
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.blue;
        Vector3 currentPlayerPos = PlayerController.Instance
            .transform.position;
        currentPlayerPos.x = 0f;
        currentPlayerPos.y = 0f;
        Vector3 localPosOfPlayer =
            PlayerController.Instance.transformFollowPlayer.InverseTransformPoint(currentPlayerPos);
        for (int i = 0; i < moveNodes.Length; i++)
        {
            
            Gizmos.DrawWireCube(PlayerController.Instance.transformFollowPlayer.TransformPoint(moveNodes[i].targetPos + localPosOfPlayer),Vector3.one);
        }*/
        
    }
}

[Serializable]
public struct MoveNode
{
    public float delay;
    [Header("Offset with mid bottom screen")]
    public Vector3 targetPos;

    public float moveTime;
    public float moveSpeed;
}
