using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReactionExperimentManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject selectDropdown;
    public GameObject startButtton;
    public GameObject saveButton;
    public GameObject againButton;


    [Header("결과지")]
    public GameObject result_img;
    Text result_txt;
    List<float> reaction_result = new List<float>();


    [Header("네모난 박스")]
    string[] image_label = { "A", "S", "D", "F" };
    public List<Image> image_box = new List<Image>();
    List<Text> image_txt = new List<Text>();
    List<Image> exp_box = new List<Image>();


    [Header("실험 설정")]
    // 실험 설명
    public Text decript;


    // 걍 색상
    Color resetC = Color.white;
    // 선택해야 할 색상
    Color signalC = Color.cyan;


    // 실험 종류 선택하기
    public enum Reaction_Type
    {
        simple,
        choice_2,
        choice_3,
        choice_4,
    }
    public Reaction_Type r_Type;


    // 실험 시작여부 확인
    bool startExp;
    // 기다림 여부 확인
    bool wait;

    public float minTime;
    public float maxTime;

    // 기다리는 시간
    float waitingTime = 0;
    // 반응하는시간
    float reactionTime = 0;
    // 타이머
    float timer = 0;


    // 선택한 상자 ================= [Choice만 해당함!]
    int select_Box;
    // 눌린 키 가져오기
    int pushedKey = -1;
    // 실험 한 횟수
    int trial_number = 0;


    void Start()
    {
        result_img.SetActive(false);
        result_txt = result_img.GetComponentInChildren<Text>();

        wait = false;
        startExp = false;
        decript.enabled = false;

        foreach (Image box in image_box)
        {
            box.enabled = false;
            image_txt.Add(box.transform.GetChild(0).GetComponent<Text>());
        }

        foreach (Text txt in image_txt)
        {
            txt.text = "";
        }

        selectDropdown.SetActive(true);
        startButtton.SetActive(true);
        saveButton.SetActive(false);
        againButton.SetActive(false);
    }


    // 다시 시작하기
    public void OnRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // csv 파일로 저장
    public void Dev_AppendToReport()
    {
        for (int i = 0; i < reaction_result.Count; i++)
        {
            ExportManager.AppendToReport(
                new string[1]
                {
                    reaction_result[i].ToString(),
                }
            );
        }
        saveButton.SetActive(false);
    }

    // 실험 방식 선택 ============================ [드랍다운용]
    public void OnSelectExperiment(int value)
    {
        r_Type = (Reaction_Type)value;
    }

    // 실험 시작 클릭 ============================ [버튼용]
    public void OnStartClick()
    {
        // 필요없는 메뉴 닫기
        selectDropdown.SetActive(false);
        startButtton.SetActive(false);
        decript.enabled = true;

        int idx = image_box.Count / 2;

        switch (r_Type)
        {
            case Reaction_Type.simple:
                image_box[idx].enabled = true;
                image_txt[idx].text = image_label[(int)r_Type];

                // 리스트에 넣기
                exp_box.Add(image_box[idx]);
                break;

            case Reaction_Type.choice_2:
                image_box[idx - 1].enabled = true;
                image_box[idx + 1].enabled = true;

                image_txt[idx - 1].text = image_label[(int)r_Type - 1];
                image_txt[idx + 1].text = image_label[(int)r_Type];

                // 리스트에 넣기
                exp_box.Add(image_box[idx - 1]);
                exp_box.Add(image_box[idx + 1]);
                break;

            case Reaction_Type.choice_3:
                image_box[idx - 2].enabled = true;
                image_box[idx].enabled = true;
                image_box[idx + 2].enabled = true;

                image_txt[idx - 2].text = image_label[(int)r_Type - 2];
                image_txt[idx].text = image_label[(int)r_Type - 1];
                image_txt[idx + 2].text = image_label[(int)r_Type];

                // 리스트에 넣기
                exp_box.Add(image_box[idx - 2]);
                exp_box.Add(image_box[idx]);
                exp_box.Add(image_box[idx + 2]);
                break;

            case Reaction_Type.choice_4:
                image_box[idx - 3].enabled = true;
                image_box[idx - 1].enabled = true;
                image_box[idx + 1].enabled = true;
                image_box[idx + 3].enabled = true;

                image_txt[idx - 3].text = image_label[(int)r_Type - 3];
                image_txt[idx - 1].text = image_label[(int)r_Type - 2];
                image_txt[idx + 1].text = image_label[(int)r_Type - 1];
                image_txt[idx + 3].text = image_label[(int)r_Type];

                // 리스트에 넣기
                exp_box.Add(image_box[idx - 3]);
                exp_box.Add(image_box[idx - 1]);
                exp_box.Add(image_box[idx + 1]);
                exp_box.Add(image_box[idx + 3]);
                break;
        }

        // 기다릴 시간 설정!!!
        SetWaitingTime();
        wait = true;
        startExp = true;
    }



    // 기다리는 시간 설정! 0 ~ 5초
    void SetWaitingTime()
    {
        waitingTime = Random.Range(minTime, maxTime);
    }

    // 키보드 값
    void KeyInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            pushedKey = 0;
            //print(0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            pushedKey = 1;
            //print(1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            pushedKey = 2;
            //print(2);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            pushedKey = 3;
            //print(3);
        }
        else
        {
            pushedKey = -1;
            //print(-1);
        }
    }

    // 결과 출력하기
    void Print_Result()
    {
        for (int i = 0; i < reaction_result.Count; i++)
        {
            string rt = ($"\n {r_Type} - {i}: {reaction_result[i]}");
            print(1);
            result_txt.text += rt;
        }
    }

    // 박스 랜덤 선택!!!
    void SelectRandomBox(int maxValue)
    {
        select_Box = Random.Range(0, maxValue);
    }



    // 실험 시퀀스!
    void Experiment(int value)
    {
        // 1. 일단 먼저 기다리다가...충분히 기다렸다면, 박스 랜덤 선택하기
        if (timer >= waitingTime)
        {
            wait = false;
            timer = 0;
            SelectRandomBox(value + 1);
        }

        // 2. 선택한 박스 가져오기
        Image boxC = exp_box[select_Box];

        // 3-1. 기다려야한다면, 기다림 타이머 돌리기
        if (wait) timer += Time.deltaTime;

        // 3-2. 시작했다면, 반응시간 타이머 돌리고, 박스랑 동일한 버튼을 누르면, 반응 시간 기록하고 리셋하기
        else
        {
            reactionTime += Time.deltaTime;
            boxC.color = signalC;

            // 누른 버튼과 박스가 일치한다면! 기록하고 리셋하기
            if (pushedKey == exp_box.IndexOf(boxC))
            {
                // 반응시간 저장
                reaction_result.Add(reactionTime);
                //print(reactionTime);

                // 반응시간 리셋
                reactionTime = 0;

                // 다시 기다리기
                wait = true;

                // 기다릴 시간 리셋
                SetWaitingTime();

                // 실험 횟수 증가
                trial_number++;

                // 색상 리셋
                boxC.color = resetC;
            }
        }
    }

    void Update()
    {
        // 시작 안했으면 그냥 리턴
        if (!startExp) return;

        KeyInput();
        Experiment((int)r_Type);

        // 30번 했다면, 테스트 끝!
        if (trial_number >= 30)
        {
            Print_Result();
            result_img.SetActive(true);
            saveButton.SetActive(true);
            againButton.SetActive(true);
            startExp = false;
        }
    }
}
