using UnityEngine;

namespace UnityTools.Config
{
    public class Configs
    {
#if ENABLE_INPUT_SYSTEM
        public static bool leftMouseDown { get => UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame; }
        public static bool leftMouseUp { get => UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame; }
        public static bool leftMouse { get => UnityEngine.InputSystem.Mouse.current.leftButton.isPressed; }
        public static bool rightMouseDown { get =>  UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame; }
        public static bool rightMouseUp { get =>  UnityEngine.InputSystem.Mouse.current.rightButton.wasReleasedThisFrame; }
        public static bool rightMouse { get => UnityEngine.InputSystem.Mouse.current.rightButton.isPressed; }
        public static bool middleMouseDown { get =>  UnityEngine.InputSystem.Mouse.current.middleButton.wasPressedThisFrame; }
        public static bool middleMouseUp { get =>  UnityEngine.InputSystem.Mouse.current.middleButton.wasReleasedThisFrame; }
        public static bool middleMouse { get => UnityEngine.InputSystem.Mouse.current.middleButton.isPressed; }
#else
        public static bool leftMouseDown { get => Input.GetMouseButtonDown(0); }
        public static bool leftMouseUp { get => Input.GetMouseButtonUp(0); }
        public static bool leftMouse { get => Input.GetMouseButton(0); }
        public static bool rightMouseDown { get => Input.GetMouseButtonDown(1); }
        public static bool rightMouseUp { get => Input.GetMouseButtonUp(1); }
        public static bool rightMouse { get => Input.GetMouseButton(1); }
        public static bool middleMouseDown { get => Input.GetMouseButtonDown(2); }
        public static bool middleMouseUp { get => Input.GetMouseButtonUp(2); }
        public static bool middleMouse { get => Input.GetMouseButton(2); }
#endif
    }
}