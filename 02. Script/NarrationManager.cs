using System.Collections;
using TMPro; // TextMeshPro�� ����Ϸ��� �ʿ�!
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class NarrationManager : MonoBehaviour
{
    public static bool isTyping = false;
    public int narrationIndex = 0;

    public CanvasGroup narrationCanvasGroup;

    [Header("Title UI")]
    public GameObject TitleImage;
    public TextMeshProUGUI TitleText;
    

    [Header("Dialog UI")]
    public GameObject narrationPanel; // �����̼� �г�
    private Button narrationNextBtn; // ���� ��ư
    private RectTransform narrationRecT;
    public TextMeshProUGUI narrationText; // �����̼� �ؽ�Ʈ

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        narrationNextBtn = narrationPanel.GetComponent<Button>();
        narrationNextBtn.onClick.AddListener(() =>
        {
            isTyping = false;
        });
        narrationIndex = 0;
        narrationRecT = narrationPanel.GetComponent<RectTransform>();
        ResetDialog();
    }
    public void ShowDialog()
    {
        ResetDialog(); // �ؽ�Ʈ �ʱ�ȭ
        narrationPanel.SetActive(true);
        Vector3 worldTargetPos = narrationPanel.transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(narrationCanvasGroup.DOFade(1, 0.3f));
        sequence.Join(narrationRecT.DOMove(worldTargetPos, 0.5f).SetEase(Ease.OutQuad));
        sequence.Join(narrationRecT.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetEase(Ease.OutBack));
    }

    public void HideDialog()
    {
        DOTween.Kill(narrationText);  // �ؽ�Ʈ �ִϸ��̼� �ߴ�
        narrationText.text = "";
        Vector3 worldTargetPos = narrationPanel.transform.position;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(narrationCanvasGroup.DOFade(0, 0.3f));
        sequence.Join(narrationRecT.DOMove(worldTargetPos, 0.5f).SetEase(Ease.InQuad));
        sequence.Join(narrationRecT.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack));
        narrationPanel.SetActive(false);
    }

    public void ResetDialog()
    {
        DOTween.Kill(narrationText);
        narrationText.text = "";
        isTyping = false;
    }

    public IEnumerator ShowNarration(string text, float duration)
    {
        isTyping = true;
        DOTween.Kill(narrationText); // ���� �ִϸ��̼� �ߴ�
        narrationText.text = "";

        narrationText.DOText(text, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                //isTyping = false; 
                narrationIndex += 1;
            });

        yield return new WaitUntil(() => isTyping == false);
    }
}
