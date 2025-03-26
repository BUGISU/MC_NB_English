using System.Collections;
using UnityEngine;


public class Mission2_GameManager : MonoBehaviour
{
    public static Mission2_GameManager instance { get; private set; }
    [SerializeField] Mission2_DataManager mission2_DataManager;
    [SerializeField] Mission2_UIManager mission2_UIManager;

    public bool tutorial = false;
    public bool clear = false;

    public string currentAnswer = "";

    void Awake()
    {
        if (instance == null)instance = this;
        else Destroy(gameObject);
        Init(); //초기화
    }
    private void Start()
    {
        OnStart();
    }
    private void OnStart()
    {
        StopAllCoroutines();
        StartCoroutine(_OnStart());
    }
    private void OnEnd()
    {
        StopAllCoroutines();
        StartCoroutine(_OnEnd());
    }
    public void OnCorrectAnswer()
    {
        StopCoroutine(_OnCorrectAnswer());
        StartCoroutine(_OnCorrectAnswer());
    }
    public void OnWrongAnswer()
    {
        StopCoroutine(_OnWrongAnswer());
        StartCoroutine(_OnWrongAnswer());
    }
    private void Init()
    {
        mission2_DataManager.GenerateRandomQuizList(); //전체 문제에서 랜덤으로 문제 5개 뽑기
    }
    IEnumerator _OnStart()
    {
        yield return StartCoroutine(mission2_UIManager._Mission2_Start());  
        yield return new WaitUntil(() => clear == true);
        clear = false;
        while (mission2_DataManager.currentQuizIndex <5)
        {
            mission2_DataManager.randomQuizObjects[mission2_DataManager.currentQuizIndex-1].SetActive(false); //이전 문제는 비활성화
            mission2_DataManager.SetupQuiz();
            yield return new WaitUntil(() => clear == true);
            clear = false;
        }
        OnEnd();
        yield return null;
    }
    IEnumerator _OnEnd()
    {
        yield return StartCoroutine(mission2_UIManager._Mission2_Clear());
        yield return null;
    }
    IEnumerator _OnCorrectAnswer()
    {
        StartCoroutine(mission2_UIManager._CorrectAnswer());
        mission2_DataManager.currentQuizIndex++;
        yield return null;
    }
    IEnumerator _OnWrongAnswer()
    {
        StartCoroutine(mission2_UIManager._WrongAnswer());
        yield return null;
    }


}
