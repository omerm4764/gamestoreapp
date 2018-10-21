using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStoreApp.Models.VM
{
    public class AlertInformation
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public AlertType AlertType { get; set; }
    }
    public enum AlertType
    {
        success,
        danger,
        warning,
        info
    }
}