using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _3rdEyE.Models;
using _3rdEyE.ManagingTools;
namespace _3rdEyE.BLL
{
    public class BaseBLL
    {
        public AppUser CurrentUser;
        public DBEnityModelContainer db = new DBEnityModelContainer();
        public BaseBLL()
        {
            CurrentUser = CommonClass.GetCurrentUser();
        }
    }
}