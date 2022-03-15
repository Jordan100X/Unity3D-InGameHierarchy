
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[Serializable]
public class HierarchyObject : MonoBehaviour, IHierarchyObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerMoveHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    //[SerializeField]
    //public IHierarchyObject Parent { get; set; }
    //[SerializeField]
    //public List<IHierarchyObject> Children { get; set; }
    [field: SerializeField]
    public SerializableObject SerializableReference;
    [field: SerializeField]
    public SerializableObject Parent
    {
        get
        {
            return SerializableReference.Parent;
        }
        set
        {
            SerializableReference.Parent = value;
        }
    }
    [field: SerializeField]
    public List<SerializableObject> Children
    {
        get
        {
            return SerializableReference.Children;
        }
        set
        {
            SerializableReference.Children = value;
        }
    }
    [field: SerializeField]
    public GameObject ChildContainer { get; set; }
    [field: SerializeField]
    public Toggle ExpandChildrenButton;
    [field: SerializeField]
    public string Title 
    {
        get
        {
            return SerializableReference.Title;
        }
        set
        {
            SerializableReference.Title = value;
        }
    }
    [field: SerializeField]
    public TMP_InputField TextObject { get; set; }
    public Sprite CollapsedSprite;
    public Sprite ExpandedSprite;
    public Image HighlightImage = null;
    public Image UnderlineImage = null;
    public HierarchyObject CurrentDragTarget = null;
    public bool Selected = false;

    /// <summary>
    /// This is a pretty bad way of updating text.
    /// TODO: Move so it happens automatically
    /// </summary>
    public void UpdateText()
    {
        if (TextObject.text != Title)
        {
            TextObject.text = Title;
        }
    }

    public void ToggleChildrenVisibility(bool show)
    {
        ExpandChildrenButton.GetComponent<Image>().sprite = show ? ExpandedSprite : CollapsedSprite;
        foreach (var item in Children)
        {
            item.HierarchyReference.gameObject.SetActive(show);
        }
        UpdateSize();
        LayoutRebuilder.ForceRebuildLayoutImmediate(HierarchyParent.Instance.transform as RectTransform);
    }

    public const float BASE_WIDTH = 300;
    public const float BASE_HEIGHT = 50;
    public const float BASE_OFFSET = 25;

    public virtual void UpdateSize()
    {
        float height = gameObject.activeSelf ? BASE_HEIGHT : 0;
        float width = gameObject.activeSelf ? BASE_WIDTH : 0;
        float minx = float.MaxValue;
        float maxx = float.MinValue;
        for (int i = 0; i < Children.Count; i++)
        {
            if (Children[i].HierarchyReference.gameObject.activeSelf)
            {
                RectTransform childTransform = (Children[i].HierarchyReference.transform as RectTransform);
                height += childTransform.sizeDelta.y;
                if (childTransform.position.x < minx)
                {
                    minx = childTransform.position.x;
                }
                if (childTransform.position.x + childTransform.rect.width > maxx)
                {
                    maxx = childTransform.position.x + childTransform.rect.width;
                }
            }
        }
        (transform as RectTransform).sizeDelta = new Vector2(Mathf.Max(maxx - minx + BASE_OFFSET, BASE_WIDTH), height);
        if (Parent != null)
        {
            Parent.HierarchyReference.UpdateSize();
            LayoutRebuilder.ForceRebuildLayoutImmediate(Parent.HierarchyReference.ChildContainer.transform as RectTransform);
        }
    }

    protected static int spawnCounter = 0;

    public void SpawnChild()
    {
        GameObject spawnableObject = HierarchyParent.Instance.SpawnablePrefab;
        GameObject temp = Instantiate(spawnableObject);
        temp.name = "Spawned Object " + spawnCounter.ToString("000");
        temp.transform.SetParent(ChildContainer.transform, false);
        Children.Add(temp.GetComponent<HierarchyObject>().SerializableReference);
        spawnCounter++;
        //temp.GetComponent<HierarchyObject>().UpdateHeight();
        temp.GetComponent<HierarchyObject>().Parent = this.SerializableReference;
        if (ExpandChildrenButton != null)
        {
            // Setting isOn won't trigger events if set to the same value, so turn if off then back on
            ExpandChildrenButton.isOn = false;
            ExpandChildrenButton.isOn = true;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(HierarchyParent.Instance.transform as RectTransform);
    }

    public void SelectObjectForDrag(bool isHighlighted, bool isUnderlined)
    {
        HighlightImage.enabled = isHighlighted && !isUnderlined;
        UnderlineImage.enabled = isHighlighted && isUnderlined;
    }

    public void Select()
    {
        Selected = true;
    }

    public void Deselect()
    {
        Selected = false;
        TextObject.enabled = false;
        TextObject.GetComponent<Image>().enabled = false;
    }

    const float DOUBLE_CLICK_TIMER = .25f;
    IEnumerator PrepareForDoubleClick()
    {
        TextObject.enabled = true;
        TextObject.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(DOUBLE_CLICK_TIMER);
        if (!Selected)
        {
            Deselect();
        }
    }

    public void ClearChildren()
    {
        foreach (SerializableObject child in Children)
        {
            Destroy(child.HierarchyReference.gameObject);
        }
        Children.Clear();
    }

    #region UnityEvents
    void Awake()
    {
        SerializableReference = new SerializableObject();
        SerializableReference.HierarchyReference = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    void OnEnable()
    {
        //UpdateHeight();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
        StartCoroutine(PrepareForDoubleClick());
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // The bottom percentage of an object that must be selected to underline
        // ex: At .33 the bottom third of an object must be selected to underline it
        const float UNDERLINE_THRESHOLD = .33f;
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
        //Debug.Log(eventData.pointerEnter.GetComponentInParent<HierarchyObject>().transform.name);
        HierarchyObject draggedObject = eventData.pointerEnter?.GetComponentInParent<HierarchyObject>();
        if (draggedObject != null)
        {
            // Don't update self
            if (draggedObject != this)
            {
                // Just update highlight mode
                if (draggedObject == CurrentDragTarget)
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
                    Vector2 normalizedCollision = Rect.PointToNormalized((eventData.pointerEnter.transform as RectTransform).rect, localPoint);
                    bool isUnderlining = normalizedCollision.y < UNDERLINE_THRESHOLD;
                    CurrentDragTarget?.SelectObjectForDrag(true, isUnderlining);
                }
                // Change object being highlighted
                else
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
                    Vector2 normalizedCollision = Rect.PointToNormalized((eventData.pointerEnter.transform as RectTransform).rect, localPoint);
                    bool isUnderlining = normalizedCollision.y < UNDERLINE_THRESHOLD;
                    CurrentDragTarget?.SelectObjectForDrag(false, false);
                    CurrentDragTarget = draggedObject;
                    CurrentDragTarget.SelectObjectForDrag(true, isUnderlining);
                }
            }
        }
        else
        {
            CurrentDragTarget?.SelectObjectForDrag(false, false);
            CurrentDragTarget = draggedObject;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log(MethodBase.GetCurrentMethod().Name);
        if (CurrentDragTarget != null)
        {
            SerializableObject oldParent = Parent;
            // Lazy but efficient way to tell if we are highlighting vs underlining object
            bool firstChild = CurrentDragTarget.UnderlineImage.enabled;
            // Remove child from parent
            Parent.Children.Remove(SerializableReference);
            // Add as child of Drag Target
            Parent = CurrentDragTarget.SerializableReference;
            CurrentDragTarget.Children.Add(SerializableReference);
            transform.SetParent(CurrentDragTarget.ChildContainer.transform);
            if (firstChild)
            {
                transform.SetAsFirstSibling();
            }
            // Disable selection
            CurrentDragTarget.SelectObjectForDrag(false, false);
            oldParent.HierarchyReference.UpdateSize();
            Parent.HierarchyReference.UpdateSize();
        }
    }
    #endregion
}
