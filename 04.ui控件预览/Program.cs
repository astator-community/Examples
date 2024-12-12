using System;
using System.Threading.Tasks;
using Astator.Platform.Droid.Ui;
using Astator.Script;
using Microsoft.Maui.Graphics;

namespace _04.ui控件预览
{
    public class Program
    {
        [EntryMethod]
        public static async Task MainAsync(string workDir)
        {
            var xml =
        @"
        <linear orientation='vertical'>
            <card>
                <text margin='10,0' h='60' text='ui控件预览' textColor='#000000' textSize='22' textStyle='bold' gravity='center'/>
            </card>
            
            <scroll>
                <linear orientation='vertical'>
                    <text text='文本控件:' textSize='18' textStyle='bold' margin='10,15,0,5'/>
                    <text text='普通文本' textSize='16' margin='20,5'/>
                    <text text='加粗文本' textSize='16' textStyle='bold' margin='20,5'/>
                    <text text='加粗斜体文本' textSize='16' textStyle='bold|italic' margin='20,5'/>
                    <text autoLink='web' text='超链接文本: https://www.baidu.com' textSize='16'  margin='20,5'/>
                    <edit hint='我是编辑框提示' textSize='16' w='240' margin='20,5'/>

                    <text text='按钮控件:' textSize='18' textStyle='bold' margin='10,15,0,5'/>
                    <btn text='按钮'  margin='20,5'/>
                    <btn text='透明按钮' bg='#00ffffff'  margin='20,5'/>
                    <btn text='圆角按钮' bg='#e2a3a3'  margin='20,5' radius='15'/>
                    <btn id='btn' text='点我' w='200' bg='#b9a2b0' margin='20,5'/>

                     <text text='图片控件:' textSize='18' textStyle='bold' margin='10,15,0,5'/>
                     <img src='appicon.png' w='50' h='50' margin='20,5' />
                     <img src='appicon.png' radius='25' w='50' h='50' margin='20,5' />

                    <text text='复选框控件:' textSize='18' textStyle='bold' margin='10,15,0,5'/>
                    <check checked='true' text='默认颜色' margin='20,0'/>
                    <check checked='true' text='修改状态框颜色' color='#e2a3a3' margin='20,0'/>
                    <check checked='true' text='修改状态框和文本颜色' color='#e2a3a3' textColor='red' margin='20,0'/>

                    <text text='开关控件:' textSize='18' textStyle='bold' margin='10,15,0,5'/>
                    <switch checked='true' text='默认颜色' margin='20,0'/>
                    <switch checked='true' text='修改按钮颜色' color='#e2a3a3' w='240' margin='20,0'/>
                    <switch checked='true' text='修改按钮和文本颜色' color='#e2a3a3' textColor='red' w='240' margin='20,0'/>
                    
                    <text text='单选框控件:' textSize='18' textStyle='bold' margin='10,15,0,5'/>
                    <radioGroup orientation='vertical' position='1' margin='20,5'>
                        <radio text='选项1'/>
                        <radio text='选项2'/>
                        <radio text='选项3'/>
                        <radio text='选项4'/>
                    </radioGroup>
                    <radioGroup orientation='horizontal' position='3' margin='20,5'>
                        <radio color='#e2a3a3' text='选项1'/>
                        <radio color='#e2a3a3' text='选项2'/>
                        <radio color='#e2a3a3' text='选项3'/>
                        <radio color='#e2a3a3' text='选项4'/>
                    </radioGroup>

                    <text text='下拉框控件:' textSize='18' textStyle='bold' margin='10,15,0,5'/>
                    <spinner entries='选项1|选项2|选项3|选项4' position='2' margin='20,0'/>
                    <spinner textColor='red' entries='选项1|选项2|选项3|选项4' position='1' margin='20,0'/>
                    <web url='https://astator.site/docs'/>
                </linear>
            </scroll>
        </linear>";


            var activity = await TemplateActivity.CreateAsync(workDir);

            activity.RunOnUiThread(() =>
            {
                activity.SetStatusBarColor(Color.Parse("#f0f3f6"), Color.Parse("#000000"));
                var layout = activity.ParseXml(xml);
                activity.Show(layout);
            });


        }
    }

}
