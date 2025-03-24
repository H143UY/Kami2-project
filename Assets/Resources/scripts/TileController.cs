using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TileController : MonoBehaviour, IPointerClickHandler
{
    public int x, y;
    private Image tileImage;

    public void SetPosition(int posX, int posY)
    {
        x = posX;
        y = posY;
    }

    public void SetColor(Color newColor)
    {
        tileImage = GetComponent<Image>();
        if (tileImage != null)
        {
            tileImage.color = newColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GridManager.Instance.ChangeConnectedTiles(x, y, tileImage.color, ColorPickerController.selectedColor);
        Debug.Log($"🖱 Clicked on ({x}, {y}) - Old Color: {tileImage.color}, New Color: {ColorPickerController.selectedColor}");
    }
}