using Common.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Data
{
    public interface IValidate<TEntity>
    {
        ResultModel Validate(TEntity entity);
    }
}
