﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCloudClient.NetworkResults
{
    public class GetFolderTokenResult
    {
        public bool Succeed { get; set; }
        public List<Models.FileAndEditorMap> TokenHolders { get; set; }

        public string FailMessage()
        {
            string message = "Some users are editing some files:";
            foreach (Models.FileAndEditorMap map in TokenHolders)
            {
                message += "\n" + map.Message();
            }
            return message;
        }

        public static GetFolderTokenResult FromJson(string json)
        {
            JToken token = JToken.Parse(json);
            return FromJson(token);
        }
        
        public static GetFolderTokenResult FromJson(JToken json)
        {
            GetFolderTokenResult result = new GetFolderTokenResult();
            result.Succeed = json["Succeed"].ToString().Equals("true");
            result.TokenHolders = new List<Models.FileAndEditorMap>();
            JArray maps = (JArray)json["TokenHolders"];
            foreach (var item in maps)
            {
                result.TokenHolders.Add(new Models.FileAndEditorMap
                {
                    Name = item["Name"].ToString(),
                    TokenHolder = Models.User.FromJson(item["TokenHolder"])
                });
            }
            return result;
        }
    }
}
