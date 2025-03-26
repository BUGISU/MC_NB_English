using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mission2_UIManager : MonoBehaviour
{
    public GameObject[] WordGameObejcts;

    [SerializeField] private GameObject Mission2Title;
    [SerializeField] private GameObject BlackBoard;
    [SerializeField] private GameObject ClearParticles;
    [SerializeField] private NPCController WizardNPC_Animator;

    public GameObject MissionClear;
    public TextMeshProUGUI[] ChoiceText; //선택지 텍스트

    private Vector3 moveToPos;     // 이동할 위치
    private Vector3 scaleToSize;   // 최종 크기
    private float duration = 1.5f; // 애니메이션 시간


    [SerializeField] private NarrationManager narrationManager;
    [SerializeField] private Mission2_DataManager mission2_DataManager;

    //미션2 시작 , 튜토리얼
    public IEnumerator _Mission2_Start()
    {
        Mission2Title.SetActive(true);
        BlackBoard.GetComponent<Transform>().DOLocalMove(new Vector3(0, 0, 0), 2f)
                                .SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(2f);
        MoveAndShrink();
        Show_BlackBoard();

        narrationManager.ShowDialog();
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
            narrationManager.ShowNarration("이번에는 단어 조각을 드래그해서 문장을 완성해 볼 거예요!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
            narrationManager.ShowNarration("아래에 있는 단어 조각을 드래그해서\n위에 있는 빈 칸에 올려놓아 주세요!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
            narrationManager.ShowNarration("단어를 순서대로 잘 생각하고 움직여 보세요!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
            narrationManager.ShowNarration("첫 번째 단어는 뭘까요?\n드래그해서 빈 칸에 넣어 보세요!", 1f));
        narrationManager.HideDialog();
        GameManager.instance.CanTouch = true;
        yield return null;
    }
    public IEnumerator _Mission2_Clear()
    {
        GameObject wizar = WizardNPC_Animator.gameObject;
        wizar.SetActive(true);
        wizar.GetComponent<Transform>().DOLocalMove(new Vector3(wizar.transform.position.x, -3.9f,wizar.transform.position.z), 0.5f);
        narrationManager.ShowDialog();
        ClearParticles.SetActive(true);
        WizardNPC_Animator.SetAnimatorTrigger("Jump");
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
           narrationManager.ShowNarration("Great job!\n오늘 영어 마법 모험을 통해 멋진 단어와 문장을 배웠어요.", 1f));
        WizardNPC_Animator.SetAnimatorTrigger("HI");
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
         narrationManager.ShowNarration("다음 모험에서 또 만나요!", 1f));
        narrationManager.HideDialog();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Introduction");
    }
    //미션 타이틀 이동 및 축소
    private void MoveAndShrink()
    {
        moveToPos = new Vector3(-842, 677, 0);
        scaleToSize = new Vector3(0.3f, 0.3f, 0.3f);
        Sequence sequence = DOTween.Sequence();
        // 이동 트윈 추가
        sequence.Join(Mission2Title.GetComponent<Transform>().DOLocalMove(moveToPos, duration)
                             .SetEase(Ease.InOutQuad));
        // 크기 트윈 추가
        sequence.Join(Mission2Title.GetComponent<Transform>().DOScale(scaleToSize, duration)
                             .SetEase(Ease.InOutQuad));
    }
    //블랙보드 보이기
    private void Show_BlackBoard()
    {
        BlackBoard.SetActive(true);
        scaleToSize = new Vector3(1f, 1f, 1f);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(BlackBoard.GetComponent<Transform>().DOScale(scaleToSize, duration)
                            .SetEase(Ease.InOutQuad));
        //미션 셋팅
        mission2_DataManager.SetupQuiz();
    }

    public IEnumerator _CorrectAnswer()
    {
        SoundManager.instance.PlaySFX("success01");
        narrationManager.ShowDialog();
        if (Mission2_GameManager.instance.tutorial)
        {
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration("정답입니다!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"완벽한 문장이 되었어요!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"이런식으로 멋진 문장을 만들어봐요!", 1f));
            Mission2_GameManager.instance.tutorial = false;
        }
        else
        {
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration("정답입니다!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"완벽한 문장이 되었어요!", 1f));
        }
        narrationManager.HideDialog();
        Mission2_GameManager.instance.clear = true;
        yield return null;
    }

    public IEnumerator _WrongAnswer()
    {
        SoundManager.instance.PlaySFX("wrong01");
        narrationManager.ShowDialog();
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"흠... 여긴 다른 단어가 더 잘 어울릴 것 같아요!", 1f));
        }
        else
        {
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"앗, 순서를 다시 생각해볼까요?", 1f));
        }
        narrationManager.HideDialog();
        yield return null;
    }
}
