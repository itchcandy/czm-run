using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Looper : MonoBehaviour
{
    public float spawnZ = 10, resetZ = -5, speed = 10, spawnOffsetZ = 2;
    public List<float> spawnX;
    List<GameObject> obstacles, activeObstacles, inactiveObstacles;
    string obstaclesPath = "Obstacles/";
    float spawnDelay;

    void Awake()
    {
        obstacles = new List<GameObject>();
        activeObstacles = new List<GameObject>();
        inactiveObstacles = new List<GameObject>();
        GameObject[] g = Resources.LoadAll<GameObject>("Obstacles/");
        foreach(var h in g)
        {
            GameObject t = Instantiate(h);
            t.SetActive(false);
            obstacles.Add(t);
            inactiveObstacles.Add(t);
        }
        spawnDelay = spawnOffsetZ / speed;
    }

	void Start ()
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(var t in g)
            inactiveObstacles.Remove(t);
        activeObstacles.AddRange(g);
        Debug.Log("SpawnDelay: " + spawnDelay);
        InvokeRepeating("Spawn", 0, spawnDelay);
	}
	
	void Update ()
    {
        Move();
	}

    void Move()
    {
        List<GameObject> gl = new List<GameObject>();
        foreach (var t in activeObstacles)
        {
            t.transform.position += Vector3.back * Time.deltaTime * speed;
            if (t.transform.position.z < resetZ)
                gl.Add(t);
        }
        foreach(var t in gl)
        {
            t.SetActive(false);
            activeObstacles.Remove(t);
            inactiveObstacles.Add(t);
        }
    }

    public void Reset(GameObject g)
    {
        g.SetActive(false);
        activeObstacles.Remove(g);
        inactiveObstacles.Add(g);
    }

    void Spawn()
    {
        int i = Random.Range(0, inactiveObstacles.Count);
        int j = Random.Range(0, spawnX.Count);
        GameObject g = inactiveObstacles[i];
        g.transform.position = new Vector3(spawnX[j], 0, spawnZ);
        g.SetActive(true);
        inactiveObstacles.RemoveAt(i);
        activeObstacles.Add(g);
    }
}
