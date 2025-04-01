using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlacksmithUI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            DontDestroyOnLoad(instance);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

