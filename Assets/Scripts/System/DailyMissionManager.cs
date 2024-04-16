using UnityEngine;

public class DailyMissionManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

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