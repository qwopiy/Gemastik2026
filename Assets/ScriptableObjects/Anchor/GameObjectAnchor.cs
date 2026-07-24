using UnityEngine;

[CreateAssetMenu(fileName = "NewGameObjectAnchor", menuName = "ScriptableObjects/GameObject Anchor")]
public class GameObjectAnchorSO : ScriptableObject
{
    [System.NonSerialized]
    public GameObject value;

    private void OnDisable()
    {
        value = null;
    }
}