using UnityEngine;

public class Temp : MonoBehaviour
{
    public float timescale;
    private void Update()
    {
        Time.timeScale = timescale;
    }
}