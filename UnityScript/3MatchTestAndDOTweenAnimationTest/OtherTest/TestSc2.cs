using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class TestSc2 : MonoBehaviour
{
    public GameObject startPanel;
    public Text startTxt;
    public GameObject board;

    private CanvasGroup cvg;

    private void Awake()
    {
        cvg = startPanel.GetComponent<CanvasGroup>();

        InitSet();
    }

    void InitSet()
    {

        Vector3 s = board.transform.localScale;
        board.transform.eulerAngles = new Vector3(0, -180, 0);
        board.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(startPanel.transform.DOMove(Camera.main.WorldToScreenPoint (Vector3.zero),1.3f).SetEase(Ease.OutCirc));
        seq.AppendInterval(1.3f);
        seq.Append(startPanel.transform.DOMove(Camera.main.WorldToScreenPoint(new Vector3(-80, 0, 0)), 1));
        seq.AppendCallback(() =>
        {
            board.transform.DOScale(s, 1);
            board.transform.DORotate(Vector3.zero, 1);
        });
        seq.Play();

        cvg.DOFade(1, 1);

        startTxt.DOText("Start Game",2f);
    }
}
