using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class TouchObjectDetector : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject selectedObject; // ���� ���õ� ������Ʈ, ui�������� �����
    public LayerMask targetLayer;

    public float rayDistance = 100f;
    private float zPosition; // ������Ʈ�� Z�� ��ġ�� ����

    private string sceneName;

    private bool isDragging = false; // �巡�� ������ ����

    private Vector3 offset; // ��ġ ���� �� ��ġ ���� ����
    private Vector3 objOriginPos; // ������Ʈ�� ���� ��ġ�� ����
    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
                Debug.LogError("MainCamera�� �����ϴ�! ī�޶� �Ҵ��ϼ���.");
        }
        sceneName = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if (GameManager.instance.CanTouch)
        {
#if UNITY_EDITOR
            HandleMouseInput();
#endif
            HandleTouchInput();
        }
    }

    // ���콺 Ŭ����
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 Ŭ�� ����
        {
            Vector2 mousePosition = Input.mousePosition;
            DetectObject(mousePosition);
        }

        if (Input.GetMouseButton(0) && isDragging) // ���콺�� �巡�� ���� ��
        {
            MoveObject(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
        }
    }
    // ����̽� ��ġ��
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                DetectObject(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                MoveObject(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                StopDragging();
            }
        }
    }

    private void DetectObject(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red, 1f);
        if (Physics.Raycast(ray, out hit, rayDistance, targetLayer == 0 ? ~0 : targetLayer)) // ~0 == ��� ���̾�
        {
            if (sceneName == "Mission1")
            {
                Mission1_Detect(hit);
            }
            else if (sceneName == "Mission2")
            {
                Mission2_Detect(hit, screenPosition);
            }
        }
        else
        {
            Debug.Log("��Ʈ�� ������Ʈ�� �����ϴ�.");
        }
    }

    // �̼�1���� ����ϴ� ��ġ�Լ�
    private void Mission1_Detect(RaycastHit hit)
    {
        GameObject target = hit.collider.gameObject;
        if (target.CompareTag("alphabet") && !NarrationManager.isTyping)
        {
            target.transform.DOScale(target.transform.localScale * 0.9f, 0.1f).SetLoops(2, LoopType.Yoyo);

            if (target.name == Mission1_GameManager.instance.answer_en)
            {
                Debug.Log("�����Դϴ�!");
                TouchSelf touchSelf = target.GetComponent<TouchSelf>();
                if (touchSelf != null)
                {
                    touchSelf.OnClick_Correct();
                }
            }
            else
            {
                Debug.Log("������ �ƴ� ������Ʈ�Դϴ�.");
                TouchSelf touchSelf = target.GetComponent<TouchSelf>();
                if (touchSelf != null)
                {
                    touchSelf.OnClick_Wrong();
                }
            }
        }
    }

    // �̼�2���� ����ϴ� ��ġ�Լ�
    private void Mission2_Detect(RaycastHit hit, Vector2 screenPosition)
    {
        selectedObject = hit.collider.gameObject;

        objOriginPos = selectedObject.transform.position;
        zPosition = selectedObject.transform.position.z;

        TouchSelf touchSelf = selectedObject.GetComponent<TouchSelf>();
        if (selectedObject.CompareTag("alphabet") && selectedObject != null)
        {
            //Debug.Log($"������Ʈ ������! �̸�: {selectedObject.name}");
            isDragging = true;
            Vector3 worldPosition = GetWorldPosition(screenPosition);
            offset = selectedObject.transform.position - worldPosition;
        }
    }

    private void MoveObject(Vector2 screenPosition)
    {
        if (selectedObject == null)
        {
            Debug.LogWarning("MoveObject ���� �� selectedObject�� �����ϴ�!");
            return;
        }

        Vector3 newWorldPosition = GetWorldPosition(screenPosition) + offset;
        selectedObject.transform.position = new Vector3(newWorldPosition.x, newWorldPosition.y, zPosition);
    }

    private void StopDragging()
    {
        isDragging = false;
        if (selectedObject == null)
        {
            Debug.LogWarning("StopDragging ȣ�� �� selectedObject�� �����ϴ�!");
            return;
        }
        // ������Ʈ�� ���� ��ġ�� �̵���Ŵ
        selectedObject.transform.DOMove(objOriginPos, 0.15f)
            .OnComplete(() =>
            {
                // �巡�װ� ���� �� BoxCollider �ٽ� Ȱ��ȭ!
                var collider = selectedObject.GetComponent<BoxCollider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
                // �ʿ信 ���� selectedObject �ʱ�ȭ
                selectedObject = null;
            });
    }


    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, zPosition));

        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        Debug.LogWarning("GetWorldPosition ��� ����");
        return Vector3.zero;
    }

    IEnumerator DelayBetween()
    {
        yield return new WaitForSeconds(0.3f);
        //selectedObject.GetComponent<TouchSelf>().OnClick(); // ������ �̺�Ʈ ���� ����
    }
}
