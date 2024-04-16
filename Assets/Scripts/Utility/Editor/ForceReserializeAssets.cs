using UnityEditor;

namespace Utility.Editor
{
    public class ForceReserializeAssets
    {
        [MenuItem("Tools/Force Reserialize Assets")]
        private static void ForceReserialzed()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}