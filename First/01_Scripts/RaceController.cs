using UnityEngine;

public class RaceController : MonoBehaviour
{
    public Transform[] Racepoints;
    [HideInInspector] public RacePoint[] RacePointsInfo;

    private void Awake()
    {
        RacePointsInfo = new RacePoint[Racepoints.Length];
        for(int i = 0; i < Racepoints.Length; i++)
        {
            RacePointsInfo[i] = Racepoints[i].GetComponent<RacePoint>();
        }
    }
}