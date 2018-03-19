using System;

namespace Sherlock.Framework.Environment.Modules
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SherlockFeatureAttribute : Attribute
    {
        public SherlockFeatureAttribute(string featureName)
        {
            this.FeatureName = featureName;
        }

        public string FeatureName { get; set; }
    }
}