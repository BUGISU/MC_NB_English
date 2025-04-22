using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


//Mission 1: Search for words adventure
public class Mission1_UIManager : MonoBehaviour
{

    [SerializeField] private NarrationManager narrationManager;

    [Header("Private Mission1-Object")]
    [SerializeField] private GameObject Mission1WordGroup;
    public GameObject Mission1Title;

    private Vector3 moveToPos;     // �̵��� ��ġ
    private Vector3 scaleToSize;   // ���� ũ��

    [SerializeField] private Button NextButton; // ���� ��ư
    [SerializeField] private float duration = 1.5f; // �ִϸ��̼� �ð�

    void Awake()
    {
        NextButton.onClick.AddListener(() =>
        {
          SceneManager.LoadScene("Mission2");
        });
    }
    public IEnumerator _Mission1_Start()
    {
        Mission1Title.SetActive(true);
        yield return new WaitForSeconds(1f);
        MoveAndShrink();
        narrationManager.narrationPanel.SetActive(true);
        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("ù ��° �̼��Դϴ�.",StringKeys.EN_MISSION1_0));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("��¦�̴� �ܾ�� �߿���'���'�� ã���ּ���!", StringKeys.EN_MISSION1_1));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("ã�Ҵٸ�, ��¦ ��ġ�� ������!", StringKeys.EN_MISSION1_2));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("�׷�, �غ�Ƴ���?", StringKeys.EN_MISSION1_3));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("���� ������ �����մϴ�!", StringKeys.EN_MISSION1_4));
        Mission1WordGroup.SetActive(true);
        narrationManager.HideDialog();
        narrationManager.narrationPanel.SetActive(false);
        Mission1_GameManager.instance.tutorial = true;
        GameManager.instance.CanTouch = true;
        yield return null;
    }
    public IEnumerator _Mission1_Tutorial()
    {
        Mission1_GameManager.instance.answer_en = "Apple";
        Mission1_GameManager.instance.answer_kr = "���";
        narrationManager.ShowDialog();
        Mission1Title.GetComponent<TextMeshProUGUI>().text = $"�̼� 1, ����� ã�ƶ�!";
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration(StringUtil.KoreanParticle("����� ���ϴ� �ܾ ã�ƺ�����!"), StringKeys.EN_ANSWER_12));
        Mission1WordGroup.SetActive(true);
        narrationManager.HideDialog();
        yield return null;
    }
    public IEnumerator _FindWord(string answer_en, string answer_kr, int QuizNum)
    {
        //Debug.Log("FindWord");
        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration(StringUtil.KoreanParticle($"{answer_kr}��/�� ���ϴ� �ܾ�, {answer_en}�� ã�ƺ�����!"),StringKeys.EN_ANSWER_12));
        Mission1Title.GetComponent<TextMeshProUGUI>().text = StringUtil.KoreanParticle($"�̼� {QuizNum}, {answer_kr}��/�� ã�ƶ�!");
        Mission1WordGroup.SetActive(true);
        narrationManager.HideDialog();
        yield return null;
    }
    public IEnumerator _CorrectAnswer(string answer_en, string answer_kr)
    {
        SoundManager.instance.PlaySFX("success01");
        //SoundManager.instance.PlaySFX("success01");
        narrationManager.ShowDialog();
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Correct");
        if (answer_en.Equals("Apple") && Mission1_GameManager.instance.tutorial)
        {
            Debug.Log("CorrectAnswer_Tutorial");
            yield return CoroutineRunner.instance.RunAndWait("Correct",
               narrationManager.ShowNarration("�����Դϴ�!",StringKeys.EN_ANSWER_0));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
               narrationManager.ShowNarration(StringUtil.KoreanParticle($"{Mission1_GameManager.instance.answer_en} {Mission1_GameManager.instance.answer_kr}��/�� ��Ȯ�ϰ� ã�Ҿ��!"), 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
              narrationManager.ShowNarration($"�� ���߾��!", StringKeys.EN_ANSWER_1));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"�̷������� �ܾ ã�ƺ�����!", StringKeys.EN_ANSWER_2));
            Mission1_GameManager.instance.tutorial = false;
        }
        //else
        //{
        //    Debug.Log("CorrectAnswer_Quiz");
        //    yield return CoroutineRunner.instance.RunAndWait("Correct",
        //        narrationManager.ShowNarration("�����Դϴ�!", StringKeys.EN_ANSWER_0));
        //    yield return CoroutineRunner.instance.RunAndWait("Correct",
        //       narrationManager.ShowNarration(StringUtil.KoreanParticle($"{answer_kr}��/�� ��Ȯ�ϰ� ã�Ҿ��!"), StringKeys.EN_ANSWER_11));
        //    yield return CoroutineRunner.instance.RunAndWait("Correct",
        //      narrationManager.ShowNarration($"�� ���߾��!", StringKeys.EN_ANSWER_1));
        //    Mission1_GameManager.instance.clear = true; //������ �����.
        //}
        narrationManager.HideDialog();
        yield return null;
    }
    public IEnumerator _WrongAnswer(string answer_en, string answer_kr)
    {
        SoundManager.instance.PlaySFX("wrong01");
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Fail");
        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("Correct",
        narrationManager.ShowNarration(StringUtil.KoreanParticle($"�ƽ�����, �װ� {answer_kr}��/�� �ƴϿ���."),StringKeys.EN_ANSWER_4));
        yield return CoroutineRunner.instance.RunAndWait("Correct",
           narrationManager.ShowNarration($"�ٽ� �� �� �� ã�ƺ���� ?", StringKeys.EN_ANSWER_3));
        narrationManager.HideDialog();
        yield return null;
    }
    public IEnumerator _NextMission()
    {
        narrationManager.ShowDialog();
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Jump");
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
        narrationManager.ShowNarration($"������! ù ��° �̼��� �����߽��ϴ�!", StringKeys.EN_MISSION1_5));
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
           narrationManager.ShowNarration($"���� �� �ų��� ������ ��ٸ��� �־��.", StringKeys.EN_MISSION1_6));
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Clear");
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
        narrationManager.ShowNarration($"���� �̼����� ���!", StringKeys.EN_MISSION1_7));
        narrationManager.HideDialog();
        Mission1_GameManager.instance.nextMission = true;
        yield return null;
    }

    private void MoveAndShrink()
    {
        moveToPos = new Vector3(-768, 707, 0);
        scaleToSize = new Vector3(0.4f, 0.4f, 0.4f);
        Sequence sequence = DOTween.Sequence();
        // �̵� Ʈ�� �߰�
        sequence.Join(Mission1Title.GetComponent<Transform>().DOLocalMove(moveToPos, duration)
                             .SetEase(Ease.InOutQuad));
        // ũ�� Ʈ�� �߰�
        sequence.Join(Mission1Title.GetComponent<Transform>().DOScale(scaleToSize, duration)
                             .SetEase(Ease.InOutQuad));
        sequence.OnComplete(() => Debug.Log("�̵��� ũ�� ���� �Ϸ�!"));
    }
}



