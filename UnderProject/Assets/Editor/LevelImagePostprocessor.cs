using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelImagePostprocessor : AssetPostprocessor
{
    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter importer = assetImporter as TextureImporter;

        importer.isReadable = true;
        importer.filterMode = FilterMode.Point;
        importer.compressionQuality = 0;
        importer.SaveAndReimport();
    }
}
