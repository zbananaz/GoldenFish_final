using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unicorn
{
    public class TouchInteraction : MonoBehaviour,IPointerDownHandler
    {
        [SerializeField] private LevelController levelController;

        public void OnPointerDown(PointerEventData eventData)
        {
            // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
             var touchPosWorld = Camera.main.ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(touchPosWorld.x, touchPosWorld.y), Vector2.zero
                ,Mathf.Infinity, LayerMask.GetMask("Coin"));

            if (hit.collider != null)
            {
                hit.collider.GetComponent<Coin>().OnClick();
            }
            
        }
    }
}
