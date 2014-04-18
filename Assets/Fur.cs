using UnityEngine;
using System.Collections;

public class Fur : MonoBehaviour {
	public int nBones;
	public int nLines;
	public Material mat;
	public float swingDuration;

	void Start () {
		var nBonesPlus1 = nBones + 1;
		var bones = new Transform[nBonesPlus1 * nBonesPlus1 * 2];
		var poses = new Matrix4x4[bones.Length];

		for (var z = 0; z <= nBones; z++) {
			for (var x = 0; x <= nBones; x++) {
				var boneindex = x + z * nBonesPlus1;
				var tr0 = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
				tr0.gameObject.name = string.Format("Bottom({0}x{1})", x, z);
				Destroy(tr0.GetComponent<Collider>());
				var tr1 = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
				tr1.gameObject.name = string.Format("Top({0}x{1})", x, z);
				var swing = tr1.gameObject.AddComponent<Swing>();
				swing.duration = swingDuration;
				swing.phase = (2f * z + x) * 0.1f;
				swing.scale = 2f;
				Destroy(tr1.GetComponent<Collider>());
				tr0.parent = transform;
				tr0.localPosition = new Vector3(10f * x, 0f, 10f * z);
				tr1.parent = tr0;
				tr1.localPosition = new Vector3(0f, 10f, 0f);
				bones[2 * boneindex] = tr0;
				bones[2 * boneindex + 1] = tr1;
				poses[2 * boneindex] = tr0.worldToLocalMatrix * transform.localToWorldMatrix;
				poses[2 * boneindex + 1] = tr1.worldToLocalMatrix * transform.localToWorldMatrix;
			}
		}

		var dx = 10f / nLines;
		for (var bonez = 0; bonez < nBones; bonez++) {
			for (var bonex = 0; bonex < nBones; bonex++) {
				var mesh = new Mesh();
				var vertices = new Vector3[nLines * nLines * 2];
				var indices = new int[vertices.Length];
				var weights = new BoneWeight[vertices.Length];
				
				var boneindex = bonex + bonez * nBonesPlus1;
				var w0 = new BoneWeight();
				var w1 = new BoneWeight();
				w0.boneIndex0 = 2 * boneindex;
				w1.boneIndex0 = 2 * boneindex + 1;
				w0.boneIndex1 = 2 * (boneindex + 1);
				w1.boneIndex1 = 2 * (boneindex + 1) + 1;
				w0.boneIndex2 = 2 * (boneindex + nBonesPlus1);
				w1.boneIndex2 = 2 * (boneindex + nBonesPlus1) + 1;
				w0.boneIndex3 = 2 * (boneindex + nBonesPlus1 + 1);
				w1.boneIndex3 = 2 * (boneindex + nBonesPlus1 + 1) + 1;
				for (var linez = 0; linez < nLines; linez++) {
					for (var linex = 0; linex < nLines; linex++) {
						var ix = linex + bonex * nLines;
						var iz = linez + bonez * nLines;
						var lineindex = linez * nLines + linex;
						vertices[2 * lineindex] = new Vector3(ix * dx, 0f, iz * dx);
						vertices[ 2* lineindex + 1] = new Vector3(ix * dx, 10f, iz * dx);
						indices[2 * lineindex] = 2 * lineindex;
						indices[2 * lineindex + 1] = 2 * lineindex + 1;
						var tx = (float)linex / nLines;
						var tz = (float)linez / nLines;
						w0.weight0 = w1.weight0 = (1f - tx) * (1f - tz);
						w0.weight1 = w1.weight1 = tx * (1f - tz);
						w0.weight2 = w1.weight2 = (1f - tx) * tz;
						w0.weight3 = w1.weight3 = tx * tz;
						weights[2 * lineindex] = w0;
						weights[2 * lineindex + 1] = w1;
					}
				}

				mesh.vertices = vertices;
				mesh.SetIndices(indices, MeshTopology.Lines, 0);
				mesh.boneWeights = weights;
				mesh.bindposes = poses;
				mesh.RecalculateBounds();

				var go = new GameObject(string.Format("Fur({0}x{1})", bonex, bonez));
				go.transform.parent = transform;
				go.transform.localPosition = Vector3.zero;
				var skin = go.AddComponent<SkinnedMeshRenderer>();

				skin.bones = bones;
				skin.sharedMesh = mesh;
				skin.sharedMaterial = mat;
				skin.localBounds = mesh.bounds;
			}
		}
	}
}
