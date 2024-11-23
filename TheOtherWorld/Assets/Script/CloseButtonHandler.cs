using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CloseButtonHandler : MonoBehaviour
{
    public QuestPopUpHandler popupWindow;

    public void OnButtonClick()
    {
        var seq = DOTween.Sequence();

        float originScale = gameObject.transform.localScale.x;
        float shrinkRate = 0.2f;
        
        seq.Append(transform.DOScale(originScale - originScale * shrinkRate, 0.1f));
        seq.Append(transform.DOScale(originScale + originScale * shrinkRate, 0.1f));
        seq.Append(transform.DOScale(originScale, 0.1f));

        seq.Play().OnComplete(() => {
            popupWindow.Hide();
        });
    }
}
