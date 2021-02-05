﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using SuperBMDLib.BMD;

namespace SuperBMDLib.Rigging
{
    public class Weight
    {
        public int WeightCount { get; private set; }
        public List<float> Weights { get; private set; }
        public List<int> BoneIndices { get; private set; }
        public Matrix4 FinalTransformation { get; private set; }

        public Weight()
        {
            Weights = new List<float>();
            BoneIndices = new List<int>();
            FinalTransformation = Matrix4.Zero;
        }

        public void AddWeight(float weight, int boneIndex)
        {
            Weights.Add(weight);
            BoneIndices.Add(boneIndex);
            if (boneIndex > 0)
            {

            }
            WeightCount++;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Weight))
                return false;

            WeightEqualityComparer comp = new WeightEqualityComparer();
            return comp.Equals(this, obj as Weight);
        }

        public override int GetHashCode()
        {
            WeightEqualityComparer comp = new WeightEqualityComparer();
            return comp.GetHashCode(this);
        }

        public void Transform(List<Bone> skeleton)
        {
            FinalTransformation = Matrix4.Zero;

            for (int i = 0; i < WeightCount; i++)
            {
                Matrix4 boneIBMMatrix = skeleton[BoneIndices[i]].InverseBindMatrix;
                Matrix4 boneTransMatrix = skeleton[BoneIndices[i]].TransformationMatrix;
                float weight = Weights[i];

                FinalTransformation = FinalTransformation + ((boneIBMMatrix * boneTransMatrix) * weight);
            }
        }
    }

    class WeightEqualityComparer : IEqualityComparer<Weight>
    {
        public bool Equals(Weight x, Weight y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x.WeightCount != y.WeightCount)
                return false;

            for (int i = 0; i < x.WeightCount; i++)
            {
                if (x.BoneIndices[i] != y.BoneIndices[i] || x.Weights[i] != y.Weights[i])
                    return false;
            }

            return true;
        }

        public int GetHashCode(Weight obj)
        {
            int hash = 0;

            for (int i = 0; i < obj.WeightCount; i++)
            {
                float weightHash = (obj.Weights[i] * 10) * obj.BoneIndices[i];
                hash ^= (int)weightHash;
            }

            return hash;
        }
    }
}
