using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using mips_control.View;
using MahApps.Metro.Controls;
using RJCP.IO.Ports;
using Splat;


namespace mips_control
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		
		public CompositionContainer _container;
		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
				var catalog = new AggregateCatalog();
				catalog.Catalogs.Add(new DirectoryCatalog("."));
				catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly));
			   // catalog.Catalogs.Add(new AssemblyCatalog(typeof(TCompressionCommandView).Assembly));
				_container = new CompositionContainer(catalog);
				_container.ComposeParts(this);
			}
			catch (CompositionException compositionException)
			{
				System.Diagnostics.Trace.WriteLine(compositionException.StackTrace);

			}


			base.OnStartup(e);
			DependencyObject ob = this._container.GetExportedValue<TwaveCompressionCommandView>();
			Application.Current.MainWindow = (MetroWindow)ob;
			Application.Current.MainWindow.Show();
		}

		private void RegisterDependencies()
		{
			//Locator.CurrentMutable.RegisterLazySingleton(() => new seria(), typeof(TCompressionCommandViewModel));
			//Locator.CurrentMutable.RegisterLazySingleton(() => new TCompressionCommandViewModel(),typeof(TCompressionCommandViewModel));
			//Locator.CurrentMutable.RegisterLazySingleton(() => new TCompressionCommandView(), typeof(TCompressionCommandView));

		}
	}
}
