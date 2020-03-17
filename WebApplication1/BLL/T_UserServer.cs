using Common;
using Common.Core;
using Common.AutoMapper;
using DAL;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ModelDto;

namespace BLL
{
    public class T_UserServer : DbBase<t_user>, IDependencyTransient
    {
        public T_UserServer(EFDbcontext _DbContext):base(_DbContext)
        {
            base.DbContext = _DbContext;
        }

        public override ResultModel Validate(t_user entity)
        {
            if (entity.UserName == "")
            {

            }
            return new ResultModel() { Success=true,Message="通过验证" };
        }

    }
}
