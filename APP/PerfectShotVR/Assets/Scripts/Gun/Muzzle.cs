using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

// 총이 실제로 발사되는 부분은 총구이다.
public class Muzzle : MonoBehaviour
{
    private LineRenderer bulletLineRenderer;
    private AudioSource audioSource;

    public AudioClip fireSound;
    public GameObject hitholePrefab;

    private void Awake()
    {
        // 라인렌더러 초기화 부분.
        bulletLineRenderer = GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
        // 오디오 소스 초기화 부분.
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = fireSound;
    }

    // Shot의 값을 가져오면서 발포하는 이펙트를 처리함.
    // 소리 효과 등을 이곳에 가져옴.
    public void Fire()
    {
        // 총이 발사되는 소리부터 재생
        audioSource.Play();
        // 총알은 일단 총구로부터 시작
        bulletLineRenderer.SetPosition(0, transform.position);
        Shot();
    }

    // 발사 처리
    // 맞았을 경우에 맞은 지점의 vector3를 반환한다. 맞지 않았을 경우 null 반환.
    protected void Shot()
    {
        // 충돌한 경우 조건문 진입
        // Debug.Log($"레이캐스트 : ({transform.position.x}, {transform.position.y}, {transform.position.z})에서 ({transform.forward.x}, {transform.forward.y}, {transform.forward.z})으로");
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 300))
        {
            ITarget target = hit.collider.GetComponent<ITarget>();
            // 맞은 게 표적이고 활성화 상태일 때
            if (target != null && target.GetState()) target.OnHit(hit);
            // 타겟이 맞지 않은 경우
            else Debug.Log($"타겟 대신 {hit.collider.name} 맞음");
        }
    }
}
