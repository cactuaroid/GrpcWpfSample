using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace GrpcChatSample.Server.Wpf
{
    /// <summary>
    /// App.xaml の相互作用ロジック
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
