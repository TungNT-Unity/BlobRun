using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    [HideInInspector] public List<Road> listRoads;
    
    // Start is called before the first frame update
    void Start()
    {
        int count = 0;
        foreach (Transform trans in transform)
        {
            Road childRoad = trans.GetComponent<Road>();
            childRoad.id = count;
            childRoad.OnSpawnRoadEvent = new MyIntFloatEvent();
            childRoad.OnSpawnRoadEvent.AddListener(OnRoadChangeSize);
            listRoads.Add(childRoad);
            count++;
        }
    }

    public void OnRoadChangeSize(int roadNumber,float scaleZ)
    {
        if(roadNumber >= listRoads.Count -1 )
            return;
        for (int i = roadNumber + 1; i < listRoads.Count; i++)
        {
            listRoads[i].transform.position += listRoads[i].transform.forward * scaleZ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
