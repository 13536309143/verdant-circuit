using System.Collections;
using TMPro;
using UnityEngine;

namespace TinyFarm
{
    /// <summary>One input path for keyboard controls and future automation commands.</summary>
    public sealed class FarmRobotController : MonoBehaviour
    {
        [SerializeField] private int fieldSize = 4;
        [SerializeField] private float spacing = 1.3f;
        [SerializeField] private float groundHeight = 0.19f;
        [SerializeField] private float moveDuration = 0.16f;
        [SerializeField] private Transform robotVisual;
        [SerializeField] private Transform cellMarker;
        [SerializeField] private TMP_Text coordinateLabel;
        [SerializeField] private TMP_Text stepsLabel;
        [SerializeField] private TMP_Text statusLabel;
        [SerializeField] private int cellX = 1;
        [SerializeField] private int cellY = 1;
        private bool isMoving;
        private float nextInputTime;
        private float statusUntil;
        private int steps;

        public Vector2Int Cell { get { return new Vector2Int(cellX, cellY); } }
        public int Steps { get { return steps; } }
        public bool IsMoving { get { return isMoving; } }

        public void Configure(Transform visual, Transform marker, TMP_Text coordinates,
            TMP_Text stepCount, TMP_Text status)
        {
            robotVisual = visual;
            cellMarker = marker;
            coordinateLabel = coordinates;
            stepsLabel = stepCount;
            statusLabel = status;
            ResetPosition();
        }

        private void Awake()
        {
            SnapToCell();
            UpdateLabels();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) ResetPosition();
            if (!isMoving && Time.unscaledTime >= nextInputTime)
            {
                int dx = 0;
                int dy = 0;
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) dy = 1;
                else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) dy = -1;
                else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) dx = -1;
                else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) dx = 1;
                if (dx != 0 || dy != 0)
                {
                    TryMove(dx, dy);
                    nextInputTime = Time.unscaledTime + 0.20f;
                }
            }
            if (robotVisual != null && !isMoving)
                robotVisual.localPosition = new Vector3(0, Mathf.Sin(Time.time * 2.5f) * 0.018f, 0);
            if (statusLabel != null && Time.unscaledTime > statusUntil)
                statusLabel.text = isMoving ? "ON THE MOVE" : "READY TO EXPLORE";
        }

        public bool TryMove(int dx, int dy)
        {
            if (!Application.isPlaying || isMoving || Mathf.Abs(dx) + Mathf.Abs(dy) != 1)
                return false;
            int targetX = cellX + dx;
            int targetY = cellY + dy;
            if (targetX < 0 || targetX >= fieldSize || targetY < 0 || targetY >= fieldSize)
            {
                if (statusLabel != null) statusLabel.text = "EDGE OF FIELD";
                statusUntil = Time.unscaledTime + 0.65f;
                return false;
            }
            cellX = targetX;
            cellY = targetY;
            steps++;
            StartCoroutine(MoveToCell(new Vector3(dx, 0, dy)));
            UpdateLabels();
            return true;
        }

        private IEnumerator MoveToCell(Vector3 facing)
        {
            isMoving = true;
            Vector3 start = transform.position;
            Vector3 destination = CellPosition();
            Quaternion initialRotation = robotVisual != null ? robotVisual.rotation : Quaternion.identity;
            Quaternion targetRotation = Quaternion.LookRotation(facing);
            float elapsed = 0;
            while (elapsed < moveDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / moveDuration);
                float eased = t * t * (3 - 2 * t);
                transform.position = Vector3.Lerp(start, destination, eased);
                if (robotVisual != null)
                {
                    robotVisual.rotation = Quaternion.Slerp(initialRotation, targetRotation, eased);
                    robotVisual.localPosition = new Vector3(0, Mathf.Sin(t * Mathf.PI) * 0.09f, 0);
                }
                yield return null;
            }
            transform.position = destination;
            if (robotVisual != null) robotVisual.localPosition = Vector3.zero;
            if (cellMarker != null) cellMarker.position = destination + Vector3.up * 0.015f;
            isMoving = false;
        }

        public void ResetPosition()
        {
            StopAllCoroutines();
            isMoving = false;
            cellX = 1;
            cellY = 1;
            steps = 0;
            statusUntil = 0;
            nextInputTime = Time.unscaledTime;
            if (robotVisual != null)
            {
                robotVisual.localPosition = Vector3.zero;
                robotVisual.localRotation = Quaternion.identity;
            }
            SnapToCell();
            UpdateLabels();
        }

        private Vector3 CellPosition()
        {
            float offset = (fieldSize - 1) * spacing * 0.5f;
            return new Vector3(cellX * spacing - offset, groundHeight, cellY * spacing - offset);
        }

        private void SnapToCell()
        {
            transform.position = CellPosition();
            if (cellMarker != null) cellMarker.position = CellPosition() + Vector3.up * 0.015f;
        }

        private void UpdateLabels()
        {
            if (coordinateLabel != null)
                coordinateLabel.text = string.Format("CELL  {0:00} / {1:00}", cellX + 1, cellY + 1);
            if (stepsLabel != null) stepsLabel.text = string.Format("{0:000}  STEPS", steps);
            if (statusLabel != null) statusLabel.text = "READY TO EXPLORE";
        }
    }
}
