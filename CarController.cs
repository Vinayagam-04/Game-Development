using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBrakeForce;
    private bool isBraking;

    // Settings
    [SerializeField] private float motorForce = 2000f; 
    [SerializeField] private float brakeForce = 2000f; // Reduced brake force
    [SerializeField] private float maxSteerAngle = 35f;
    [SerializeField] private float resetForce = 5f;
    [SerializeField] private float speedFactor = 10f; // Steering sensitivity factor

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    // Wheel Transforms
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private Rigidbody carRigidbody;

    private void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();

        // Increase mass to improve grip
        carRigidbody.mass = 1500f; 

        SetWheelColliderFriction();
    }

    private void Update()
    {
        GetInput();
        HandleReset();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        float torque = verticalInput * motorForce;
        frontLeftWheelCollider.motorTorque = torque;
        frontRightWheelCollider.motorTorque = torque;

        // Apply smooth braking
        currentBrakeForce = isBraking ? Mathf.Lerp(currentBrakeForce, brakeForce, Time.deltaTime * 5) : 0f;
        ApplyBraking();
    }

    private void ApplyBraking()
    {
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void HandleSteering()
    {
        float speed = carRigidbody.velocity.magnitude;
        float steerLimit = Mathf.Lerp(maxSteerAngle, maxSteerAngle / 2, speed / speedFactor);
        currentSteerAngle = steerLimit * horizontalInput;

        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        if (wheelCollider == null || wheelTransform == null) return;

        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    private void HandleReset()
    {
        if (IsCarFlipped() || Input.GetKeyDown(KeyCode.R))
        {
            ResetCarPosition();
        }
    }

    private bool IsCarFlipped()
    {
        return Vector3.Dot(transform.up, Vector3.up) < 0.5f;
    }

    private void ResetCarPosition()
    {
        transform.position += Vector3.up * 2f;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
    }

    private void SetWheelColliderFriction()
    {
        WheelFrictionCurve forwardFriction = new WheelFrictionCurve
        {
            extremumSlip = 0.3f, // Reduced for better grip
            extremumValue = 1f,
            asymptoteSlip = 0.7f,
            asymptoteValue = 0.85f,
            stiffness = 2.5f // Increased stiffness for better control
        };

        WheelFrictionCurve sidewaysFriction = new WheelFrictionCurve
        {
            extremumSlip = 0.2f,
            extremumValue = 1f,
            asymptoteSlip = 0.5f,
            asymptoteValue = 0.85f,
            stiffness = 2.5f
        };

        ApplyFriction(frontLeftWheelCollider, forwardFriction, sidewaysFriction);
        ApplyFriction(frontRightWheelCollider, forwardFriction, sidewaysFriction);
        ApplyFriction(rearLeftWheelCollider, forwardFriction, sidewaysFriction);
        ApplyFriction(rearRightWheelCollider, forwardFriction, sidewaysFriction);
    }

    private void ApplyFriction(WheelCollider wheel, WheelFrictionCurve forwardFriction, WheelFrictionCurve sidewaysFriction)
    {
        wheel.forwardFriction = forwardFriction;
        wheel.sidewaysFriction = sidewaysFriction;
    }
}
