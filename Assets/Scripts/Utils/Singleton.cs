using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Singleton<T> where T : class, new()
    {
        protected bool IsInit = false;
        private static readonly object LockObject = new object();
        private static T s_instance;

        public static T Instance
        {
            get
            {
                lock( LockObject )
                {
                    if( s_instance != null )
                    {
                        return s_instance;
                    }

                    s_instance = new T();
                    ( s_instance as Singleton<T> )?.Init();
                    return s_instance;
                }
            }
        }


        public static void DestroyInstance()
        {
            lock( LockObject )
            {
                s_instance = null;
            }
        }

        protected Singleton() { }

        public virtual void Init()
        {
            IsInit = true;
        }
    }
}