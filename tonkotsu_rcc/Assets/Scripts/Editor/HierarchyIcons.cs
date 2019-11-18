﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using System.Reflection;
using System.Linq;

[InitializeOnLoad]
public class HierarchyIcons
{
    static Texture2D referenceTexture;
    static Texture2D balanceTexture;

    static HierarchyIcons()
    {
        // Init
        referenceTexture = AssetDatabase.LoadAssetAtPath("Assets/Textures/ScriptingTextures/ExclamationMark.png", typeof(Texture2D)) as Texture2D;
        balanceTexture = AssetDatabase.LoadAssetAtPath("Assets/Textures/ScriptingTextures/1200px-Question_mark_grey.svg.png", typeof(Texture2D)) as Texture2D;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyIconCB;
    }

    static void HierarchyIconCB(int instanceID, Rect selectionRect)
    {
        var foundGameObjects = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        
        if (foundGameObjects)
        {
                var components = foundGameObjects.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    FieldInfo[] fieldsRequired = GetAllRequired(components, i);

                    FieldInfo[] fieldsSerializeField = GetAllSerializedFields(fieldsRequired);
                    
                    bool componentHasAllRef = true;

                    for (int j = 0; j < fieldsSerializeField.Length; j++)
                    {
                        if(fieldsSerializeField[j] != null)
                        {
                            var o = fieldsSerializeField[j].GetValue(components[i]);


                            if (o == null)
                            {
                                componentHasAllRef = false;
                            }
                            else
                            {
                                try
                                {
                                    string unassignedCheck = ((Object)o).name;
                                }
                                catch (UnassignedReferenceException unassigned)
                                {
                                    componentHasAllRef = false;
                                }
                            }
                        }

                    if (!componentHasAllRef)
                    {
                        DrawIcon(selectionRect);
                    }
                }           
            }
        }
    }

    private static FieldInfo[] GetAllRequired(Component[] components, int i)
    {
        return components[i].GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(f => f.GetCustomAttribute<RequiredAttribute>() != null).ToArray();
    }

    private static FieldInfo[] GetAllSerializedFields(FieldInfo[] fieldsArray)
    {
        FieldInfo[] foundFields = new FieldInfo[fieldsArray.Length];

        for(int i = 0; i < fieldsArray.Length; i++)
        {
            if(fieldsArray[i].GetCustomAttribute<SerializeField>() != null)
            {
                foundFields[i] = fieldsArray[i];
            }
        }
        
        return foundFields;
    }

    static void DrawIcon(Rect selectionRect)
    {
        // place the icoon to the right of the list:
        Rect r = new Rect(selectionRect);
        r.width = 20;
        r.x = r.width - 20;

        GUI.Label(r, referenceTexture);     
    }
}
