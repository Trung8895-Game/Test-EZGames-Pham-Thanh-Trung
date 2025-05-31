using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform ringCenter;
    [SerializeField] private Transform followTarget;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private bool isIntro = true;
    [SerializeField] private CinemachineVirtualCamera mainCam;

    private void Update()
    {
        if (isIntro)
        {
            transform.RotateAround(ringCenter.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }

    public void EndIntroAndFollow(Transform target)
    {
        isIntro = false;
        followTarget = target;
        StartCoroutine(SnapToPlayerView());
    }

    private IEnumerator SnapToPlayerView()
    {
        
        yield return new WaitForSeconds(0.2f);
        mainCam.Follow = followTarget;
        mainCam.LookAt = followTarget.GetComponent<PlayerController>().head;
    }
}
