using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExperimentManager2 : MonoBehaviour
{
    public static ExperimentManager2 Instance;
    ExperimentManager2()
    {
        Instance = this;
    }
    // 벽
    public RectTransform Wall_img;
    public RectTransform Road_img;

    // 타겟지점
    public RectTransform target_img;

    // 시작지점
    public RectTransform start_img;
    public GameObject report_btn;

    // 고정 폭
    float wide = 60f;
    float wall = 60f;

    // IV1: 너비 (3개)
    float[] width = { 40f, 90f, 140f };
    // IV2: 간격 (2)
    float[] distance = { 660f, 880f };

    // IV1 * IV2
    public List<Vector2> condition = new List<Vector2>();

    // 실험횟수
    public int counter = 0;

    // 실험 횟수 = 조건 갯수
    int maxCounter = 6;

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
        startCanvas.SetActive(true);

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

    public void OnMouseHitWall()
    {
        // 마우스 리셋
        SetCursor();

        // 타이머 리셋
        timer = 0;
    }

    public void OnMouseEnterGoal()
    {
        // 성공
        success = true;
        isClicked = true;
    }

    // 6개 컨디션 만들기
    void SetCondition()
    {
        // 실험 조합 6개 리스트에 담기
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
    }

    // mouse 위치 조정하기
    void SetCursor()
    {
        Vector2 view = Camera.main.ScreenToViewportPoint(start_img.position);
        // 마우스 커서 위치를 start_img 좌표로 바꾸기

        SetCursorPos((int)(Screen.width * view.x), (int)(Screen.height * view.y));
    }

    void SetTarget()
    {
        if (counter < condition.Count)
        {
            // 타겟의 너비 바꾸기
            target_img.sizeDelta = new Vector2(wide, condition[counter].x);
            target_img.anchoredPosition = new Vector2((condition[counter].y / 2 - wide / 2), 0);

            start_img.sizeDelta = new Vector2(wide, condition[counter].x);
            start_img.anchoredPosition = new Vector2(-(condition[counter].y / 2 - wide / 2), 0);

            // 트랙 세팅
            Wall_img.sizeDelta = new Vector2(condition[counter].y, condition[counter].x + wall);
            Road_img.sizeDelta = new Vector2(condition[counter].y, condition[counter].x);
        }
    }

    // IV 더하기
    void Add_IV()
    {
        for (int i = 0; i < condition.Count; i++)
        {
            string iv = ($"\n IV1(W): {condition[i]}");
            IV.text += iv;
        }
    }

    // DV 더하기
    void Add_DV()
    {
        completionTime.Add(timer);
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
                new string[3]
                {
                    condition[i].x.ToString(),
                    condition[i].y.ToString(),
                    completionTime[i].ToString(),
                }
            );
        }
        report_btn.SetActive(false);
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

        if (!testing)
        {
            // 타겟 리셋
            SetTarget();

            // 마우스 리셋
            SetCursor();

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

