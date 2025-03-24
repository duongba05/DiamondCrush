using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    public float tileSize = 1f;
    public GameObject[] candyPrefabs;
    public GameObject[,] allCandies;
    public Vector2 offset;

    void Start()
    {
        allCandies = new GameObject[width, height];
        offset = new Vector2(-(width - 1) * tileSize / 2, -(height - 1) * tileSize / 2); // giải thích 
        SetupBoard();
    }

    void SetupBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x * tileSize, y * tileSize) + offset;
                GameObject candy = Instantiate(candyPrefabs[Random.Range(0, candyPrefabs.Length)], pos, Quaternion.identity);
                candy.transform.parent = transform; // giải thích 
                candy.name = $"Candy ({x},{y})";
                allCandies[x, y] = candy; 
            }
        }
    }

    public void CheckMatches3()
    {
        bool hasMatches = false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Kiểm tra ngang
                if (x < width - 2 &&
                    allCandies[x, y] != null && // Thêm kiểm tra null
                    allCandies[x + 1, y] != null && // Thêm kiểm tra null
                    allCandies[x + 2, y] != null && // Thêm kiểm tra null
                    allCandies[x, y].tag == allCandies[x + 1, y].tag &&
                    allCandies[x, y].tag == allCandies[x + 2, y].tag)
                {
                    Destroy(allCandies[x, y]);
                    Destroy(allCandies[x + 1, y]);
                    Destroy(allCandies[x + 2, y]);
                    allCandies[x, y] = null;
                    allCandies[x + 1, y] = null;
                    allCandies[x + 2, y] = null;
                    hasMatches = true;
                }

                // Kiểm tra dọc
                if (y < height - 2 &&
                    allCandies[x, y] != null && // Thêm kiểm tra null
                    allCandies[x, y + 1] != null && // Thêm kiểm tra null
                    allCandies[x, y + 2] != null && // Thêm kiểm tra null
                    allCandies[x, y].tag == allCandies[x, y + 1].tag &&
                    allCandies[x, y].tag == allCandies[x, y + 2].tag)
                {
                    Destroy(allCandies[x, y]);
                    Destroy(allCandies[x, y + 1]);
                    Destroy(allCandies[x, y + 2]);
                    allCandies[x, y] = null;
                    allCandies[x, y + 1] = null;
                    allCandies[x, y + 2] = null;
                    hasMatches = true;
                }
            }
        }

        if (hasMatches)
        {
            StartCoroutine(FillBoard());
        }
    }
    public void CheckMatches4()
    {
        bool hasMatches = false;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Kiểm tra ngang
                if (x < width - 3 &&
                    allCandies[x, y] != null && // Thêm kiểm tra null
                    allCandies[x + 1, y] != null && // Thêm kiểm tra null
                    allCandies[x + 2, y] != null && // Thêm kiểm tra null
                    allCandies[x + 3, y] != null && // Thêm kiểm tra null
                    allCandies[x, y].tag == allCandies[x + 1, y].tag &&
                    allCandies[x, y].tag == allCandies[x + 2, y].tag &&
                    allCandies[x, y].tag == allCandies[x + 3, y].tag)
                {
                    Destroy(allCandies[x, y]);
                    Destroy(allCandies[x + 1, y]);
                    Destroy(allCandies[x + 2, y]);
                    Destroy(allCandies[x + 3, y]);
                    allCandies[x, y] = null;
                    allCandies[x + 1, y] = null;
                    allCandies[x + 2, y] = null;
                    allCandies[x + 3, y] = null;
                    hasMatches = true;
                }

                // Kiểm tra dọc
                if (y < height - 3 &&
                    allCandies[x, y] != null && // Thêm kiểm tra null
                    allCandies[x, y + 1] != null && // Thêm kiểm tra null
                    allCandies[x, y + 2] != null && // Thêm kiểm tra null
                    allCandies[x, y + 3] != null && // Thêm kiểm tra null
                    allCandies[x, y].tag == allCandies[x, y + 1].tag &&
                    allCandies[x, y].tag == allCandies[x, y + 2].tag &&
                    allCandies[x, y].tag == allCandies[x, y + 3].tag)
                {
                    Destroy(allCandies[x, y]);
                    Destroy(allCandies[x, y + 1]);
                    Destroy(allCandies[x, y + 2]);
                    Destroy(allCandies[x, y + 3]);
                    allCandies[x, y] = null;
                    allCandies[x, y + 1] = null;
                    allCandies[x, y + 2] = null;
                    allCandies[x, y + 3] = null;
                    hasMatches = true;
                }
            }
        }

        if (hasMatches)
        {
            StartCoroutine(FillBoard());
        }
    }

    IEnumerator FillBoard()
    {
        yield return new WaitForSeconds(0.5f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allCandies[x, y] == null)
                {
                    for (int yAbove = y + 1; yAbove < height; yAbove++)
                    {
                        if (allCandies[x, yAbove] != null)
                        {
                            allCandies[x, y] = allCandies[x, yAbove];
                            allCandies[x, yAbove] = null;
                            Candy candyScript = allCandies[x, y].GetComponent<Candy>();
                            candyScript.row = y;
                            StartCoroutine(MoveCandy(allCandies[x, y], new Vector2(x * tileSize, y * tileSize) + offset));
                            break;
                        }
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = height - 1; y >= 0; y--)
            {
                if (allCandies[x, y] == null)
                {
                    Vector2 spawnPos = new Vector2(x * tileSize, height * tileSize) + offset;
                    GameObject candy = Instantiate(candyPrefabs[Random.Range(0, candyPrefabs.Length)], spawnPos, Quaternion.identity);
                    candy.transform.parent = transform;
                    candy.name = $"Candy ({x},{y})";
                    allCandies[x, y] = candy;
                    Candy candyScript = candy.GetComponent<Candy>();
                    candyScript.column = x;
                    candyScript.row = y;
                    StartCoroutine(MoveCandy(candy, new Vector2(x * tileSize, y * tileSize) + offset));
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        CheckMatches3();
        CheckMatches4();
    }

    IEnumerator MoveCandy(GameObject candy, Vector2 targetPos)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector2 startPos = candy.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            candy.transform.position = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }
        candy.transform.position = targetPos;
    }
}