using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExperimentManager : MonoBehaviour
{
    public static ExperimentManager Instance;
    ExperimentManager()
    {
        Instance = this;
    }

    // 타겟지점
    public Transform target;
    Vector3 targetScale;
    TargetClick tc;

    // 시작지점
    public Transform start;


    // IV1: 3개
    float[] scale = { 0.05f, 0.1f, 0.2f };

    // IV2: 4개
    float[] distance = { 3f, 6f, 9f, 12f };

    // IV1* IV2
    public List<Vector2> condition = new List<Vector2>();
    public int counter = 0;
    int maxCounter = 12;

    // DV
    public List<float> completionTime = new List<float>();
    float timer;

    // 테스트 시작 여부
    bool startTest;
    bool testing;

    // 결과지
    public GameObject resultCanvas;
    public GameObject startCanvas;
    public Text IV;
    public Text DV;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);



    void Start()
    {
        // 타겟 비활성화
        target.gameObject.SetActive(false);
        targetScale = target.localScale;
        tc = target.GetComponent<TargetClick>();

        // 테스트 컨디션 만들기
        SetCondition();

        // 독립변수 기록하기
        Add_IV();

        // 처음엔 아직 테스트 시작 안함
        startTest = false;
        testing = false;

        // 결과지 비활성화
        resultCanvas.SetActive(false);
    }

    void SetCondition()
    {
        // 컨디션 만들기
        for (int i = 0; i < scale.Length; i++)
        {
            for (int j = 0; j < distance.Length; j++)
            {
                condition.Add(new Vector2(scale[i], distance[j]));
            }
        }

        // 컨디션 랜덤 셔플
        for (int i = 0; i < condition.Count; i++)
        {
            Vector2 temp = condition[i];
            int randomIndex = Random.Range(i, condition.Count);
            condition[i] = condition[randomIndex];
            condition[randomIndex] = temp;
        }
    }

    void SetCursor()
    {
        // start의 월드 좌표를 스크린 좌표로 가져오기
        Vector2 mPos = Camera.main.WorldToScreenPoint(start.position);

        // 마우스 커서 위치를 start 좌표로 바꾸기
        SetCursorPos((int)mPos.x, (int)mPos.y);//Call this when you want to set the mouse position
    }


    void SetTarget()
    {
        if (counter < condition.Count)
        {
            // 타겟의 크기
            targetScale.x = condition[counter].x;
            target.localScale = targetScale;

            // 타겟의 위치: 시작 위치에서 오른쪽 방향으로 distance만큼
            target.position = start.position + (Vector3.right * condition[counter].y);
        }
    }
    void Add_IV()
    {
        for (int i = 0; i < condition.Count; i++)
        {
            string iv = ($"\n IV1: {condition[i].x}, IV2: {condition[i].y}");
            IV.text += iv;
        }
    }

    void Add_DV()
    {
        completionTime.Add(timer);
        string a = ($"\n 실험{counter}: {timer}");
        timer = 0;
        DV.text += a;
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnStartTest()
    {
        startTest = true;
        startCanvas.SetActive(false);
    }


    void Update()
    {
        if (!startTest)
            return;

        if (counter >= maxCounter)
        {
            resultCanvas.SetActive(true);
            return;
        }

        if (!testing)
        {
            // 마우스 리셋
            SetCursor();

            // 타겟 리셋
            SetTarget();

            // 타겟 활성화
            target.gameObject.SetActive(true);

            // 타이머 시작
            timer += Time.deltaTime;

            // 테스트 시작
            testing = true;
        }

        if (tc.isClicked && counter < maxCounter)
        {
            // 테스트 끝
            testing = false;

            // 타이머 정지, 종속변수(시간) 기록하기
            Add_DV();

            // 테스트 카운터 올리기
            counter++;

            tc.isClicked = false;
        }
    }
}

