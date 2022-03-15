
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[Serializable]
public class SerializableObject
{
    [field: NonSerialized]
    public HierarchyObject HierarchyReference = null;
    [field: SerializeField]
    public SerializableObject Parent = null;
    [field: SerializeField]
    public List<SerializableObject> Children = new List<SerializableObject>();
    [field: SerializeField]
    public string Title = "";
}
