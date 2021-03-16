using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.ViewModels
{
	public class CheckBoxViewModel : CWTBaseViewModel
    {
        public int CheckBoxId { get; set; }
        public string CheckBoxText { get; set; }
        public bool IsChecked { get; set; }

        public CheckBoxViewModel()
        {
            IsChecked = false;
        }
    }
}