using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByPattern : MonoBehaviour
{
    [SerializeField] private MoveNode[] moveNodes;

    private float _currentTimeDelay;

    private int _curMoveNode;
    private float _startTime;
    private float _distance;
    int _lastNode;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentTimeDelay = 0f;
        _curMoveNode = 1;
        _startTime = Time.time;
        _distance = Vector3.Distance(moveNodes[0].targetPos, moveNodes[_curMoveNode].targetPos);
        _lastNode = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTimeDelay += Time.deltaTime;
        if (_currentTimeDelay >= moveNodes[_curMoveNode].delay)
        {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - _startTime) * moveNodes[_curMoveNode].moveSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / _distance;

            
            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(moveNodes[_lastNode].targetPos, moveNodes[_curMoveNode].targetPos, fractionOfJourney);
            if (Vector3.Distance(transform.position, moveNodes[_lastNode].targetPos) < .2f)
            {
                _curMoveNode = (_curMoveNode + 1 >= moveNodes.Length) ? 0 : _curMoveNode + 1;
                _currentTimeDelay = 0;
                _lastNode = (_curMoveNode - 1 < 0) ? moveNodes.Length - 1 : _curMoveNode - 1;
                _distance = Vector3.Distance(moveNodes[_lastNode].targetPos, moveNodes[_curMoveNode].targetPos);
            }
        }
    }
}

public struct MoveNode
{
    public float delay;
    public Vector3 targetPos;
    public float moveSpeed;
}
