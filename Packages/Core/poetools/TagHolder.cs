using UnityEngine;

namespace poetools
{
    public class TagHolder : MonoBehaviour
    {
        public Tag[] tags;

        public bool Is(params Tag[] desiredTags)
        {
            foreach (var desiredTag in desiredTags)
            {
                foreach (var tag1 in tags)
                {
                    if (tag1 == desiredTag)
                        return true;
                }
            }

            return false;
        }
    }
}