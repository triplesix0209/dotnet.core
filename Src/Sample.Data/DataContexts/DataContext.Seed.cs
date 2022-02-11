using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Sample.Data.Entities;

namespace Sample.Data.DataContexts
{
    public partial class DataContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var assembly = Assembly.GetExecutingAssembly();

            #region [setting]

            var settings = new List<(string Key, SettingEntity Data)>();
            using (var stream = assembly.GetManifestResourceStream("Sample.Data.Resources.Setting.json"))
            using (var reader = new StreamReader(stream))
            {
                var data = JArray.Parse(reader.ReadToEnd());
                foreach (var item in data)
                {
                    var setting = new SettingEntity
                    {
                        Id = Guid.Parse(item.Value<string>("id")),
                        Code = item.Value<string>("code"),
                        Description = item.Value<string>("description"),
                        Value = item.Value<string>("value"),
                    };
                    settings.Add((setting.Code, setting));
                }
            }

            if (settings.Any())
                builder.Entity<SettingEntity>().HasData(settings.Select(x => x.Data));

            #endregion
        }
    }
}
