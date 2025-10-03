using UnityEngine;

[System.Serializable]
public class VignetteData
{
	[SerializeField] private VignetteID id;
	[SerializeField] private Color color;
	[SerializeField] private float intensity;
	[SerializeField] private float smoothness;
	[SerializeField] private bool roundness;

	public VignetteID ID => id;
	public Color Color => color;
	public float Intensity => intensity; 
	public float Smoothness => smoothness;
	public bool Roundness => roundness;
}