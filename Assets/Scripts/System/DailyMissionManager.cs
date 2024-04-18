using UnityEngine;

public class DailyMissionManager : MonoBehaviour
{
    public class DailyMission
    {
        public bool completed;
        public int dailyMissionPoint;
        public int idx;
        public int missionCount;
        public MissionType missionType;
        public string rewardCondition;
    }
}