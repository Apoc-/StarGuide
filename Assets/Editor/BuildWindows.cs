using UnityEditor;

namespace Editor
{
    public class BuildScripts
    {
        [MenuItem("Build/Build Windows")]
        public static void BuildWindows()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] {"Assets/Scenes/GameScene.unity"};
            buildPlayerOptions.locationPathName = "./dist/StarGuide.exe";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows;
            buildPlayerOptions.options = BuildOptions.None;

            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
    }
}