using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorPickerController : MonoBehaviour, IPointerClickHandler
{
    public static Color selectedColor = Color.white;

    public void OnPointerClick(PointerEventData eventData)
    {
        Image img = GetComponent<Image>();
        if (img != null)
        {
            selectedColor = img.color;
            Debug.Log($" Màu được chọn: {selectedColor}");
        }
    }
}
