using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PondManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform gridParent;
    public GameObject pondUI;

    private PondCell[,] grid;
    private Vector2Int hiddenItemPos = new Vector2Int(2, 2);
    private int gridSize = 5;

    public int maxAttempts = 3;
    public int currentAttempts = 0;
    private bool itemFound = false;
    private bool isMiniGameActive = false;

    public ItemObject rewardItem;

    void Start()
    {
        GenerateGrid();
        pondUI.SetActive(false);
    }

    void GenerateGrid()
    {
        grid = new PondCell[gridSize, gridSize];

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject obj = Instantiate(cellPrefab, gridParent);
                PondCell cell = obj.GetComponent<PondCell>();
                cell.Init(new Vector2Int(x, y), this);
                grid[x, y] = cell;
            }
        }
    }

    public void CheckCell(Vector2Int pos)
    {
        if (itemFound || currentAttempts >= maxAttempts)
            return;

        bool hit = (pos == hiddenItemPos);
        grid[pos.x, pos.y].Reveal(hit);

        currentAttempts++;

        if (hit)
        {
            itemFound = true;
            Debug.Log("Item found!");

            // Tambahkan item ke inventory
            if (rewardItem != null)
            {
                PlayerInventory.Instance.StoreItem(rewardItem);
                Debug.Log("Item reward telah ditambahkan ke inventory.");
            }
            else
            {
                Debug.LogWarning("Reward item belum di-assign di Inspector.");
            }

            StartCoroutine(WaitBeforeExit());
        }
        else if (currentAttempts >= maxAttempts)
        {
            Debug.Log("Out of attempts.");
            ExitMiniGame();
        }
        else
        {
            Debug.Log("Nothing here.");
        }
    }


    public void AddAttempts(int value)
    {
        maxAttempts += value;
        Debug.Log($"Stick digunakan. Max attempts sekarang: {maxAttempts}");
    }

    public void StartMiniGame()
    {
        pondUI.SetActive(true);
        Time.timeScale = 0f;
        isMiniGameActive = true;
        currentAttempts = 0;
        itemFound = false;

        foreach (var cell in grid)
        {
            cell.ResetCell();
        }
    }

    public void ExitMiniGame()
    {
        pondUI.SetActive(false);
        Time.timeScale = 1f;
        isMiniGameActive = false;
        Debug.Log("Mini game exited.");
    }

    public void ToggleMiniGame()
    {
        if (isMiniGameActive)
            ExitMiniGame();
        else
            StartMiniGame();
    }

    IEnumerator WaitBeforeExit()    
    {
        yield return new WaitForSecondsRealtime(3f); // ⏳ Tunggu 3 detik (waktu real walau game paused)
        ExitMiniGame();
    }


    public bool IsMiniGameActive() => isMiniGameActive;
}
