using UnityEngine;

namespace UnityTools.Config
{
    public class Configs
    {
#if ENABLE_INPUT_SYSTEM
        public static bool leftMouseDown  => UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame; 
        public static bool leftMouseUp  => UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame; 
        public static bool leftMouse  => UnityEngine.InputSystem.Mouse.current.leftButton.isPressed; 
        public static bool rightMouseDown  =>  UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame; 
        public static bool rightMouseUp  =>  UnityEngine.InputSystem.Mouse.current.rightButton.wasReleasedThisFrame; 
        public static bool rightMouse  => UnityEngine.InputSystem.Mouse.current.rightButton.isPressed; 
        public static bool middleMouseDown  =>  UnityEngine.InputSystem.Mouse.current.middleButton.wasPressedThisFrame; 
        public static bool middleMouseUp  =>  UnityEngine.InputSystem.Mouse.current.middleButton.wasReleasedThisFrame; 
        public static bool middleMouse  => UnityEngine.InputSystem.Mouse.current.middleButton.isPressed;
        /// <summary>
        /// 屏幕当前鼠标点击的位置
        /// </summary>
        public static Vector2 screenPosition =>  UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#else
        public static bool leftMouseDown => Input.GetMouseButtonDown(0);
        public static bool leftMouseUp => Input.GetMouseButtonUp(0);
        public static bool leftMouse => Input.GetMouseButton(0);
        public static bool rightMouseDown => Input.GetMouseButtonDown(1);
        public static bool rightMouseUp => Input.GetMouseButtonUp(1);
        public static bool rightMouse => Input.GetMouseButton(1);
        public static bool middleMouseDown => Input.GetMouseButtonDown(2);
        public static bool middleMouseUp => Input.GetMouseButtonUp(2);
        public static bool middleMouse => Input.GetMouseButton(2);
        /// <summary>
        /// 屏幕当前鼠标点击的位置
        /// </summary>
        public static Vector2 screenPosition => Input.mousePosition;
#endif
    }
}