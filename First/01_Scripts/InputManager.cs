using UnityEngine;

public class InputManager : MonoBehaviour
{
    public byte JoyNumber;

    public float XAxis()
    {
        return Input.GetAxis("Joy" + JoyNumber.ToString() + " xAxis");
    }
    public float YAxis()
    {
        return Input.GetAxis("Joy" + JoyNumber.ToString() + " yAxis");
    }
    public float RightXAxis()
    {
        return Input.GetAxis("Joy" + JoyNumber.ToString() + " RightXAxis");
    }
    public float RightYAxis()
    {
        return Input.GetAxis("Joy" + JoyNumber.ToString() + " RightYAxis");
    }
    public bool Dash()
    {
        return Input.GetButtonDown("Joy" + JoyNumber.ToString() + " Dash");
    }
    public bool UseObject()
    {
        return Input.GetButtonDown("Joy" + JoyNumber.ToString() + " UseObject");
    }
}