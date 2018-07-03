using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerInputManager : MonoBehaviour {

    public LineRenderer linePointer;
    public Material lineMaterial;
    public bool lefthand;
    public LayerMask teleportLayerMask;
    public GameObject cylinder;
    public GameObject cameraRig;
    public GameObject head;
    public float throwForce = 2.0f;
    public GameObject menu;
    public List<GameObject> rubeObjects;
    public List<GameObject> rubePrefabs;
    public List<Text> quantityTexts;

    public Text teleportMenu;
    public Text teleportTriggerMenu;
    public Text grabMenu;
    public Text releaseMenu;
    public Text menuMenu;
    public Text placePrefabMenu;

    public Material cheatingMaterial;
    public List<int> quantities;
    
    private const string INACTIVE_LAYER = "Inactive";

    private Vector3 headsetOffset;
    private const string THROWABLE_TAG = "Throwable";
    private const string STRUCTURE_TAG = "Structure";
    private int menuIndex;
    private bool teleportUsed = false;
    private bool grabUsed = false;
    private bool menuUsed = false;
    private bool cheating = false;
    private List<int> quanRemaining;

    private static GameObject rightHandObject;
    private static GameObject leftHandObject;
    private static bool leftHandLast;

    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        if (linePointer != null)
        {
            linePointer.material = lineMaterial;
        }
        menuIndex = 0;
        quanRemaining = new List<int>();
        for (int i = 0; i < quantities.Count; i++)
        {
            quanRemaining.Add(quantities[i]);
            quantityTexts[i].text = quantities[i].ToString();
        }
    }

    void Update () {
		if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (lefthand)
            {
                UpdatePointer();
                teleportMenu.gameObject.SetActive(false);
                if (!teleportUsed)
                {
                    teleportTriggerMenu.gameObject.SetActive(true);
                }
            }
        }
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (lefthand)
            {
                Teleport();
                teleportTriggerMenu.gameObject.SetActive(false);
            }
        }
        if (Controller.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (!lefthand)
            {
                if (quanRemaining[menuIndex] > 0)
                {
                    ActivateMenu();
                }
                menuMenu.gameObject.SetActive(false);
                if (!menuUsed)
                {
                    placePrefabMenu.gameObject.SetActive(true);
                }
            }
        }
        if (Controller.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (!lefthand)
            {
                DeactivateMenu();
                placePrefabMenu.gameObject.SetActive(false);
            }
        }
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            if (!lefthand)
            {
                if (Controller.GetAxis().x > 0)
                {
                    ScrollMenu("right", menuIndex);
                } else
                {
                    ScrollMenu("left", menuIndex);
                }

            }
        }
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (!lefthand)
            {
                if (Controller.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
                {
                    if (quanRemaining[menuIndex] > 0)
                    {
                        menuUsed = true;
                        Object.Instantiate(rubePrefabs[menuIndex], (transform.position + transform.forward), Quaternion.identity);
                        quanRemaining[menuIndex] = quanRemaining[menuIndex] - 1;
                        Debug.Log(quanRemaining[menuIndex].ToString());
                        quantityTexts[menuIndex].text = quanRemaining[menuIndex].ToString();
                        if (quanRemaining[menuIndex] == 0)
                        {
                            ScrollMenu("right", menuIndex);
                        }
                    }
                }
            }
        }
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            teleportUsed = false;
            grabUsed = false;
            menuUsed = false;
            if (teleportMenu != null)
            {
                teleportMenu.gameObject.SetActive(true);
            }
            if (grabMenu != null)
            {
                grabMenu.gameObject.SetActive(true);
            }
            if (menuMenu != null)
            {
                menuMenu.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == THROWABLE_TAG || other.gameObject.tag == STRUCTURE_TAG)
        {
            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                GrabObject(other);
            }
            if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                ReleaseObject(other);
            }
        }
    }

    void UpdatePointer ()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 30.0f, teleportLayerMask))
        {
            linePointer.gameObject.SetActive(true);
            cylinder.SetActive(true);
            linePointer.SetPosition(0, transform.position);
            linePointer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, hit.point.z));
            cylinder.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        } else
        {
            linePointer.gameObject.SetActive(false);
            cylinder.SetActive(false);
        }
    }

    void Teleport ()
    {
        headsetOffset = new Vector3(head.transform.position.x - cameraRig.transform.position.x, 0, head.transform.position.z - cameraRig.transform.position.z);
        linePointer.gameObject.SetActive(false);
        cylinder.SetActive(false);
        cameraRig.transform.position = cylinder.transform.position - headsetOffset;
        teleportUsed = true;
    }

    void GrabObject(Collider other)
    {
        Debug.Log(other.gameObject);
        other.transform.SetParent(transform);
        other.GetComponent<Rigidbody>().isKinematic = true;
        Controller.TriggerHapticPulse(2000);
        if (lefthand)
        {
            leftHandObject = other.gameObject;
            leftHandLast = true;
        } else
        {
            rightHandObject = other.gameObject;
            leftHandLast = false;
        }
        grabMenu.gameObject.SetActive(false);
        if (!grabUsed)
        {
            releaseMenu.gameObject.SetActive(true);
        }
        if (cheating)
        {
            other.gameObject.GetComponent<Renderer>().material = cheatingMaterial;
            other.gameObject.layer = LayerMask.NameToLayer(INACTIVE_LAYER);
        }
    }

    void ReleaseObject(Collider other)
    {
        if (leftHandObject != null && rightHandObject != null && leftHandObject == rightHandObject)
        {
            if (lefthand && !leftHandLast) {
                leftHandObject = null;
                return;
            }
            if (!lefthand && leftHandLast) {
                rightHandObject = null;
                return;
            }
        }

        other.transform.SetParent(null);
        if (other.gameObject.tag == THROWABLE_TAG)
        {
            Rigidbody rigid = other.GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.velocity = Controller.velocity * throwForce;
            rigid.angularVelocity = Controller.angularVelocity;
        }

        if (lefthand)
        {
            leftHandLast = false;
            leftHandObject = null;
        } else
        {
            leftHandLast = true;
            rightHandObject = null;
        }
        grabUsed = true;
        releaseMenu.gameObject.SetActive(false);
    }

    void ActivateMenu()
    {
        menu.SetActive(true);
        for (int i = 0; i < rubeObjects.Count; i++)
        {
            if (i == menuIndex)
            {
                rubeObjects[i].SetActive(true);
            } else
            {
                rubeObjects[i].SetActive(false);
            }
        }
    }

    void DeactivateMenu()
    {
        menu.SetActive(false);
    }

    void ScrollMenu(string direction, int start)
    {
        rubeObjects[menuIndex].SetActive(false);
        if (direction == "right")
        {
            if (menuIndex == rubeObjects.Count - 1)
            {
                menuIndex = 0;
            } else
            {
                menuIndex++;
            }
        } else
        {
            if (menuIndex == 0)
            {
                menuIndex = rubeObjects.Count - 1;
            }
            else
            {
                menuIndex--;
            }
        }
        rubeObjects[menuIndex].SetActive(true);
        if (quanRemaining[menuIndex] == 0)
        {
            if (menuIndex != start)
            {
                ScrollMenu(direction, start);
            } 
            if (menuIndex == start && quanRemaining[menuIndex] <= 0)
            {
                rubeObjects[menuIndex].SetActive(false);
            }
        }
    }

    public void SetCheating(bool cheating)
    {
        this.cheating = cheating;
    }
}
