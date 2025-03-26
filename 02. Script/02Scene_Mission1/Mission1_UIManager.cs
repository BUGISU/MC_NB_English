using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


//Mission 1: Search for words adventure
public class Mission1_UIManager : MonoBehaviour
{

    [SerializeField] private NarrationManager narrationManager;

    [Header("Private Mission1-Object")]
    [SerializeField] private GameObject Mission1WordGroup;
    [SerializeField] private GameObject Mission1Title;

    private Vector3 moveToPos;     // �̵��� ��ġ
    private Vector3 scaleToSize;   // ���� ũ��

    [SerializeField] private float duration = 1.5f; // �ִϸ��̼� �ð�


    public IEnumerator _Mission1_Start()
    {
        Mission1Title.SetActive(true);
        yield return new WaitForSeconds(1f);
        MoveAndShrink();
        narrationManager.narrationPanel.SetActive(true);
        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("ù ��° �̼��Դϴ�.", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("��¦�̴� �ܾ�� �߿���'����(apple)'�� ã���ּ���!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("ã�Ҵٸ�, ��¦ ��ġ�� ������!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("�׷�, �غ�Ƴ���?", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("���� ������ �����մϴ�!", 1f));
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
           narrationManager.ShowNarration(StringUtil.KoreanParticle("����� ���ϴ� �ܾ�, '����(Apple)'�� ã�ƺ�����!"), 1f));
        Mission1WordGroup.SetActive(true);
        narrationManager.HideDialog();
        yield return null;
    }
    public IEnumerator _FindWord(string answer_en, string answer_kr, int QuizNum)
    {
        //Debug.Log("FindWord");
        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration(StringUtil.KoreanParticle($"{answer_kr}��/�� ���ϴ� �ܾ�, {answer_en}�� ã�ƺ�����!"), 1f));
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
               narrationManager.ShowNarration("�����Դϴ�!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
               narrationManager.ShowNarration(StringUtil.KoreanParticle($"{Mission1_GameManager.instance.answer_en} {Mission1_GameManager.instance.answer_kr}��/�� ��Ȯ�ϰ� ã�Ҿ��!"), 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
              narrationManager.ShowNarration($"�� ���߾��!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"�̷������� �ܾ ã�ƺ�����!", 1f));
            Mission1_GameManager.instance.tutorial = false;
        }
        else
        {
            Debug.Log("CorrectAnswer_Quiz");
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration("�����Դϴ�!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
               narrationManager.ShowNarration(StringUtil.KoreanParticle($"{answer_kr}��/�� ��Ȯ�ϰ� ã�Ҿ��!"), 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
              narrationManager.ShowNarration($"�� ���߾��!", 1f));
            Mission1_GameManager.instance.clear = true; //������ �����.
        }
        narrationManager.HideDialog();
        yield return null;
    }
    public IEnumerator _WrongAnswer(string answer_en, string answer_kr)
    {
        SoundManager.instance.PlaySFX("wrong01");
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Fail");
        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("Correct",
        narrationManager.ShowNarration(StringUtil.KoreanParticle($"�ƽ�����, �װ� {answer_kr}��/�� �ƴϿ���."),1f));
        yield return CoroutineRunner.instance.RunAndWait("Correct",
           narrationManager.ShowNarration($"�ٽ� �� �� �� ã�ƺ���� ?", 1f));
        narrationManager.HideDialog();
        yield return null;

    }
    public IEnumerator _NextMission()
    {
        narrationManager.ShowDialog();
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Jump");
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
        narrationManager.ShowNarration($"������! ù ��° �̼��� �����߽��ϴ�!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
           narrationManager.ShowNarration($"���� �� �ų��� ������ ��ٸ��� �־��.", 1f));
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Clear");
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
        narrationManager.ShowNarration($"���� �̼����� ���!", 1f));
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



