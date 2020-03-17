using Common;
using Common.Core;
using DAL;
using Model.EntityCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class BaseTransactionHelper : EFTransactionBase, IDependencyTransient
    {
        public BaseTransactionHelper(EFDbcontext _DbContext):base(_DbContext)
        {
            DbContext = _DbContext;
        }
    }
}
