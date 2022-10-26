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

        private void Start() {
            _target = GameObject.FindGameObjectWithTag("Player");
            _targetController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
            StartCoroutine(Destroy());
        }

        private void FixedUpdate() {
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
            
            if (collision.gameObject.tag == "Player") {
                Destroy(this.gameObject);
            }
            if (collision.gameObject.tag == "Wall") {
                Destroy(this.gameObject);
            }
        
        }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(15f);
        Destroy(this.gameObject);
    }

}