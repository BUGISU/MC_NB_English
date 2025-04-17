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
    private bool isIntroStarted = false; // 중복 호출 방지 플래그

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
        startButton.onClick.RemoveAllListeners();   // 기존 이벤트 제거 후 추가 (중복 방지)
        startButton.onClick.AddListener(Intro_Start);
    }

    private void InitBloom()
    {
        if (volume == null)
        {
            Debug.LogError("Volume이 비어있습니다! Hierarchy에서 Global Volume을 연결하세요.");
            return;
        }

        if (!volume.profile.TryGet(out bloom))
        {
            Debug.LogError("Volume에 Bloom이 없습니다! Volume Profile에서 Bloom 추가 후 연결하세요.");
            return;
        }

        bloom.intensity.overrideState = true;
        bloom.intensity.value = bloomStartIntensity;
    }

    private void Intro_Start()
    {
        if (isIntroStarted)
        {
            Debug.LogWarning("Intro_Start가 이미 실행 중입니다!");
            return;
        }

        isIntroStarted = true;

        // 버튼 비활성화 또는 클릭 불가 상태로 전환 (UI 관리)
        startButton.interactable = false;

        if (cameraMoveCoroutine != null)
            StopCoroutine(cameraMoveCoroutine);

        CoroutineRunner.instance.StopAll(); // 전역 코루틴 멈춤

        StartCoroutine(_Intro_Start());
    }

    private IEnumerator _Intro_Start()
    {
        titlePanel.SetActive(false);
        narrationManager.narrationPanel.SetActive(true);
        yield return new WaitForSeconds(1f);

        npcController.SetAnimatorTrigger("HI");
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
            narrationManager.ShowNarration("안녕하세요, 모험가님!", StringKeys.EN_INTRO_0), 5f);
        yield return CoroutineRunner.instance.RunAndWait("mission1_cut1",
            narrationManager.ShowNarration("지금부터 영어 마법 세계로 함께 떠나볼까요?", StringKeys.EN_INTRO_1), 5f);

        narrationManager.narrationPanel.SetActive(false);
        npcController.HideNpcCharacter();

        CameraMove();

        yield return new WaitForSeconds(1f);

        SparkleVFX.SetActive(true);

        AnimateBloomValue();

        yield return new WaitForSeconds(2f);

        // 씬 이동! (씬 이동 이후는 보통 별도 로딩씬이 관리하는 게 안전함)
        GameManager.instance.OnLoadScene("Mission1");

        // 이후 로직은 필요에 따라 초기화
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
            Debug.LogWarning("Bloom이 초기화되지 않았습니다! InitBloom 호출 필요.");
            return;
        }

        Debug.Log("Bloom 애니메이션 시작!");

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
