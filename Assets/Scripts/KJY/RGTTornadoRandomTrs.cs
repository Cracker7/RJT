using System;
using UnityEngine;


public class RGTTornadoRandomTrs : MonoBehaviour
{
    [SerializeField] private Transform[] positions; // ����� ��ġ �迭
    [SerializeField] private GameObject obj; // �̵��� ������Ʈ
    [SerializeField] private float speed = 10f; // �̵� �ӵ�
    [SerializeField] Transform originPos;


    private Vector3 Opos;
    private int currentIndex = 0; // ���� ��ǥ ��ġ �ε���
    private Vector3 targetPosition; // ��ǥ ��ġ

    private void Start()
    {
        // �ʱ� ��ǥ ��ġ ����
        SetNextTargetPosition();
    }

    private void Update()
    {
        MoveToNextPosition();
        //Ray ray = new Ray(transform.position, transform.forward);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity, palyer))
        //{
        //    hit.collider.gameObject.transform.position = originPos.position;
        //}
    }

    // ��ǥ ��ġ�� �̵��ϴ� �Լ�
    private void MoveToNextPosition()
    {
        // �ε巴�� ��ǥ ��ġ�� �̵�
        obj.transform.position = Vector3.MoveTowards(
            obj.transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // ��ǥ ��ġ�� �����ϸ� ���� ��ġ ����
        if (Vector3.Distance(obj.transform.position, targetPosition) < 0.1f)
        {
            SetNextTargetPosition();
        }
    }

    // ���� ��ǥ ��ġ ���� �Լ�
    private void SetNextTargetPosition()
    {
        currentIndex = (currentIndex + 1) % positions.Length;
        targetPosition = positions[currentIndex].position;
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.layer == LayerMask.NameToLayer("carbody"))
        {
            other.gameObject.transform.position = originPos.position;
        }
    }
}