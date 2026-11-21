using System.Diagnostics;

namespace AmplaData.Web.MetaData
{
    public static class AmplaModelMetadataProvider
    {
        public static void LogMetadataRequest(
            System.Type modelType,
            string propertyName)
        {
            Debug.WriteLine(
                "CreateMetadata for {0} {1}",
                modelType,
                propertyName);
        }
    }
}