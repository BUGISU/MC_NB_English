using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Introduction_UIManager : MonoBehaviour
{
    [SerializeField] private NarrationManager narrationManager;
    public NPCController npcController;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI touchScreen;
    [SerializeField] private TextMeshProUGUI narrationText;

    [Header("Button")]
    [SerializeField] private Button startButton;

    [Header("GameObject")]
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject SparkleVFX;

    [Header("Wave Effect Variable")]
    [SerializeField] private float amplitude = 10f;
    [SerializeField] private float delayBetweenChars = 0.1f;
    private TMP_TextInfo textInfo;
    private Vector3[][] originalVertices;

    [Header("Bloom Effect Settings")]
    public Volume volume;
    private Bloom bloom;
    public float bloomStartIntensity = 0f;
    public float bloomEndIntensity = 20f;
    public float bloomTweenDuration = 2f;

    private float moveAmount = 30f;
    private float duration = 1f;
    private float camDuration = 1.5f;
    private float blinkDuration = 1.5f;

    private Coroutine cameraMoveCoroutine;
    private bool isIntroStarted = false; // �ߺ� ȣ�� ���� �÷���

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        InitBloom();
        StartWaveAnimation();
        Blink_TouchScreen();
    }

    private void Init()
    {
        narrationText.text = "";
        startButton.onClick.RemoveAllListeners();   // ���� �̺�Ʈ ���� �� �߰� (�ߺ� ����)
        startButton.onClick.AddListener(Intro_Start);
    }

    private void InitBloom()
    {
        if (volume == null)
        {
            Debug.LogError("Volume�� ����ֽ��ϴ�! Hierarchy���� Global Volume�� �����ϼ���.");
            return;
        }

        if (!volume.profile.TryGet(out bloom))
        {
            Debug.LogError("Volume�� Bloom�� �����ϴ�! Volume Profile���� Bloom �߰� �� �����ϼ���.");
            return;
        }

        bloom.intensity.overrideState = true;
        bloom.intensity.value = bloomStartIntensity;
    }

    private void Intro_Start()
    {
        if (isIntroStarted)
        {
            Debug.LogWarning("Intro_Start�� �̹� ���� ���Դϴ�!");
            return;
        }

        isIntroStarted = true;

        // ��ư ��Ȱ��ȭ �Ǵ� Ŭ�� �Ұ� ���·� ��ȯ (UI ����)
        startButton.interactable = false;

        if (cameraMoveCoroutine != null)
            StopCoroutine(cameraMoveCoroutine);

        CoroutineRunner.instance.StopAll(); // ���� �ڷ�ƾ ����

        StartCoroutine(_Intro_Start());
    }

    private IEnumerator _Intro_Start()
    {
        titlePanel.SetActive(false);
        narrationManager.narrationPanel.SetActive(true);
        yield return new WaitForSeconds(1f);

        npcController.SetAnimatorTrigger("HI");
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
            narrationManager.ShowNarration("�ȳ��ϼ���, ���谡��!", StringKeys.EN_INTRO_0), 5f);
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
            narrationManager.ShowNarration("���ݺ��� ���� ���� ����� �Բ� ���������?", StringKeys.EN_INTRO_1), 5f);

        narrationManager.narrationPanel.SetActive(false);
        npcController.HideNpcCharacter();

        CameraMove();

        yield return new WaitForSeconds(1f);

        SparkleVFX.SetActive(true);

        AnimateBloomValue();

        yield return new WaitForSeconds(2f);

        // �� �̵�! (�� �̵� ���Ĵ� ���� ���� �ε����� �����ϴ� �� ������)
        GameManager.instance.OnLoadScene("Mission1");

        // ���� ������ �ʿ信 ���� �ʱ�ȭ
    }
    private void CameraMove()
    {
        if (cameraMoveCoroutine != null)
            StopCoroutine(cameraMoveCoroutine);

        cameraMoveCoroutine = StartCoroutine(_CameraMove());
    }

    private IEnumerator _CameraMove()
    {
        mainCamera.transform.DORotate(new Vector3(-4.189f, -12.595f, -0.22f), camDuration)
            .SetEase(Ease.InOutQuad);

        mainCamera.transform.DOMove(new Vector3(5.94f, 2.2f, 3.02f), camDuration)
            .SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(camDuration);
    }

    private void AnimateBloomValue()
    {
        if (bloom == null)
        {
            Debug.LogWarning("Bloom�� �ʱ�ȭ���� �ʾҽ��ϴ�! InitBloom ȣ�� �ʿ�.");
            return;
        }

        Debug.Log("Bloom �ִϸ��̼� ����!");

        DOTween.To(
            () => bloom.intensity.value,
            x => bloom.intensity.value = x,
            bloomEndIntensity,
            bloomTweenDuration
        )
        .SetEase(Ease.Linear);
    }

    private void Blink_TouchScreen()
    {
        touchScreen.DOFade(0, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    private void StartWaveAnimation()
    {
        TMP_Text tmpText = titleText;
        tmpText.ForceMeshUpdate();
        textInfo = tmpText.textInfo;

        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] = textInfo.meshInfo[i].vertices.Clone() as Vector3[];
        }

        StartCoroutine(WaveCoroutine(tmpText));
    }

    private IEnumerator WaveCoroutine(TMP_Text tmpText)
    {
        while (true)
        {
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                AnimateSingleChar(tmpText, i);
                yield return new WaitForSeconds(delayBetweenChars);
            }
        }
    }

    private void AnimateSingleChar(TMP_Text tmpText, int charIndex)
    {
        int materialIndex = textInfo.characterInfo[charIndex].materialReferenceIndex;
        int vertexIndex = textInfo.characterInfo[charIndex].vertexIndex;

        Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
        Vector3[] original = originalVertices[materialIndex];

        DOTween.Kill($"CharTween_{charIndex}");

        DOTween.Sequence()
            .Append(DOVirtual.Float(0, amplitude, duration / 2f, (value) =>
            {
                Vector3 offset = new Vector3(0, value, 0);
                vertices[vertexIndex + 0] = original[vertexIndex + 0] + offset;
                vertices[vertexIndex + 1] = original[vertexIndex + 1] + offset;
                vertices[vertexIndex + 2] = original[vertexIndex + 2] + offset;
                vertices[vertexIndex + 3] = original[vertexIndex + 3] + offset;

                UpdateMesh(tmpText, materialIndex, vertices);
            }))
            .Append(DOVirtual.Float(amplitude, 0, duration / 2f, (value) =>
            {
                Vector3 offset = new Vector3(0, value, 0);
                vertices[vertexIndex + 0] = original[vertexIndex + 0] + offset;
                vertices[vertexIndex + 1] = original[vertexIndex + 1] + offset;
                vertices[vertexIndex + 2] = original[vertexIndex + 2] + offset;
                vertices[vertexIndex + 3] = original[vertexIndex + 3] + offset;

                UpdateMesh(tmpText, materialIndex, vertices);
            }))
            .SetId($"CharTween_{charIndex}");
    }

    private void UpdateMesh(TMP_Text tmpText, int materialIndex, Vector3[] vertices)
    {
        tmpText.textInfo.meshInfo[materialIndex].mesh.vertices = vertices;
        tmpText.UpdateGeometry(tmpText.textInfo.meshInfo[materialIndex].mesh, materialIndex);
    }
}
