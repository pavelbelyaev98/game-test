using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace JustAFewPeppers.Tests
{
    static class MachineTestActions
    {
        // Public model preparation for suites testing another boundary; new mechanism tests use actual input.
        public static void StartPreparedBatch(HarvestState state)
        {
            bool rawHeld = state.IsHeld, finishedHeld = state.FinishedHeld;
            if (rawHeld) state.Release(state.RawPose, true);
            if (finishedHeld) state.ReleaseFinished(state.FinishedPose, true);
            Assert.That(state.BeginOperation(), Is.True);
            Assert.That(state.MoveOperation(.94f), Is.Positive);
            if (rawHeld) state.PickUp();
            if (finishedHeld) state.PickUpFinished();
        }

        public static void Aim(YardSession session, Vector3 position, Vector3 target)
        {
            var pose = new GameObject("Mechanism approach").transform; pose.position = position;
            var direction = target - (position + Vector3.up * 1.65f);
            pose.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, 0);
            session.player.ResetTo(pose); Object.Destroy(pose.gameObject);
            float pitch = Mathf.Atan2(-direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;
            session.player.Step(Vector2.zero, new Vector2(0, -pitch / session.player.lookSensitivity), false, false, 1f / 60);
            Physics.SyncTransforms(); session.targeting.Refresh();
        }

        public static void AimMechanism(YardSession session, bool grouping)
        {
            var target = grouping ? session.handling.finished.outputTarget : session.handling.machine.operationTarget;
            var point = target.transform.position + (grouping ? Vector3.up * .2f : Vector3.zero);
            Aim(session, new Vector3(point.x, .04f, point.z - 2.3f), point);
        }

        public static IEnumerator KeyboardStroke(YardSession session, Keyboard keyboard, bool grouping, bool aim = true)
        {
            if (aim) AimMechanism(session, grouping);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E)); yield return null; yield return null;
            Assert.That(session.handling.machine.Engaged, Is.True, "The intended slider captures E.");
            int previous = grouping ? session.handling.State.GroupsCompleted : session.handling.State.OperationsCompleted;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.E, grouping ? Key.D : Key.W));
            float deadline = Time.realtimeSinceStartup + 2;
            while ((grouping ? session.handling.State.GroupsCompleted : session.handling.State.OperationsCompleted) == previous && Time.realtimeSinceStartup < deadline) yield return null;
            Assert.That(grouping ? session.handling.State.GroupsCompleted : session.handling.State.OperationsCompleted, Is.EqualTo(previous + 1));
            InputSystem.QueueStateEvent(keyboard, new KeyboardState()); yield return null; yield return null;
        }
    }
}
