using UnityEngine;
using System.Collections;

public class DamageTextTest : MonoBehaviour
{
    // public UnityEngine.AI.NavMeshAgent navmesh_agent = null;
    UnityEngine.AI.NavMeshAgent navmesh_agent = null;

    public Transform hit_bone_node = null;
    public Transform camera_transform = null;

    Color critical_color = new Color(1f, 195f / 255f, 0f);

    // Use this for initialization
    void Start()
	{
	
	}

    /*
     * void SpawnDamageText(int damage, bool critical, Color color)
    {
        if (damage > 0)
        {
            var damage_text = DamageTextPool.Instance.GetDamageText();
            if (damage_text != null)
            {
                Vector3 damage_text_offset = navmesh_agent.radius * (UnityEngine.Random.Range(-1.0f, 1.0f) * camera_transform.right + UnityEngine.Random.Range(1.0f, 1.5f) * Vector3.up - camera_transform.forward);
                damage_text.SetDamageText(hit_bone_node.position + damage_text_offset, damage, critical, false, color);
            }
        }
    }
     */

    // Update is called once per frame
    void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
            Debug.LogError("aaaa");
			DamageText dt = DamageTextPool.Instance.GetDamageText();
			dt.SetDamageText(Vector3.zero, Random.Range(1, 100), false, false, Color.red);

			dt = DamageTextPool.Instance.GetDamageText();
			dt.SetDamageText(Vector3.right, Random.Range(100, 1000), false, false, Color.red);

			dt = DamageTextPool.Instance.GetDamageText();
			dt.SetDamageText(-Vector3.right, Random.Range(1000, 10000), false, false, Color.red);

			dt = DamageTextPool.Instance.GetDamageText();
			dt.SetDamageText(Vector3.up, Random.Range(10000, 99999), false, false, Color.red);

			dt = DamageTextPool.Instance.GetDamageText();
			dt.SetDamageText(Vector3.up + Vector3.right, Random.Range(1, 99999), false, false, Color.red);

			dt = DamageTextPool.Instance.GetDamageText();
			dt.SetDamageText(Vector3.up - Vector3.right, Random.Range(1, 99999), false, false, Color.red);
		}
        else if( Input.GetKeyDown(KeyCode.Z))
        {
            var damage_text = DamageTextPool.Instance.GetDamageText();
            if (damage_text != null)
            {
                

                Vector3 damage_text_offset = 0.5f * (UnityEngine.Random.Range(-1.0f, 1.0f) * camera_transform.right + UnityEngine.Random.Range(1.0f, 1.5f) * Vector3.up - camera_transform.forward);
                damage_text.SetDamageText(hit_bone_node.position + damage_text_offset, Random.Range(1, 100000) , true, false, critical_color);
            }

        }

	}
}
