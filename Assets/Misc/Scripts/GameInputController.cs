using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInputController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private bool activeOnAwake;

    private void Awake()
    {
        if (activeOnAwake)
        {
            ActiveInput();
        }
        else
        {
            DeactiveInput();
        }
    }

    public void ActiveInput()
    {
        inputActions.Enable();
    }

    public void DeactiveInput()
    {
        inputActions.Disable();
    }
}