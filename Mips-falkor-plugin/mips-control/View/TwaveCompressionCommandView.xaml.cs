using MahApps.Metro.Controls;
using mips_control.Data;
using mips_control.ViewModel;
using ReactiveUI;
using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace mips_control.View
{
	/// <summary>
	/// Interaction logic for TWaveCompressorCommandView.xaml
	/// </summary>

	[Export]
	[PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class TwaveCompressionCommandView : MetroWindow, IViewFor<TwaveCompressionCommandViewModel>
	{
		
		public TwaveCompressionCommandView()
		{
			InitializeComponent();
		}
		[ImportingConstructor]
		public TwaveCompressionCommandView(TwaveCompressionCommandViewModel viewModel):this()
		{
			this.ViewModel = viewModel;
			this.WhenAnyValue(x => x.ViewModel).BindTo(this, view => view.DataContext);
			this.ViewModel.RemoveCommand.Subscribe(unit =>
			{
				var itemsToRemove = this.CommandDataGrid.SelectedItems.Cast<CompressionCommandViewModel>();
				using (this.ViewModel.SelectedCommandViewModelList.SuppressChangeNotifications())
				{
					this.ViewModel.SelectedCommandViewModelList.RemoveAll(itemsToRemove);
				}
			});

		}

		public TwaveCompressionCommandViewModel ViewModel { get; set; }
		object IViewFor.ViewModel
		{
			get { return ViewModel; }
			set { ViewModel = value as TwaveCompressionCommandViewModel; }
		}
	}
}
