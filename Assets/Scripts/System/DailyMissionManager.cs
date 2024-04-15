using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyMissionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class DailyMission
    {
        public int idx;
        public string rewardCondition;
        public MissionType missionType;
        public int missionCount;
        public int dailyMissionPoint;
        public bool completed;
    }
}