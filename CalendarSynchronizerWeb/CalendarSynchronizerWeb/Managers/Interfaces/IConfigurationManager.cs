﻿using Microsoft.Extensions.Options;

namespace CalendarSynchronizerWeb.Managers.Interfaces
{
    public interface IConfigurationManager<out T> : IOptions<T> where T : class, new()
    {
        public void ChangeAppSettingValue(Action<T> applyChanges);
        public T GetAppSettingValue();
    }
}
