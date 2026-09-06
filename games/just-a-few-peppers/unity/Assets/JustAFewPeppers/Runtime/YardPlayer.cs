using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class YardPlayer : MonoBehaviour
    {
        public CharacterController body;
        public Camera view;
        [Min(.1f)] public float walkSpeed = 3.2f;
        [Min(.01f)] public float lookSensitivity = .1f;
        float pitch;
        float verticalSpeed;

        public void Step(Vector2 move, Vector2 look, float deltaTime)
        {
            // Mouse delta is already displacement; multiplying it by frame time changes sensitivity with FPS.
            transform.Rotate(0, look.x * lookSensitivity, 0);
            pitch = Mathf.Clamp(pitch - look.y * lookSensitivity, -80, 80);
            view.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
            move = Vector2.ClampMagnitude(move, 1);
            if (body.isGrounded && verticalSpeed < 0) verticalSpeed = -2;
            verticalSpeed = Mathf.Max(verticalSpeed - 20 * deltaTime, -30);
            var velocity = (transform.right * move.x + transform.forward * move.y) * walkSpeed;
            velocity.y = verticalSpeed;
            body.Move(velocity * deltaTime);
        }

        public void ResetTo(Transform spawn)
        {
            // Keep small per-frame motion at high frame rates; the controller's default cutoff can swallow it.
            body.minMoveDistance = 0;
            body.enabled = false;
            transform.SetPositionAndRotation(spawn.position, spawn.rotation);
            pitch = 0;
            verticalSpeed = 0;
            view.transform.localRotation = Quaternion.identity;
            body.enabled = true;
        }
    }
}
