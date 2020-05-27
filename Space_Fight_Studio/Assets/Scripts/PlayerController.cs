using UnityEngine;
using UnityEngine.Events;

public class PlayerController: MonoBehaviour
{
	[SerializeField] private int m_JumpForce = 15;
	[SerializeField] private int m_DashForce = 50;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] private bool m_AirControl = false;
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private Transform m_GroundCheck;
	[SerializeField] private Transform m_CeilingCheck;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;
	private int nb_jumps = 2;
	private int nb_dash = 2;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;
	private Vector3 m_Velocity = Vector3.zero;
	// public float startDashTime;


	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	// public BoolEvent OnCrouchEvent; CROUCH
	// private bool m_wasCrouching = false; CROUCH

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	// private void OnCollisionEnter2D(Collision2D col)
	// {
	// 	Debug.Log(col.gameObject.tag);
	// 	if (col.gameObject.tag == "Player")
	// 	{
	// 		Debug.Log ("Collision!");
	// 		col.gameObject.GetComponent<Rigidbody2D>().AddForce (new Vector2(1000f, 0f));
	// 	}
	// }

	public void Move(float move, bool jump, bool dash, float move2)
	{
		if (dash && nb_dash > 0)
		{
			// Debug.Log("dash !");
			nb_dash -= 1;
			if (m_FacingRight)
			{
				// Debug.Log("positiv move");
				m_Rigidbody2D.AddForce(Vector2.right * m_DashForce, ForceMode2D.Impulse);
			}
			else if (!m_FacingRight) {
				// Debug.Log("neg move");
				m_Rigidbody2D.AddForce(Vector2.left * m_DashForce, ForceMode2D.Impulse);
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// Move the character by finding the target velocity
			// Vector3 targetVelocityY = new Vector2(m_Rigidbody2D.velocity.x, move2);
			Vector3 targetVelocityX = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			//Enable Vertical movement 
			if (move2 < 0f) //If the Down key is press
				targetVelocityX = new Vector2(move * 10f, move2 * 0.25f);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, (targetVelocityX), ref m_Velocity, m_MovementSmoothing);

			if (move > 0 && !m_FacingRight)
			{
				Flip();
			}
			else if (move < 0 && m_FacingRight)
			{
				Flip();
			}
		}

		if (jump && nb_jumps > 0)
		{
			m_Grounded = false;
			nb_jumps -= 1;
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
			m_Rigidbody2D.AddForce(Vector2.up * m_JumpForce, ForceMode2D.Impulse);
		}

		if (m_Grounded) {
			nb_jumps = 2;
			nb_dash = 2;
		}
	}


	private void Flip()
	{
		m_FacingRight = !m_FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
