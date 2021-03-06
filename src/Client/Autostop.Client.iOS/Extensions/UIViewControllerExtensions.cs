﻿using Autostop.Client.Abstraction.ViewModels;
using Autostop.Client.iOS.UI;
using CoreGraphics;
using UIKit;

namespace Autostop.Client.iOS.Extensions
{
    public static class UIViewControllerExtensions
    {
        public static NavigationBarSearchTextField CreateSearchViewOnNavigationBar(this UIViewController viewController, ISearchableViewModel vm)
        {
	        var navigationBar = ((UINavigationController)UIApplication.SharedApplication.KeyWindow.RootViewController).NavigationBar;

			viewController.EdgesForExtendedLayout = UIRectEdge.None;
            viewController.NavigationItem.HidesBackButton = true;

			var frame = new CGRect(0, 0, navigationBar.Frame.Size.Width - 20, 40);
			var searchTextField = new NavigationBarSearchTextField(frame, vm);
            viewController.NavigationItem.TitleView = searchTextField;
            searchTextField.BecomeFirstResponder();

	        return searchTextField;
        }
    }
}