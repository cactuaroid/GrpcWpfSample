using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace GrpcChatSample2.Server.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [ImportMany]
        private List<IService> m_services = null;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MefManager.Initialize();

            MefManager.Container.ComposeParts(this);
            m_services.ForEach((x) => x.Start());
        }
    }
}
