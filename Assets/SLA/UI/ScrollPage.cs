using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollPage : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] RectTransform _content;
        public RectTransform Content { get { return _content; } set { _content = value; } }
        [SerializeField] List<Vector2> _children = new List<Vector2>();

        public int PageCount { get { return _children.Count; } } 

        private ScrollRect _scrollRect;
        public ScrollRect ScrollRect { get { return _scrollRect ?? (_scrollRect = GetComponent<ScrollRect>()); } }

        private Vector2 _startedPosition;

        public Vector2 StartedPosition
        {
            get
            {
                if (_startedPosition == default(Vector2))
                {
                    _startedPosition = Content.position;
                }
                return _startedPosition;
            }
        }

        [SerializeField] int _page;
        public int Page 
        { 
            get 
            { 
                if (!_isInitialized)
                {
                    Init();
                }
                return _page; 
            } 
            set 
            {
                MovePage(value, false);
            }
        }

        private const float WillStoppingVelocity = 400f;

        private bool _inertia;
        private ScrollRect.MovementType _movementType;

        private int _firstPage;

        [SerializeField] Button _pageLeft;
        [SerializeField] Button _pageRight;

        public UnityEvent onChangedPage;
        public UnityEvent onInitialized;

        private bool _slipping;
        private bool _isInitialized = false;

        void Init()
        {
            _isInitialized = true;
            _children.Clear();
            foreach (Transform t in Content.transform)
            {
                if (t.gameObject != null)
                {
                    _children.Add((Vector2)t.position);
                }
            }

            if (ScrollRect.horizontal)
            {
                _children.Sort((a, b) => { return a.x < b.x ? -1 : 1; });
            }
            else
            {
                _children.Sort((a, b) => { return a.y < b.y ? -1 : 1; });
            }
            Content.position = GetPosition();
            AppearPageButtons();
            if (onInitialized != null)
            {
                onInitialized.Invoke();
            }
        }

        Vector2 GetPosition(int page)
        {
            if (_children.Count <= 1)
            {
                return StartedPosition;
            }
            return StartedPosition - (_children[1] - _children[0]) * (page - _firstPage);
        }

        Vector2 GetPosition()
        {
            return GetPosition(Page);
        }

        int CalculatePage()
        {
            int page = Page;
            if (ScrollRect.horizontal)
            {
                if (ScrollRect.velocity.x > WillStoppingVelocity)
                {
                    page--;
                }
                else if (ScrollRect.velocity.x < -WillStoppingVelocity)
                {
                    page++;
                }
                else
                {
                    page = FindPage();
                }
            }
            else
            {
                if (ScrollRect.velocity.y > WillStoppingVelocity)
                {
                    page--;
                }
                else if (ScrollRect.velocity.y < -WillStoppingVelocity)
                {
                    page++;
                }
                else
                {
                    page = FindPage();
                }
            }
            return page;
        }

        int FindPage()
        {
            float nearDistance = float.MaxValue;
            int nearPage = 0;
            for (int page = 0; page < PageCount; page++)
            {
                float distance = Vector2.Distance(Content.position, GetPosition(page));
                if (distance < nearDistance)
                {
                    nearDistance = distance;
                    nearPage = page;
                }
                else
                {
                    break;
                }
            }
            return nearPage;
        }

        void Awake()
        {
            _inertia = ScrollRect.inertia;
            _movementType = ScrollRect.movementType;
            SetPageButton(_pageLeft, OnClickPageLeft);
            SetPageButton(_pageRight, OnClickPageRight);
            _slipping = false;
        }

        void Start()
        {
            if (!_isInitialized)
            {
                Init();
            }
        }

        void SetPageButton(Button button, UnityAction action)
        {
            if (button != null)
            {
                button.onClick.AddListener(action);
            }
        }

        void LateUpdate()
        {
            if (!_slipping)
            {
                return;
            }

            if (ScrollRect.horizontal)
            {
                if (Mathf.Abs(ScrollRect.velocity.x) < WillStoppingVelocity || IsNearEnd(true))
                {
                    Page = FindPage();
                    _slipping = false;
                }
            }
            else
            {
                if (Mathf.Abs(ScrollRect.velocity.y) < WillStoppingVelocity || IsNearEnd(false))
                {
                    Page = FindPage();
                    _slipping = false;
                }
            }
        }

        bool IsNearEnd(bool horizontal)
        {
            if (horizontal)
            {
                if (Content.position.x < GetPosition(PageCount - 1).x || Content.position.x > GetPosition(0).x)
                {
                    return true;
                }
            }
            else
            {
                if (Content.position.y < GetPosition(PageCount - 1).y || Content.position.y > GetPosition(0).y)
                {
                    return true;
                }
            }
            return false;
        }

        public void Resize(int firstPage)
        {
            _firstPage = firstPage;
            Invoke("Init", 0.1f);
            _page = firstPage;
        }

        public void MovePage(int page, bool immediately)
        {
            if (!_isInitialized)
            {
                Init();
            }
            int org = _page;
            _page = Mathf.Clamp(page, 0, _children.Count - 1);
            if (immediately)
            {
                Content.transform.position = GetPosition(page);
            }
            else
            {
                TweenPage();
            }
            if (org != _page)
            {
                AppearPageButtons();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            iTween.Stop(Content.gameObject);
            _slipping = false;
            EnableScrollRect(true);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _slipping = true;
        }

        void TweenPage()
        {
            Vector2 movingPosition = GetPosition();
            float movingDistance = Vector2.Distance(Content.transform.position, movingPosition);
            float speed = Mathf.Clamp(movingDistance / ScrollRect.velocity.magnitude, 0.3f, 1f);
            ScrollRect.StopMovement();
            EnableScrollRect(false);
            iTween.MoveTo(Content.gameObject, iTween.Hash("position", (Vector3)movingPosition, "easetype", iTween.EaseType.easeOutCubic,
                                                          "time", speed, "oncomplete", "OnFinishedMoving", "oncompletetarget", gameObject));
        }

        void OnFinishedMoving()
        {
            EnableScrollRect(true);
            if (onChangedPage != null)
            {
                onChangedPage.Invoke();
            }
        }

        void EnableScrollRect(bool isEnable)
        {
            if (isEnable)
            {
                ScrollRect.inertia = _inertia;
                ScrollRect.movementType = _movementType;
                ScrollRect.StopMovement();
            }
            else
            {
                ScrollRect.inertia = false;
                ScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            }
        }

        void AppearPageButtons()
        {
            if (_pageLeft != null)
            {
                _pageLeft.gameObject.SetActive(Page != 0);
            }
            if (_pageRight != null)
            {
                _pageRight.gameObject.SetActive(Page != _children.Count - 1);
            }
        }

        public void OnClickPageLeft()
        {
            Page--;
        }

        public void OnClickPageRight()
        {
            Page++;
        }
    }
}
