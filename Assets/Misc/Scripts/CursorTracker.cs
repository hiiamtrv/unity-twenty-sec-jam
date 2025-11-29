using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTracker : MonoBehaviour
{
    [SerializeField] private string selectedObject;
    [SerializeField] private bool pointerOverGameobject;
    private void Update()
    {
        selectedObject = EventSystem.current.currentSelectedGameObject?.name ?? "null";
        pointerOverGameobject = EventSystem.current.IsPointerOverGameObject();
    }
}