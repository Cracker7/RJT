using TMPro;
using UnityEngine;

public class RGTDeadCntManger : MonoBehaviour
{
    //[SerializeField] private PlayerKMS player;
    [SerializeField] private TextMeshProUGUI deadCnt;

    private void Start()
    {
        TheDeadCnt();
    }

    private void TheDeadCnt()
    {
        string Cnt = PlayerKMS.DeadCnt.ToString();

        deadCnt.text = ("Áö¸° È½¼ö : ") + Cnt;
    }
    
}
