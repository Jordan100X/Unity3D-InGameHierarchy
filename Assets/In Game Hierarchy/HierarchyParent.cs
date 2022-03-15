
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Text.Json;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class HierarchyParent : HierarchyObject
{
    public static HierarchyParent Instance
    {
        get
        {
            return m_Instance;
        }
    }
    private static HierarchyParent m_Instance = null;

    public GameObject SpawnablePrefab = null;

    void Awake()
    {
        if (m_Instance != null)
        {
            Debug.LogError("Multiple HierarchyParent objects instantiated");
        }
        else
        {
            m_Instance = this;
        }
        SerializableReference.HierarchyReference = this;
    }

    const string EXTENSION = "*.hierarchy";
    public void Save()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        //ofn.filter = "All Files\0*.*\0\0";
        ofn.filter = EXTENSION;
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = UnityEngine.Application.dataPath;
        ofn.title = "Save Hierarchy";
        ofn.defExt = EXTENSION;
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        if (DllTest.GetSaveFileName(ofn))
        {
            Debug.Log("file:///" + ofn.file);
            string outFile = JsonUtility.ToJson(SerializableReference);
            File.WriteAllText(ofn.file, outFile);
        }
    }

    public void Load()
    {
        ClearChildren();

        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        //ofn.filter = "All Files\0*.*\0\0";
        ofn.filter = EXTENSION;
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = UnityEngine.Application.dataPath;
        ofn.title = "Load Hierarchy";
        ofn.defExt = EXTENSION;
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        if (DllTest.GetOpenFileName(ofn))
        {
            Debug.Log("file:///" + ofn.file);
            string inFile = File.ReadAllText(ofn.file);
            SerializableReference = JsonUtility.FromJson<SerializableObject>(inFile);
            SerializableReference.HierarchyReference = this;
            SpawnChildren(SerializableReference);
            Debug.Log(inFile);
        }
    }

    public void SpawnChildren(SerializableObject serializableObject)
    {
        foreach (var child in serializableObject.Children)
        {
            SpawnChild(child, serializableObject);
            SpawnChildren(child);
        }
    }

    public static void SpawnChild(SerializableObject serializableObject, SerializableObject parentObject)
    {
        GameObject spawnableObject = HierarchyParent.Instance.SpawnablePrefab;
        GameObject temp = Instantiate(spawnableObject);
        temp.name = "Spawned Object " + spawnCounter.ToString("000");
        temp.transform.SetParent(parentObject.HierarchyReference.ChildContainer.transform, false);
        HierarchyObject childHierarchy = temp.GetComponent<HierarchyObject>();
        childHierarchy.SerializableReference = serializableObject;
        serializableObject.HierarchyReference = childHierarchy;
        spawnCounter++;
        //temp.GetComponent<HierarchyObject>().UpdateHeight();
        serializableObject.Parent = parentObject;
        //if (ExpandChildrenButton != null)
        //{
        //    // Setting isOn won't trigger events if set to the same value, so turn if off then back on
        //    ExpandChildrenButton.isOn = false;
        //    ExpandChildrenButton.isOn = true;
        //}
        //LayoutRebuilder.ForceRebuildLayoutImmediate(HierarchyParent.Instance.transform as RectTransform);
        // TODO: Move somewhere else
        childHierarchy.UpdateText();
    }

    public override void UpdateSize()
    {
        // Do nothing
    }
}
