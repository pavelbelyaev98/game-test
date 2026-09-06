using UnityEngine;

namespace JustAFewPeppers
{
    public sealed class YardPlayer : MonoBehaviour
    {
        public CharacterController body;
        public Camera view;
        [Min(.1f)] public float walkSpeed = 3.2f;
        [Min(.1f)] public float sprintSpeed = 5.4f;
        [Min(.1f)] public float jumpHeight = .8f;
        [Min(.1f)] public float gravity = 20;
        [Min(0)] public float coyoteTime = .1f;
        [Min(0)] public float jumpBufferTime = .12f;
        [Min(.01f)] public float lookSensitivity = .1f;
        float pitch;
        float verticalSpeed;
        float groundGrace = -1;
        float jumpBuffer = -1;

        public void Step(Vector2 move, Vector2 look, bool sprint, bool jumpPressed, float deltaTime)
        {
            if (deltaTime <= 0) return;
            // Mouse delta is already displacement; multiplying it by frame time changes sensitivity with FPS.
            transform.Rotate(0, look.x * lookSensitivity, 0);
            pitch = Mathf.Clamp(pitch - look.y * lookSensitivity, -80, 80);
            view.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
            move = Vector2.ClampMagnitude(move, 1);
            var velocity = (transform.right * move.x + transform.forward * move.y) * (sprint ? sprintSpeed : walkSpeed);
            if (jumpPressed) jumpBuffer = jumpBufferTime + Mathf.Epsilon;
            float stepOffset = body.stepOffset;
            // Short collision steps keep landing/jump timing useful even during a slow rendered frame.
            while (deltaTime > 0)
            {
                float dt = Mathf.Min(deltaTime, 1f / 60);
                bool grounded = body.isGrounded && verticalSpeed <= 0;
                if (grounded)
                {
                    groundGrace = coyoteTime + Mathf.Epsilon;
                    verticalSpeed = -2;
                }
                if (jumpBuffer > 0 && groundGrace > 0)
                {
                    verticalSpeed = Mathf.Sqrt(2 * gravity * jumpHeight);
                    ClearJumpRequest();
                }
                body.stepOffset = grounded && verticalSpeed <= 0 ? stepOffset : 0;
                var displacement = velocity * dt;
                displacement.y = verticalSpeed * dt - .5f * gravity * dt * dt;
                verticalSpeed = Mathf.Max(verticalSpeed - gravity * dt, -30);
                var collision = body.Move(displacement);
                if ((collision & CollisionFlags.Above) != 0 && verticalSpeed > 0) verticalSpeed = 0;
                if ((collision & CollisionFlags.Below) != 0 && verticalSpeed < 0) verticalSpeed = -2;
                groundGrace -= dt;
                jumpBuffer -= dt;
                deltaTime -= dt;
            }
            body.stepOffset = stepOffset;
        }

        public void ClearJumpRequest()
        {
            groundGrace = -1;
            jumpBuffer = -1;
        }

        public void ResetTo(Transform spawn)
        {
            // Keep small per-frame motion at high frame rates; the controller's default cutoff can swallow it.
            body.minMoveDistance = 0;
            body.enabled = false;
            transform.SetPositionAndRotation(spawn.position, spawn.rotation);
            pitch = 0;
            verticalSpeed = 0;
            ClearJumpRequest();
            view.transform.localRotation = Quaternion.identity;
            body.enabled = true;
        }
    }
}
