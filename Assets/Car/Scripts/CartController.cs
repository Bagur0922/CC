using UnityEngine;

public class KartController : MonoBehaviour
{
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    public float motorTorque = 1500f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 3000f;

    private bool isDrifting = false;

    private void Update()
    {
        // 바퀴 시각적 회전 동기화
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel, rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransform);
    }

    private void FixedUpdate()
    {
        // 입력 받기
        float steer = Input.GetAxis("Horizontal");
        float throttle = Input.GetAxis("Vertical");

        // 핸들 조향
        frontLeftWheel.steerAngle = steer * maxSteerAngle;
        frontRightWheel.steerAngle = steer * maxSteerAngle;

        // 엔진 동력
        frontLeftWheel.motorTorque = throttle * motorTorque;
        frontRightWheel.motorTorque = throttle * motorTorque;

        // 브레이크
        if (Input.GetKey(KeyCode.Space))
        {
            StartDrift();
        }
        else
        {
            StopDrift();
        }
    }

    private void StartDrift()
    {
        if (!isDrifting)
        {
            isDrifting = true;

            // 바퀴의 측면 마찰력을 줄여 드리프트 효과 적용
            ModifyWheelFriction(rearLeftWheel, 0.5f, 0.3f);
            ModifyWheelFriction(rearRightWheel, 0.5f, 0.3f);

            Debug.Log("Drift started!");
        }
    }

    private void StopDrift()
    {
        if (isDrifting)
        {
            isDrifting = false;

            // 바퀴의 측면 마찰력을 원래 상태로 복구
            ModifyWheelFriction(rearLeftWheel, 1f, 1f);
            ModifyWheelFriction(rearRightWheel, 1f, 1f);

            Debug.Log("Drift stopped!");
        }
    }

    private void ModifyWheelFriction(WheelCollider wheel, float forwardStiffness, float sidewaysStiffness)
    {
        // WheelFrictionCurve를 수정하여 마찰 계수 조정
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

        forwardFriction.stiffness = forwardStiffness;
        sidewaysFriction.stiffness = sidewaysStiffness;

        wheel.forwardFriction = forwardFriction;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void UpdateWheelPose(WheelCollider collider, Transform trans)
    {
        // Wheel Collider의 위치 및 회전을 Mesh에 반영
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }
}
