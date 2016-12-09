using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Fast implementation of Pool object using array as a stack
/// </summary>
public class Pool
{
    #region variables

    private int size = 2; // initial size of the pool
    private GameObject m_poolObject; // Prefab that will be instatitated
    private Transform m_objectsHolder; // Prefab that will be instatitated

    //private Stack<GameObject> m_pool = new Stack<GameObject>(); // pool stack
    private GameObject[] m_poolStack;

    private int m_activeCount; // number of objects currently on scene
    private int m_inactiveCount; // number of objects in the pool ready for spawning

    #endregion

    #region getters/setters

    /// <summary>
    /// Number of active spawned GameObjects
    /// </summary>
    public int ActiveCount
    {
        get { return m_activeCount; }
    }

    /// <summary>
    /// Number of inactive GameObjects in pool stack
    /// </summary>
    public int InactiveCount
    {
        get { return m_inactiveCount; }
    }

    #endregion

    #region methods

    /// <summary>
    /// Creates a new Pool for holding game objects.
    /// </summary>
    /// <param name="gameObject">GameObject prefab to spawn</param>
    /// <param name="parentTransform">Transform parent for spawned objects. If the value is left as null, 
    /// the spawned objects will be attached to scene root.</param>
    public Pool(GameObject gameObject, Transform parentTransform = null)
    {
        m_poolStack = new GameObject[size];
        m_objectsHolder = parentTransform;
        m_poolObject = gameObject;
    }

    /// <summary>
    /// Spawns the object. Note that the Awake() and Start() will be called only once. OnDisable() and OnEnable() will be called insted,
    /// since the method calls SetActive(true) on spawned Object;
    /// </summary>
    /// <returns>Instance from prefab</returns>
    public GameObject Spawn()
    {
        return Spawn(Vector3.zero);
    }

    /// <summary>
    /// Spawns the object with desired position. Note that the Awake() and Start() will be called only once. OnDisable() and OnEnable() will be called insted,
    /// since the method calls SetActive(true) on spawned Object;
    /// </summary>
    /// <param name="position">Position where the object shoul be spawned</param>
    /// <returns>Instance from prefab</returns>

    public GameObject Spawn(Vector3 position)
    {
        return Spawn(position, Quaternion.identity);
    }

    /// <summary>
    /// Spawns the object with desired position and rotation. Note that the Awake() and Start() will be called only once. OnDisable() and OnEnable() will be called insted,
    /// since the method calls SetActive(true) on spawned Object;
    /// </summary>
    /// <param name="position">Position where the object shoul be spawned</param>
    /// <param name="rotation">Rotation of the spawned object</param>
    /// <returns></returns>
    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject go; // object to return

        if (m_inactiveCount == 0)
        {
            if (m_objectsHolder == null)
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
            go = m_poolStack[--m_inactiveCount];
        }

        // set position and rotation
        go.transform.position = position; 
        go.transform.rotation = rotation;
        go.SetActive(true);

        // increment active count
        m_activeCount++;
        return go;
    }

    /// <summary>
    /// Returns the GameObject back to pool 
    /// </summary>
    /// <param name="go"></param>
    public void Remove(GameObject go)
    {
        if (m_inactiveCount == m_poolStack.Length)
        {
            // Double the pools size if it's full.
            EnlargeStack(size * 2);
        }

        go.SetActive(false);

        m_activeCount--; // decrement active count
        m_poolStack[m_inactiveCount++] = go;
    }

    /// <summary>
    /// Enlarges the pool stack by provided size
    /// </summary>
    /// <param name="newSize"></param>
    private void EnlargeStack(int newSize)
    {
        if (newSize <= size) return; // if new size is same or less size

        GameObject[] tmp = new GameObject[newSize];
        for (int i = 0; i < size; i++)
        {
            tmp[i] = m_poolStack[i];
        }
        m_poolStack = tmp;
        size = newSize;
    }

    #endregion
}
