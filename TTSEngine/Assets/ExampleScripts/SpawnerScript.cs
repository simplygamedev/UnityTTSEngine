using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript {
    public GameObject prefabtospawn;

    protected int initialSpawnAmount; 

    const string empty = "Empty";

    private List<GameObject> prefabslist;

    static readonly Vector3 vectorzero = Vector3.zero;
    static readonly Quaternion rotationzero = Quaternion.identity;

    public int ReturnLength() {
        return prefabslist.Count;
    }

    public SpawnerScript(GameObject obj, int spawnamount) {
        this.prefabtospawn = obj;
        this.initialSpawnAmount = spawnamount;
        initializeObjectPool();
    }

    private void initializeObjectPool() {
        this.prefabslist = new List<GameObject>();

        for (int i = 0; i < this.initialSpawnAmount; i++) {
            createObjectInstance(vectorzero,rotationzero);
        }
    }

    private GameObject createObjectInstance(Vector3 vector, Quaternion quat) {
        var temp = GameObject.Instantiate(prefabtospawn, vector, quat);
        GameObject.DontDestroyOnLoad(temp); //If we state this here it means the created spawners must also have DontDestroyOnLoad called
        temp.SetActive(false);
        this.prefabslist.Add(temp);
        return temp;
    }

    public void recallSpawnedInstances() //Sets all the spawned instances back to inactive state 
    {
        for(int i=0;i<prefabslist.Count;i++)
        {
            prefabslist[i].SetActive(false);
        }
    }

    public virtual GameObject spawnPrefabIntoWorld(Vector3 pos, Quaternion rot) //This allows us to specify the world position to spawn the object at
    {
        for (int i = 0; i < prefabslist.Count; i++)
        {
            if (!prefabslist[i].activeInHierarchy)
            {
                prefabslist[i].transform.position = pos;
                prefabslist[i].transform.rotation = rot;
                prefabslist[i].SetActive(true);
                return prefabslist[i];
            }
        }

        var temp = GameObject.Instantiate(prefabtospawn, pos, rot);
        temp.SetActive(true);
        return temp;
    }

    public virtual GameObject spawnPrefabIntoWorld() {
        for (int i = 0; i < prefabslist.Count; i++) {
            if (!prefabslist[i].activeInHierarchy) {
                prefabslist[i].transform.position = vectorzero;
                prefabslist[i].transform.rotation = rotationzero;
                prefabslist[i].SetActive(true);
                return prefabslist[i];
            }
        }

        var temp = createObjectInstance(vectorzero, rotationzero);
        temp.SetActive(true);
        return temp;
    }
}

