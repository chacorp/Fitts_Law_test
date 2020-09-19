using UnityEngine;

public class FollowCursor : MonoBehaviour
{

    void Update()
    {
        if (Cursor.visible)
            Cursor.visible = false;

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + transform.forward * 10f;
    }
}
