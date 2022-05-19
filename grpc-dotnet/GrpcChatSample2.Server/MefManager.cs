using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace GrpcChatSample2.Server
{
    public class MefManager
    {
        public static CompositionContainer Container { get; private set; }

        public static void Initialize()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            //Adds all the parts found in the same assembly
            //catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            catalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));

            //Create the CompositionContainer with the parts in the catalog
            Container = new CompositionContainer(catalog);
        }
    }
}
