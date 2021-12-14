using System;
using System.Collections.Generic;
using System.IO;

namespace ContratosMVVM.Services
{
    public enum INIConfig { MySQLIP, BaseClipp, APIKey, NumLoja,
        UltimaSync
    };
    public class INIFileService
    {
        private readonly Dictionary<INIConfig, string> _defaultValues = new();


        private readonly Dictionary<INIConfig, string> _loadedConfigs;
        public INIFileService()
        {
            _defaultValues.Add(INIConfig.UltimaSync, default);
            if (!File.Exists("config.ini"))
            {
                File.Create("config.ini").Close();
            }
            _loadedConfigs = new Dictionary<INIConfig, string>();
            foreach (var item in File.ReadAllLines("config.ini"))
            {
                if (item.Split('=').Length != 2)
                {
                    continue;
                }
                var (key, value) = (
                    Enum.Parse<INIConfig>(item.Split('=')[0]),
                    item.Split('=')[1]
                );

                _loadedConfigs.Add(key, value);
            }


        }

        public string Get(INIConfig config)
        {
            if (_loadedConfigs.ContainsKey(config)) return _loadedConfigs.GetValueOrDefault(config);
            _loadedConfigs.Add(config, _defaultValues[config]);

            File.AppendAllLines("config.ini", new[] { $"{Enum.GetName(config)}={_defaultValues[config]}" });
            return _loadedConfigs.GetValueOrDefault(config);
        }

        public void Set(INIConfig config, string value)
        {
            if (_loadedConfigs.ContainsKey(config)) return;
            _loadedConfigs.Add(config, string.Empty);

            File.AppendAllLines("config.ini", new[] { $"{Enum.GetName(config)}={value}" });
        }
    }

}