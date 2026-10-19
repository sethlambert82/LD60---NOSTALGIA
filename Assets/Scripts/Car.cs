using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [SerializeField] Transform[] frontTires = new Transform[2];

    Rigidbody rb;
    [SerializeField] Transform carBody;
    public Vector2 controlVector = Vector2.zero;
    [SerializeField] float acceleration = 15;
    [SerializeField] float decay = 0.65f;
    public float maxSpeed = 55;
    [SerializeField] float maxTurnRate = 30f;
    [SerializeField] float maxForce = 300;

    Vector3 steeringForce = Vector3.zero;
    Vector3 steeringAcceleration = Vector3.zero;
    Vector3 steeringVelocity = Vector3.zero;
    public Vector3 heading = Vector3.forward;
    public Vector3 crossTrack = Vector3.zero;
    public float speed;
    float steeredSpeed = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        steeringVelocity = rb.transform.forward;

    }

    // Update is called once per frame
    void Update()
    {
        //This is the fallback, old steering algorithm
        Vector3 planarVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, Vector3.up);
        speed = planarVelocity.magnitude;
        steeredSpeed = steeringVelocity.magnitude;
        if (speed > 0.05f)
        {
            heading = planarVelocity / speed;
            crossTrack = Vector3.Cross(steeringVelocity, Vector3.up);
        }

        steeringForce = steeringVelocity/steeredSpeed * (controlVector.y * acceleration - (speed > 0.05f ? decay : 0));
        if (Mathf.Abs(controlVector.x*maxTurnRate) >0.5f)
        {
            float radius = GetPivotRadius(controlVector.x * maxTurnRate);

            if(radius >0.1f)
            {
                float centripetalAcceleration = rb.linearVelocity.sqrMagnitude / radius;
                steeringForce += centripetalAcceleration * crossTrack * -Mathf.Sign(controlVector.x);
                if(speed>0.05f && Mathf.Abs(controlVector.y)<0.1f) steeringForce += steeringVelocity/steeredSpeed * -decay * 2f;

                //rb.transform.rotation = rb.transform.rotation * Quaternion.Euler(0, turnAngle*Time.deltaTime, 0);

            }
        }

        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);

        //rb.AddForce(steeringForce, ForceMode.Force);
        


        steeringAcceleration = steeringForce / rb.mass;
        steeringVelocity += steeringAcceleration * Time.deltaTime;
        steeringVelocity = Vector3.ClampMagnitude(steeringVelocity, maxSpeed);



        rb.linearVelocity = new Vector3(steeringVelocity.x, rb.linearVelocity.y + Physics.gravity.y*Time.deltaTime, steeringVelocity.z);


        foreach (Transform t in frontTires)
        {
            t.transform.localEulerAngles = Vector3.up * (-90 + maxTurnRate * controlVector.x);
        }

        //rb.transform.forward = heading;
        //rb.AddTorque(Vector3.up*(maxTurnRate * controlVector.x), ForceMode.Force);
        if(speed>0.05f)carBody.transform.forward = Vector3.Lerp(carBody.transform.forward, steeringVelocity,0.5f);
    }


    Vector3 GetPivot(float turnAngle)
    {
        //1.2m between axles
        if (Mathf.Abs(turnAngle) < 0.1f) return Vector3.zero;
        if (rb.linearVelocity.sqrMagnitude < 0.1f) return Vector3.zero;

        float sign = Mathf.Sign(turnAngle);
        return rb.transform.TransformDirection(new Vector3(1.2f * Mathf.Tan(Mathf.Deg2Rad*(90 - turnAngle)) + (sign*0.52f),0,0));
    }

    float GetPivotRadius(float turnAngle)
    {
        //Debug.Log($"Mag below 0.1: {Mathf.Abs(turnAngle)<0.1f}. Velocity too low: {rb.linearVelocity.sqrMagnitude}");


        if (Mathf.Abs(turnAngle) < 0.1f || rb.linearVelocity.sqrMagnitude<0.1f) return 0;

        return 1.2f * Mathf.Tan(Mathf.Deg2Rad * (90 - Mathf.Abs(turnAngle))) - 0.52f;

    }


    private void OnSteer(InputValue value)
    {
        controlVector.x = value.Get<float>();
    }

    private void OnGas(InputValue value)
    {
        if (!RaceManager.raceBegun) return;
        controlVector.y = value.Get<float>();
    }



    //private void OnDrawGizmosSelected()
    //{
    //    if (rb == null) return;
    //    Vector3 refPosition = rb.position + Vector3.up * 2f;
    //
    //
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(refPosition, refPosition + crossTrack);
    //
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawLine(refPosition, refPosition + steeringVelocity);
    //
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(refPosition, refPosition + rb.linearVelocity);
    //
    //    Gizmos.DrawSphere(refPosition + crossTrack * GetPivotRadius(controlVector.x * maxTurnRate) * controlVector.x, 0.25f);
    //}
    //
    //
    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(0,0,Screen.width, Screen.height));
    //
    //    GUILayout.Label($"Command Vector: {controlVector.ToString("F3")}");
    //    GUILayout.Label($"Steering Acceleration: {steeringAcceleration.ToString("F3")}");
    //    GUILayout.Label($"Steering Velocity: {steeringVelocity.ToString("F3")}");
    //    GUILayout.Label($"Linear Velocity: {rb.linearVelocity}");
    //    GUILayout.Label($"Speed: {speed.ToString("F3")}");
    //    GUILayout.Label($"Heading: {heading.ToString("F3")}");
    //    GUILayout.Label($"CrossTrack: {crossTrack.ToString("F3")}");
    //
    //
    //    GUILayout.EndArea();
    //
    //}
}
