using UnityEngine;

public class TargetClick : MonoBehaviour
{
    // 스태틱으로 만들기
    public static TargetClick Instance;
    TargetClick()
    {
        Instance = this;
    }

    // 클릭 변수
    public bool isClicked = false;

    // UI용 함수
    public void OnTargetClick()
    {
        isClicked = true;
    }
}
