using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// ScriptableRendererFeature를 상속받아 커스텀 렌더 패스를 추가하는 클래스
public class Water_Volume : ScriptableRendererFeature
{
    // 커스텀 렌더 패스 클래스 정의
    class CustomRenderPass : ScriptableRenderPass
    {
        public RTHandle source; // 렌더 타겟 핸들
        private Material _material; // 렌더링에 사용할 머티리얼
        private RTHandle tempRenderTarget; // 임시 렌더 타겟 핸들

        // 생성자에서 머티리얼을 받아 초기화
        public CustomRenderPass(Material mat)
        {
            _material = mat;
            // RTHandle을 생성하여 임시 렌더 타겟을 할당
            tempRenderTarget = RTHandles.Alloc("_TemporaryColourTexture", name: "_TemporaryColourTexture");
        }

        // 렌더 패스 실행
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // 반사 카메라에서는 효과를 적용하지 않음
            if (renderingData.cameraData.cameraType != CameraType.Reflection)
            {
                // 커맨드 버퍼 생성
                CommandBuffer commandBuffer = CommandBufferPool.Get();

                // 첫 번째 블릿: 원본 소스를 임시 렌더 타겟으로 복사하면서 머티리얼 적용
                Blitter.BlitCameraTexture(commandBuffer, source, tempRenderTarget, _material, 0);
                // 두 번째 블릿: 임시 렌더 타겟을 다시 원본 소스로 복사
                Blitter.BlitCameraTexture(commandBuffer, tempRenderTarget, source);

                // 커맨드 버퍼 실행
                context.ExecuteCommandBuffer(commandBuffer);
                // 커맨드 버퍼 해제
                CommandBufferPool.Release(commandBuffer);
            }
        }

        // 카메라 렌더링이 끝날 때 호출되는 정리 메서드
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            // 할당된 RTHandle을 해제하여 메모리 누수를 방지
            if (tempRenderTarget != null)
            {
                tempRenderTarget.Release();
            }
        }
    }

    // 설정 클래스 정의 (Serializable로 설정하여 인스펙터에서 조정 가능)
    [System.Serializable]
    public class _Settings
    {
        public Material material = null; // 사용할 머티리얼
        public RenderPassEvent renderPass = RenderPassEvent.AfterRenderingSkybox; // 렌더 패스 이벤트 설정
    }

    public _Settings settings = new _Settings(); // 설정 인스턴스 생성
    CustomRenderPass m_ScriptablePass; // 커스텀 렌더 패스 인스턴스

    // 렌더 기능 생성 (초기화)
    public override void Create()
    {
        // 머티리얼이 설정되지 않았을 경우, Resources 폴더에서 기본 머티리얼 로드
        if (settings.material == null)
        {
            settings.material = (Material)Resources.Load("Water_Volume");
        }

        // 커스텀 렌더 패스 생성
        m_ScriptablePass = new CustomRenderPass(settings.material);
        // 렌더 패스 이벤트 설정
        m_ScriptablePass.renderPassEvent = settings.renderPass;
    }

    // 렌더 패스를 렌더러에 추가
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // cameraColorTargetHandle을 사용하여 소스를 설정 (cameraColorTarget은 더 이상 사용되지 않음)
        m_ScriptablePass.source = renderer.cameraColorTargetHandle;
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
