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
    public TextMeshProUGUI[] ChoiceText; //������ �ؽ�Ʈ

    private Vector3 moveToPos;     // �̵��� ��ġ
    private Vector3 scaleToSize;   // ���� ũ��
    private float duration = 1.5f; // �ִϸ��̼� �ð�


    [SerializeField] private NarrationManager narrationManager;
    [SerializeField] private Mission2_DataManager mission2_DataManager;

    //�̼�2 ���� , Ʃ�丮��
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
            narrationManager.ShowNarration("�̹����� �ܾ� ������ �巡���ؼ� ������ �ϼ��� �� �ſ���!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
            narrationManager.ShowNarration("�Ʒ��� �ִ� �ܾ� ������ �巡���ؼ�\n���� �ִ� �� ĭ�� �÷����� �ּ���!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
            narrationManager.ShowNarration("�ܾ ������� �� �����ϰ� ������ ������!", 1f));
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
            narrationManager.ShowNarration("ù ��° �ܾ�� �����?\n�巡���ؼ� �� ĭ�� �־� ������!", 1f));
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
           narrationManager.ShowNarration("Great job!\n���� ���� ���� ������ ���� ���� �ܾ�� ������ ������.", 1f));
        WizardNPC_Animator.SetAnimatorTrigger("HI");
        yield return CoroutineRunner.instance.RunAndWait("mission2_cut1",
         narrationManager.ShowNarration("���� ���迡�� �� ������!", 1f));
        narrationManager.HideDialog();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Introduction");
    }
    //�̼� Ÿ��Ʋ �̵� �� ���
    private void MoveAndShrink()
    {
        moveToPos = new Vector3(-842, 677, 0);
        scaleToSize = new Vector3(0.3f, 0.3f, 0.3f);
        Sequence sequence = DOTween.Sequence();
        // �̵� Ʈ�� �߰�
        sequence.Join(Mission2Title.GetComponent<Transform>().DOLocalMove(moveToPos, duration)
                             .SetEase(Ease.InOutQuad));
        // ũ�� Ʈ�� �߰�
        sequence.Join(Mission2Title.GetComponent<Transform>().DOScale(scaleToSize, duration)
                             .SetEase(Ease.InOutQuad));
    }
    //������ ���̱�
    private void Show_BlackBoard()
    {
        BlackBoard.SetActive(true);
        scaleToSize = new Vector3(1f, 1f, 1f);
        Sequence sequence = DOTween.Sequence();
        sequence.Join(BlackBoard.GetComponent<Transform>().DOScale(scaleToSize, duration)
                            .SetEase(Ease.InOutQuad));
        //�̼� ����
        mission2_DataManager.SetupQuiz();
    }

    public IEnumerator _CorrectAnswer()
    {
        SoundManager.instance.PlaySFX("success01");
        narrationManager.ShowDialog();
        if (Mission2_GameManager.instance.tutorial)
        {
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration("�����Դϴ�!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"�Ϻ��� ������ �Ǿ����!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"�̷������� ���� ������ ��������!", 1f));
            Mission2_GameManager.instance.tutorial = false;
        }
        else
        {
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration("�����Դϴ�!", 1f));
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"�Ϻ��� ������ �Ǿ����!", 1f));
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
            narrationManager.ShowNarration($"��... ���� �ٸ� �ܾ �� �� ��︱ �� ���ƿ�!", 1f));
        }
        else
        {
            yield return CoroutineRunner.instance.RunAndWait("Correct",
            narrationManager.ShowNarration($"��, ������ �ٽ� �����غ����?", 1f));
        }
        narrationManager.HideDialog();
        yield return null;
    }
}
