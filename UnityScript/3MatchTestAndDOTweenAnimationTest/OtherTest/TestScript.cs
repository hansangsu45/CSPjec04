
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
    public Sprite front;
    public Sprite back;
    public Ease ease;

    private bool isFront = true;
    private bool isChanging = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && !isChanging)
        {
            isChanging = true;
            Sequence seq = DOTween.Sequence();
            seq.Append( transform.DORotateQuaternion(Quaternion.Euler(0, 90, 0), 0.5f) );
            seq.AppendCallback(() => 
            { 
                GetComponent<Image>().sprite = isFront ? back : front;
                Sequence seq2 = DOTween.Sequence();
                seq2.Append(transform.DORotateQuaternion(Quaternion.Euler(0, isFront ? 180 : 0, 0), 0.5f));
                seq2.AppendCallback(() => { isFront = !isFront; isChanging = false;});
                seq2.Play();
            });
            seq.Play();
        }

        if(Input.GetKeyDown(KeyCode.B) && !isChanging)
        {
            transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 1.2f).SetEase(ease);
            Vector3 v = Camera.main.WorldToScreenPoint(new Vector3(0, 1, 0));
            transform.DOMove(v,0.3f);
            transform.DORotateQuaternion(Quaternion.Euler(0, 179, 0), 0.3f);
        }
    }
}
