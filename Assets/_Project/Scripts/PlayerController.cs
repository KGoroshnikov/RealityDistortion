using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerContoller : Character
{
    [Header("Player Setttings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;

    private Vector3 jointOriginalPos;
    private float timer = 0;
    [SerializeField] private Transform joint;
    [SerializeField] private float bobSpeed = 10f;
    [SerializeField] private Vector3 bobAmount = new Vector3(.15f, .05f, 0f);
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Animator animator;
    public enum state{
        Running, Idle, NoUse
    }
    public state m_state;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    [SerializeField] private Vector2 mouseSensitivity;
    [SerializeField] private float maxLookAngle = 85f;

    [SerializeField] private InputActionReference wasd, look, jump;
    private Action<InputAction.CallbackContext> jumpDelegate;

    private float forceDamping = 0.95f;
    private float minForceThreshold = 0.1f;

    private Vector3 dir;

    private bool canJump = true;

    


    [SerializeField] private Interaction interaction;
    [SerializeField] private VHCController vHCController;
    private bool canBeUnFreezed = true;
    [SerializeField] private AudioSource walkAudio;
    [SerializeField] private float audioThresold;

    [SerializeField] private AudioSource portalSound;
    [SerializeField] private GameObject[] ignorePortalSoundPortals;

    void Awake()
    {
        jumpDelegate = ctx => Jump();   
    }

    void Start(){
        base.Start();
        jointOriginalPos = joint.localPosition;
        m_state = state.Idle;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GetSens();
    }

    public void GetSens(){
        mouseSensitivity.x = PlayerPrefs.GetFloat("PlayerSens", 0.4f);
        mouseSensitivity.y = mouseSensitivity.x * 0.375f;
    }

    void OnEnable() {
        jump.action.performed += jumpDelegate;
    }

    void OnDisable() {
        jump.action.performed -= jumpDelegate;
    }

    void FixedUpdate(){
        base.FixedUpdate();
        CalculateExternalForce();

        MoveCharacter(dir * walkSpeed);
    }

    void Update()
    {
        if (m_state != state.NoUse){
            Rotating();
            dir = new Vector3(wasd.action.ReadValue<Vector2>().x, 0, wasd.action.ReadValue<Vector2>().y);
            UpdateAnim(dir);
            HeadBob();
        }
    }

    public void SetRunningMode(){
        canJump = false;
        walkSpeed = 6f;
        bobSpeed = 15f;
    }
    
    public void SetWalkMode(){
        canJump = true;
        walkSpeed = 3.5f;
        bobSpeed = 10f;
    }

    public void DoPortalSound(Transform portal){
        if (ignorePortalSoundPortals.Contains(portal.gameObject))return;
        portalSound.Play();
    }

    private void Jump()
    {
        if (!isGrounded || !canJump) return;
        ApplyExplosionForce(Vector3.up * jumpForce, 0.55f);
        r.linearVelocity = new Vector3(r.linearVelocity.x, 0, r.linearVelocity.z);
    }

    void CalculateExternalForce(){
        if (externalForce.magnitude > minForceThreshold)
        {
            externalForce = new Vector3(externalForce.x * forceDamping,
                                        externalForce.y * (forceDamping),
                                        externalForce.z * forceDamping);
        }
        else
        {
            externalForce = Vector3.zero;
        }
    }

    public void ApplyExplosionForce(Vector3 force, float _forceDamping, bool OverrideEverything = false)
    {
        float weight = Mathf.Clamp01(force.magnitude / (externalForce.magnitude + force.magnitude));
        forceDamping = Mathf.Lerp(forceDamping, _forceDamping, weight);
        externalForce += force;

        if (OverrideEverything){ 
            externalForce = force;
            forceDamping = _forceDamping;
            r.linearVelocity = Vector3.zero;
        }
    }

    void UpdateAnim(Vector3 dir){
        if (dir.magnitude >= 0.05f && m_state != state.Running)
        {
            m_state = state.Running;
        }
        else if (dir.magnitude < 0.05f && m_state != state.Idle)
        {
            m_state = state.Idle;
        }
    }

    private void HeadBob()
    {
        if(m_state == state.Running)
        {
            timer += Time.deltaTime * bobSpeed;
            joint.localPosition = new Vector3(jointOriginalPos.x + Mathf.Sin(timer) * bobAmount.x, jointOriginalPos.y + Mathf.Sin(timer) * bobAmount.y, jointOriginalPos.z + Mathf.Sin(timer) * bobAmount.z);
            if (Mathf.Sin(timer) >= audioThresold){
                walkAudio.Play();
            }
        }
        else
        {
            timer = 0;
            joint.localPosition = new Vector3(Mathf.Lerp(joint.localPosition.x, jointOriginalPos.x, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.y, jointOriginalPos.y, Time.deltaTime * bobSpeed), Mathf.Lerp(joint.localPosition.z, jointOriginalPos.z, Time.deltaTime * bobSpeed));
        }
    }

    void Rotating(){
        yaw = transform.localEulerAngles.y + look.action.ReadValue<Vector2>().x * mouseSensitivity.x;
        pitch -= mouseSensitivity.y * look.action.ReadValue<Vector2>().y;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
        r.rotation = Quaternion.Euler(new Vector3(0, yaw, 0));
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
    }

    public void ResetCamRot(){
        yaw = 0;
        pitch = 0;
        Rotating();
    }

    public void FreezePlayer(bool forceFreeze){
        if (forceFreeze){
            vHCController.SetInputRection(false);
            canBeUnFreezed = false;
        }

        m_state = state.NoUse;
        MakeMeStatic();

        interaction.SetActive(false);
    }
    public void UnfreezePlayer(bool forceUnFreeze){
        if (!canBeUnFreezed && !forceUnFreeze) return;
        if (forceUnFreeze) vHCController.SetInputRection(true);
        canBeUnFreezed = true;
        
        m_state = state.Idle;
        MakeMeNONStatic();

        interaction.SetActive(true);
    }

    void OnGUI()
    {
        return;
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.normal.textColor = Color.red;
        guiStyle.fontSize = 20;
        float yOffset = 50;
        GUI.Label(new Rect(10, yOffset + 10, 300, 200), "state: " + m_state.ToString(), guiStyle);
        GUI.Label(new Rect(10, yOffset + 30, 300, 200), "moveAmount: " + moveAmount.ToString(), guiStyle);
        GUI.Label(new Rect(10, yOffset + 50, 300, 200), "isGrounded: " + isGrounded.ToString(), guiStyle);
        GUI.Label(new Rect(10, yOffset + 70, 300, 200), "velocity: " + r.linearVelocity.ToString(), guiStyle);
        GUI.Label(new Rect(10, yOffset + 90, 300, 200), "externalForce: " + externalForce.ToString(), guiStyle);
        GUI.Label(new Rect(10, yOffset + 110, 300, 200), "platformVelocity: " + platformVelocity.ToString(), guiStyle);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + originOffset, transform.position.z),
                                     bounds.extents.x);
    }
}
