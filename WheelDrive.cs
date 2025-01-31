using UnityEngine;

public class WheelDrive : MonoBehaviour
{
    public WheelCollider[] wheelColliders; // Wheel Colliders for each wheel
    public Transform[] wheelMeshes;       // Wheel meshes for visual update
    public float motorForce = 1500f;      // Motor power for movement
    public float steeringAngle = 30f;     // Steering angle for turning

    void FixedUpdate()
    {
        // Get user input
        float verticalInput = Input.GetAxis("Vertical"); // Forward/Backward
        float horizontalInput = Input.GetAxis("Horizontal"); // Left/Right

        // Apply motor force to rear wheels
        wheelColliders[2].motorTorque = verticalInput * motorForce; // Rear Left
        wheelColliders[3].motorTorque = verticalInput * motorForce; // Rear Right

        // Apply steering to front wheels
        wheelColliders[0].steerAngle = horizontalInput * steeringAngle; // Front Left
        wheelColliders[1].steerAngle = horizontalInput * steeringAngle; // Front Right

        // Update wheel mesh positions to match the physics
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            UpdateWheelPose(wheelColliders[i], wheelMeshes[i]);
        }
    }

    void UpdateWheelPose(WheelCollider collider, Transform mesh)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        mesh.position = position;
        mesh.rotation = rotation;
    }
}
