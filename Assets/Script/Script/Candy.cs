using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour
{
    private Vector2 firstTouchPos;
    private Vector2 finalTouchPos;
    private bool isBeingHeld = false;
    private Board board;
    public int column;
    public int row;

    void Start()
    {
        board = FindObjectOfType<Board>();
        column = Mathf.RoundToInt((transform.position.x - board.offset.x) / board.tileSize);
        row = Mathf.RoundToInt((transform.position.y - board.offset.y) / board.tileSize);

        // Đảm bảo column và row ban đầu không vượt giới hạn
        column = Mathf.Clamp(column, 0, board.width - 1);
        row = Mathf.Clamp(row, 0, board.height - 1);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(firstTouchPos))
                isBeingHeld = true;
        }

        if (Input.GetMouseButtonUp(0) && isBeingHeld)
        {
            finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isBeingHeld = false;
            TrySwapCandy();
        }
    }

    void TrySwapCandy()
    {
        Vector2 direction = finalTouchPos - firstTouchPos;
        int targetColumn = column;
        int targetRow = row;

        // Xác định hướng kéo
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            targetColumn += direction.x > 0 ? 1 : -1;
        else
            targetRow += direction.y > 0 ? 1 : -1;

        // Kiểm tra chặt chẽ hơn để tránh vượt giới hạn mảng
        if (targetColumn >= 0 && targetColumn < board.width &&
            targetRow >= 0 && targetRow < board.height &&
            column >= 0 && column < board.width &&
            row >= 0 && row < board.height) // Kiểm tra cả column/row hiện tại
        {
            GameObject targetCandy = board.allCandies[targetColumn, targetRow];
            if (targetCandy != null)
            {
                StartCoroutine(SwapCandies(targetCandy.GetComponent<Candy>()));
            }
        }
    }

    IEnumerator SwapCandies(Candy targetCandy)
    {
        // Kiểm tra lại trước khi hoán đổi
        if (column < 0 || column >= board.width || row < 0 || row >= board.height ||
            targetCandy.column < 0 || targetCandy.column >= board.width ||
            targetCandy.row < 0 || targetCandy.row >= board.height)
        {
            Debug.LogError($"Invalid swap attempt: ({column}, {row}) to ({targetCandy.column}, {targetCandy.row})");
            yield break; // Thoát nếu chỉ số không hợp lệ
        }

        Vector2 startPos = transform.position;
        Vector2 targetPos = targetCandy.transform.position;

        // Hoán đổi trong mảng
        board.allCandies[column, row] = targetCandy.gameObject;
        board.allCandies[targetCandy.column, targetCandy.row] = gameObject;

        // Cập nhật column/row
        int tempColumn = column;
        int tempRow = row;
        column = targetCandy.column;
        row = targetCandy.row;
        targetCandy.column = tempColumn;
        targetCandy.row = tempRow;

        // Hiệu ứng di chuyển
        float duration = 0.3f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector2.Lerp(startPos, targetPos, t);
            targetCandy.transform.position = Vector2.Lerp(targetPos, startPos, t);
            yield return null;
        }

        transform.position = targetPos;
        targetCandy.transform.position = startPos;

        // Kiểm tra match sau khi hoán đổi
        board.CheckMatches();
    }
}