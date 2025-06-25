using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PondCell : MonoBehaviour
{
    public Button button;
    public Image background;
    public Color hitColor = Color.yellow;
    public Color missColor = Color.blue;

    private Vector2Int cellPosition;
    private PondManager manager;
    private bool revealed = false;

    public void Init(Vector2Int pos, PondManager pondManager)
    {
        cellPosition = pos;
        manager = pondManager;
        button.onClick.AddListener(OnClick);
        ResetCell();
    }

    public void OnClick()
    {
        if (!revealed)
            manager.CheckCell(cellPosition);
    }

    public void Reveal(bool hasItem)
    {
        revealed = true;
        button.interactable = false;
        background.color = hasItem ? hitColor : missColor;
    }

    public void ResetCell()
    {
        revealed = false;
        button.interactable = true;
        background.color = Color.white;
    }
}
