using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Core
{
    public class DIHelper
    {
        public static IServiceProvider ServiceProvider;

        public static object GetService<IService>()
        {
            return ServiceProvider.GetService(typeof(IService));
        }
    }
}
