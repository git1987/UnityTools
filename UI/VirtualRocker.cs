extern alias EditorCoreModule;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityTools.UI
{
    public class VirtualRocker : MonoBehaviour
    {
        public static GameObject CreatePrefab(GameObject canvasObj)
        {
            Canvas canvas = canvasObj.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = canvasObj.GetComponentInChildren<Canvas>();
            }
            else
            {
                Debuger.LogError("请选择带有Canvas组件的GameObject");
                return null;
            }
            GameObject rocker = new GameObject("VirtualRocker");
            VirtualRocker virtualRocker = rocker.AddComponent<VirtualRocker>();
            rocker.transform.SetParent(canvas.transform);
            RectTransform rect = rocker.AddComponent<RectTransform>();
            Tools.RectTransformSetSurround(rect);

            GameObject bg = new GameObject("pointBg");
            rect = bg.AddComponent<RectTransform>();
            virtualRocker.pointBg = rect;
            rect.SetParent(rocker.transform);
            rect.sizeDelta = Vector2.one * 200;
            rect.anchoredPosition = new Vector2(0, 250);
            rect.anchorMin = new Vector2(.5f, 0);
            rect.anchorMax = new Vector2(.5f, 0);
            bg.AddComponent<Image>();

            GameObject pointer = new GameObject("pointer");
            rect = pointer.AddComponent<RectTransform>();
            virtualRocker.pointer = rect;
            rect.SetParent(bg.transform);
            rect.sizeDelta = new Vector2(10, 100);
            rect.anchoredPosition = Vector2.zero;
            rect.pivot = new Vector2(.5f, 0);
            pointer.AddComponent<Image>();

            GameObject point = new GameObject("point");
            rect = point.AddComponent<RectTransform>();
            virtualRocker.point = rect;
            rect.SetParent(bg.transform);
            rect.sizeDelta = Vector2.one * 50;
            rect.anchoredPosition = Vector2.zero;
            point.AddComponent<Image>();

            GameObject area = new GameObject("area");
            rect = area.AddComponent<RectTransform>();
            virtualRocker.areaRect = rect;
            rect.SetParent(rocker.transform);
            rect.sizeDelta = Vector2.one * 500;
            rect.anchoredPosition = new Vector2(0, 250);
            rect.anchorMin = new Vector2(.5f, 0);
            rect.anchorMax = new Vector2(.5f, 0);
            rocker.transform.localScale = Vector3.one;
            return rocker;
        }
        public bool isClick { private set; get; }
        public bool unenableHide;
        /// <summary>
        /// 摇杆区域背景
        /// </summary>
        [SerializeField]
        private RectTransform pointBg;
        /// <summary>
        /// 摇杆点
        /// </summary>
        [SerializeField]
        RectTransform point;
        /// <summary>
        /// 摇杆方向指针
        /// </summary>
        [SerializeField]
        RectTransform pointer;
        /// <summary>
        /// 可触发摇杆的区域
        /// </summary>
        [SerializeField]
        private RectTransform areaRect;
        public Vector3 clickMousePos;
        public Vector2 rate = new Vector2(1080, 1920);

        public Vector2 Direction
        {
            get { return point.anchoredPosition / pointBg.sizeDelta.x / 2; }
        }
        protected virtual void Awake()
        {
            CanvasScaler cs = this.GetComponentInParent<CanvasScaler>();
            if (cs == null)
                Debuger.LogError("CavasScaler Component is null!");
            else
            {
                rate = cs.referenceResolution;
                ResetRocker();
            }
            if (pointBg.GetComponent<Image>().sprite == null)
                Debuger.LogError("bg's sprite is null![Click this goto gameObject]", pointBg.gameObject);
            if (point.GetComponent<Image>().sprite == null)
                Debuger.LogError("point's sprite is null![Click this goto gameObject]", point.gameObject);
            if (pointer.GetComponent<Image>().sprite == null)
                Debuger.LogError("pointer's sprite is null![Click this goto gameObject]", pointer.gameObject);
        }

        protected virtual void CheckShowRocker()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                float screenRate = Screen.height * rate.x / (Screen.width * rate.y);
                int width, height;
                if (screenRate == 1)
                {
                    //9/16：分辨率不变
                    width = (int)rate.x;
                    height = (int)rate.y;
                }
                else if (screenRate < 1)
                {
                    //高度变矮：高度发生变化（降低）
                    width = (int)rate.x;
                    height = (int)((int)rate.y * screenRate);
                }
                else
                {
                    //高度拉高：宽度发生变化（增加）
                    width = (int)((int)rate.x / screenRate);
                    height = (int)rate.y;
                }
                //(transform as RectTransform).sizeDelta = new Vector2(width, height);
                clickMousePos = Input.mousePosition;
                float x = clickMousePos.x * width / Screen.width - (width / 2);
                float y = clickMousePos.y * height / Screen.height;
                //
                if (Mathf.Abs(x) <= areaRect.sizeDelta.x / 2 &&
                    y >= areaRect.anchoredPosition.y - areaRect.sizeDelta.y / 2 &&
                    y <= areaRect.anchoredPosition.y + areaRect.sizeDelta.y / 2)
                {
                    isClick = true;
                    pointBg.anchoredPosition = new Vector2(x, y);
                    pointBg.gameObject.SetActive(true);
                }
            }
        }

        protected virtual void ResetRocker()
        {
            isClick = false;
            pointBg.anchoredPosition = areaRect.anchoredPosition;
            point.anchoredPosition = Vector3.zero;
            if (unenableHide) pointBg.gameObject.SetActive(false);
        }

        protected virtual void UpdateRocker()
        {
            if (isClick)
            {
                /*设置point位置点*/
                point.anchoredPosition = (Vector2)(Input.mousePosition - clickMousePos);
                if (point.anchoredPosition.magnitude > pointBg.sizeDelta.x / 2 - point.sizeDelta.x / 2)
                    point.anchoredPosition = point.anchoredPosition.normalized *
                                                 (pointBg.sizeDelta.x / 2 - point.sizeDelta.x / 2);
                /*设置pointer的方向*/
                float angle = Vector2.SignedAngle(Vector2.up, Direction);
                pointer.eulerAngles = new Vector3(0, 0, angle);
                pointer.gameObject.SetActive(Direction != Vector2.zero);
            }
        }

        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckShowRocker();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ResetRocker();
            }
            UpdateRocker();
        }
    }
}
