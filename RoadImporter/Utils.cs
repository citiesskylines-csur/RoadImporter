using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using ColossalFramework;
using UnityEngine;

namespace RoadImporter
{
    public class Utils
    {
        public static void CopyFromGame(object source, object target)
        {
            Debug.Log("Invoke copyfromgame");
            Debug.Log($"source:{source}, target:{target}");
            if (source == null || target == null) return;
            FieldInfo[] fields = target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fields)
            {
                FieldInfo gameFieldInfo = source.GetType().GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                Debug.Log(gameFieldInfo);
                Debug.Log(fieldInfo);
                if (gameFieldInfo.FieldType == typeof(NetInfo.Lane[]))
                {
                    NetInfo.Lane[] gameObjects = (NetInfo.Lane[])gameFieldInfo.GetValue(source);
                    int len = gameObjects.Length;
                    CSNetInfo.Lane[] savedObjects = new CSNetInfo.Lane[len];
                    for (int i = 0; i < len; i++)
                    {
                        savedObjects[i] = new CSNetInfo.Lane();
                        CopyFromGame(gameObjects[i], savedObjects[i]);
                    }
                    fieldInfo.SetValue(target, savedObjects);
                }
                else if (gameFieldInfo.FieldType == typeof(NetInfo.Segment[]))
                {
                    NetInfo.Segment[] gameObjects = (NetInfo.Segment[])gameFieldInfo.GetValue(source);
                    int len = gameObjects.Length;
                    CSNetInfo.Segment[] savedObjects = new CSNetInfo.Segment[len];
                    for (int i = 0; i < len; i++)
                    {
                        savedObjects[i] = new CSNetInfo.Segment();
                        CopyFromGame(gameObjects[i], savedObjects[i]);
                    }
                    fieldInfo.SetValue(target, savedObjects);
                }
                else if (gameFieldInfo.FieldType == typeof(NetInfo.Node[]))
                {
                    NetInfo.Node[] gameObjects = (NetInfo.Node[])gameFieldInfo.GetValue(source);
                    int len = gameObjects.Length;
                    CSNetInfo.Node[] savedObjects = new CSNetInfo.Node[len];
                    for (int i = 0; i < len; i++)
                    {
                        savedObjects[i] = new CSNetInfo.Node();
                        CopyFromGame(gameObjects[i], savedObjects[i]);
                    }
                    fieldInfo.SetValue(target, savedObjects);
                }
                else if (gameFieldInfo.FieldType == typeof(NetLaneProps))
                {
                    NetLaneProps gameObjects = (NetLaneProps)gameFieldInfo.GetValue(source);
                    int len = gameObjects.m_props.Length;
                    CSNetInfo.Prop[] savedObjects = new CSNetInfo.Prop[len];
                    for (int i = 0; i < len; i++)
                    {
                        savedObjects[i] = new CSNetInfo.Prop();
                        CopyFromGame(gameObjects.m_props[i], savedObjects[i]);
                    }
                    fieldInfo.SetValue(target, savedObjects);
                }
                else if (gameFieldInfo.FieldType == typeof(Vector3))
                {
                    Vector3 vec = (Vector3)gameFieldInfo.GetValue(source);
                    float[] vals = { vec[0], vec[1], vec[2] };
                    fieldInfo.SetValue(target, vals);
                }
                else if (gameFieldInfo.FieldType.IsEnum)
                {
                    object val = Enum.ToObject(fieldInfo.FieldType, gameFieldInfo.GetValue(source));
                    fieldInfo.SetValue(target, val);
                }
                else if (gameFieldInfo.FieldType.IsSubclassOf(typeof(PrefabInfo)))
                {
                    PrefabInfo prefab = (PrefabInfo)gameFieldInfo.GetValue(source);
                    if (prefab == null)
                    {
                        fieldInfo.SetValue(target, "");
                    }
                    else
                    {
                        fieldInfo.SetValue(target, prefab.name);
                    }
                    
                }
                else
                {
                    fieldInfo.SetValue(target, gameFieldInfo.GetValue(source));
                } 

            }
        }

