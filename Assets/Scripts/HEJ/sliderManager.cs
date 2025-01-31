using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class sliderManager : MonoBehaviour
{
    public Slider slider;
    public int speed;
    public float minPos;
    public float maxPos;
    public RectTransform pass;
    public int atkNum;

    public void SetAtk()
    {
        slider.value = 0;
        minPos = pass.anchoredPosition.x;
        maxPos = pass.sizeDelta.x + minPos;
        StartCoroutine(ComAtk());
    }

    IEnumerator ComAtk()
    {
        yield return null;
        while (!(Input.GetKeyDown(KeyCode.Space) || slider.value == slider.maxValue))
        {
            slider.value += Time.deltaTime * speed;
            yield return null;
        }
        if(slider.value >= minPos && slider.value <= maxPos)
        {

        }
    }
}
