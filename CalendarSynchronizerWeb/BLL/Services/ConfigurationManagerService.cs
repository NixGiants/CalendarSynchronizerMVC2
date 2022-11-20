using BLL.Intrfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace BLL.Services
{
    public class ConfigurationManagerService<T>//: IConfigurationManagerService<T> where T : class, new()
    {       
        //private readonly IWebHostBuilder _environment;
        //private readonly IOptionsMonitor<T> _options;
        //private readonly IConfigurationRoot _configuration;
        //private readonly string _section;
        //private readonly string _file;
        //public ConfigurationManagerService(
        //    IWebHostBuilder environment,
        //    IOptionsMonitor<T> options,
        //    IConfigurationRoot configuration,
        //    string section,
        //    string file)
        //{
        //    _environment = environment;
        //    _options = options;
        //    _configuration = configuration;
        //    _section = section;
        //    _file = file;
        //}
        //public T Value => _options.CurrentValue;
        //public T Get(string name) => _options.Get(name);
        //public void ChangeAppSettingValue(Action<T> applyChanges)
        //{
        //    var fileProvider = _environment.ContentRootFileProvider;
        //    var fileInfo = fileProvider.GetFileInfo(_file);
        //    var physicalPath = fileInfo.PhysicalPath;
        //    var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
        //    var sectionObject = jObject.TryGetValue(_section, out JToken section) ?
        //        JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());
        //    applyChanges(sectionObject);
        //    jObject[_section] = JObject.Parse(JsonConvert.SerializeObject(sectionObject));
        //    File.WriteAllText(physicalPath, JsonConvert.SerializeObject(jObject, Formatting.Indented));
        //    _configuration.Reload();
        //}

        //public T GetAppSettingValue()
        //{
        //    var fileProvider = _environment.ContentRootFileProvider;
        //    var fileInfo = fileProvider.GetFileInfo(_file);
        //    var physicalPath = fileInfo.PhysicalPath;
        //    var jObject = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(physicalPath));
        //    var sectionObject = jObject.TryGetValue(_section, out JToken section) ?
        //        JsonConvert.DeserializeObject<T>(section.ToString()) : (Value ?? new T());
        //    return sectionObject;
        //}
    }
}
