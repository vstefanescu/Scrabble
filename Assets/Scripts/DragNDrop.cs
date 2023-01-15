using UnityEngine;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour
{
    // variable to store the original parent of the button
    private Transform originalParent;
    void Start() {
        EventTrigger[] buttons = GetComponentsInChildren<EventTrigger>();
        for (int i = 0; i < buttons.Length; i++)
        {
            EventTrigger.Entry dragEntry = new EventTrigger.Entry();
            dragEntry.eventID = EventTriggerType.Drag;
            dragEntry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            buttons[i].triggers.Add(dragEntry);
            EventTrigger.Entry dropEntry = new EventTrigger.Entry();
            dropEntry.eventID = EventTriggerType.Drop;
            dropEntry.callback.AddListener((data) => { OnDrop((PointerEventData)data); });
            buttons[i].triggers.Add(dropEntry);
            // store the original parent of the button
            originalParent = buttons[i].transform.parent;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        // Get the button that's being dragged
        GameObject button = eventData.pointerDrag;
        // Set the button's parent to the node that the pointer is currently over
        button.transform.SetParent(eventData.pointerEnter.transform);
        // Fix the button position on the node
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        RectTransform nodeRect = eventData.pointerEnter.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = nodeRect.anchoredPosition;
    }

    public void OnDrop(PointerEventData eventData) {
    // Get the button that was dropped
    GameObject button = eventData.pointerDrag;
    // Get the parent object of the node that the pointer is currently over
    GameObject nodesParent = eventData.pointerEnter.transform.parent.gameObject;
    // Get the index of the node that the pointer is currently over
    int nodeIndex = eventData.pointerEnter.transform.GetSiblingIndex();
    // Check if the node already has a child (another button)
    if (eventData.pointerEnter.transform.childCount > 0)
    {
        // if so, return the button to its original position
        button.transform.SetParent(originalParent);
    }
    else
    {
        // if not, attach the button to the node
        button.transform.SetParent(nodesParent.transform.GetChild(nodeIndex));
        // Fix the button position on the node
        RectTransform buttonRect = button.GetComponent<RectTransform>();
        RectTransform nodeRect = eventData.pointerEnter.GetComponent<RectTransform>();
        buttonRect.anchoredPosition = nodeRect.anchoredPosition;
    }
    }
}
