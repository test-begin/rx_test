using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    VRM.VRMMeta mVRMmeta;
    VRM.VRMHumanoidDescription mVRMHumanoidDescription;

    void Start ()
    {
        LoadVRM();
    }

    void Update ()
    {
	}

    void LoadVRM()
    {
        var path = Application.dataPath + "/Resources/Alicia/VRM/AliciaSolid.vrm";

        // Byte列を得る
        var bytes = System.IO.File.ReadAllBytes(path);

        var context = new VRM.VRMImporterContext();

        // GLB形式をParseしてチャンクからJSONを取得しParseします
        context.Parse(path, bytes);

        // Load前にmetaを読むときはこれ
        var meta = context.ReadMeta();
        Debug.LogFormat("meta: title:{0}", meta.Title);

        // ParseしたJSONをもとにシーンを構築します
        context.Load();
        OnLoaded(context.Root);


        ////////////////// 以下テスト //////////////////

        //context.Nodes; // モデルのボーンのリスト
        //context.GLTF.extensions.VRM.humanoid.humanBones // VRMが定義しているボーンとモデルのボーンの対応リスト
        //（glTF_VRM_Humanoid.ToDescription()を参考）
        KeyValuePair<VRM.VRMBone, Transform>[] array;
        array = context.GLTF.extensions.VRM.humanoid.humanBones
            .Where(x => x.node >= 0 && x.node < context.Nodes.Count)
                .Select(x => new KeyValuePair<VRM.VRMBone, Transform>(x.vrmBone, context.Nodes[x.node]))
                    .ToArray();
        foreach (var pair in array)
        {
            Debug.Log(pair);
        }
    }

    void OnLoaded(GameObject root)
    {
        var array_renderer = root.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in array_renderer)
        {
            renderer.enabled = true;
        }
    }
}
