using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 所有要在StartUp中使用DI的接口默认要实现此接口,生存周期为Transient
    /// </summary>
    public interface IDependencyTransient
    {
    }

    /// <summary>
    /// 所有要在StartUp中使用DI的接口默认要实现此接口,生存周期为Scoped
    /// </summary>
    public interface IDependencyScoped
    {
    }
}
