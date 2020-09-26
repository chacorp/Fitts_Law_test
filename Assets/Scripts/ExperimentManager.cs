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
    public RectTransform target_img;

    // 시작지점
    public RectTransform start_img;
    public Transform start;

    // 고정 높이
    float height = 950f;

    // IV1: 너비 (3개)
    float[] width = { 10f, 20f, 30f };
    //float[] width = { 15f, 30f, 45f };

    // IV2: 거리 (4개)
    float[] distance = { 150f, 300f, 450f, 600f };

    // IV1 * IV2: 조합 x * y
    public List<Vector2> condition = new List<Vector2>();
    public List<float> mousePos = new List<float>();
    public List<float> targetPos = new List<float>();
    public List<bool> successList = new List<bool>();
    public Vector2 startPos;
    // 실험횟수
    public int counter = 0;

    // 실험 횟수 = 조건 갯수
    int maxCounter = 12;

    // DV: 걸린 시간
    public List<float> completionTime = new List<float>();
    float timer;

    // 테스트 시작 여부
    bool beforeStart;
    bool testing;

    // 결과지
    public GameObject resultCanvas;
    public GameObject startCanvas;
    public Text IV;
    public Text DV;

    // 클릭 변수
    public bool isClicked = false;

    // 성공 여부
    bool success = false;

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    void Start()
    {
        // 테스트 컨디션 만들기
        SetCondition();

        // 독립변수 기록하기
        Add_IV();

        // 처음엔 아직 테스트 시작 안함
        beforeStart = false;
        testing = false;

        // 결과지 비활성화
        resultCanvas.SetActive(false);
    }


    void OnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 타겟의 범위 내에서 클릭했다면,
            if (target_img.position.x - condition[counter].x / 2 < Input.mousePosition.x && Input.mousePosition.x < target_img.position.x + condition[counter].x / 2)
            {
                // 성공
                success = true;
            }
            mousePos.Add(Input.mousePosition.x);
            isClicked = true;
        }
    }

    // IV1 * IV2 만들기
    void SetCondition()
    {
        // 실험 조합 12개 만들기
        for (int i = 0; i < width.Length; i++)
        {
            for (int j = 0; j < distance.Length; j++)
            {
                condition.Add(new Vector2(width[i], distance[j]));
            }
        }

        // 조합 랜덤 셔플
        for (int i = 0; i < condition.Count; i++)
        {
            Vector2 temp = condition[i];
            int randomIndex = Random.Range(i, condition.Count);
            condition[i] = condition[randomIndex];
            condition[randomIndex] = temp;
        }
        startPos = start_img.position;
    }

    // mouse 위치 조정하기
    void SetCursor()
    {
        // 마우스 커서 위치를 start_img 좌표로 바꾸기
        SetCursorPos((int)start_img.position.x, (int)start_img.position.y);
    }

    void SetTarget()
    {
        if (counter < condition.Count)
        {
            // 타겟의 크기
            target_img.sizeDelta = new Vector2(condition[counter].x, height);

            // 타겟의 위치: 마우스 시작 위치에서 오른쪽 방향으로 distance만큼
            target_img.anchoredPosition = new Vector2(start_img.anchoredPosition.x + condition[counter].y, 0);

            targetPos.Add(target_img.position.x);
        }
    }

    void Add_IV()
    {
        for (int i = 0; i < condition.Count; i++)
        {
            string iv = ($"\n IV1(W): {condition[i].x}, IV2(D): {condition[i].y}");
            IV.text += iv;
        }
    }

    void Add_DV()
    {
        completionTime.Add(timer);
        successList.Add(success);
        string a;
        if (success)
        {
            a = ($"\n 실험{counter}: {timer}, success");
        }
        else
        {
            a = ($"\n 실험{counter}: {timer}, failed");
        }
        timer = 0;
        DV.text += a;
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnStartTest()
    {
        beforeStart = true;
        startCanvas.SetActive(false);
    }

    // 출력한 .csv에 더하기(없으면 새로 만들어서 추가하기)
    public void Dev_AppendToReport()
    {
        for (int i = 0; i < condition.Count; i++)
        {
            ExportManager.AppendToReport(
                new string[6]
                {
                    condition[i].x.ToString(),
                    condition[i].y.ToString(),
                    completionTime[i].ToString(),
                    successList[i].ToString(),
                    mousePos[i].ToString(),
                    targetPos[i].ToString()
                }
            );
        }
    }

    void Update()
    {
        if (!beforeStart)
            return;

        if (counter >= maxCounter)
        {
            resultCanvas.SetActive(true);
            return;
        }

        // 마우스 클릭하면 위치 기록
        OnMouseClick();

        if (!testing)
        {
            // 마우스 리셋
            SetCursor();

            // 타겟 리셋
            SetTarget();

            // 테스트 시작
            testing = true;
        }
        else
        {
            // 타이머 시작
            timer += Time.deltaTime;
        }


        if (isClicked && counter < maxCounter)
        {
            //Debug.Log(Input.mousePosition);

            // 테스트 끝
            testing = false;

            // 타이머 정지, 종속변수(시간) 기록하기
            Add_DV();

            // 성공여부 리셋
            success = false;

            // 테스트 카운터 올리기
            counter++;

            // 클릭 변수 리셋
            isClicked = false;
        }
    }
}

