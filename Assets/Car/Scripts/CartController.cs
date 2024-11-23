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
        // ���� �ð��� ȸ�� ����ȭ
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel, rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransform);
    }

    private void FixedUpdate()
    {
        // �Է� �ޱ�
        float steer = Input.GetAxis("Horizontal");
        float throttle = Input.GetAxis("Vertical");

        // �ڵ� ����
        frontLeftWheel.steerAngle = steer * maxSteerAngle;
        frontRightWheel.steerAngle = steer * maxSteerAngle;

        // ���� ����
        frontLeftWheel.motorTorque = throttle * motorTorque;
        frontRightWheel.motorTorque = throttle * motorTorque;

        // �극��ũ
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

            // ������ ���� �������� �ٿ� �帮��Ʈ ȿ�� ����
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

            // ������ ���� �������� ���� ���·� ����
            ModifyWheelFriction(rearLeftWheel, 1f, 1f);
            ModifyWheelFriction(rearRightWheel, 1f, 1f);

            Debug.Log("Drift stopped!");
        }
    }

    private void ModifyWheelFriction(WheelCollider wheel, float forwardStiffness, float sidewaysStiffness)
    {
        // WheelFrictionCurve�� �����Ͽ� ���� ��� ����
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

        forwardFriction.stiffness = forwardStiffness;
        sidewaysFriction.stiffness = sidewaysStiffness;

        wheel.forwardFriction = forwardFriction;
        wheel.sidewaysFriction = sidewaysFriction;
    }

    private void UpdateWheelPose(WheelCollider collider, Transform trans)
    {
        // Wheel Collider�� ��ġ �� ȸ���� Mesh�� �ݿ�
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }
}
