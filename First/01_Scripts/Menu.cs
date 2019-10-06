using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool FourPlayers;
    private bool SelectionChanged;
    public RectTransform SelectionImage;

    private void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("MenuSelection")) > .9f && !SelectionChanged)
        {
            SelectionChanged = true;
            FourPlayers = !FourPlayers;
        }
        if(Mathf.Abs(Input.GetAxisRaw("MenuSelection")) < .1f)
        {
            SelectionChanged = false;
        }

        if (FourPlayers)
        {
            SelectionImage.anchoredPosition = new Vector2(0, -200);
        }
        else
        {
            SelectionImage.anchoredPosition = new Vector2(0, 200);
        }

        if (Input.GetButtonDown("Validation"))
        {
            if (FourPlayers)
            {
                SceneManager.LoadScene("");
            }
            else
            {
                SceneManager.LoadScene("");
            }
        }
    }
}