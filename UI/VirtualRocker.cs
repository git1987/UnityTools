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
            if (canvasObj == null)
            {
                return null;
            }
            Canvas canvas = canvasObj.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = canvasObj.GetComponentInChildren<Canvas>();
                if (canvas == null)
                {
                    canvas.GetComponentInParent<Canvas>();
                }
                else
                {
                    Debuger.LogError("子级或者父级都没有Canvas组件！请选择带有Canvas组件的GameObject");
                    return null;
                }
            }
            GameObject rocker = new GameObject("Rocker");
            VirtualRocker virtualRocker = rocker.AddComponent<VirtualRocker>();
            rocker.transform.SetParent(canvas.transform);
            RectTransform rect = rocker.AddComponent<RectTransform>();
            Tools.RectTransformSetSurround(rect);
            GameObject bg = new GameObject("bg");
            rect = bg.AddComponent<RectTransform>();
            virtualRocker.pointBg = rect;
            rect.SetParent(rocker.transform);
            rect.sizeDelta = Vector2.one * 200;
            rect.anchoredPosition = new Vector2(0, 250);
            rect.anchorMin = new Vector2(.5f, 0);
            rect.anchorMax = new Vector2(.5f, 0);
            bg.AddComponent<Image>();
            GameObject point = new GameObject("point");
            rect = point.AddComponent<RectTransform>();
            virtualRocker.pointRect = rect;
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
        [SerializeField]
        private RectTransform pointBg, pointRect;

        //摇杆区域尺寸
        public RectTransform areaRect;
        public Vector3 clickMousePos;
        public Vector2 rate = new Vector2(1080, 1920);

        public Vector2 Direction
        {
            get { return pointRect.anchoredPosition / pointBg.sizeDelta.x / 2; }
        }

        protected void Awake()
        {
            CanvasScaler cs = this.GetComponentInParent<CanvasScaler>();
            if (cs == null)
                Debuger.LogError("CavasScaler Component is null!");
            else
            {
                rate = cs.referenceResolution;
                ResetRocker();
            }
        }

        void CheckShowRocker()
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

        void ResetRocker()
        {
            isClick = false;
            pointBg.anchoredPosition = areaRect.anchoredPosition;
            pointRect.anchoredPosition = Vector3.zero;
            if (unenableHide) pointBg.gameObject.SetActive(false);
        }

        void UpdateRocker()
        {
            if (isClick)
            {
                pointRect.anchoredPosition = (Vector2)(Input.mousePosition - clickMousePos);
                if (pointRect.anchoredPosition.magnitude > pointBg.sizeDelta.x / 2 - pointRect.sizeDelta.x / 2)
                    pointRect.anchoredPosition = pointRect.anchoredPosition.normalized *
                                                 (pointBg.sizeDelta.x / 2 - pointRect.sizeDelta.x / 2);
            }
        }

        private void Update()
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
