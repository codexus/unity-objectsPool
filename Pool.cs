using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pool {

    #region variables

    private int m_initialSize = 2; // initial size of the pool
    private GameObject m_poolObject; // Prefab that will be instatitated
    private Transform m_objectsHolder; // Prefab that will be instatitated

    private Stack<GameObject> m_pool = new Stack<GameObject>(); // pool stack
    private int m_activeObjects; // number of objects currently on scene

    #endregion

    #region getters/setters

    /// <summary>
    /// Number of active spawned GameObjects
    /// </summary>
    public int ActiveCount
    {
        get { return m_activeObjects; }
    }

    /// <summary>
    /// Number of inactive GameObjects in pool stack
    /// </summary>
    public int InactiveCount
    {
        get { return m_pool.Count; }
    }

    #endregion

    #region methods

    public Pool(GameObject gameObject, Transform objectsHolder = null)
    {
        m_pool = new Stack<GameObject>();
        m_objectsHolder = objectsHolder;
        m_poolObject = gameObject;
    }
    
    public GameObject Spawn()
    {
        return Spawn(Vector3.zero);
    }
    
    public GameObject Spawn(Vector3 position)
    {
        return Spawn(position, Quaternion.identity);
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject go; // object to return

        if (m_pool.Count == 0)
        {
            if(m_objectsHolder == null)
            {
                go = GameObject.Instantiate(m_poolObject, position, rotation) as GameObject;
            }
            else
            {
                go = GameObject.Instantiate(m_poolObject, position, rotation, m_objectsHolder) as GameObject;
            }
        }
        else
        {
            go = m_pool.Pop();
        }

        go.transform.position = position;
        go.transform.rotation = rotation;
        go.SetActive(true);

        m_activeObjects++;
        return go;
    }

    public void Remove(GameObject go)
    {
        go.SetActive(false);
        m_pool.Push(go);
        m_activeObjects--;
    }
    
    #endregion
}
