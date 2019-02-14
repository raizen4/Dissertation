using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Client_Mobile.ViewModels
{
	using Prism.Navigation;

	public class LoginPageViewModel : ViewModelBase
	{
		public LoginPageViewModel(INavigationService navigationService)
			: base(navigationService)
		{
			Title = "Login Page";
		}
	}
}
