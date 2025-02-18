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

        // 보조 카메라 우선순위 증가
        subVCam.Priority = 15;
        mainVCam.Priority = 10;

        // 일정 시간 대기
        yield return new WaitForSeconds(switchDuration);

        // 다시 원래 카메라로 복귀
        subVCam.Priority = 5;
        mainVCam.Priority = 10;

        isSwitching = false;
    }
}
