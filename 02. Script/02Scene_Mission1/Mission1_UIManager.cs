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

    private Vector3 moveToPos;     // 이동할 위치
    private Vector3 scaleToSize;   // 최종 크기

    [SerializeField] private float duration = 1.5f; // 애니메이션 시간


    public IEnumerator _Mission1_Start()
    {
        Mission1Title.SetActive(true);
        yield return new WaitForSeconds(1f);
        MoveAndShrink();
        narrationManager.narrationPanel.SetActive(true);
        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("첫 번째 미션입니다.", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("반짝이는 단어들 중에서'애플(apple)'을 찾아주세요!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("찾았다면, 살짝 터치해 보세요!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("그럼, 준비됐나요?", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration("영어 모험을 시작합니다!", 1f));
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
        Mission1_GameManager.instance.answer_kr = "사과";
        narrationManager.ShowDialog();
        Mission1Title.GetComponent<TextMeshProUGUI>().text = $"미션 1, 사과를 찾아라!";
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration(StringUtil.KoreanParticle("사과를 뜻하는 단어, '애플(Apple)'를 찾아보세요!"), 1f));
        Mission1WordGroup.SetActive(true);
        narrationManager.HideDialog();
        yield return null;
    }
    public IEnumerator _FindWord(string answer_en, string answer_kr, int QuizNum)
    {
        //Debug.Log("FindWord");
        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
           narrationManager.ShowNarration(StringUtil.KoreanParticle($"{answer_kr}을/를 뜻하는 단어, {answer_en}를 찾아보세요!"), 1f));
        Mission1Title.GetComponent<TextMeshProUGUI>().text = StringUtil.KoreanParticle($"미션 {QuizNum}, {answer_kr}을/를 찾아라!");
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
               narrationManager.ShowNarration("정답입니다!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
               narrationManager.ShowNarration(StringUtil.KoreanParticle($"{Mission1_GameManager.instance.answer_en} {Mission1_GameManager.instance.answer_kr}을/를 정확하게 찾았어요!"), 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
              narrationManager.ShowNarration($"참 잘했어요!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"이런식으로 단어를 찾아보세요!", 1f));
            Mission1_GameManager.instance.tutorial = false;
        }
        else
        {
            Debug.Log("CorrectAnswer_Quiz");
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration("정답입니다!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
               narrationManager.ShowNarration(StringUtil.KoreanParticle($"{answer_kr}을/를 정확하게 찾았어요!"), 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
              narrationManager.ShowNarration($"참 잘했어요!", 1f));
            Mission1_GameManager.instance.clear = true; //정답을 맞췄다.
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
        narrationManager.ShowNarration(StringUtil.KoreanParticle($"아쉽지만, 그건 {answer_kr}이/가 아니에요."),1f));
        yield return CoroutineRunner.instance.RunAndWait("Correct",
           narrationManager.ShowNarration($"다시 한 번 잘 찾아볼까요 ?", 1f));
        narrationManager.HideDialog();
        yield return null;

    }
    public IEnumerator _NextMission()
    {
        narrationManager.ShowDialog();
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Jump");
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
        narrationManager.ShowNarration($"멋져요! 첫 번째 미션을 성공했습니다!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
           narrationManager.ShowNarration($"이제 더 신나는 모험이 기다리고 있어요.", 1f));
        Mission1_GameManager.instance.npcController.SetAnimatorTrigger("Clear");
        yield return CoroutineRunner.instance.RunAndWait("FinishMission",
        narrationManager.ShowNarration($"다음 미션으로 출발!", 1f));
        narrationManager.HideDialog();
        Mission1_GameManager.instance.nextMission = true;
        yield return null;
    }

    private void MoveAndShrink()
    {
        moveToPos = new Vector3(-768, 707, 0);
        scaleToSize = new Vector3(0.4f, 0.4f, 0.4f);
        Sequence sequence = DOTween.Sequence();
        // 이동 트윈 추가
        sequence.Join(Mission1Title.GetComponent<Transform>().DOLocalMove(moveToPos, duration)
                             .SetEase(Ease.InOutQuad));
        // 크기 트윈 추가
        sequence.Join(Mission1Title.GetComponent<Transform>().DOScale(scaleToSize, duration)
                             .SetEase(Ease.InOutQuad));
        sequence.OnComplete(() => Debug.Log("이동과 크기 조정 완료!"));
    }
}



