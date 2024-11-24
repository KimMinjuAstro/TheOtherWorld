using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CheckButtonHandler : MonoBehaviour
{
   [System.Serializable]
   public class ButtonSelectedEvent : UnityEvent<int> { }

   public ButtonSelectedEvent onSelectedButtonInfo;
   private GameObject giftPopUpUI;
   public GameObject commonUI;

   private Vector3 originalPosition;

   private void Awake()
   {
      Button button = GetComponent<Button>();
      button.onClick.AddListener(OnInfoButtonClick);
   }

   private void OnInfoButtonClick()
   {
      GiftBoxHandler giftBox = GiftBoxHandler.GiftBox;

      if (giftBox != null)
      {
         // 선택된 버튼의 ID 정보를 이벤트로 전달
         onSelectedButtonInfo?.Invoke(giftBox.ButtonId);
         
         Debug.Log($"Selected Button ID: {giftBox.ButtonId}");

         Transform giftBoxes = GameObject.Find("GiftBox").transform;
         foreach (Transform gB in giftBoxes)
         {
            int buttonId = gB.gameObject.GetComponent<GiftBoxHandler>().ButtonId;
            if (buttonId != giftBox.ButtonId)
            {
               gB.gameObject.SetActive(false);
            }
         }

         originalPosition = giftBox.transform.position;
         giftBox.StopAnimation();
         gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
         
         Vector2 centerPosition = new Vector2(123f, -50f);
         RectTransform rectTransform = giftBox.GetComponent<RectTransform>();

         Sequence sequence = DOTween.Sequence();

         sequence.Append(rectTransform.DOAnchorPos(centerPosition, 1f).SetEase(Ease.InQuart))
            .Join(rectTransform.DOScale(1.5f, 0.5f)) 
            .Append(rectTransform.DOScale(1.3f, 0.5f)) 
            .Join(rectTransform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360)) // 회전 효과
            .OnComplete(() =>
            {
               giftPopUpUI = GameObject.Find("GiftPopUp");
               StartCoroutine(SetActiveDelay(giftPopUpUI, 1.0f, false, giftBox.gameObject));
               
            });
      }
      else
      {
         Debug.Log("No button currently selected");
      }
   }

   IEnumerator SetActiveDelay(GameObject go ,float time, bool tf, GameObject gb)
   {
      yield return new WaitForSeconds(time);
      gb.transform.position = originalPosition;
      go.SetActive(tf);
      commonUI.SetActive(true);
   }
   
   private void OnDestroy()
   {
      Button button = GetComponent<Button>();
      button.onClick.RemoveListener(OnInfoButtonClick);
   }
}