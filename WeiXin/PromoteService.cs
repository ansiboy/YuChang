using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuChang.Core
{
    public class PromoteService
    {
        private AccessToken accessToken;
        public PromoteService(AccessToken accessToken)
        {
            this.accessToken = accessToken;
        }

        public string GenerateSquareCode(int sceneId)
        {
            //var sceneId = 1;
            var url = "qrcode/create?access_token=" + accessToken;
            //var values = new Dictionary<string, string>();
            //values["action_name"] = "QR_LIMIT_SCENE";
            //values["action_info"] = "{\"scene\": {\"scene_id\": @0}}".Replace("@0", sceneId.ToString());
            var values = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": @0}}}"
                            .Replace("@0", sceneId.ToString());
            var data = Utility.GetWeiXinJson(url, values);
            var ticket = data["ticket"] as string;

            return ticket;

        }
    }
}
