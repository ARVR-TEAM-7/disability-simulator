using UnityEngine;
using Unity.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class Shoot : MonoBehaviour
{
  public GameObject bulletPrefab;
  public float bulletSpeed = 20f;
  public float cooldownDuration = 0.25f;
  private float lastShotTime = 0f;
  public AudioSource deagleSound;
  public GameObject casingPrefab;
  public GameObject muzzleFlashPrefab;

  [SerializeField] private Animator gunAnimator;
  [SerializeField] private Transform casingExitLocation;
  [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
  void Update()
  {
    var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
    UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, rightHandDevices);

    if (rightHandDevices.Count > 0)
    {
      UnityEngine.XR.InputDevice rightController = rightHandDevices[0];

      bool triggerButtonPressed;
      if (rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerButtonPressed))
      {
        if (triggerButtonPressed && Time.time >= lastShotTime + cooldownDuration)
        {
          ShootBullet();
          lastShotTime = Time.time;
          rightController.SendHapticImpulse(0, 0.4f, 0.1f);
        }
      }
    }
  }

[ContextMenu("Shoot")]
  void ShootBullet()
  {
    gunAnimator.speed = 3.0f;
    gunAnimator.SetTrigger("Fire");
    deagleSound.PlayOneShot(deagleSound.clip);
    if (muzzleFlashPrefab)
    {
      GameObject tempFlash = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);

      Destroy(tempFlash, destroyTimer);
    }
    GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
    Rigidbody rb = bullet.GetComponent<Rigidbody>();
    rb.AddForce(transform.forward * bulletSpeed, ForceMode.Force);
    Destroy(bullet, 2.0f);
  }
}