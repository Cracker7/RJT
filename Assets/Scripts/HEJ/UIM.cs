using UnityEngine;
using UnityEngine.UI;

public class UIM : MonoBehaviour
{
    RawImage star2;
    RectTransform star;

    public Transform player;
    Vector2 player2;

    private void Awake()
    {
        star2 = GetComponent<RawImage>();
        star = GetComponent<RectTransform>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        player2 = star.anchoredPosition;
    }
}
