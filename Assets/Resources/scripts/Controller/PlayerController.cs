using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Color selectedColor;

    public void SelectColor(Image colorButton)
    {
        selectedColor = colorButton.color;
        Debug.Log($"🎨 Đã chọn màu: {selectedColor}");
    }
}