using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    public CinemachineCamera mainVCam;
    public CinemachineCamera subVCam;
    public float switchDuration = 3f;

    private bool isSwitching = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSwitching)
        {
            StartCoroutine(SwitchCinemachineCamera());
        }
    }

    private IEnumerator SwitchCinemachineCamera()
    {
        isSwitching = true;

        // ���� ī�޶� �켱���� ����
        subVCam.Priority = 15;
        mainVCam.Priority = 10;

        // ���� �ð� ���
        yield return new WaitForSeconds(switchDuration);

        // �ٽ� ���� ī�޶�� ����
        subVCam.Priority = 5;
        mainVCam.Priority = 10;

        isSwitching = false;
    }
}
