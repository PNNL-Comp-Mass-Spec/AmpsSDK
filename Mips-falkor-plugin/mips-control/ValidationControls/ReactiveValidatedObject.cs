using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using mips_control.Data;
using Mips_net.Commands;
using ReactiveUI;

namespace mips_control.ValidationControls
{
	public class ReactiveValidatedObject: ReactiveObject, INotifyDataErrorInfo
	{
		protected Dictionary<string, List<string>> _errors =
			new Dictionary<string, List<string>>();

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		protected ReactiveValidatedObject()
		{
			
		}

		//public IObservable<EventPattern<DataErrorsChangedEventArgs>> ErrorsChangedObservable
		//{
		//	get
		//	{
		//		return Observable.FromEventPattern<EventHandler<DataErrorsChangedEventArgs>, DataErrorsChangedEventArgs>(handler =>
		//			ErrorsChanged += handler, handler => ErrorsChanged -= handler);
		//	}
		//}

		public void RaiseErrorChanged(string propertyName)
		{
			//EventHandler<DataErrorsChangedEventArgs> handler = ErrorsChanged;
			//if (handler == null) return;
			//var arg = new DataErrorsChangedEventArgs(propertyName);
			//handler.Invoke(this, arg);

			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}

		public System.Collections.IEnumerable GetErrors(string propertyName)
		{
			List<string> errorsList = new List<string>();
			if (propertyName != null)
			{
				_errors.TryGetValue(propertyName, out errorsList);
				return errorsList;
			}
			else
				return null;
		}

		public bool HasErrors
		{
			get
			{
				var errorCount = _errors.Values.FirstOrDefault(l => l.Count > 0);
				if (errorCount != null)
				{
					return true;
				}
				else
				{
					return false;
				}

			}
		}

		public  void Validation(int? commandValue, string command, [CallerMemberName]string commandValueProperty = null)
		{
			if (commandValueProperty == null)
			{
				return;
			}
			//Task task = new Task(() => PropertyValidation(commandValue, command, commandValueProperty));
			//task.Start();
			Task.Run(()=>PropertyValidation( commandValue,  command, commandValueProperty)).Wait();

		}

		protected object _lock = new object();

		protected virtual void PropertyValidation(int? commandValue,  string command, string commandValueProperty)
		{
		}

		
	}
}
