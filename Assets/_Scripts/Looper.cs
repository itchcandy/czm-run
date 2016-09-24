using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Looper : MonoBehaviour
{
    public float spawnZ = 10, resetZ = -5, speed = 10, spawnOffsetZ = 2, llx = -6, lrx = -3, rlx = 3, rrx = 6;
    public List<float> spawnX;
    public Material road;
    public GameObject ground;
    List<GameObject> obstacles, activeObstacles, inactiveObstacles, activeBuildings;
    GameObject[] buildings;
    string obstaclesPath = "Obstacles/";
    float spawnDelay;
    Vector2 offset;

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
        buildings = Resources.LoadAll<GameObject>("Buildings/");
        activeBuildings = new List<GameObject>();
        spawnDelay = spawnOffsetZ / speed;
        offset = Vector2.zero;
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
        offset.y = (offset.y - 2 * Time.deltaTime) % 512;
        road.SetTextureOffset("_MainTex", offset);
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
        foreach (var t in gl)
        {
            t.SetActive(false);
            activeObstacles.Remove(t);
            inactiveObstacles.Add(t);
        }
        gl.Clear();
        foreach (var t in activeBuildings)
        {
            t.transform.position += Vector3.back * Time.deltaTime * speed;
            if (t.transform.position.z < resetZ)
                gl.Add(t);
        }
        foreach (var t in gl)
        {
            activeBuildings.Remove(t);
            GameObject.Destroy(t);
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
        if (inactiveObstacles.Count <= 0)
            return;
        int i = Random.Range(0, inactiveObstacles.Count);
        int j = Random.Range(0, spawnX.Count);
        GameObject g = inactiveObstacles[i];
        g.transform.position = new Vector3(spawnX[j], 0, spawnZ);
        g.SetActive(true);
        inactiveObstacles.RemoveAt(i);
        activeObstacles.Add(g);
        SpawnBuildings();
    }

    void SpawnBuildings()
    {
        float y = ground.transform.position.y;
        int i = Random.Range(0, buildings.Length);
        GameObject g = GameObject.Instantiate(buildings[i]);
        Vector3 ps = new Vector3(spawnX[0], y, spawnZ);
        ps.x = Random.Range(2 * llx - lrx, llx);
        ps.z = spawnZ + Random.Range(-2, 2);
        g.transform.position = ps;
        activeBuildings.Add(g);
        g.transform.SetParent(ground.transform);

        i = Random.Range(0, buildings.Length);
        g = GameObject.Instantiate(buildings[i]);
        ps.x = Random.Range(llx, lrx);
        ps.z = spawnZ + Random.Range(-2, 2);
        g.transform.position = ps;
        activeBuildings.Add(g);
        g.transform.SetParent(ground.transform);

        i = Random.Range(0, buildings.Length);
        g = GameObject.Instantiate(buildings[i]);
        ps.x = Random.Range(rlx, rrx);
        ps.z = spawnZ + Random.Range(-2, 2);
        g.transform.position = ps;
        activeBuildings.Add(g);
        g.transform.SetParent(ground.transform);

        i = Random.Range(0, buildings.Length);
        g = GameObject.Instantiate(buildings[i]);
        ps.x = Random.Range(rrx, 2*rrx - rlx);
        ps.z = spawnZ + Random.Range(-2, 2);
        g.transform.position = ps;
        activeBuildings.Add(g);
        g.transform.SetParent(ground.transform);
    }
}
