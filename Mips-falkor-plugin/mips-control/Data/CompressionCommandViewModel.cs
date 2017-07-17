using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using mips_control.ValidationControls;
using Mips_net.Commands;
using Mips_net.Device;
using ReactiveUI;

namespace mips_control.Data
{
	public class CompressionCommandViewModel : ReactiveValidatedObject
	{
		public string twaveCommand;
		public int? twaveCommandValue;
		


		public CompressionCommandViewModel(string command)
		{
			this.TWaveCommand = command;
			this.WhenAny(x => x.TWaveCommandValue, x => x).Subscribe(observedChange => Validation(TWaveCommandValue,TWaveCommand, observedChange.GetPropertyName()));

			
			//this.WhenAnyValue(x => x.HasErrors).Where(x=>x)
			//	.Do(x=>HasError=true);
		}

		public string TWaveCommand
		{
			get { return this.twaveCommand; }
			set { this.RaiseAndSetIfChanged(ref this.twaveCommand, value); }
		}

		public int? TWaveCommandValue
		{
			get { return this.twaveCommandValue; }
			set { this.RaiseAndSetIfChanged(ref this.twaveCommandValue, value); }
		}


		public bool EnableParameterValue
		{
			get
			{
				if (TWaveCommand == TWaveParameter.s.ToString() || TWaveCommand == TWaveParameter.r.ToString() ||
				    TWaveCommand == "[")
					return false;
				return true;
			}
		}

		

		protected override void PropertyValidation( int? commandValue, string command, string commandValueProperty)
		{
			lock (_lock)
			{
				//PropertyInfo prop = TypeDescriptor.GetProperty(propName)
				List<string> errorsList;
				if (!_errors.TryGetValue(commandValueProperty, out errorsList))
					errorsList = new List<string>();
				else errorsList.Clear();
				if (commandValue == null)
				{
					if (command != TWaveParameter.s.ToString() && command != TWaveParameter.r.ToString() && (command != "[") && (command != "]"))
						errorsList.Add("The parameter value can't be null or empty.");

				}
				else
				{
					switch (command)
					{
						case "M":
							if (!Regex.IsMatch(commandValue.ToString(), RegexExpression.modeRegex.ToString()))
								errorsList.Add("Mode can only be 0 or 1 or 2");
							break;
						case "S":
							if (!Regex.IsMatch(commandValue.ToString(), RegexExpression.switchStateRegex.ToString()))
								errorsList.Add("Switch state can be only 0 or 1");
							break;
						default:
							if (!Regex.IsMatch(commandValue.ToString(), RegexExpression.commandValueRegex.ToString()))
								errorsList.Add("Value cannot be zero");
							break;
					}


				}

				_errors[commandValueProperty] = errorsList;
				if (errorsList.Count > 0)
				{
					//HasError = true;
					RaiseErrorChanged(commandValueProperty);
				}

				
			}


		}

		
		





	}
}
