using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Animator))]
public class AnimatorRagdoll : MonoBehaviour 
{
	[SerializeField] private bool startRagdolled = false;
	private Animator animator;
	private List<Transform> bones;

	private bool _ragdolled;
	public bool Ragdolled
	{
		get
		{
			return _ragdolled;
		}
		set
		{
			animator.enabled = !value;
			foreach(Transform bone in bones)
			{
				bone.GetComponent<Collider>().enabled = value;
				bone.GetComponent<Rigidbody>().isKinematic = !value;
			}
			_ragdolled = value;
		}
	}

	public Transform GetBone(HumanBodyBones bone)
	{
		return animator.GetBoneTransform(bone);
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		bones = new List<Transform>();
		bones.Add(GetBone(HumanBodyBones.Hips));
		bones.Add(GetBone(HumanBodyBones.LeftUpperLeg));
		bones.Add(GetBone(HumanBodyBones.LeftLowerLeg));
		bones.Add(GetBone(HumanBodyBones.RightUpperLeg));
		bones.Add(GetBone(HumanBodyBones.RightLowerLeg));
		bones.Add(GetBone(HumanBodyBones.LeftUpperArm));
		bones.Add(GetBone(HumanBodyBones.LeftLowerArm));
		bones.Add(GetBone(HumanBodyBones.RightUpperArm));
		bones.Add(GetBone(HumanBodyBones.RightLowerArm));
		bones.Add(GetBone(HumanBodyBones.Chest));
		bones.Add(GetBone(HumanBodyBones.Head));
	}

	private void Start()
	{
		Ragdolled = startRagdolled;
	}

#if UNITY_EDITOR
	[ContextMenu("Select Bones")]
	public void SelectBones()
	{
		animator = GetComponent<Animator>();
		List<GameObject> bones = new List<GameObject>();
		bones.Add(GetBone(HumanBodyBones.Hips).gameObject);
		bones.Add(GetBone(HumanBodyBones.LeftUpperLeg).gameObject);
		bones.Add(GetBone(HumanBodyBones.LeftLowerLeg).gameObject);
		bones.Add(GetBone(HumanBodyBones.RightUpperLeg).gameObject);
		bones.Add(GetBone(HumanBodyBones.RightLowerLeg).gameObject);
		bones.Add(GetBone(HumanBodyBones.LeftUpperArm).gameObject);
		bones.Add(GetBone(HumanBodyBones.LeftLowerArm).gameObject);
		bones.Add(GetBone(HumanBodyBones.RightUpperArm).gameObject);
		bones.Add(GetBone(HumanBodyBones.RightLowerArm).gameObject);
		bones.Add(GetBone(HumanBodyBones.Chest).gameObject);
		bones.Add(GetBone(HumanBodyBones.Head).gameObject);
		Selection.objects = bones.ToArray();
	}

	[ContextMenu("Toggle Ragdoll")]
	public void EditorToggleRagdoll()
	{
		if (!animator)
			Awake ();
		Ragdolled = !Ragdolled;
	}

	[ContextMenu("Remove Ragdoll")]
	public void EditorRemoveRagdoll()
	{
		Awake ();
		foreach(Transform bone in bones)
		{
			DestroyImmediate(bone.GetComponent<CharacterJoint>());
			DestroyImmediate(bone.GetComponent<Rigidbody>());
			DestroyImmediate(bone.GetComponent<Collider>());
		}
		DestroyImmediate (this);
	}
#endif

	public void FreezeAfterDelay(float delay)
	{
		Invoke ("Freeze", delay);
	}

	public void Freeze()
	{
		foreach(Transform bone in bones)
		{
			Destroy(bone.GetComponent<CharacterJoint>());
			Destroy(bone.GetComponent<Rigidbody>());
			Destroy(bone.GetComponent<Collider>());
		}
		Destroy (this);
	}
}
