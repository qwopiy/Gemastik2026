using UnityEngine;
using UnityEditor;
using System.IO;

public class FoodDisplaySOGenerator : EditorWindow
{
    // Variabel untuk menyimpan inputan dari Inspector/Window
    private string rawInputString = "";
    private Sprite baseSprite;
    private Sprite maskSprite;
    private Sprite brandSprite;
    private string savePath = "Assets/ScriptableObjects/DisplayItems"; // Folder default penyimpanan

    // Mendaftarkan tool ini ke dalam menu bar Unity
    [MenuItem("Tools/Food Display SO Generator")]
    public static void ShowWindow()
    {
        GetWindow<FoodDisplaySOGenerator>("Food SO Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Input Data (Pisahkan dengan Semicolon ';')", EditorStyles.boldLabel);

        // Area teks untuk memasukkan string
        rawInputString = EditorGUILayout.TextArea(rawInputString, GUILayout.Height(150));

        EditorGUILayout.Space();
        GUILayout.Label("Assign Sprites", EditorStyles.boldLabel);

        // Input untuk Sprite
        baseSprite = (Sprite)EditorGUILayout.ObjectField("Base Sprite", baseSprite, typeof(Sprite), false);
        maskSprite = (Sprite)EditorGUILayout.ObjectField("Mask Sprite", maskSprite, typeof(Sprite), false);
        brandSprite = (Sprite)EditorGUILayout.ObjectField("Brand Sprite", brandSprite, typeof(Sprite), false);

        EditorGUILayout.Space();
        GUILayout.Label("Save Settings", EditorStyles.boldLabel);

        // Path folder untuk menyimpan file SO yang dibuat
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        EditorGUILayout.Space();

        // Tombol untuk mengeksekusi pembuatan SO
        if (GUILayout.Button("Generate Scriptable Objects", GUILayout.Height(30)))
        {
            GenerateScriptableObjects();
        }
    }

    private void GenerateScriptableObjects()
    {
        if (string.IsNullOrWhiteSpace(rawInputString))
        {
            Debug.LogWarning("Input string masih kosong!");
            return;
        }

        // Memastikan folder penyimpanan ada, jika belum maka dibuat otomatis
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            AssetDatabase.Refresh();
        }

        // Memisahkan data berdasarkan semicolon (;)
        string[] items = rawInputString.Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        int generatedCount = 0;

        foreach (string item in items)
        {
            string cleanItem = item.Trim();
            if (string.IsNullOrEmpty(cleanItem)) continue;

            // Memisahkan nama dan warna berdasarkan karakter '#'
            string[] parts = cleanItem.Split('#');

            // Format yang benar harus menghasilkan minimal 4 bagian: [Nama, Hex1, Hex2, Hex3]
            if (parts.Length < 4)
            {
                Debug.LogWarning($"Format salah, diabaikan: {cleanItem}");
                continue;
            }

            string itemName = parts[0].Trim();

            // Menambahkan kembali '#' di depan hex karena terpotong oleh fungsi Split
            string hex1 = "#" + parts[1].Trim();
            string hex2 = "#" + parts[2].Trim();
            string hex3 = "#" + parts[3].Trim();

            // Membuat instance ScriptableObject baru
            FoodDisplaySO newSO = ScriptableObject.CreateInstance<FoodDisplaySO>();

            // Mengisi data berdasarkan instruksi
            newSO.itemName = itemName;
            newSO.ExpiredDate = "";
            newSO.BrandClaim = "";

            newSO.baseSprite = baseSprite;
            newSO.maskSprite = maskSprite;
            newSO.brandSprite = brandSprite;
            newSO.conditionSprite = null; // Dikosongkan sesuai instruksi

            // Konversi dari string hex ke Unity Color
            if (ColorUtility.TryParseHtmlString(hex1, out Color pColor)) newSO.primaryColor = pColor;
            if (ColorUtility.TryParseHtmlString(hex2, out Color sColor)) newSO.secondaryColor = sColor;
            if (ColorUtility.TryParseHtmlString(hex3, out Color tColor)) newSO.tertiaryColor = tColor;

            // Menyimpan file asset ke dalam project
            string assetName = itemName.Replace(" ", ""); // Hapus spasi untuk nama file (opsional)
            string fullPath = $"{savePath}/{assetName}.asset";

            AssetDatabase.CreateAsset(newSO, fullPath);
            generatedCount++;
        }

        // Menyimpan semua perubahan dan menyegarkan database Asset Unity
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"<color=green>Berhasil membuat {generatedCount} ScriptableObjects!</color>");
    }
}