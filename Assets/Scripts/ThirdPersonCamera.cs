using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Car follow;
    [SerializeField] Camera camera;
    [SerializeField] Transform crane;
    [SerializeField] Transform boom;
    [SerializeField] float sensitivity = 1;
    [SerializeField] float easing = 1;

    public float yaw = 0;
    public float pitch = 0;
    Vector2 lookDelta = Vector2.zero;

    float timeSinceCameraAngleInput = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Quaternion currentYaw, currentPitch;

        float duck = Mathf.Lerp(0, -3, follow.speed / follow.maxSpeed * easing * Time.deltaTime);

        currentYaw = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, follow.heading, Vector3.up), 0);
        currentPitch = Quaternion.Euler(0, pitch, 0);
        crane.rotation = Quaternion.Lerp(crane.rotation, currentYaw, Time.deltaTime * easing);
        camera.transform.localRotation = Quaternion.Euler(-duck, 0, 0);

        ////lookDelta = player.actions .controls.Player.Look.ReadValue<Vector2>();
        //crane.localRotation = Quaternion.Slerp(Quaternion.Euler(0, yaw, 0), Quaternion.Euler(0, yaw + lookDelta.x / sensitivity, 0), Time.deltaTime / easing) *
        //   Quaternion.Slerp(Quaternion.Euler(pitch, 0, 0), Quaternion.Euler(pitch + lookDelta.y / sensitivity, 0, 0), Time.deltaTime / easing);
        //
        //yaw += lookDelta.x / sensitivity;
        //pitch -= lookDelta.y / sensitivity;
        //pitch = Mathf.Clamp(pitch, -5f, 80f);


        //if(timeSinceCameraAngleInput > 3)
        //{
        //    yaw = Mathf.Lerp(yaw, Vector3.SignedAngle(Vector3.forward, follow.heading, Vector3.up), Time.deltaTime / easing);
        //}


        timeSinceCameraAngleInput += Time.deltaTime;
    }

    private void OnLook(InputValue value)
    {
        //Debug.Log("Look");
        lookDelta = value.Get<Vector2>();
        timeSinceCameraAngleInput = 0;
    }
}
