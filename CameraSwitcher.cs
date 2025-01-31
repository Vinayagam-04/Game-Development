using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    // References to the Cinemachine FreeLook cameras
    [SerializeField] private CinemachineFreeLook carCamera;
    [SerializeField] private CinemachineFreeLook characterCamera;

    // Input keys to switch between car and character cameras
    [SerializeField] private KeyCode switchToCarKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode switchToCharacterKey = KeyCode.Alpha2;

    // Cached reference to the CinemachineBrain
    private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        // Ensure only one FreeLook camera is active at the start
        if (carCamera != null && characterCamera != null)
        {
            carCamera.Priority = 10; // Higher priority means active
            characterCamera.Priority = 0; // Lower priority means inactive
        }

        // Cache the CinemachineBrain reference
        cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();

        if (cinemachineBrain == null)
        {
            Debug.LogError("CinemachineBrain not found on the Main Camera.");
        }
    }

    private void Update()
    {
        // Handle input for switching cameras
        if (Input.GetKeyDown(switchToCarKey))
        {
            SwitchToCar();
        }

        if (Input.GetKeyDown(switchToCharacterKey))
        {
            SwitchToCharacter();
        }
    }

    private void SwitchToCar()
    {
        if (carCamera != null && characterCamera != null)
        {
            // Set priority to make car camera active
            carCamera.Priority = 10;
            characterCamera.Priority = 0;
        }
    }

    private void SwitchToCharacter()
    {
        if (carCamera != null && characterCamera != null)
        {
            // Set priority to make character camera active
            carCamera.Priority = 0;
            characterCamera.Priority = 10;
        }
    }
}
