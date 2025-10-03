using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteManager : MonoBehaviour, IVignetteManager
{
	[SerializeField] private VolumeProfile profile;

	[SerializeField] private List<VignetteData> datas;

	private Vignette vignetteComponent;
	private Tween tween;

	void IVignetteManager.SetVignetteData(VignetteID id, float transitionTime)
	{
		VignetteData data = datas.Find(x => x.ID == id);

		if(vignetteComponent == null)
		{
			profile.TryGet(out vignetteComponent);
		}

		vignetteComponent.rounded.value = data.Roundness;

		Color beforeColor = vignetteComponent.color.value;
		float beforeIntensity = vignetteComponent.intensity.value;
		float beforeSmoothness = vignetteComponent.smoothness.value;

		tween?.Kill();
		tween = DOVirtual.Float(0, 1, transitionTime, x =>
		{
			vignetteComponent.color.value = Color.Lerp(beforeColor, data.Color, x);
			vignetteComponent.intensity.value = Mathf.Lerp(beforeIntensity, data.Intensity, x);
			vignetteComponent.smoothness.value = Mathf.Lerp(beforeSmoothness, data.Smoothness, x);
		});
	}

	private void OnDestroy()
	{
		tween?.Kill();
	}

	#if UNITY_EDITOR
	private void OnApplicationQuit()
	{
		VignetteData data = datas.Find(x => x.ID == VignetteID.Default);

		if (vignetteComponent == null)
		{
			profile.TryGet(out vignetteComponent);
		}

		vignetteComponent.rounded.value = data.Roundness;
		vignetteComponent.color.value = data.Color;
		vignetteComponent.intensity.value = data.Intensity;
		vignetteComponent.smoothness.value = data.Smoothness;
	}
	#endif
}
