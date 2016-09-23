using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace BlueOpexEmployeesManager.Common
{
    public class SecurityHelper
    {
        public class EMPrincipal : IPrincipal
        {
            //some custom properties
            //public bool IsValid()
            //{
            //    //custom authentication logic
            //}

            private IIdentity identity;
            public IIdentity Identity
            {
                get { return this.identity; }
            }

            public bool IsInRole(string role)
            {
                return true;
            }
        }
    }
}