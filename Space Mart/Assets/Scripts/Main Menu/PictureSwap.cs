using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PictureChange : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerExitHandler, IDeselectHandler
{
    [Header("Portraits")]
    [SerializeField] private Image workerPortraitImage;     //Blank Image on canvas
    [SerializeField] private Sprite defaultPortrait;        //Default Image
    [SerializeField] private Sprite hoverOrSelectedPortrait; //Image to change to


    public void Awake()
    {
        SetPortrait(defaultPortrait);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetPortrait(hoverOrSelectedPortrait);
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        SetPortrait(defaultPortrait);
    }

   
    public void OnSelect(BaseEventData eventData)
    {
        SetPortrait(hoverOrSelectedPortrait);
    }

    
    public void OnDeselect(BaseEventData eventData)
    {
        SetPortrait(defaultPortrait);
    }

    private void SetPortrait(Sprite newSprite)
    {
        if (workerPortraitImage != null && newSprite != null)
        {
            workerPortraitImage.sprite = newSprite;
        }
    }
}
