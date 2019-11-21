using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using System.Reflection;
using System.Linq;

[InitializeOnLoad]
public class HierarchyIcons
{
    static Texture2D referenceTextureRequired;
    static Texture2D referenceTextureBalance;

    static HierarchyIcons()
    {
        // Init
        referenceTextureRequired = AssetDatabase.LoadAssetAtPath("Assets/Textures/ScriptingTextures/ExclamationMark.png", typeof(Texture2D)) as Texture2D;
        referenceTextureBalance = AssetDatabase.LoadAssetAtPath("Assets/Textures/ScriptingTextures/1200px-Question_mark_grey.svg.png", typeof(Texture2D)) as Texture2D;
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
                    #region Required
                    
                    FieldInfo[] fieldsRequired = GetAllRequired(components, i);

                    FieldInfo[] fieldsSerializeField = GetAllSerializedFields(fieldsRequired);
                    
                    bool componentHasAllRefRequired = true;

                    for (int j = 0; j < fieldsSerializeField.Length; j++)
                    {
                        if(fieldsSerializeField[j] != null)
                        {
                            var o = fieldsSerializeField[j].GetValue(components[i]);


                            if (o == null)
                            {
                                componentHasAllRefRequired = false;
                            }
                            else
                            {
                                try
                                {
                                    string unassignedCheck = ((Object)o).name;
                                }
                                catch (UnassignedReferenceException unassigned)
                                {
                                    componentHasAllRefRequired = false;
                                }
                            }
                        }
                    }

                    if (!componentHasAllRefRequired)
                    {
                        DrawIconRequired(selectionRect);
                    }

                    #endregion

                    #region Balance
                    
                    FieldInfo[] fieldsBalance = GetAllBalance(components, i);

                    FieldInfo[] fieldsSerializeFieldInBalance = GetAllSerializedFields(fieldsBalance);
                    
                    bool componentHasAllRefBalance = true;

                    for (int k = 0; k < fieldsSerializeFieldInBalance.Length; k++)
                    {
                        if(fieldsSerializeFieldInBalance[k] != null)
                        {
                            var o = fieldsSerializeFieldInBalance[k].GetValue(components[i]);

                            System.Type tp = o.GetType();
                            
                            if (o.Equals(GetDefault(tp)))
                            {
                                componentHasAllRefBalance = false;
                            }
                        }
                    }

                    if (!componentHasAllRefBalance)
                    {
                        DrawIconBalance(selectionRect);
                    }
                    
                    #endregion
                    
            }           
            
        }
    }

    static FieldInfo[] GetAllRequired(Component[] components, int i)
    {
        return components[i].GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(f => f.GetCustomAttribute<RequiredAttribute>() != null).ToArray();
    }

    static FieldInfo[] GetAllBalance(Component[] components, int i)
    {
        return components[i].GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        .Where(f => f.GetCustomAttribute<BalanceAttribute>() != null).ToArray();
    }

    static FieldInfo[] GetAllSerializedFields(FieldInfo[] fieldsArray)
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

    static void DrawIconRequired(Rect selectionRect)
    {
        // place the icoon to the right of the list:
        Rect r = new Rect(selectionRect);
        r.width = 20;
        r.x = r.width - 20;

        GUI.Label(r, referenceTextureRequired);     
    }

    static void DrawIconBalance(Rect selectionRect)
    {
        // place the icon to the right of the list:
        Rect r = new Rect(selectionRect);
        r.width = 20;
        r.x = r.width - 10;

        GUI.Label(r, referenceTextureBalance);     
    }

    private static object GetDefault(System.Type type)
    {
        if(type.IsValueType)
        {
            return System.Activator.CreateInstance(type);
        }

        return null;
    }
}