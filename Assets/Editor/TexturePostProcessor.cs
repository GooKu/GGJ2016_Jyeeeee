using UnityEngine;
using UnityEditor;

public class TexturePostProcessor : AssetPostprocessor
{
	void OnPostprocessTexture(Texture2D texture)
	{
		TextureImporter importer = assetImporter as TextureImporter;
		importer.spritePixelsPerUnit = 128;
		//importer.anisoLevel = 0;
		//importer.filterMode = FilterMode.Trilinear;

		//Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
		//if (asset)
		//{
		//	EditorUtility.SetDirty(asset);
		//}
		//else
		//{
		//	texture.anisoLevel = 0;
		//	texture.filterMode = FilterMode.Trilinear;
		//}
	}
}