using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Mission1_GameManager : MonoBehaviour
{
    public static Mission1_GameManager instance { get; private set; }
   
    [SerializeField] Mission1_UIManager mission1_UIManager;
    [SerializeField] Mission1_DataManager mission1_DataManager;
    public NPCController npcController;

    public string answer_kr;
    public string answer_en;
    public bool tutorial = false;
    public bool clear = false;
    public bool nextMission = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        mission1_DataManager.GenerateRandomIndexList(); //중복 없는 인덱스 리스트 생성
        OnStart();
    }

    //게임 시작
    public void OnStart()
    {
        StopCoroutine(_OnStart());
        StartCoroutine(_OnStart());
    }
    //게임 종료
    public void OnEnd()
    {
        StopCoroutine(_OnEnd());
        StartCoroutine(_OnEnd());
    }
   
    //정답을 맞췄을 때        
    public void OnCorrectAnswer()
    {
        StopCoroutine(_OnCorrectAnswer(answer_en,answer_kr));
        StartCoroutine(_OnCorrectAnswer(answer_en, answer_kr));
    }
    public void OnWrongAnswer()
    {
       
        StopCoroutine(_OnWrongAnswer(answer_en, answer_kr));
        StartCoroutine(_OnWrongAnswer(answer_en, answer_kr));
    }
    IEnumerator _OnStart()
    {
        StartCoroutine(mission1_UIManager._Mission1_Start());
        yield return new WaitUntil(() => tutorial == true);
        StartCoroutine(mission1_UIManager._Mission1_Tutorial());
        yield return new WaitUntil(() => tutorial == false);
        while (mission1_DataManager.QuizCount < mission1_DataManager.word_list_en.Length-1)
        {
            mission1_DataManager.NextQuize_SetData();
            StartCoroutine(mission1_UIManager._FindWord(answer_en, answer_kr, mission1_DataManager.QuizCount+1));
            yield return new WaitUntil(() => clear == true);
            clear = false;
        }
        OnEnd();
    }
    IEnumerator _OnEnd()
    {
        StartCoroutine(mission1_UIManager._NextMission()); //다음 미션으로 넘어가기
        yield return new WaitUntil(() => nextMission == true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Mission2");
        yield return null;
    }
    IEnumerator _OnCorrectAnswer(string answer_en, string answer_kr)
    {
        StartCoroutine(mission1_UIManager._CorrectAnswer(answer_en, answer_kr));
        yield return null;
    }

    IEnumerator _OnWrongAnswer(string answer_en, string answer_kr)
    {
        
        StartCoroutine(mission1_UIManager._WrongAnswer(answer_en, answer_kr));
        yield return null;
    }
  
}
