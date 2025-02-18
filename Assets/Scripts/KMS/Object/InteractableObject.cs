using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;

public class InteractableObject : MonoBehaviour
{
    public float maxDurability;
    public float currentDurability;

    public Transform mountPoint;
    public ObjectSpecificData objectData;

    public IMovement movementController;
    public IInputHandler inputHandler;
    public RGTHpBar hpBar;
    private float damagePerSecond = 1f;
    public bool timeDamage = false;

    private bool collisionTriggered = false;      // �浹�� �߻��ߴ��� ���� �÷���
    private float collisionCooldown = 0.7f;           // �浹 ��ٿ� �ð� (�� ����)
    private Coroutine collisionCooldownCoroutine = null; // �������� �ڷ�ƾ ����

    // HP ������Ʈ�� ���� ��������Ʈ ����
    public delegate void OnRideUpdateDelegate();
    public OnRideUpdateDelegate onRideUpdate;

    public delegate void OnRideColUpdateDelegate();
    public OnRideColUpdateDelegate onRideCol;

    public event Action OnDestroyCalled;
    public event Action OnHpBarTr;

    public virtual void Awake()
    {
        Init();
        currentDurability = objectData.durability;
        maxDurability = objectData.durability;

        if (hpBar != null)
        {
            hpBar.UpdateHpBar(maxDurability, maxDurability);
        }
    }

    private void OnEnable()
    {
        collisionCooldownCoroutine = StartCoroutine(CollisionCooldownCoroutine());
    }

    //private void Start()
    //{
    //    hpBar.UpdateHpBar(maxDurability, maxDurability);

    //}

    private void Update()
    {
        if(onRideUpdate != null && OnDestroyCalled != null)
        {
            UpdateHpBarTr();
        }
    }

    public void Init()
    {
        movementController = GetComponent<IMovement>();
        inputHandler = GetComponent<IInputHandler>();
        hpBar = FindFirstObjectByType<RGTHpBar>();
        if(mountPoint == null)
        {
            mountPoint = transform;
        }
    }

    // �ܺο��� ȣ���ϸ� �ڷ�ƾ�� ���� HP�� �ε巴�� ���ҽ�ŵ�ϴ�.
    public void StartHpDecrease()
    {
        // Debug.Log("ü�� �Լ� �����");
        // currentDurability -= 1f;
        // hpBar.UpdateHpBar(currentDurability, maxDurability);
        if (!timeDamage) return;
        StartCoroutine(DecreaseHpCoroutine());
    }

    // 1�� ���� 5��ŭ HP�� �ε巴�� ���ҽ�Ű�� �ڷ�ƾ
    private IEnumerator DecreaseHpCoroutine()
    {
        while(currentDurability > 0){
            Debug.Log("ü�� �Լ� �����");

            currentDurability -= damagePerSecond * Time.deltaTime;

            Debug.Log("���� ü��" + currentDurability);

            hpBar.UpdateHpBar(maxDurability, currentDurability);

            yield return null;
        }
        
        if (currentDurability <= 0)
        {
            DestroyObject();
        }
    }

    // �浹�̳� Ʈ���� �߻� �� ȣ��� onRideUpdate�� ��ϵ� �Լ��Դϴ�.
    public void HandleCollisionDamage()
    {
        currentDurability -= maxDurability/3;
        Debug.Log("�浹/Ʈ���ŷ� ���� ü�� ����. ���� ü��: " + currentDurability);

        hpBar.UpdateHpBar(maxDurability, currentDurability);

        if (currentDurability <= 0)
        {
            DestroyObject();
        }
    }


    // hp�� ��ġ�� ������Ʈ �ϴ� �Լ�
    public void UpdateHpBarTr()
    {   
        hpBar.UpdatePosition(transform);

        // �̺�Ʈ�� ��ϵǾ��ִ°� �ִٸ� ����
        OnHpBarTr?.Invoke();
    }

    // ü���� 0�� �Ǹ� �ı��Ǵ� �Լ� ���� �� ������Ʈ ����
    public void DestroyObject()
    {
        Debug.Log("������Ʈ �ı���");

        OnDestroyCalled?.Invoke();

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("��ֹ��� �ε���");
        if (!collisionTriggered /*&& collision.gameObject.CompareTag("Obstacle")*/)
        {
            onRideCol?.Invoke();
            collisionCooldownCoroutine = StartCoroutine(CollisionCooldownCoroutine());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("������ ����");
        if (!collisionTriggered && other.CompareTag("Platform"))
        {
            Debug.Log("Trigger ���� ����");
            onRideCol?.Invoke();
            collisionCooldownCoroutine = StartCoroutine(CollisionCooldownCoroutine());
        }
    }

    private IEnumerator CollisionCooldownCoroutine()
    {
        collisionTriggered = true;
        yield return new WaitForSeconds(collisionCooldown);
        collisionTriggered = false;
        collisionCooldownCoroutine = null;
    }

    private void OnDestroy()
    {
        // ��������Ʈ ����
        onRideUpdate = null;
    }
}