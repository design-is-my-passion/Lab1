using UnityEngine;

public class TimeController : MonoBehaviour
{
    struct Components {
        public Vector3 pos;
        public Vector3 vel;
    }

    private Components[,] recordedComponents;
    private TimeControlled[] timeObjects;
    private int recordMax = 1000000;
    private int recordIndex = 0;

    private bool wasRewinding = false;

    void Start()
    {
        timeObjects = FindObjectsByType<TimeControlled>(FindObjectsSortMode.None);
        recordedComponents = new Components[timeObjects.Length, recordMax];
    }

    void Update()
    {
        bool rewinding = Input.GetKey(KeyCode.R);
        if (rewinding) {
            Debug.Log("Rewinding: " + recordIndex + " " + timeObjects.Length);
            if (!wasRewinding) {
                wasRewinding = true;
                Time.timeScale = 0.0f;
                GetComponent<PlayerMovement>().normalUI.enabled = true;
                GetComponent<PlayerMovement>().gameOver.enabled = false;

                GetComponent<PlayerMovement>().flagText.SetActive(false);
                GetComponent<PlayerMovement>().normalText.SetActive(true);
            }

            if (recordIndex == 0)
            {
                return;
            }

            recordIndex--;
            for (int i = 0; i < timeObjects.Length; i++)
            {
                TimeControlled timeObject = timeObjects[i];
                Components components = recordedComponents[i, recordIndex];
                timeObject.body.transform.position = components.pos;
                timeObject.body.linearVelocity = components.vel;
            }
        } else {
            if (wasRewinding)
            {
                Time.timeScale = 1.0f;
                wasRewinding = false;
            }

            if (recordIndex >= recordMax)
            {
                // TODO: Compress time components
                // Will just log for now
                Debug.Log("Time Controller Full");
                return;
            }

            if (GetComponent<PlayerMovement>().gameOver.enabled)
            {
                return;
            }

            for (int i = 0; i < timeObjects.Length; i++)
            {
                TimeControlled timeObject = timeObjects[i];
                Components components = new Components();
                components.pos = timeObject.body.transform.position;
                components.vel = timeObject.body.linearVelocity;
                recordedComponents[i, recordIndex] = components;
            }
            recordIndex++;
        }
    }

    public void ClearRecord()
    {
        recordIndex = 0;
    }
}
