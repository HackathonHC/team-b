using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace SLA
{
    public class Gradation : BaseVertexEffect
    {
        [SerializeField]
        Color _color = Color.white;

        [SerializeField]
        bool[] _targetVertices = {false, false, true, true};

        public override void ModifyVertices (List<UIVertex> verts)
        {
            for(int i=0; i<verts.Count ; ++i)
            {
                if (_targetVertices[i % _targetVertices.Length])
                {
                    var vert = verts[i];
                    vert.color = _color;
                    verts[i] = vert;
                }
            }
        }
    }
}
