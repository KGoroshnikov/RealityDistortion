using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuCam : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float maxAngle = 30f;
    [SerializeField] private float smoothTime = 0.3F;
    [SerializeField] private float circleSpeed;
    [SerializeField] private float circleRadius;

    private Vector3 delautPos;
    private Quaternion velocity;
    private Vector2 screenCenter;
    private Quaternion defaultRotation;

    void Start()
    {
        delautPos = cam.position;
        defaultRotation = cam.rotation;
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    void Update()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 offset = mousePos - screenCenter;
        float normalizedX = Mathf.Clamp(offset.x / screenCenter.x, -1f, 1f);
        float normalizedY = Mathf.Clamp(offset.y / screenCenter.y, -1f, 1f);
        float rotationX = -normalizedY * maxAngle;
        float rotationY = normalizedX * maxAngle;
        cam.rotation = SmoothDamp(cam.rotation, 
            defaultRotation * Quaternion.Euler(rotationX, rotationY, 0), ref velocity, smoothTime);

        cam.position = delautPos + new Vector3(Mathf.Sin(Time.time * circleSpeed) * circleRadius, Mathf.Cos(Time.time * circleSpeed) * circleRadius, 0);
    }

    public static Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time) {
		if (Time.deltaTime < Mathf.Epsilon) return rot;
		var Dot = Quaternion.Dot(rot, target);
		var Multi = Dot > 0f ? 1f : -1f;
		target.x *= Multi;
		target.y *= Multi;
		target.z *= Multi;
		target.w *= Multi;
		var Result = new Vector4(
			Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
			Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
			Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
			Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
		).normalized;
		var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
		deriv.x -= derivError.x;
		deriv.y -= derivError.y;
		deriv.z -= derivError.z;
		deriv.w -= derivError.w;		
		return new Quaternion(Result.x, Result.y, Result.z, Result.w);
	}
}
