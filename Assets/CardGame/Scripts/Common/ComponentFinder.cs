using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class ComponentFinder
    {
        public static List<T> FindAll<T>() where T : class
        {
            var results = new List<T>();

            var rootObjects = UnityEngine.SceneManagement.SceneManager
                .GetActiveScene()
                .GetRootGameObjects();

            foreach (var root in rootObjects)
            {
                var found = root.GetComponentsInChildren<T>(true);
                results.AddRange(found);
            }
            return results;
        }
    }
}

