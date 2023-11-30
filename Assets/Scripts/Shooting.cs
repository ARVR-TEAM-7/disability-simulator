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
  public bool isRightHand;

  [SerializeField] private Animator gunAnimator;
  [SerializeField] private Transform casingExitLocation;
  [Tooltip("Specify time to destory the casing object")][SerializeField] private float destroyTimer = 2f;
  void Update()
  {
    var handDevices = new List<UnityEngine.XR.InputDevice>();
    if (isRightHand)
    {
      UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, handDevices);

      if (handDevices.Count > 0)
      {
        UnityEngine.XR.InputDevice rightController = handDevices[0];

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
    else
    {
      UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.LeftHanded, handDevices);

      if (handDevices.Count > 0)
      {
        UnityEngine.XR.InputDevice leftController = handDevices[0];

        bool triggerButtonPressed;
        if (leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerButtonPressed))
        {
          if (triggerButtonPressed && Time.time >= lastShotTime + cooldownDuration)
          {
            ShootBullet();
            lastShotTime = Time.time;
            leftController.SendHapticImpulse(0, 0.4f, 0.1f);
          }
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