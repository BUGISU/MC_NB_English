using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Mission1_GameManager : MonoBehaviour
{
    public static Mission1_GameManager instance { get; private set; }
    [SerializeField] private NarrationManager narrationManager;

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
        mission1_DataManager.GenerateRandomIndexList(); //�ߺ� ���� �ε��� ����Ʈ ����
        OnStart();
    }

    //���� ����
    public void OnStart()
    {
        StopCoroutine(_OnStart());
        StartCoroutine(_OnStart());
    }
    //���� ����
    public void OnEnd()
    {
        StopCoroutine(_OnEnd());
        StartCoroutine(_OnEnd());
    }
   
    //������ ������ ��        
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
            SoundManager.instance.PlayNarration(StringKeys.EN_ANSWER_12);
            mission1_UIManager.Mission1Title.GetComponent<TextMeshProUGUI>().text = StringUtil.KoreanParticle($"�̼� {mission1_DataManager.QuizCount}, {answer_kr}��/�� ã�ƶ�!");
            //StartCoroutine(mission1_UIManager._FindWord(answer_en, answer_kr, mission1_DataManager.QuizCount+1));
            yield return new WaitUntil(() => clear == true);
            clear = false;
        }
        OnEnd();
    }
    IEnumerator _OnEnd()
    {
        StartCoroutine(mission1_UIManager._NextMission()); //���� �̼����� �Ѿ��
        yield return new WaitUntil(() => nextMission == true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Mission2");
        yield return null;
    }
    IEnumerator _OnCorrectAnswer(string answer_en, string answer_kr)
    {
        if (answer_en.Equals("Apple") && tutorial)
        {
            StartCoroutine(mission1_UIManager._CorrectAnswer(answer_en, answer_kr));
        }
        else
        {
            SoundManager.instance.PlaySFX("success01");
            npcController.SetAnimatorTrigger("Correct");
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration("�����Դϴ�!", StringKeys.EN_ANSWER_0));
            clear = true; //������ �����.
        }
            yield return null;
    }

    IEnumerator _OnWrongAnswer(string answer_en, string answer_kr)
    {
        if (answer_en.Equals("Apple") && tutorial)
        {
            StartCoroutine(mission1_UIManager._WrongAnswer(answer_en, answer_kr));
        }
        else
        {
            Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Fail");
            SoundManager.instance.PlaySFX("wrong01");
            yield return CoroutineRunner.instance.RunAndWait("Correct",
                   narrationManager.ShowNarration(StringUtil.KoreanParticle($"�ƽ�����, �װ� {answer_kr}��/�� �ƴϿ���."), StringKeys.EN_ANSWER_4));
        }
         
        yield return null;
    }
  
}
