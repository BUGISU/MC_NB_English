using System.Collections;
using UnityEngine;


public class Mission2_GameManager : MonoBehaviour
{
    public static Mission2_GameManager instance { get; private set; }
    [SerializeField] Mission2_DataManager mission2_DataManager;
    [SerializeField] Mission2_UIManager mission2_UIManager;
    [SerializeField] private NarrationManager narrationManager;
    public bool tutorial = false;
    public bool clear = false;

    public string currentAnswer = "";

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        Init(); //�ʱ�ȭ
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
        mission2_DataManager.GenerateRandomQuizList(); //��ü �������� �������� ���� 5�� �̱�
    }
    IEnumerator _OnStart()
    {
        yield return StartCoroutine(mission2_UIManager._Mission2_Start());
        yield return new WaitUntil(() => clear == true);
        clear = false;
        while (mission2_DataManager.currentQuizIndex < 5)
        {
            mission2_DataManager.randomQuizObjects[mission2_DataManager.currentQuizIndex - 1].SetActive(false); //���� ������ ��Ȱ��ȭ
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
        if (tutorial)
        {
            StartCoroutine(mission2_UIManager._CorrectAnswer());
        }
        else
        {
            SoundManager.instance.PlaySFX("success01");
            yield return CoroutineRunner.instance.RunAndWait("Correct",
narrationManager.ShowNarration("�����Դϴ�!", StringKeys.EN_ANSWER_0));
            Mission2_GameManager.instance.clear = true;
        }
        mission2_DataManager.currentQuizIndex++;
        yield return null;
    }
    IEnumerator _OnWrongAnswer()
    {
        if (Mission2_GameManager.instance.tutorial)
        {
            StartCoroutine(mission2_UIManager._WrongAnswer());
        }
        else
        {
            SoundManager.instance.PlaySFX("wrong01");
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                yield return CoroutineRunner.instance.RunAndWait("Correct",
                narrationManager.ShowNarration($"��... ���� �ٸ� �ܾ �� �� ��︱ �� ���ƿ�!", StringKeys.EN_ANSWER_7));
            }
            else
            {
                yield return CoroutineRunner.instance.RunAndWait("Correct",
                narrationManager.ShowNarration($"������ �ٽ� �����غ����?", StringKeys.EN_ANSWER_8));
            }
            yield return null;
        }
        yield return null;
    }


}
