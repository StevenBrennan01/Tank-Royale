using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq; USED FOR THE SIMPLER EXPRESSIONS

public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    public class PooledObjectInfo
    {
        public string LookUpString;

        public List<GameObject> InactiveObjects = new List<GameObject>();
    }

    public static GameObject spawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        //PooledObjectInfo currentPool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);
        PooledObjectInfo currentPool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookUpString == objectToSpawn.name)
            {
                currentPool = p;
                break;
            }
        }

        if (currentPool == null)
        {
            currentPool = new PooledObjectInfo() { LookUpString = objectToSpawn.name };
            ObjectPools.Add(currentPool);
        }

        //GameObject spawnableObj = currentPool.InactiveObjects.FirstOrDefault();
        GameObject spawnableObj = null;
        foreach (GameObject obj in currentPool.InactiveObjects)
        {
            if (obj != null)
            {
                spawnableObj = obj;
                break;
            }
        }

        if (spawnableObj == null)
        {
            // IF THERE ARE NOT INACTIVE OBJECTS, CREATE A NEW ONE
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        }

        else
        {
            //IF THERE IS AN INACTIVE OBJECT, REACTIVATE IT
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            currentPool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        // LAMBDA EXPRESSION
        //PooledObjectInfo currentPool = ObjectPools.Find(p => p.LookupString == obj.name);

        string goName = obj.name.Substring(0, obj.name.Length - 7); // REMOVES THE (CLONE) PORTION OF THE OBJECT BY USING -7 ON THE NAME

        PooledObjectInfo currentPool = null;
        foreach (PooledObjectInfo p in ObjectPools)
        {
            if (p.LookUpString == goName)
            {
                currentPool = p;
                break;
            }
        }
        if (currentPool == null)
        {
            Debug.Log("Trying to release object that is not pooled " + obj.name);
        }
        else
        {
            obj.SetActive(false);
            currentPool.InactiveObjects.Add(obj);
        }
    }
}