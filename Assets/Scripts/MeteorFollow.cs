using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFollow : MonoBehaviour
{
    [Header("REFERENCES")] 
        public Rigidbody _rb;
        public GameObject _target;
        //public GameObject _explosionPrefab;

        [Header("MOVEMENT")] 
        public float _speed = 15;
        public float _rotateSpeed = 95;

        [Header("PREDICTION")] 
        public float _maxDistancePredict = 100;
        public float _minDistancePredict = 5;
        public float _maxTimePrediction = 5;
        private Vector3 _standardPrediction, _deviatedPrediction;

        [Header("DEVIATION")] 
        public float _deviationAmount = 50;
        public float _deviationSpeed = 2;

        private CharacterController _targetController;

        private GameObject Player;
        private float i = 0;

        private void Start() {
            Player = GameObject.FindGameObjectWithTag("Player");
            _target = GameObject.FindGameObjectWithTag("Player");
            _targetController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
            StartCoroutine(Destroy(15f));
        }

        private void FixedUpdate() {

            //FindObjectOfType<AudioManager>().PlaySound("Meteor");
            //FindObjectOfType<AudioManager>().VolumeAdd("Meteor");

            if(Player != null) {
                float distance = transform.position.z - Player.transform.position.z;

                if(distance < -5f){
                    FindObjectOfType<AudioManager>().StopSound("Meteor");
                    Destroy(this.gameObject);
                }
            }

            _rb.velocity = transform.forward * _speed;

            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));

            PredictMovement(leadTimePercentage);

            AddDeviation(leadTimePercentage);

            RotateRocket();
        }

        private void PredictMovement(float leadTimePercentage) {
            var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

            _standardPrediction = _target.transform.position + _targetController.velocity * predictionTime;
        }

        private void AddDeviation(float leadTimePercentage) {
            var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
            
            var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

            _deviatedPrediction = _standardPrediction + predictionOffset;
        }

        private void RotateRocket() {
            var heading = _deviatedPrediction - transform.position;

            var rotation = Quaternion.LookRotation(heading);
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _standardPrediction);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_standardPrediction, _deviatedPrediction);
        }

        private void OnCollisionEnter(Collision collision) {

            if (collision.gameObject.tag == "Wall" ||collision.gameObject.tag == "Ground" ) {
                Destroy(this.gameObject);
            }

            if (collision.gameObject.tag == "Player"){
                StartCoroutine(Destroy(1f));
            }
        
        }

    private IEnumerator Destroy(float i)
    {
        yield return new WaitForSeconds(i);
        FindObjectOfType<AudioManager>().StopSound("Meteor");
        Destroy(this.gameObject);
    }

}
