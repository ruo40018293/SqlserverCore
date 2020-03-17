using Common;
using DAL;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class T_SystemServer : DbBase<t_system>, IDependencyTransient
    {
        public T_SystemServer(EFDbcontext _DbContext) : base(_DbContext)
        {
            base.DbContext = _DbContext;
        }
    }
}
