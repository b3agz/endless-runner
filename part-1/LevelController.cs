using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public LevelPiece[] levelPieces;
    public Transform _camera;
    public int drawDistance;

    public float pieceLength;
    public float speed;

    Queue<GameObject> activePieces = new Queue<GameObject>();
    List<int> probabilityList = new List<int>();

    int currentCamStep = 0;
    int lastCamStep = 0;

    private void Start() {

        BuildProbabilityList();

        // Spawn starting level pieces.
        for (int i = 0; i < drawDistance; i++) {

            SpawnNewLevelPiece();

        }

        currentCamStep = (int)(_camera.transform.position.z / pieceLength);
        lastCamStep = currentCamStep;

    }

    private void Update() {

        // Move camera forward.
        _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, _camera.transform.position + Vector3.forward, Time.deltaTime * speed);

        // Calculate current camera position in level pieces and check if it has changed since the last frame.
        currentCamStep = (int)(_camera.transform.position.z / pieceLength);
        if (currentCamStep != lastCamStep) {

            // If it has changed, updated the last camera position and spawn/despawn level pieces.
            lastCamStep = currentCamStep;
            DespawnLevelPiece();
            SpawnNewLevelPiece();
            

        }

    }

    void SpawnNewLevelPiece () {

        // Get a LevelPiece index from the probabilityList and use it to spawn a new LevelPiece and add it to the activePieces queue.
        int pieceIndex = probabilityList[Random.Range(0, probabilityList.Count)];
        GameObject newLevelPiece = Instantiate(levelPieces[pieceIndex].prefab, new Vector3(0f, 0f, (currentCamStep + activePieces.Count) * pieceLength), Quaternion.identity);
        activePieces.Enqueue(newLevelPiece);

    }

    void DespawnLevelPiece () {

        // Remove next (oldest) LevelPiece from activePieces queue and destroy associated gameObject.
        GameObject oldLevelPiece = activePieces.Dequeue();
        Destroy(oldLevelPiece);

    }

    void BuildProbabilityList () {

        // Create a list of integegers representing levelPiece indexes. The higher the probability, the more that index will appear in the list.
        int index = 0;
        foreach (LevelPiece piece in levelPieces) {

            for (int i = 0; i < piece.probability; i++) {

                probabilityList.Add(index);

            }

            index++;

        }

    }

}

[System.Serializable]
public class LevelPiece {

    public string name;
    public GameObject prefab;
    public int probability = 1;

}
