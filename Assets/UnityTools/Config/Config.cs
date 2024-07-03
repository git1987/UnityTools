using UnityEngine;
namespace UnityTools
{
    public class Config
    {
#if ENABLE_INPUT_SYSTEM
        public static bool leftMouseDown => UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
        public static bool leftMouseUp => UnityEngine.InputSystem.Mouse.current.leftButton.wasReleasedThisFrame;
        public static bool leftMouse => UnityEngine.InputSystem.Mouse.current.leftButton.isPressed;
        public static bool rightMouseDown => UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame;
        public static bool rightMouseUp => UnityEngine.InputSystem.Mouse.current.rightButton.wasReleasedThisFrame;
        public static bool rightMouse => UnityEngine.InputSystem.Mouse.current.rightButton.isPressed;
        public static bool middleMouseDown => UnityEngine.InputSystem.Mouse.current.middleButton.wasPressedThisFrame;
        public static bool middleMouseUp => UnityEngine.InputSystem.Mouse.current.middleButton.wasReleasedThisFrame;
        public static bool middleMouse => UnityEngine.InputSystem.Mouse.current.middleButton.isPressed;
        /// <summary>
        /// 屏幕当前鼠标点击的位置
        /// </summary>
        public static Vector2 screenPosition => UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#elif (UNITY_WEBGL || UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        public static bool leftMouseDown => Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
        public static bool leftMouseUp => Input.touchCount   > 0 && Input.touches[0].phase == TouchPhase.Ended;
        public static bool leftMouse =>
            Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Stationary
                                  || Input.touches[0].phase == TouchPhase.Moved);
        public static bool rightMouseDown => Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
        public static bool rightMouseUp => Input.touchCount   > 0 && Input.touches[0].phase == TouchPhase.Ended;
        public static bool rightMouse =>
            Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Stationary
                                  || Input.touches[0].phase == TouchPhase.Moved);
        public static bool middleMouseDown => Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began;
        public static bool middleMouseUp => Input.touchCount   > 0 && Input.touches[0].phase == TouchPhase.Ended;
        public static bool middleMouse =>
            Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Stationary
                                  || Input.touches[0].phase == TouchPhase.Moved);
        /// <summary>
        /// 屏幕中第一个手势的位置
        /// </summary>
        public static Vector2 screenPosition => Input.touches.Length > 0 ? Input.touches[0].position : Vector2.zero;
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

        public class RichTextColor
        {
            /// <summary>
            /// FFFFFF
            /// </summary>
            public const string White = "FFFFFF";
            /// <summary>
            /// 000000
            /// </summary>
            public const string Black = "000000";
            /// <summary>
            /// 808080
            /// </summary>
            public const string Gray = "808080";
            /// <summary>
            /// FF0000
            /// </summary>
            public const string Red = "FF0000";
            /// <summary>
            /// FF8000
            /// </summary>
            public const string Orange = "FF8000";
            /// <summary>
            /// FFFF00
            /// </summary>
            public const string Yellow = "FFFF00";
            /// <summary>
            /// 00FF00
            /// </summary>
            public const string Green = "00FF00";
            /// <summary>
            /// 00FFFF
            /// </summary>
            public const string Cyan = "00FFFF";
            /// <summary>
            /// 0000FF
            /// </summary>
            public const string Blue = "0000FF";
            /// <summary>
            /// 8000FF
            /// </summary>
            public const string Purple = "8000FF";
        }
    }
}