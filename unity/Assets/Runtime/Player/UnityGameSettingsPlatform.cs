using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SomethingDownThere
{
    public sealed class UnityGameSettingsPlatform : IGameSettingsPlatform
    {
        private readonly bool applyToSystem;
        private readonly RenderPipelineAsset originalPipeline;
        private readonly UniversalRenderPipelineAsset pipeline;
        private readonly int originalVSync, originalFrameLimit, originalTextures;
        private readonly AnisotropicFiltering originalFiltering;
        private readonly float originalVolume;
        private DisplaySelection editorDisplay;
        public DisplaySelection CurrentDisplay => Application.isEditor ? editorDisplay
            : new DisplaySelection(Screen.width, Screen.height, Mode(Screen.fullScreenMode));
        public Vector2Int[] Resolutions { get; }
        public DisplaySelection NativeDisplay { get; }
        public bool RenderingAvailable => !applyToSystem || pipeline != null;

        public UnityGameSettingsPlatform(bool applyToSystem = true)
        {
            this.applyToSystem = applyToSystem;
            NativeDisplay = DesktopWindow.RecommendedDisplay(Screen.currentResolution.width, Screen.currentResolution.height);
            editorDisplay = new DisplaySelection(Mathf.Max(960, Screen.width), Mathf.Max(540, Screen.height), 0);
            Resolutions = Screen.resolutions.Select(r => new Vector2Int(r.width, r.height))
                .Concat(new[] { new Vector2Int(960, 540), new Vector2Int(1280, 720), new Vector2Int(1600, 900), new Vector2Int(Screen.width, Screen.height) })
                .Where(r => r.x >= 960 && r.y >= 540 && r.x <= Screen.currentResolution.width && r.y <= Screen.currentResolution.height)
                .Distinct().OrderBy(r => r.x).ThenBy(r => r.y).ToArray();
            if (Resolutions.Length == 0) Resolutions = new[] { new Vector2Int(1280, 720) };
            if (!applyToSystem) return;
            originalVSync = QualitySettings.vSyncCount; originalFrameLimit = Application.targetFrameRate;
            originalTextures = QualitySettings.globalTextureMipmapLimit; originalFiltering = QualitySettings.anisotropicFiltering;
            originalVolume = AudioListener.volume;
            originalPipeline = QualitySettings.renderPipeline;
            if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset source)
            {
                pipeline = Object.Instantiate(source);
                pipeline.name = source.name + " (player settings)";
                pipeline.hideFlags = HideFlags.DontSave;
                QualitySettings.renderPipeline = pipeline;
            }
        }

        public void Apply(GamePreferenceValues values, bool focused)
        {
            if (!applyToSystem) return;
            QualitySettings.vSyncCount = values.VSync ? 1 : 0;
            Application.targetFrameRate = values.VSync ? -1 : values.FrameLimit;
            if (QualitySettings.globalTextureMipmapLimit != values.TextureLimit) QualitySettings.globalTextureMipmapLimit = values.TextureLimit;
            QualitySettings.anisotropicFiltering = (AnisotropicFiltering)values.Filtering;
            AudioListener.volume = values.MasterVolume / 100f;
            if (pipeline != null)
            {
                if (!Mathf.Approximately(pipeline.renderScale, values.RenderScale / 100f)) pipeline.renderScale = values.RenderScale / 100f;
                if (pipeline.msaaSampleCount != values.Msaa) pipeline.msaaSampleCount = values.Msaa;
            }
        }

        public void SetDisplay(DisplaySelection selection)
        {
            editorDisplay = selection;
            if (applyToSystem && !Application.isEditor)
                Screen.SetResolution(selection.Width, selection.Height, selection.Mode == 0 ? FullScreenMode.FullScreenWindow
                    : selection.Mode == 1 ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed);
        }

        private static int Mode(FullScreenMode mode) => mode == FullScreenMode.ExclusiveFullScreen ? 1 : mode == FullScreenMode.Windowed ? 2 : 0;

        public void Dispose()
        {
            if (!applyToSystem) return;
            QualitySettings.vSyncCount = originalVSync; Application.targetFrameRate = originalFrameLimit;
            QualitySettings.globalTextureMipmapLimit = originalTextures; QualitySettings.anisotropicFiltering = originalFiltering;
            AudioListener.volume = originalVolume;
            if (pipeline != null)
            {
                if (QualitySettings.renderPipeline == pipeline) QualitySettings.renderPipeline = originalPipeline;
                Object.Destroy(pipeline);
            }
        }
    }
}
