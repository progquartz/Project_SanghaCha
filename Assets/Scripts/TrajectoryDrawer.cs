using Unity.VisualScripting;
using UnityEngine;

public class TrajectoryDrawer : MonoBehaviour
{
    [SerializeField] private Transform projecTilePrefab;
    [SerializeField] Collider2D touchCollider2D;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] LineRenderer lineRenderer;

    [SerializeField] private float launchForce = 30f;
    [SerializeField] private float launchGravity = 0.5f;
    [SerializeField] private float trajactoryTimeStep = 0.05f;
    [SerializeField] private int trajectoryStepCount = 15;

    private Vector2 velocity, startMousePos, currentMousePos;
    private bool isTouchStarted = false;

    private void Update()
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
                    isTouchStarted = true;
                    startMousePos = worldTouchPos;
                }
                break;

            case TouchPhase.Stationary:
            case TouchPhase.Moved:
                // 터치 당기기
                if (isTouchStarted)
                {
                    currentMousePos = worldTouchPos;
                    velocity = (startMousePos - currentMousePos) * launchForce;

                    DrawTrajectory();
                }
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                if (isTouchStarted)
                {
                    isTouchStarted = false;
                }
                break;
        }
    }

    private void DrawTrajectory()
    {
        Vector3[] position = new Vector3[trajectoryStepCount];
        for (int i = 0; i < trajectoryStepCount; i++)
        {
            float t = i * trajactoryTimeStep;
            Vector3 pos = (Vector2)spawnPoint.position + velocity * t + launchGravity * Physics2D.gravity * t * t;
            Debug.Log("pos" + i + " : " + pos);
            position[i] = pos;

            
        }
        lineRenderer.SetPositions(position);
    }
}
