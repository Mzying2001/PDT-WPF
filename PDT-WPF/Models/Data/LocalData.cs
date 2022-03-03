using Newtonsoft.Json;
using PDT_WPF.Utils;
using System;
using System.IO;

namespace PDT_WPF.Models.Data
{
    /// <summary>
    /// 保存在本地的数据
    /// </summary>
    public static class LocalData
    {
        public const string DATA_PATH = "./Data";
        public const string SETTINGS_JSON = DATA_PATH + "/Settings.json";

        public static Settings Settings { get; set; }


        /// <summary>
        /// 保存所有数据
        /// </summary>
        public static void SaveAllData()
        {
            SaveSettings();
        }


        /// <summary>
        /// 保存设置
        /// </summary>
        public static void SaveSettings()
        {
            try
            {
                File.WriteAllText(SETTINGS_JSON, JsonConvert.SerializeObject(Settings));
            }
            catch (Exception e)
            {
                MessageBoxHelper.ShowError(e);
            }
        }


        /// <summary>
        /// 初始化本地数据
        /// </summary>
        static LocalData()
        {
            try
            {
                if (!Directory.Exists(DATA_PATH))
                    Directory.CreateDirectory(DATA_PATH);
            }
            catch (Exception e)
            {
                MessageBoxHelper.ShowError(e);
            }


            //初始化设置
            if (File.Exists(SETTINGS_JSON))
            {
                try
                {
                    Settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SETTINGS_JSON));
                }
                catch (Exception e)
                {
                    MessageBoxHelper.ShowError(e);
                    Settings = Settings.Default;
                }
            }
            else
            {
                Settings = Settings.Default;
            }
        }
    }
}
