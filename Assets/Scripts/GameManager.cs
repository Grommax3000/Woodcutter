using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<TreeData> treeDatas = new List<TreeData>();
    public List<Tree> trees = new List<Tree>();
    public RectTransform content;
    public TreeButton buttonPrefab;
    public GameObject visualPrefab;
    public LayerMask maskPositionRay;
    public LayerMask maskAllowRay;
    public Material plantingTreeMat;
    public Color allowColor;
    public Color notAllowColor;

    public static GameManager instance;
    public UnityEvent OnPlant;

    private bool planting = false;
    private GameObject visualPlant;
    private TreeData curTreeData;
    private bool allowPlant = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for(int i = 0; i < treeDatas.Count; i++)
        {
            TreeButton newButton = Instantiate(buttonPrefab, content);
            newButton.SetData(treeDatas[i]);
        }
    }

    public void StartPlanting(TreeData data)
    {
        curTreeData = data;
        planting = true;
    }

    public void EndPlanting()
    {
        if (allowPlant == true)
        {
            GameObject newTree = Instantiate(curTreeData.prefab, visualPlant.transform.position, visualPlant.transform.rotation);
            trees.Add(newTree.GetComponent<Tree>());
            OnPlant.Invoke();
        }
        Destroy(visualPlant);
        planting = false;
    }

    private void Update()
    {
        if(planting == true)
        {
            if(Input.GetMouseButtonUp(0))
            {
                EndPlanting();
            }
        }
    }

    private void FixedUpdate()
    {
        if (planting == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit1;
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction, out hit1, 1000, maskPositionRay))
            {
                Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction * hit1.distance, Color.yellow);
                if(visualPlant == null)
                {
                    visualPlant = Instantiate(visualPrefab, hit1.point, Quaternion.identity);
                }
                else
                {
                    visualPlant.transform.position = hit1.point;
                }
            }

            RaycastHit hit2;
            if (Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), ray.direction, out hit2, 1000, maskAllowRay))
            {
                if (hit2.collider.CompareTag("Ground"))
                {
                    plantingTreeMat.color = allowColor;
                    allowPlant = true;
                }
                else
                {
                    plantingTreeMat.color = notAllowColor;
                    allowPlant = false;
                }
            }
        }
        else
        {
            if(visualPlant != null)
            {
                Destroy(visualPlant);
            }
        }
    }
}
