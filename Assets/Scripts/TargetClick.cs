using UnityEngine;

public class TargetClick : MonoBehaviour
{
    public bool isClicked = false;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 눌렸다!
            isClicked = true;
        }
    }
}
