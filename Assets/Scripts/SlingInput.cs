using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.Touch;

public class SlingInput : MonoBehaviour
{
    private Collider2D touchCollider2D;
    private Vector2 touchStartPosition;
    [SerializeField] private Transform slingStartPosition;
    

    private bool isTouchStarted = false;
    private float maxPowerDrag = 2.5f;

    [SerializeField] GameObject tempThrowablePrefab;
    private GameObject tempThrowable;

    private void Awake()
    {
        touchCollider2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        ListenInput();
    }

    void OnTouched()
    {
        Debug.Log("OnTouched");
    }

    private void ListenInput()
    {
        if (Input.touchCount <= 0) return;

            // 터치 입력 시,
        Touch touch = Input.GetTouch(0);
        Vector2 worldTouchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                // 터치 시작 지점 확인
                if (touchCollider2D == Physics2D.OverlapPoint(worldTouchPos))
                {
                    touchStartPosition = slingStartPosition.position;
                    isTouchStarted = true;
                    tempThrowable =  Instantiate(tempThrowablePrefab, slingStartPosition.position, Quaternion.identity);
                }
                break;

            case TouchPhase.Stationary:
            case TouchPhase.Moved:
                // 터치 당기기
                if (isTouchStarted)
                {
                    Debug.Log((worldTouchPos - touchStartPosition).magnitude);
                    if ((worldTouchPos - touchStartPosition).magnitude > maxPowerDrag)
                    {
                        Vector2 normDelta = (worldTouchPos - touchStartPosition).normalized;

                        worldTouchPos = new Vector2(touchStartPosition.x + normDelta.x * maxPowerDrag,
                            touchStartPosition.y + normDelta.y * maxPowerDrag);
                    }
                    tempThrowable.transform.position = worldTouchPos;
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (isTouchStarted)
                {
                    // 터치 놓기.
                    isTouchStarted = false;
                    if ((worldTouchPos - touchStartPosition).magnitude > maxPowerDrag)
                    {
                        Vector2 normDelta = (worldTouchPos - touchStartPosition).normalized;
                        worldTouchPos = new Vector2(touchStartPosition.x + (normDelta.x * maxPowerDrag),
                            touchStartPosition.y + (normDelta.y * maxPowerDrag));
                    }
                    Debug.Log((worldTouchPos - touchStartPosition).magnitude);
                    tempThrowable.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    tempThrowable.GetComponent<Rigidbody2D>().AddForce((touchStartPosition - worldTouchPos) * 10, ForceMode2D.Impulse);
                }
                break;
        }

    }


}
