using System;
using System.Threading.Tasks;
using Astator.Platform.Droid.Permissions;
using Astator.Platform.Droid.Script;
using Astator.Platform.Droid.Ui;
using Astator.Platform.Droid.Ui.Base;
using Astator.Script;
using Microsoft.Maui.Graphics;

namespace _05.简易悬浮窗
{
    public class Program
    {
        [EntryMethod]
        public static async Task MainAsync(string workDir)
        {
            var xml = @"
        <linear orientation='vertical'>
            <card>
                <text margin='10,0' h='60' text='悬浮窗' textColor='#000000' textSize='22' textStyle='bold' gravity='center'/>
            </card>
            <linear orientation='vertical'>
                <btn id='show1' text='开启悬浮窗' textColor='#ffffff' margin='20,5' bg='#444f7d'/>
            </linear>
        </linear>";

            var activity = await TemplateActivity.CreateAsync(workDir);

            activity.RunOnUiThread(() =>
            {
                var layout = activity.ParseXml(xml);
                activity.Show(layout);

                activity.SetStatusBarColor(Color.Parse("#f0f3f6"), Color.Parse("#000000"));

                activity["show1"].On("click", new OnClickListener((v) =>
                {
                    if (!PermissionHelper.CheckFloaty())
                    {
                        if (PermissionHelper.ReqFloatyAsync().Result)
                            Globals.Toast("申请悬浮窗权限成功");

                        return;
                    }

                    var floaty = new Floaty(activity, "robot.png", 48, 0, 200,
                        [
                        "discover.png",
                        "tag.png",
                        "location.png",
                        "mine.png",
                        ],
                        [
                        new OnClickListener((v) => { Console.Write("第1个选项"); Globals.Toast("第1个选项"); }),
                        new OnClickListener((v) => { Console.Write("第2个选项"); Globals.Toast("第2个选项"); }),
                        new OnClickListener((v) => { Console.Write("第3个选项"); Globals.Toast("第3个选项"); }),
                        new OnClickListener((v) => { Console.Write("第4个选项"); Globals.Toast("第4个选项"); }),
                        ]);

                    activity.OnFinishedCallbacks.Add(() =>
                    {
                        //释放悬浮窗
                        floaty.Dispose();
                    });

                }));


            });

        }
    }

}
