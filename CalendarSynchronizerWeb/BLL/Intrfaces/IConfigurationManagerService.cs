using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Intrfaces
{
    public interface IConfigurationManagerService<out T> :IOptions<T> where T:class, new()
    {
        public void ChangeAppSettingValue(Action<T> applyChanges);
        public T GetAppSettingValue();
    }
}
