using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float maxDurability; // 물체의 최대 내구도
    public float currentDurability; // 물체의 현재 내구도
    public bool isOccupied = false; // 현재 사용 중인지 여부

    public Transform mountPoint; // 탑승 위치
    public ObjectSpecificData objectData; // 물체의 고유 데이터

    public IMovement movementController; // 물체의 이동 컨트롤러
    public IInputHandler inputHandler; // 탑승시 사용할 입력 방식
    
    //public string mountAnimationName; // 탑승 애니메이션 이름
    //public MountType mountType; // 탑승 유형

    private void Awake()
    {
        Init();
        currentDurability = objectData.durability;
        maxDurability = objectData.durability;
    }

    public void Init()
    {
        movementController = GetComponent<IMovement>();
        inputHandler = GetComponent<IInputHandler>();
    }

    //// 플레이어가 상호작용 시 호출되는 함수
    //public virtual void Interact(PlayerKMS player)
    //{
    //    if (!isOccupied)
    //    {
    //        if (inputHandler != null)
    //            player.ChangeInput(inputHandler); // 플레이어의 입력 방식을 변경
            
    //        player.EnterObject(this); // 플레이어를 물체에 탑승시킴
    //        isOccupied = true; // 물체를 사용 중으로 표시
    //    }
    //}

    // 물체가 데미지를 입었을 때 호출되는 함수
    public virtual void TakeDamage(float damage)
    {
        currentDurability -= damage; // 내구도 감소
        if (currentDurability <= 0)
        {
            DestroyObject(); // 내구도가 0 이하가 되면 물체를 파괴

            //var player = FindObjectsByType<PlayerKMS>();
            // var player = FindFirstObjectByType<PlayerKMS>();
            // player.durabilityZero();
        }
    }

    // 물체가 파괴될 때 호출되는 함수
    private void DestroyObject()
    {
        // 파괴 로직 구현
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        TakeDamage(collision.impulse.magnitude);
        //Debug.Log("받은 충격량" + collision.impulse.magnitude);
    }
}
