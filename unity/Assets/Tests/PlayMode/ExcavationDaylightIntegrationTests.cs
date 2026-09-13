using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace SomethingDownThere.Tests
{
    public sealed class ExcavationDaylightIntegrationTests
    {
        [UnityTest]
        public IEnumerator CacheFollowsExcavationRestoreResetAndReceiverLifecycle()
        {
            var root = new GameObject("Daylight validation fixture");
            root.SetActive(false);
            var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            var terrain = root.AddComponent<TerrainVolume>();
            terrain.Configure(new Vector3Int(16, 24, 16), .125f, 8, .4f, material);
            var daylight = root.AddComponent<ExcavationDaylight>();
            var receiver = GameObject.CreatePrimitive(PrimitiveType.Cube);
            receiver.transform.SetParent(root.transform);
            receiver.transform.localPosition = new Vector3(1, 1, 1);
            receiver.transform.localScale = Vector3.one * .1f;
            var renderer = receiver.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            try
            {
                root.SetActive(true);
                // Player builds validate Unity's reserved message signatures.
                // Our custom Bounds event must not shadow the integer message.
                root.SendMessage("OnTerrainChanged", 1, SendMessageOptions.DontRequireReceiver);
                yield return Ready(daylight);
                Assert.That(daylight.PublishedRevision, Is.GreaterThan(0));
                Assert.That(daylight.SampleAmbient(new Vector3(1, 1, 1)), Is.EqualTo(.45f).Within(.001f));
                Assert.That(daylight.SampleAmbient(new Vector3(1, 3.1f, 1)), Is.EqualTo(1));
                Assert.That(renderer.sharedMaterial.shader.name, Is.EqualTo("Something Down There/Excavation Lit"));
                var grid = new ExcavationGrid(terrain.Dimensions, terrain.CellSize);
                for (float y = 3; y >= .75f; y -= .4f) grid.RemoveSphere(new Vector3(1, y, 1), .65f, out _);
                yield return terrain.Restore(grid.Capture(), terrain.ExcavationSeed);
                yield return Ready(daylight);
                Assert.That(daylight.SampleAmbient(new Vector3(1, 1, 1)), Is.GreaterThan(.55f));
                terrain.ResetExcavation();
                yield return Ready(daylight);
                Assert.That(daylight.SampleAmbient(new Vector3(1, 1, 1)), Is.EqualTo(.45f).Within(.001f));
                daylight.enabled = false;
                Assert.That(Shader.GetGlobalFloat("_ExcavationDaylightEnabled"), Is.Zero);
                Assert.That(renderer.sharedMaterial, Is.SameAs(material));
                daylight.enabled = true;
                yield return Ready(daylight);
                Assert.That(renderer.sharedMaterial.shader.name, Is.EqualTo("Something Down There/Excavation Lit"));
            }
            finally
            {
                Object.Destroy(root);
                Object.Destroy(material);
            }
        }

        private static IEnumerator Ready(ExcavationDaylight daylight)
        {
            // Always allow one LateUpdate after a terrain restore/reset event.
            yield return null;
            for (int i = 0; i < 180 && daylight.IsUpdating; i++) yield return null;
            Assert.That(daylight.IsUpdating, Is.False, "Daylight must publish rather than remain in a failed rebuild.");
        }
    }
}