        public static void CopyToGame(object source, object target)
        {
            Debug.Log("Invoke copytogame");
            Debug.Log($"source:{source}, target:{target}");
            if (source == null || target == null) return;
            FieldInfo[] fields = source.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fields)
            {
                if (fieldInfo.GetValue(source) == null)
                {
                    Debug.Log($"Field {fieldInfo.Name} is null, skipping import");
                    continue;
                }
                FieldInfo gameFieldInfo = target.GetType().GetField(fieldInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                Debug.Log(gameFieldInfo);
                Debug.Log(fieldInfo);
                if (gameFieldInfo.FieldType.IsArray)
                {
                    Array savedObjects = (Array) fieldInfo.GetValue(source);
                    int len = savedObjects.Length;
                    Type elementType = gameFieldInfo.FieldType.GetElementType();
                    gameFieldInfo.SetValue(target, Array.CreateInstance(elementType, 0));
                    for (int i = 0; i < len; i++)
                    {
                        AssetEditorRoadUtils.AddArrayElement(target, gameFieldInfo);
                        Array gameObjects = (Array)gameFieldInfo.GetValue(target);
                        CopyToGame(savedObjects.GetValue(i), gameObjects.GetValue(i));
                    }
                }
                else if (gameFieldInfo.FieldType == typeof(NetLaneProps))
                {
                    CSNetInfo.Prop[] savedObjects = (CSNetInfo.Prop[])fieldInfo.GetValue(source);
                    int len = savedObjects.Length;
                    NetLaneProps gameObjects = new NetLaneProps { m_props = new NetLaneProps.Prop[len] };
                    for (int i = 0; i < len; i++)
                    {
                        gameObjects.m_props[i] = new NetLaneProps.Prop();
                        CopyToGame(savedObjects[i], gameObjects.m_props[i]);
                    }
                    gameFieldInfo.SetValue(target, gameObjects);
                }
                else if (gameFieldInfo.FieldType == typeof(Vector3))
                {
                    
                    float[] vals = (float[]) fieldInfo.GetValue(source);
                    Vector3 vec = new Vector3(vals[0], vals[1], vals[2]);
                    gameFieldInfo.SetValue(target, vec);
                }
                else if (gameFieldInfo.FieldType.IsEnum)
                {
                    object val = Enum.ToObject(gameFieldInfo.FieldType, fieldInfo.GetValue(source));
                    gameFieldInfo.SetValue(target, val);
                }
                else if (gameFieldInfo.FieldType.IsSubclassOf(typeof(PrefabInfo)))
                {
                    string prefabName = (string)fieldInfo.GetValue(source);

                    if (prefabName.Equals(""))
                    {
                        gameFieldInfo.SetValue(target, null);
                    }
                    else
                    {
                        if (gameFieldInfo.FieldType == typeof(PropInfo))
                        {
                            PropInfo prefab = PrefabCollection<PropInfo>.FindLoaded(prefabName);
                            gameFieldInfo.SetValue(target, prefab);
                        }
                        else if (gameFieldInfo.FieldType == typeof(BuildingInfo))
                        {
                            BuildingInfo prefab = PrefabCollection<BuildingInfo>.FindLoaded(prefabName);
                            gameFieldInfo.SetValue(target, prefab);
                        }
                        else if (gameFieldInfo.FieldType == typeof(TreeInfo))
                        {
                            TreeInfo prefab = PrefabCollection<TreeInfo>.FindLoaded(prefabName);
                            gameFieldInfo.SetValue(target, prefab);
                        }
                    }

                }
                else
                {
                    gameFieldInfo.SetValue(target, fieldInfo.GetValue(source));
                }
            }
        }

        public static void SaveAsset(IAssetInfo asset, string filename, Type netAIType)
        {
            
            TextWriter writer = new StreamWriter(filename);
            if (netAIType == typeof(RoadAI))
            {
                XmlSerializer ser = new XmlSerializer(typeof(RoadAssetInfo));
                ser.Serialize(writer, (RoadAssetInfo)asset);
            }
            else if (netAIType == typeof(TrainTrackAI))
            {
                XmlSerializer ser = new XmlSerializer(typeof(TrainTrackAssetInfo));
                ser.Serialize(writer, (TrainTrackAssetInfo)asset);
            }
            else
            {
                throw new NotImplementedException("Unsupported network type!");
            }

            writer.Close();
        }

        public static IAssetInfo LoadAsset(string filename, Type netAIType)
        {
            
            StreamReader reader = new StreamReader(filename);
            IAssetInfo myNet;
            if (netAIType == typeof(RoadAI))
            {
                XmlSerializer ser = new XmlSerializer(typeof(RoadAssetInfo));
                myNet = (RoadAssetInfo)ser.Deserialize(reader);
            } else if (netAIType == typeof(TrainTrackAI))
            {
                XmlSerializer ser = new XmlSerializer(typeof(TrainTrackAssetInfo));
                myNet = (TrainTrackAssetInfo)ser.Deserialize(reader);
            }
            else
            {
                throw new NotImplementedException("Unsupported network type!");
            }
            return myNet;
        }

        public static void RefreshRoadEditor()
        {
            typeof(RoadEditorMainPanel).GetMethod("Clear", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(UnityEngine.Object.FindObjectOfType<RoadEditorMainPanel>(), null);
            typeof(RoadEditorMainPanel).GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(UnityEngine.Object.FindObjectOfType<RoadEditorMainPanel>(), null);
            typeof(RoadEditorPanel).GetMethod("OnObjectModified", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(UnityEngine.Object.FindObjectOfType<RoadEditorPanel>(), null);
        }
    }
}
