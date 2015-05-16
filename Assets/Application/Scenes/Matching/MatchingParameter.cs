using UnityEngine;
using System.Collections;

namespace TB
{
    public class MatchingParameter
    {
        static MatchingParameter _instance;
        public static MatchingParameter Instance
        {
            get
            {
                return _instance ?? (_instance = new MatchingParameter());
            }
        }

        public PlayerType playerType;

        MatchingParameter()
        {
        }
    }
}
