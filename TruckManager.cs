using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TruckManager : MonoBehaviour
{
   
    public static TruckManager instance { get; private set; }

    public List<GameObject> listPoints;

    public GameObject pointPrefab;

    private List<GameObject> fullPaths;

  //  public Find_path find_Path;

    //public GameObject prefab;

    private List<List<GameObject>> bezierLines;

    private List<GameObject> addPoints;


    public List<GameObject> AddPoints
    {
        get
        {
            return addPoints;
        }

    }

    private void Awake()
    {
        if (instance == null)
        { // Ёкземпл€р менеджера был найден
            instance = this; // «адаем ссылку на экземпл€р объекта
        }
        else if (instance == this)
        { // Ёкземпл€р объекта уже существует на сцене
            Destroy(gameObject); // ”дал€ем объект
        }
    }

    void Start()
    {
        GameObject[] additionalPoints = GameObject.FindGameObjectsWithTag("AdditionalPoint");
       
        fullPaths = new List<GameObject>(listPoints);
    
        addPoints = new List<GameObject>();

        if (listPoints != null & additionalPoints.Length <= 1)
        {
            int counter = 1;
            for (int i = 0; i < listPoints.Count - 1; i++)

            {
                GameObject point = Instantiate(pointPrefab, Middle(listPoints[i], listPoints[i + 1]), Quaternion.identity);
                point.name = (i) + " p";

                addPoints.Add(point);
                if (i == 0)
                {
                    fullPaths.Insert(i, point);
                }
                else
                {
                    fullPaths.Insert(counter, point);
                }
                counter += 2;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < addPoints.Count; i++)
        {

            if (addPoints[i].name != "tached")

            {
                int pointNumber = 0;

                string[] splitName = addPoints[i].name.Split(" ");


                pointNumber = Int32.Parse(splitName[0]); // (int)Convert.ToInt32(addPoints[i].name);


                Vector3 a = listPoints[pointNumber].transform.position;

                Vector3 b = new Vector3(addPoints[i].transform.position.x, addPoints[i].transform.position.y + 2, addPoints[i].transform.position.z);

                Vector3 normal = GetNormal(a, b, addPoints[i].transform.position);

                Debug.DrawRay(addPoints[i].transform.position, normal * 23, Color.red);
                RaycastHit hit;
                if (Physics.Raycast(addPoints[i].transform.position, normal, out hit, 27f))
                {

                    Vector3 direction = hit.point - addPoints[i].transform.position;
                    var normDirection = direction.normalized;
                    addPoints[i].transform.position = hit.point + normDirection * 10f;
                    continue;
                }
                else
                {
                    Vector3 a1 = listPoints[pointNumber].transform.position;
                    Vector3 b1 = new Vector3(addPoints[i].transform.position.x, addPoints[i].transform.position.y - 2, addPoints[i].transform.position.z);
                    Vector3 normal1 = GetNormal(a1, b1, addPoints[i].transform.position);

                    RaycastHit hit2;

                    Debug.DrawRay(addPoints[i].transform.position, normal1 * 23, Color.red);

                    if (Physics.Raycast(addPoints[i].transform.position, normal1, out hit2, 27f))
                    {
                        Vector3 direction2 = hit2.point - addPoints[i].transform.position;
                        var normDirection2 = direction2.normalized;
                        addPoints[i].transform.position = hit2.point + normDirection2 * 10f;
                        continue;
                    }

                }
            }

        }
        bezierLines = chucks(listPoints, addPoints);

    }

    // ћетод дл€ прорисовки линий между начальными точками
    public void DrawLine(List<GameObject> listPoints)
    {
        for (int i = 0; i < listPoints.Count - 1; i++)
        {
            Vector3 vec1 = new Vector3(listPoints[i].transform.position.x, listPoints[i].transform.position.y, listPoints[i].transform.position.z);
            Vector3 vec2 = new Vector3(listPoints[i + 1].transform.position.x, listPoints[i + 1].transform.position.y, listPoints[i + 1].transform.position.z);

            Debug.DrawLine(vec1, vec2);
        }
    }

    // ћетод сначала копирует список исходных точек в новый список
    // «атем находит центр отрезка между двум€ точками и заносит точку с найдеными координатами
    // во вновь созданный List FullPath. ќбразу€ полнй список точек 
    public List<GameObject> FindMiddle(List<GameObject> listPoints)
    {

        List<GameObject> fullPaths = new List<GameObject>(listPoints);

        for (int i = 0; i < listPoints.Count - 1; i++)
        {
            Vector3 vec1 = new Vector3(listPoints[i].transform.position.x, listPoints[i].transform.position.y, listPoints[i].transform.position.z);
            Vector3 vec2 = new Vector3(listPoints[i + 1].transform.position.x, listPoints[i + 1].transform.position.y, listPoints[i + 1].transform.position.z);
            Vector3 middle = Vector3.Lerp(vec1, vec2, 0.5f);
            pointPrefab.transform.position = middle;
            fullPaths.Insert(i + 1, pointPrefab);

        }
        return fullPaths;
    }

    // Ќаходит средину отрезка между двух точек
    public Vector3 Middle(GameObject point1, GameObject point2)
    {
        Vector3 vec1 = new Vector3(point1.transform.position.x, point1.transform.position.y, point1.transform.position.z);
        Vector3 vec2 = new Vector3(point2.transform.position.x, point2.transform.position.y, point2.transform.position.z);
        Vector3 middle = Vector3.Lerp(vec1, vec2, 0.5f);
        return middle;

    }


    private void OnDrawGizmos()
    {

        int sigmentsNumber = 20;

        for (int j = 0; j < bezierLines.Count; j++)
        {
            Vector3 preveousePoint = bezierLines[j][0].transform.position;

            for (int i = 0; i < sigmentsNumber + 1; i++)
            {

                float paremeter = (float)i / sigmentsNumber;

                Vector3 point = Bezier.GetPoint(bezierLines[j], paremeter);
                Gizmos.DrawLine(point, preveousePoint);
                preveousePoint = point;

            }

        }
    }

    public List<List<GameObject>> chucks(List<GameObject> values, List<GameObject> middle)
    {

        List<List<GameObject>> full = new List<List<GameObject>>();

        for (int i = 0; i < values.Count - 1; i++)
        {
            List<GameObject> tmp = new List<GameObject>();
            tmp.Add(values[i]);
            tmp.Add(middle[i]);
            tmp.Add(values[i + 1]);
            full.Add(tmp);
        }

        return full;

    }

    // ћетод строит плоскоть и возвращает перпендикул€р к этой плоскости по трем точкам.
    Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        Vector3 side1 = b - a;
        Vector3 side2 = c - a;

        return Vector3.Cross(side1, side2).normalized;
    }

}
